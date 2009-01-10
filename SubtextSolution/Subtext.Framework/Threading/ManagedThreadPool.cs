#region Disclaimer/Info
///////////////////////////////////////////////////////////////////////////////////////////////////
// Subtext WebLog
// 
// Subtext is an open source weblog system that is a fork of the .TEXT
// weblog system.
//
// For updated news and information please visit http://subtextproject.com/
// Subtext is hosted at SourceForge at http://sourceforge.net/projects/subtext
// The development mailing list is at subtext-devs@lists.sourceforge.net 
//
// This project is licensed under the BSD license.  See the License.txt file for more information.
///////////////////////////////////////////////////////////////////////////////////////////////////
#endregion

using System;
using System.Collections;
using System.Globalization;
using System.Threading;
using Subtext.Framework.Configuration;
using Subtext.Framework.Logging;

#region Credits
// Stephen Toub
// stoub@microsoft.com
// 
// ManagedThreadPool.cs
// ThreadPool written in 100% managed code.  Mimics the core functionality of
// the System.Threading.ThreadPool class.
//
// http://www.gotdotnet.com/Community/UserSamples/Details.aspx?SampleGuid=bf59c98e-d708-4f8e-9795-8bae1825c3b6
//
// HISTORY:
// v1.0.1 - Disposes of items remaining in queue when the queue is emptied
//		  - Catches errors thrown during execution of delegates
//		  - Added reset to semaphore, called during empty queue
//		  - Catches errors when unable to dequeue delegates
// v1.0.0 - Original version
// 
// August 27, 2002
// v1.0.1
#endregion

//TODO: Remove this and put in separate assembly.
namespace Subtext.Framework.Threading
{
	/// <summary>Managed thread pool.</summary>
	public static class ManagedThreadPool
	{	
		static Log Log = new Log();

		/// <summary>Queue of all the callbacks waiting to be executed.</summary>
		static Queue _waitingCallbacks;
		/// <summary>
		/// Used to signal that a worker thread is needed for processing.  Note that multiple
		/// threads may be needed simultaneously and as such we use a semaphore instead of
		/// an auto reset event.
		/// </summary>
		static Semaphore _workerThreadNeeded;
		/// <summary>List of all worker threads at the disposal of the thread pool.</summary>
		static ArrayList _workerThreads;
		/// <summary>Number of threads currently active.</summary>
		static int _inUseThreads;

		/// <summary>Initialize the thread pool.</summary>
		static ManagedThreadPool()
		{
			MaxThreads = Config.Settings.QueuedThreads;
			
			// Create our thread stores; we handle synchronization ourself
			// as we may run into situtations where multiple operations need to be atomic.
			// We keep track of the threads we've created just for good measure; not actually
			// needed for any core functionality.
			_waitingCallbacks = new Queue();
			_workerThreads = new ArrayList();
			_inUseThreads = 0;

			// Create our "thread needed" event
			_workerThreadNeeded = new Semaphore(0);
			
			// Create all of the worker threads
			for(int i = 0; i < MaxThreads; i++)
			{
				// Create a new thread and add it to the list of threads.
				Thread newThread = new Thread(new ThreadStart(ProcessQueuedItems));
				_workerThreads.Add(newThread);

				// Configure the new thread and start it
				newThread.Name = "ManagedPoolThread #" + i.ToString(CultureInfo.InvariantCulture);
				newThread.IsBackground = true;
				newThread.Start();
			}
		}

		/// <summary>Queues a user work item to the thread pool.</summary>
		/// <param name="callback">
		/// A WaitCallback representing the delegate to invoke when the thread in the 
		/// thread pool picks up the work item.
		/// </param>
		public static void QueueUserWorkItem(WaitCallback callback)
		{
			// Queue the delegate with no state
			QueueUserWorkItem(callback, null);
		}

