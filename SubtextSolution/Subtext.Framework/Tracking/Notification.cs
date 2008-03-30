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

#region Notes
 ///////////////////////////////////////////////////////////////////////////////////////////////////
 // The code in this file is freely distributable.
 // 
 // ASPNetWeblog isnot responsible for, shall have no liability for 
 // and disclaims all warranties whatsoever, expressed or implied, related to this code,
 // including without limitation any warranties related to performance, security, stability,
 // or non-infringement of title of the control.
 // 
 // If you have any questions, comments or concerns, please contact
 // Scott Watermasysk, Scott@TripleASP.Net.
 // 
 // For more information on this control, updates, and other tools to integrate blogging 
 // into your existing applications, please visit, http://aspnetweblog.com
 // 
 ///////////////////////////////////////////////////////////////////////////////////////////////////
#endregion

using System;
using System.Collections.Specialized;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;
using Subtext.Framework.Text;
using Subtext.Framework.Threading;
using Subtext.Framework.Web;

namespace Subtext.Framework.Tracking
{
	/// <summary>
	/// Summary description for Notification.
	/// </summary>
	public sealed class NotificationServices
	{	
		private NotificationServices()
		{
		}
		
		/// <summary>
		/// Posts trackbacks and pingbacks for the specified entry.
		/// </summary>
		/// <param name="entry">The entry.</param>
		public static void Run(Entry entry)
		{
			if(entry != null)
			{
				Notifier notify = new Notifier();

				notify.FullyQualifiedUrl = Config.CurrentBlog.RootUrl.ToString();
				notify.BlogName = Config.CurrentBlog.Title;

				notify.Title = entry.Title;

				notify.PostUrl = entry.FullyQualifiedUrl;

				if(entry.HasDescription)
				{
					notify.Description = entry.Description;
				}
				else
				{
					notify.Description = entry.Title;	
				}

				notify.Text = entry.Body;
				
				//This could take a while, do it on another thread
				ManagedThreadPool.QueueUserWorkItem(new System.Threading.WaitCallback(notify.Notify));
			}
		}

	}

	/// <summary>
	/// Class used to send a remote notification such as to Weblogs.com or a 
	/// trackback/pingback.
	/// </summary>
	public class Notifier
	{
		static Logging.Log Log = new Logging.Log();
		
		public Notifier(){}

		private string description;
		public string Description
		{
			get{return description;}
			set{description = value;}
		}

		private string blogName;
		public string BlogName
		{
			get{return blogName;}
			set{blogName = value;}
		}

		private string title;
		public string Title
		{
			get{return title;}
			set{title = value;}
		}

		private string fullyQualifiedUrl;
		public string FullyQualifiedUrl
		{
			get{return fullyQualifiedUrl;}
			set{fullyQualifiedUrl = value;}
		}

		private Uri postUrl;
		public Uri PostUrl
		{
			get{return postUrl;}
			set{postUrl = value;}
		}

		private string text;
		public string Text
		{
			get{return text;}
			set{text = value;}
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
			catch(Exception e)
			{
				Log.Warn("Had a problem with notification.", e);
			}
		}

		void Notify()
		{
			Configuration.Tracking track = Config.Settings.Tracking;
			
			//First, ping weblogs.com
			if(track.PingWeblogs)
			{
				using (WeblogsNotificatinProxy weblogs = new WeblogsNotificatinProxy())
				{
					weblogs.Ping(BlogName, FullyQualifiedUrl);
				}
			}

			//Get the links from the last post
			StringCollection links = HtmlHelper.GetLinks(Text);

			//Do we have links
			if(links != null && links.Count > 0)
			{
				//Create our notification Components
				using (PingBackNotificatinProxy pbnp = new PingBackNotificatinProxy())
				{
					TrackBackNotificationProxy tbnp = new TrackBackNotificationProxy();

					//for each link, try to pingback and/or trackback
					foreach (string link in links)
					{
						//get the page text
						Uri url = HtmlHelper.ParseUri(link);
						if (url == null)
							continue;

						string pageText = HttpHelper.GetPageText(url);
						if (pageText != null)
						{
							if (track.EnableTrackBacks)
							{
								if(!tbnp.TrackBackPing(pageText, url, Title, PostUrl, BlogName, Description) && track.EnablePingBacks)
								{
									Log.DebugFormat("Trackback failed to '{0}'. Let's try a PingBack.", url);
									pbnp.Ping(pageText, PostUrl, url);
								}
							}
						}
					}
				}
			}
		}
	}
}

