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
			Subtext.Framework.Configuration.Tracking tracking = Config.Settings.Tracking;
			WebTrack = tracking.EnableWebStats;
			AggTrack = tracking.EnableAggBugs;
			QueueStats = tracking.QueueStats;
		}
		
		private static bool WebTrack = false;
		private static bool AggTrack = false;
		private static bool QueueStats = false;
		
		/// <summary>
		/// Records the request in the database for statistics/tracking purposes.
		/// </summary>
		/// <param name="ev">The ev.</param>
		/// <returns></returns>
		public static bool Track(EntryView ev)
		{
			if(QueueStats)
			{
				return Stats.AddQuedStats(ev);
			}
			else
			{
				return Stats.TrackEntry(ev);
			}
		}

		/// <summary>
		/// Records the request in the database for statistics/tracking purposes.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="EntryID">The entry ID.</param>
		/// <param name="BlogId">The blog ID.</param>
		/// <returns></returns>
		public static bool Track(HttpContext context, int EntryID, int BlogId)
		{
			if(WebTrack)
			{
				if(FilterUserAgent(context.Request.UserAgent))
				{
					if(context.Request.HttpMethod != "POST")
					{
						string refUrl = GetReferral(context.Request);
						EntryView ev = new EntryView();
						ev.EntryID = EntryID;
						ev.BlogId = BlogId;
						ev.ReferralUrl = refUrl;
						ev.PageViewType = PageViewType.WebView;
						return Track(ev);
					}
				}
			}
			return false;
		}

		//TODO: Unit test this method. Also clean it up and make it more self-descriptive.
		private static string GetReferral(HttpRequest Request)
		{
			Uri uri = UrlFormats.GetUriReferrerSafe(Request);
			
			if(uri == null)
				return null;

			string url = uri.ToString();

			url = url.ToLower(System.Globalization.CultureInfo.InvariantCulture).Replace("www.",string.Empty);
			string fqu = Config.CurrentBlog.RootUrl.ToString().ToLower(System.Globalization.CultureInfo.InvariantCulture).Replace("www.",string.Empty);
			if(Regex.IsMatch(url, fqu,RegexOptions.IgnoreCase))
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