		/// <summary>Queues a user work item to the thread pool.</summary>
		/// <param name="callback">
		/// A WaitCallback representing the delegate to invoke when the thread in the 
		/// thread pool picks up the work item.
		/// </param>
		/// <param name="state">
		/// The object that is passed to the delegate when serviced from the thread pool.
		/// </param>
		public static void QueueUserWorkItem(WaitCallback callback, object state)
		{
			// Create a waiting callback that contains the delegate and its state.
			// At it to the processing queue, and signal that data is waiting.
			WaitingCallback waiting = new WaitingCallback(callback, state);
			using(TimedLock.Lock(_waitingCallbacks.SyncRoot)) { _waitingCallbacks.Enqueue(waiting); }
			_workerThreadNeeded.AddOne();
		}

		/// <summary>Empties the work queue of any queued work items.</summary>
		public static void EmptyQueue()
		{
			using(TimedLock.Lock(_waitingCallbacks.SyncRoot))
			{ 
				try 
				{
					// Try to dispose of all remaining state
					foreach(object obj in _waitingCallbacks)
					{
						WaitingCallback callback = (WaitingCallback)obj;
						if (callback.State is IDisposable) ((IDisposable)callback.State).Dispose();
					}
				} 
				catch
				{
					// Make sure an error isn't thrown.
				}

				// Clear all waiting items and reset the number of worker threads currently needed
				// to be 0 (there is nothing for threads to do)
				_waitingCallbacks.Clear();
				_workerThreadNeeded.Reset(0);
			}
		}

		/// <summary>Gets the number of threads at the disposal of the thread pool.</summary>
        public static int MaxThreads {
            get;
            private set;
        }
		
        /// <summary>Gets the number of currently active threads in the thread pool.</summary>
		public static int ActiveThreads { 
            get { 
                return _inUseThreads; 
            } 
        }

		/// <summary>Gets the number of callback delegates currently waiting in the thread pool.</summary>
		public static int WaitingCallbacks { 
            get { 
                using(TimedLock.Lock(_waitingCallbacks.SyncRoot)) { 
                    return _waitingCallbacks.Count; 
                } 
            } 
        }

		/// <summary>A thread worker function that processes items from the work queue.</summary>
		private static void ProcessQueuedItems()
		{
			// Process indefinitely
			while(true)
			{
				// Get the next item in the queue.  If there is nothing there, go to sleep
				// for a while until we're woken up when a callback is waiting.
				WaitingCallback callback = null;
				while (callback == null) {
					// Try to get the next callback available.  We need to lock on the 
					// queue in order to make our count check and retrieval atomic.
					using(TimedLock.Lock(_waitingCallbacks.SyncRoot)) {
						if (_waitingCallbacks.Count > 0)
						{
							try { callback = (WaitingCallback)_waitingCallbacks.Dequeue(); } 
							catch{} // make sure not to fail here
						}
					}

					// If we can't get one, go to sleep.
					if (callback == null) _workerThreadNeeded.WaitOne();
				}

				// We now have a callback.  Execute it.  Make sure to accurately
				// record how many callbacks are currently executing.
				try 
				{
					Interlocked.Increment(ref _inUseThreads);
					callback.Callback(callback.State);
				} 
				catch(Exception exc)
				{
					// Make sure we don't throw here.
                    try
                    {
                        Log.Error("Error while processing queued items.", exc);
                    }
                    catch
                    {
                        Log.Error("Unexpected exception while processing queued items.");
                    }
				}
				finally 
				{
					Interlocked.Decrement(ref _inUseThreads);
				}
			}
		}

		/// <summary>Used to hold a callback delegate and the state for that delegate.</summary>
		private class WaitingCallback
		{
			/// <summary>Initialize the callback holding object.</summary>
			/// <param name="callback">Callback delegate for the callback.</param>
			/// <param name="state">State with which to call the callback delegate.</param>
			public WaitingCallback(WaitCallback callback, object state)
			{
				Callback = callback;
				State = state;
			}

			/// <summary>Gets the callback delegate for the callback.</summary>
            public WaitCallback Callback {
                get;
                private set;
            }
			/// <summary>Gets the state with which to call the callback delegate.</summary>
            public object State
            {
                get;
                private set;
            }
		}
	}
}

