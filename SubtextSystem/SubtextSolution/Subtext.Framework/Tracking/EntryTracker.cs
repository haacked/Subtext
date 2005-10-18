using System;
using System.Text.RegularExpressions;
using System.Web;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;
using Subtext.Framework.Format;

namespace Subtext.Framework.Tracking
{
	/// <summary>
	/// Summary description for TrackEntry.
	/// </summary>
	public class EntryTracker
	{
		private EntryTracker()
		{

		}

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

		public static bool Track(HttpContext context, int EntryID, int BlogID)
		{
			if(WebTrack)
			{
				if(FilerUserAgent(context.Request.UserAgent))
				{
					if(context.Request.HttpMethod != "POST")
					{
						string refUrl = GetReferral(context.Request);
						EntryView ev = new EntryView();
						ev.EntryID = EntryID;
						ev.BlogID = BlogID;
						ev.ReferralUrl = refUrl;
						ev.PageViewType = PageViewType.WebView;
						return Track(ev);


					}
				}
			}
			return false;
		}

		private static string GetReferral(HttpRequest Request)
		{
			string url = UrlFormats.GetUriReferrerSafe(Request);
			if(url != null)
			{
				url = url.ToLower(System.Globalization.CultureInfo.InvariantCulture).Replace("www.",string.Empty);
				string fqu = Config.CurrentBlog.RootUrl.ToLower(System.Globalization.CultureInfo.InvariantCulture).Replace("www.",string.Empty);
				if(Regex.IsMatch(url,fqu,RegexOptions.IgnoreCase))
				{
					return null;
				}
			}
			return url;
		}

		private static bool FilerUserAgent(string agent)
		{
			return (agent != null && agent.Length > 0 && Regex.IsMatch(agent,"msie|mozilla|opera",RegexOptions.IgnoreCase));
		}
	}
}
