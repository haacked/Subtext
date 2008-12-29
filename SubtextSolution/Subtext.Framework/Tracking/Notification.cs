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

using Subtext.Framework.Components;
using Subtext.Framework.Configuration;
using Subtext.Framework.Threading;
using Subtext.Framework.Routing;
using System.Threading;

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
		public static void Run(Entry entry, Blog blog, UrlHelper urlHelper)
		{
            if (!blog.TrackbacksEnabled) {
                return;
            }

            if (!Config.Settings.Tracking.EnablePingBacks && !Config.Settings.Tracking.EnableTrackBacks) {
                return;
            }

			if(entry != null)
			{
				Notifier notify = new Notifier();

                notify.FullyQualifiedUrl = urlHelper.BlogUrl();
				notify.BlogName = blog.Title;

				notify.Title = entry.Title;

				notify.PostUrl = urlHelper.EntryUrl(entry).ToFullyQualifiedUrl(blog);

				if(entry.HasDescription) {
					notify.Description = entry.Description;
				}
				else {
					notify.Description = entry.Title;	
				}

				notify.Text = entry.Body;
				
				//This could take a while, do it on another thread
				ManagedThreadPool.QueueUserWorkItem(new WaitCallback(notify.Notify));
			}
		}

	}
}

