using System;
using System.Web;
using Subtext.Framework.Configuration;
using System.Collections.Specialized;
using Subtext.Framework.Text;
using Subtext.Framework.Web;

namespace Subtext.Framework.Tracking
{
	/// <summary>
	/// Class used to send a remote notification such as to Weblogs.com or a 
	/// trackback/pingback.
	/// </summary>
	public class Notifier
	{
		static Logging.Log Log = new Logging.Log();

		public string Description
		{
			get;
			set;
		}

		public string BlogName
		{
			get;
			set;
		}

		public string Title
		{
			get;
			set;
		}

		public string FullyQualifiedUrl
		{
			get;
			set;
		}

		public Uri PostUrl
		{
			get;
			set;
		}

        public string Text
        {
            get;
            set;
        }

		/// <summary>
		/// Notifies the specified state.
		/// </summary>
		/// <param name="state">The state.</param>
		public void Notify(object state)
		{
			try
			{
				Notify();
			}
			catch (Exception e)
			{
				Log.Warn("Had a problem with notification.", e);
			}
		}

		void Notify()
		{
			Configuration.Tracking track = Config.Settings.Tracking;

			//First, ping weblogs.com
			try
			{
				PingWeblogsDotCom(track);
			}
			catch(HttpException e)
			{
				Log.Warn("Had a problem pinging weblogs.com. continuing.", e);
			}

			//Get the links from the last post
			StringCollection links = HtmlHelper.GetLinks(Text);

			//Do we have links
			if (links == null || links.Count == 0)
				return;
			
			//Create our notification Components
			using (PingBackNotificatinProxy pbnp = new PingBackNotificatinProxy())
			{
				//for each link, try to pingback and/or trackback
				foreach (string link in links)
				{
					//get the page text
					Uri url = HtmlHelper.ParseUri(link);
					if (url == null)
						continue;

					string pageText = HttpHelper.GetPageText(url);
					if (pageText == null || !track.EnableTrackBacks)
						continue;
					
					if (!TrackBackNotificationProxy.TrackBackPing(pageText, url, Title, PostUrl, BlogName, Description) && track.EnablePingBacks)
					{
						Log.DebugFormat("Trackback failed to '{0}'. Let's try a PingBack.", url);
						pbnp.Ping(pageText, PostUrl, url);
					}				
				}
			}
		}

		private void PingWeblogsDotCom(Configuration.Tracking track)
		{
			if (track.PingWeblogs)
			{
				using (WeblogsNotificatinProxy weblogs = new WeblogsNotificatinProxy())
				{
					weblogs.Ping(BlogName, FullyQualifiedUrl);
				}
			}
		}
	}
}
