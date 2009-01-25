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
using Subtext.Framework.Components;
using Subtext.Framework.Format;
using Subtext.Framework.Logging;
using Subtext.Framework.Properties;

namespace Subtext.Framework.Tracking
{
	/// <summary>
	/// Class used to record requests for an entry.
	/// </summary>
	public class EntryTracker
	{
        static Log Log = new Log();

        public EntryTracker(StatsRepository statsRecorder) {
            Settings = statsRecorder.Settings;
            StatsRecorder = statsRecorder;
        }


        protected Subtext.Framework.Configuration.Tracking Settings {
            get;
            private set;
        }

        protected StatsRepository StatsRecorder {
            get;
            private set;
        }

		/// <summary>
		/// Records the request in the database for statistics/tracking purposes.
		/// </summary>
		/// <param name="ev">The ev.</param>
		/// <returns></returns>
		public void Track(EntryView ev)
		{
			if(Settings.QueueStats) {
				StatsRecorder.AddQuedStats(ev);
			}
			else {
				StatsRecorder.TrackEntry(ev);
			}
		}

		/// <summary>
		/// Records the request in the database for statistics/tracking purposes.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="EntryID">The entry ID.</param>
		/// <param name="BlogId">The blog ID.</param>
		/// <returns></returns>
        public void Track(ISubtextContext context, int EntryID, int BlogId)
        {
            if (context == null) {
                throw new ArgumentNullException("context", Resources.ArgumentNull_Generic);
            }

            var request = context.RequestContext.HttpContext.Request;

			if (!Settings.EnableWebStats || !FilterUserAgent(request.UserAgent) || request.HttpMethod == "POST")
				return;
            
            string refUrl = GetReferral(context);
            EntryView ev = new EntryView();
            ev.EntryId = EntryID;
            ev.BlogId = BlogId;
            ev.ReferralUrl = refUrl;
            ev.PageViewType = PageViewType.WebView;

            Track(ev);
        }

		//TODO: Unit test this method. Also clean it up and make it more self-descriptive.
		private string GetReferral(ISubtextContext context)
		{
            var request = context.RequestContext.HttpContext.Request;
			Uri uri = UrlFormats.GetUriReferrerSafe(request);

            if (uri == null) {
                return null;
            }

			string url = uri.ToString();

			url = Blog.StripWwwPrefixFromHost(url.ToString());
			string fqu = Blog.StripWwwPrefixFromHost(context.UrlHelper.BlogUrl().ToFullyQualifiedUrl(context.Blog).ToString());
			
			if(String.Equals(url, fqu, StringComparison.OrdinalIgnoreCase)) {
				return null;
			}
			
			if(url.Length == 0) {
				Log.Warn("Somehow the referral was an empty string and not null.");	
				return null;
			}

			return url;
		}

		private static bool FilterUserAgent(string agent)
		{
			return (agent != null && agent.Length > 0 && Regex.IsMatch(agent, "msie|mozilla|opera", RegexOptions.IgnoreCase));
		}
	}
}
