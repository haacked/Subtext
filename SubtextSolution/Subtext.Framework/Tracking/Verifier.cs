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
// Originally based off of code by Simon Fell http://www.pocketsoap.com/weblog/ 
// 
///////////////////////////////////////////////////////////////////////////////////////////////////

#endregion

using System;
using System.Net;
using System.Text.RegularExpressions;
using log4net;
using Subtext.Framework.Logging;
using Subtext.Framework.Web;

namespace Subtext.Framework.Tracking
{
    /// <summary>
    /// Used to verify that a trackback or pingback source actually contains a link to this site.
    /// </summary>
    public static class Verifier
    {
        private readonly static ILog Log = new Log();

        /// <summary>
        /// Checks that the contents of the source url contains the target URL.
        /// </summary>
        /// <param name="sourceUrl">The source URL.</param>
        /// <param name="targetUrl">The target URL.</param>
        /// <param name="pageTitle">The page title.</param>
        /// <returns></returns>
        public static bool SourceContainsTarget(Uri sourceUrl, Uri targetUrl, out string pageTitle)
        {
            pageTitle = string.Empty;
            string page = null;
            try
            {
                page = HttpHelper.GetPageText(sourceUrl);
            }
            catch(WebException e)
            {
                Log.Warn("Could not verify the source of a ping/trackback", e);
            }
            if(page == null || targetUrl == null)
            {
                return false;
            }

            const string pattern = @"<head.*?>.*<title.*?>(.*)</title.*?>.*</head.*?>";
            var regex = new Regex(pattern, RegexOptions.IgnoreCase | RegexOptions.Singleline);
            Match match = regex.Match(page);
            if(match.Success)
            {
                pageTitle = match.Result("$1");
                return true;
            }
            return false;
        }

        /// <summary>
        /// Checks that the contents of the source url contains the target URL.
        /// </summary>
        /// <param name="sourceUrl">The source URL.</param>
        /// <param name="targetUrl">The target URL.</param>
        /// <returns></returns>
        public static bool SourceContainsTarget(Uri sourceUrl, Uri targetUrl)
        {
            string page;
            return SourceContainsTarget(sourceUrl, targetUrl, out page);
        }
    }
}