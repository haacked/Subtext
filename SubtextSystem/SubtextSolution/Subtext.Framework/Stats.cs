#region Disclaimer/Info
///////////////////////////////////////////////////////////////////////////////////////////////////
// .Text WebLog
// 
// .Text is an open source weblog system started by Scott Watermasysk. 
// Blog: http://ScottWater.com/blog 
// RSS: http://scottwater.com/blog/rss.aspx
// Email: Dottext@ScottWater.com
//
// For updated news and information please visit http://scottwater.com/dottext and subscribe to 
// the Rss feed @ http://scottwater.com/dottext/rss.aspx
//
// On its release (on or about August 1, 2003) this application is licensed under the BSD. However, I reserve the 
// right to change or modify this at any time. The most recent and up to date license can always be fount at:
// http://ScottWater.com/License.txt
// 
// Please direct all code related questions to:
// GotDotNet Workspace: http://www.gotdotnet.com/Community/Workspaces/workspace.aspx?id=e99fccb3-1a8c-42b5-90ee-348f6b77c407
// Yahoo Group http://groups.yahoo.com/group/DotText/
// 
///////////////////////////////////////////////////////////////////////////////////////////////////
#endregion

using System;
using System.Collections.Specialized;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;
using Subtext.Framework.Providers;
using Subtext.Framework.Threading;
using Subtext.Framework.Tracking;
using Subtext.Framework.Util;
using Subtext.Framework.Logging;

namespace Subtext.Framework
{
	/// <summary>
	/// Class used for managing stats. Provides facilities for queing stats. 
	/// This is used for trackbacks and pingbacks.
	/// </summary>
	public sealed class Stats
	{
		static Log Log = new Log();

		private Stats(){}

		static EntryViewCollection queuedStatsList = null;
		static int queuedAllowCount;

		/// <summary>
		/// Static Constructor.
		/// </summary>
		static Stats()
		{
			if(Config.Settings.Tracking.QueueStats)
			{
				queuedStatsList = new EntryViewCollection();
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
			using(TimedLock.Lock(queuedStatsList.SyncRoot))
			{
				if(save)
				{
					EntryView[] eva = new EntryView[queuedStatsList.Count];
					queuedStatsList.CopyTo(eva, 0);

					ClearTrackEntryQueue(new EntryViewCollection(eva));
					
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
				using(TimedLock.Lock(queuedStatsList.SyncRoot))
				{
					//make sure the pool queue was not cleared during a wait for the lock
					if(queuedStatsList.Count >= queuedAllowCount)
					{
						EntryView[] eva = new EntryView[queuedStatsList.Count];
						queuedStatsList.CopyTo(eva,0);

						ClearTrackEntryQueue(new EntryViewCollection(eva));
						queuedStatsList.Clear();	
					
					}
				}
			}
			queuedStatsList.Add(ev);
			return true;
		}

		private static bool ClearTrackEntryQueue(EntryViewCollection evc)
		{
			ProcessStats ps = new ProcessStats(evc);
			ps.Enqueue();
			
			return true;
		}

		private class ProcessStats
		{
			public ProcessStats(EntryViewCollection evc)
			{
				_evc = evc;
			}
			protected EntryViewCollection _evc;

			public void Enqueue()
			{
				ManagedThreadPool.QueueUserWorkItem(new System.Threading.WaitCallback(Process));
			}

			private void Process(object state)
			{
				Stats.TrackEntry(this._evc);
			}
		}

		#region Data

		public static PagedViewStatCollection GetPagedViewStats(int pageIndex, int pageSize, DateTime beginDate, DateTime endDate)
		{
			return ObjectProvider.Instance().GetPagedViewStats(pageIndex, pageSize, beginDate, endDate);
		}

		public static PagedReferrerCollection GetPagedReferrers(int pageIndex, int pageSize)
		{
			return ObjectProvider.Instance().GetPagedReferrers(pageIndex, pageSize);
		}

		public static PagedReferrerCollection GetPagedReferrers(int pageIndex, int pageSize, int EntryID)
		{
			return ObjectProvider.Instance().GetPagedReferrers(pageIndex, pageSize, EntryID);
		}

		#endregion
		

		/// <summary>
		/// Calls out to the data provider to track the specified 
		/// <see cref="EntryView"/> instance.
		/// </summary>
		/// <param name="ev">Ev.</param>
		/// <returns></returns>
		public static bool TrackEntry(EntryView ev)
		{
			return ObjectProvider.Instance().TrackEntry(ev);
		}

		/// <summary>
		/// Calls out to the data provider to track the specified 
		/// <see cref="EntryViewCollection"/> instance.
		/// </summary>
		/// <param name="evc">Evc.</param>
		/// <returns></returns>
		public static bool TrackEntry(EntryViewCollection evc)
		{
			return ObjectProvider.Instance().TrackEntry(evc);
		}

		/// <summary>
		/// Performs the notification, wether it be a pingback or trackback.
		/// </summary>
		/// <param name="entry">Entry.</param>
		public static void Notify(Entry entry)
		{
			StringCollection links = TrackHelpers.GetLinks(entry.Body);

			if(links != null && links.Count > 0)
			{
				int count = links.Count;


				string description = null;
				string blogname = Config.CurrentBlog.Title;
				if(entry.HasDescription)
				{
					description = entry.Description;
				}
				else
				{
					description = entry.Title;	
				}

				PingBackNotificatinProxy pbnp = new PingBackNotificatinProxy();
				TrackBackNotificationProxy tbnp = new TrackBackNotificationProxy();

				for(int i = 0; i < count; i++)
				{
					try
					{
						string link = links[i];
						string pageText = BlogRequest.GetPageText(link);
						if(pageText != null)
						{
							pbnp.Ping(pageText,entry.Link,link);
							tbnp.TrackBackPing(pageText,link,entry.Title,entry.Link,blogname,description);
						}
					}
					catch(Exception e)
					{
						//TODO: We should only catch exceptions we expect...
						//		for the rest, let them propagate...
						//		This one occurs on a separate thread, so it may make sense
						//		to completely eat it.
						Log.Warn("Error occurred while performing a pingback or trackback.", e);
						//Do nothing, just eat it :(
					}
				}
			}
		}
	}
}

