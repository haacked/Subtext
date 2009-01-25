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
	public class StatsRepository {

        public StatsRepository(ObjectProvider repository, Subtext.Framework.Configuration.Tracking trackingSettings) {
            Repository = repository;
            Settings = trackingSettings;
        }

        protected ObjectProvider Repository {
            get;
            private set;
        }

        public Subtext.Framework.Configuration.Tracking Settings {
            get;
            private set;
        }

		static List<EntryView> _queuedStatsList = null;

		/// <summary>
		/// Static Constructor.
		/// </summary>
		static StatsRepository()
		{
			if(Config.Settings.Tracking.QueueStats)
			{
				_queuedStatsList = new List<EntryView>();
			}
		}

		/// <summary>
		/// Returns the number of stats in the queue in a thread unsafe manner.
		/// </summary>
		public static int QueueCount
		{
			get {
				return _queuedStatsList.Count;
			}
		}

		/// <summary>
		/// Clears the queue of statistics.  If save is specified, then 
		/// stats are saved to an <see cref="EntryView"/>
		/// </summary>
		/// <param name="save">Save.</param>
		/// <returns></returns>
		public void ClearQueue(bool save)
		{
			using(TimedLock.Lock(_queuedStatsList)) {
				if(save) {
					EntryView[] entryViews = new EntryView[_queuedStatsList.Count];
					_queuedStatsList.CopyTo(entryViews, 0);

					ClearTrackEntryQueue(new List<EntryView>(entryViews));
				}
				_queuedStatsList.Clear();	
			}
		}

		/// <summary>
		/// Adds <see cref="EntryView"/> instance to the stats queue.
		/// </summary>
		/// <param name="entryView">Ev.</param>
		/// <returns></returns>
		public void AddQuedStats(EntryView entryView)
		{
			//Check for the limit
			if (_queuedStatsList.Count >= Settings.QueueStatsCount)
			{
				//aquire the lock
				using(TimedLock.Lock(_queuedStatsList))
				{
					//make sure the pool queue was not cleared during a wait for the lock
					if(_queuedStatsList.Count >= Settings.QueueStatsCount)
					{
						EntryView[] entryViewCopies = new EntryView[_queuedStatsList.Count];
						_queuedStatsList.CopyTo(entryViewCopies, 0);

						ClearTrackEntryQueue(new List<EntryView>(entryViewCopies));
						_queuedStatsList.Clear();	
					
					}
				}
			}
			_queuedStatsList.Add(entryView);
		}

		private void ClearTrackEntryQueue(IEnumerable<EntryView> evc)
		{
			ProcessStats ps = new ProcessStats(evc, this);
			ps.Enqueue();
		}

		//Class to encapsulate asynch processing of stats.
		private class ProcessStats
		{
			public ProcessStats(IEnumerable<EntryView> evc, StatsRepository stats)
			{
                Stats = stats;
				entryViews = evc;
			}

            StatsRepository Stats {
                get;
                set;
            }

			protected IEnumerable<EntryView> entryViews;

			public void Enqueue()
			{
				ManagedThreadPool.QueueUserWorkItem(new System.Threading.WaitCallback(Process));
			}

			private void Process(object state)
			{
				Stats.TrackEntry(entryViews);
			}
		}

		/// <summary>
		/// Returns a pageable collection of the referrers for the specified entry.
		/// </summary>
		/// <param name="pageIndex">Index of the page.</param>
		/// <param name="pageSize">Size of the page.</param>
		/// <returns></returns>
        public IPagedCollection<Referrer> GetPagedReferrers(int pageIndex, int pageSize)
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
        public IPagedCollection<Referrer> GetPagedReferrers(int pageIndex, int pageSize, int entryId)
		{
			return Repository.GetPagedReferrers(pageIndex, pageSize, entryId);
		}

		/// <summary>
		/// Calls out to the data provider to track the specified 
		/// <see cref="EntryView"/> instance.
		/// </summary>
		/// <param name="ev">Ev.</param>
		/// <returns></returns>
		public void TrackEntry(EntryView ev)
		{
            Repository.TrackEntry(ev);
		}

		/// <summary>
		/// Calls out to the data provider to track the specified 
		/// <see cref="IEnumerable{EntryView}"/> instance.
		/// </summary>
		/// <param name="evc">Evc.</param>
		/// <returns></returns>
		public bool TrackEntry(IEnumerable<EntryView> evc)
		{
            return Repository.TrackEntry(evc);
		}
	}
}

