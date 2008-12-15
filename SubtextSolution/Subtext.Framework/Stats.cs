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

using System.Collections.Generic;
using Subtext.Extensibility.Interfaces;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;
using Subtext.Framework.Providers;
using Subtext.Framework.Threading;

namespace Subtext.Framework
{
	/// <summary>
	/// Class used for managing stats. Provides facilities for queing stats. 
	/// This is used for trackbacks and pingbacks.
	/// </summary>
	/// <remarks>
	/// Currently, we only track referrers to specific entries.
	/// </remarks>
	public static class Stats
	{
		static List<EntryView> queuedStatsList = null;

		/// <summary>
		/// Static Constructor.
		/// </summary>
		static Stats()
		{
			if(Config.Settings.Tracking.QueueStats)
			{
				queuedStatsList = new List<EntryView>();
			}
		}

		/// <summary>
		/// Returns the number of stats in the queue in a thread unsafe manner.
		/// </summary>
		public static int QueueCount
		{
			get
			{
				return queuedStatsList.Count;
			}
		}

		/// <summary>
		/// Clears the queue of statistics.  If save is specified, then 
		/// stats are saved to an <see cref="EntryView"/>
		/// </summary>
		/// <param name="save">Save.</param>
		/// <returns></returns>
		public static void ClearQueue(bool save)
		{
			using(TimedLock.Lock(queuedStatsList))
			{
				if(save)
				{
					EntryView[] entryViews = new EntryView[queuedStatsList.Count];
					queuedStatsList.CopyTo(entryViews, 0);

					ClearTrackEntryQueue(new List<EntryView>(entryViews));
				}
				queuedStatsList.Clear();	
			}
		}

		/// <summary>
		/// Adds <see cref="EntryView"/> instance to the stats queue.
		/// </summary>
		/// <param name="entryView">Ev.</param>
		/// <returns></returns>
		public static void AddQuedStats(EntryView entryView)
		{
			//Check for the limit
			if (queuedStatsList.Count >= Config.Settings.Tracking.QueueStatsCount)
			{
				//aquire the lock
				using(TimedLock.Lock(queuedStatsList))
				{
					//make sure the pool queue was not cleared during a wait for the lock
					if(queuedStatsList.Count >= Config.Settings.Tracking.QueueStatsCount)
					{
						EntryView[] entryViewCopies = new EntryView[queuedStatsList.Count];
						queuedStatsList.CopyTo(entryViewCopies, 0);

						ClearTrackEntryQueue(new List<EntryView>(entryViewCopies));
						queuedStatsList.Clear();	
					
					}
				}
			}
			queuedStatsList.Add(entryView);
		}

		private static void ClearTrackEntryQueue(IEnumerable<EntryView> evc)
		{
			ProcessStats ps = new ProcessStats(evc);
			ps.Enqueue();
		}

		//Class to encapsulate asynch processing of stats.
		private class ProcessStats
		{
			public ProcessStats(IEnumerable<EntryView> evc)
			{
				entryViews = evc;
			}
			protected IEnumerable<EntryView> entryViews;

			public void Enqueue()
			{
				ManagedThreadPool.QueueUserWorkItem(new System.Threading.WaitCallback(Process));
			}

			private void Process(object state)
			{
				TrackEntry(entryViews);
			}
		}

		#region Data
		/// <summary>
		/// Returns a pageable collection of the referrers for the specified entry.
		/// </summary>
		/// <param name="pageIndex">Index of the page.</param>
		/// <param name="pageSize">Size of the page.</param>
		/// <returns></returns>
        public static IPagedCollection<Referrer> GetPagedReferrers(int pageIndex, int pageSize)
		{
			return GetPagedReferrers(pageIndex, pageSize, NullValue.NullInt32 /* entryId */);
		}

		/// <summary>
		/// Returns a pageable collection of the referrers for the specified entry.
		/// </summary>
		/// <param name="pageIndex">Index of the page.</param>
		/// <param name="pageSize">Size of the page.</param>
		/// <param name="entryId">The entry id.</param>
		/// <returns></returns>
        public static IPagedCollection<Referrer> GetPagedReferrers(int pageIndex, int pageSize, int entryId)
		{
			return ObjectProvider.Instance().GetPagedReferrers(pageIndex, pageSize, entryId);
		}

		#endregion
		

		/// <summary>
		/// Calls out to the data provider to track the specified 
		/// <see cref="EntryView"/> instance.
		/// </summary>
		/// <param name="ev">Ev.</param>
		/// <returns></returns>
		public static void TrackEntry(EntryView ev)
		{
			ObjectProvider.Instance().TrackEntry(ev);
		}

		/// <summary>
		/// Calls out to the data provider to track the specified 
		/// <see cref="IEnumerable{EntryView}"/> instance.
		/// </summary>
		/// <param name="evc">Evc.</param>
		/// <returns></returns>
		public static bool TrackEntry(IEnumerable<EntryView> evc)
		{
			return ObjectProvider.Instance().TrackEntry(evc);
		}
	}
}

