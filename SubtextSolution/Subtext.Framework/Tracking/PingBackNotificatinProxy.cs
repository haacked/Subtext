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
using System.Text.RegularExpressions;
using CookComputing.XmlRpc;

namespace Subtext.Framework.Tracking
{
    /// <summary>
    /// Summary description for WeblogsNotificatinProxy.
    /// </summary>
    public class PingBackNotificatinProxy : XmlRpcClientProtocol
    {
        public string ErrorMessage
        {
            get { return "NoError"; }
        }

        public bool Ping(string pageText, Uri sourceUri, Uri targetUri)
        {
            if(sourceUri == null)
            {
                throw new ArgumentNullException("sourceURI");
            }

            if(targetUri == null)
            {
                throw new ArgumentNullException("targetURI");
            }

            string pingbackUrl = GetPingBackUrl(pageText, sourceUri);
            if(pingbackUrl != null)
            {
                Url = pingbackUrl;
                Notify(sourceUri.ToString(), targetUri.ToString());
                return true;
            }
            return false;
        }

        private static string GetPingBackUrl(string pageText, Uri postUrl)
        {
            if(!Regex.IsMatch(pageText, postUrl.ToString(), RegexOptions.IgnoreCase | RegexOptions.Singleline))
            {
                if(pageText != null)
                {
                    const string pattern = "<link rel=\"pingback\" href=\"([^\"]+)\" ?/?>";
                    var regex = new Regex(pattern, RegexOptions.IgnoreCase | RegexOptions.Singleline);
                    Match match = regex.Match(pageText);
                    if(match.Success)
                    {
                        return match.Result("$1");
                    }
                }
            }
            return null;
        }

        [XmlRpcMethod("pingback.ping")]
        public void Notify(string sourceURI, string targetURI)
        {
            Invoke("Notifiy", new object[] {sourceURI, targetURI});
        }
    }
}