#region Disclaimer/Info

///////////////////////////////////////////////////////////////////////////////////////////////////
// Subtext WebLog
// 
// Subtext is an open source weblog system that is a fork of the .TEXT
// weblog system.
//
// For updated news and information please visit http://subtextproject.com/
// Subtext is hosted at Google Code at http://code.google.com/p/subtext/
// The development mailing list is at subtext@googlegroups.com 
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

using System.Threading;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;
using Subtext.Framework.Routing;
using Subtext.Framework.Util;

namespace Subtext.Framework.Tracking
{
    /// <summary>
    /// Summary description for Notification.
    /// </summary>
    public static class NotificationServices
    {
        /// <summary>
        /// Posts trackbacks and pingbacks for the specified entry.
        /// </summary>
        public static void Run(Entry entry, Blog blog, UrlHelper urlHelper)
        {
            if(!blog.TrackbacksEnabled)
            {
                return;
            }

            if(!Config.Settings.Tracking.EnablePingBacks && !Config.Settings.Tracking.EnableTrackBacks)
            {
                return;
            }

            if(entry != null)
            {
                var notify = new Notifier
                {
                    FullyQualifiedUrl = urlHelper.BlogUrl(),
                    BlogName = blog.Title,
                    Title = entry.Title,
                    PostUrl = urlHelper.EntryUrl(entry).ToFullyQualifiedUrl(blog),
                    Description = entry.HasDescription ? entry.Description : entry.Title,
                    Text = entry.Body
                };

                //This could take a while, do it on another thread
                ThreadHelper.FireAndForget(notify.Notify, "Exception occured while attempting trackback notification");
            }
        }
    }
}