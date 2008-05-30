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
using System.Text.RegularExpressions;
using System.Web;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;
using Subtext.Framework.Format;
using Subtext.Framework.Logging;
using Subtext.Framework.Properties;

namespace Subtext.Framework.Tracking
{
	/// <summary>
	/// Summary description for TrackEntry.
	/// </summary>
	public static class EntryTracker
	{
		static Log Log = new Log();

		static EntryTracker()
		{
			Configuration.Tracking tracking = Config.Settings.Tracking;
			WebTrack = tracking.EnableWebStats;
			QueueStats = tracking.QueueStats;
		}
		
		private static bool WebTrack = false;
		private static bool QueueStats = false;
		
		/// <summary>
		/// Records the request in the database for statistics/tracking purposes.
		/// </summary>
		/// <param name="ev">The ev.</param>
		/// <returns></returns>
		public static void Track(EntryView ev)
		{
			if(QueueStats)
			{
				Stats.AddQuedStats(ev);
			}
			else
			{
				Stats.TrackEntry(ev);
			}
		}

		/// <summary>
		/// Records the request in the database for statistics/tracking purposes.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="EntryID">The entry ID.</param>
		/// <param name="BlogId">The blog ID.</param>
		/// <returns></returns>
        public static void Track(HttpContext context, int EntryID, int BlogId)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context", Resources.ArgumentNull_Generic);
            }

			if (!WebTrack || !FilterUserAgent(context.Request.UserAgent) || context.Request.HttpMethod == "POST")
				return;
            
            string refUrl = GetReferral(context.Request);
            EntryView ev = new EntryView();
            ev.EntryId = EntryID;
            ev.BlogId = BlogId;
            ev.ReferralUrl = refUrl;
            ev.PageViewType = PageViewType.WebView;
            Track(ev);
        }

		//TODO: Unit test this method. Also clean it up and make it more self-descriptive.
		private static string GetReferral(HttpRequest Request)
		{
			Uri uri = UrlFormats.GetUriReferrerSafe(Request);
			
			if(uri == null)
				return null;

			string url = uri.ToString();

			url = BlogInfo.StripWwwPrefixFromHost(url.ToString());
			string fqu = BlogInfo.StripWwwPrefixFromHost(Config.CurrentBlog.RootUrl.ToString());
			
			if(Regex.IsMatch(url, fqu, RegexOptions.IgnoreCase))
			{
				return null;
			}
			
			if(url.Length == 0)
			{
				Log.Warn("Somehow the referral was an empty string and not null.");	
				return null;
			}

			return url;
		}

		private static bool FilterUserAgent(string agent)
		{
			return (agent != null && agent.Length > 0 && Regex.IsMatch(agent,"msie|mozilla|opera",RegexOptions.IgnoreCase));
		}
	}
}
