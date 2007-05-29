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
using Subtext.Extensibility.Interfaces;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;
using Subtext.Framework.Providers;
using Subtext.Framework.Threading;
using Subtext.Framework.Logging;

namespace Subtext.Framework
{
	/// <summary>
	/// Class used for managing stats. Provides facilities for queing stats. 
	/// This is used for trackbacks and pingbacks.
	/// </summary>
	public static class Stats
	{
		static List<EntryView> queuedStatsList = null;
		static int queuedAllowCount;

		/// <summary>
		/// Static Constructor.
		/// </summary>
		static Stats()
		{
			if(Config.Settings.Tracking.QueueStats)
			{
				queuedStatsList = new List<EntryView>();
				queuedAllowCount = Config.Settings.Tracking.QueueStatsCount;
			}
		}

		/// <summary>
		/// Clears the queue of statistics.  If save is specified, then 
		/// stats are saved to an <see cref="EntryView"/>
		/// </summary>
		/// <param name="save">Save.</param>
		/// <returns></returns>
		public static bool ClearQueue(bool save)
		{
			using(TimedLock.Lock(queuedStatsList))
			{
				if(save)
				{
					EntryView[] eva = new EntryView[queuedStatsList.Count];
					queuedStatsList.CopyTo(eva, 0);

					ClearTrackEntryQueue(new List<EntryView>(eva));
				}
				queuedStatsList.Clear();	
			}
			return true;
		}

		/// <summary>
		/// Adds <see cref="EntryView"/> instance to the stats queue.
		/// </summary>
		/// <param name="ev">Ev.</param>
		/// <returns></returns>
		public static bool AddQuedStats(EntryView ev)
		{
			//Check for the limit
			if(queuedStatsList.Count >= queuedAllowCount)
			{
				//aquire the lock
				using(TimedLock.Lock(queuedStatsList))
				{
					//make sure the pool queue was not cleared during a wait for the lock
					if(queuedStatsList.Count >= queuedAllowCount)
					{
						EntryView[] eva = new EntryView[queuedStatsList.Count];
						queuedStatsList.CopyTo(eva, 0);

						ClearTrackEntryQueue(new List<EntryView>(eva));
						queuedStatsList.Clear();	
					
					}
				}
			}
			queuedStatsList.Add(ev);
			return true;
		}

		private static bool ClearTrackEntryQueue(IEnumerable<EntryView> evc)
		{
			ProcessStats ps = new ProcessStats(evc);
			ps.Enqueue();
			
			return true;
		}

		private class ProcessStats
		{
			public ProcessStats(IEnumerable<EntryView> evc)
			{
				_evc = evc;
			}
			protected IEnumerable<EntryView> _evc;

			public void Enqueue()
			{
				ManagedThreadPool.QueueUserWorkItem(new System.Threading.WaitCallback(Process));
			}

			private void Process(object state)
			{
				TrackEntry(this._evc);
			}
		}

		#region Data

        public static IPagedCollection<ViewStat> GetPagedViewStats(int pageIndex, int pageSize, DateTime beginDate, DateTime endDate)
		{
			return ObjectProvider.Instance().GetPagedViewStats(pageIndex, pageSize, beginDate, endDate);
		}

        public static IPagedCollection<Referrer> GetPagedReferrers(int pageIndex, int pageSize)
		{
			return GetPagedReferrers(pageIndex, pageSize, int.MinValue);
		}

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

