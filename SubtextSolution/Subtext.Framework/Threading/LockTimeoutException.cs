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
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using System.Diagnostics;
using System.Collections;
using System.Threading;
using Subtext.Framework.Properties;

namespace Subtext.Framework.Threading
{
	/// <summary>
	/// Thrown when a lock times out.
	/// </summary>
	[Serializable]
	public class LockTimeoutException : Exception
	{
#if DEBUG
		object _lockTarget;
		StackTrace _blockingStackTrace;
		static readonly Hashtable _failedLockTargets = new Hashtable();

		/// <summary>
		/// Sets the stack trace for the given lock target 
		/// if an error occurred.
		/// </summary>
		/// <param name="lockTarget">Lock target.</param>
		public static void ReportStackTraceIfError(object lockTarget)
		{
			lock (_failedLockTargets)
			{
				if (_failedLockTargets.ContainsKey(lockTarget))
				{
					ManualResetEvent waitHandle = _failedLockTargets[lockTarget] as ManualResetEvent;
					if (waitHandle != null)
					{
						waitHandle.Set();
					}
					_failedLockTargets[lockTarget] = new StackTrace();
					//Also. if you don't call GetBlockingStackTrace()
					//the lockTarget doesn't get removed from the hash 
					//table and so we'll always think there's an error
					//here (though no locktimeout exception is thrown).
				}
			}
		}

		/// <summary>
		/// Creates a new <see cref="LockTimeoutException"/> instance.
		/// </summary>
		/// <remarks>Use this exception.</remarks>
		/// <param name="lockTarget">Object we tried to lock.</param>
		public LockTimeoutException(object lockTarget)
			: base(Resources.TimeoutWaitingForLock)
		{
			lock (_failedLockTargets)
			{
				// This is safer in case somebody forgot to remove 
				// the lock target.
				ManualResetEvent waitHandle = new ManualResetEvent(false);
				_failedLockTargets[lockTarget] = waitHandle;
			}
			_lockTarget = lockTarget;
		}
		/// <summary>
		/// Stack trace of the thread that holds a lock on the object 
		/// this lock is attempting to acquire when it fails.
		/// </summary>
		/// <param name="timeout">Number of milliseconds to wait for the blocking stack trace.</param>
		public StackTrace GetBlockingStackTrace(int timeout)
		{
			if (timeout < 0)
				throw new InvalidOperationException(Resources.InvalidOperation_WaitTimeLessThanZero);

			ManualResetEvent waitHandle;
			lock (_failedLockTargets)
			{
				waitHandle = _failedLockTargets[_lockTarget] as ManualResetEvent;
			}
			if (timeout > 0 && waitHandle != null)
			{
				waitHandle.WaitOne(timeout, false);
			}
			lock (_failedLockTargets)
			{
				//Hopefully by now we have a stack trace.
				_blockingStackTrace = _failedLockTargets[_lockTarget] as StackTrace;
			}

			return _blockingStackTrace;
		}
#endif
		/// <summary>
		/// Creates a new <see cref="LockTimeoutException"/> instance.
		/// </summary>
		public LockTimeoutException()
			: base(Resources.TimeoutWaitingForLock)
		{
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="message"></param>
		public LockTimeoutException(string message)
			: base(message)
		{ }

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="message"></param>
		/// <param name="innerException"></param>
		public LockTimeoutException(string message, Exception innerException)
			: base(message, innerException)
		{ }

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		protected LockTimeoutException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{ }

		/// <summary>
		/// Returns a string representation of the exception.
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			string toString = base.ToString();
#if DEBUG
			if (_blockingStackTrace != null)
			{
				toString += "\n-------Blocking Stack Trace--------\n" + _blockingStackTrace.ToString();
			}
#endif
			return toString;
		}

	}
}
