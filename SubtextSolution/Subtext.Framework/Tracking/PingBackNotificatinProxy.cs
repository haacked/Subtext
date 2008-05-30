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
// Originally based off of code by Simon Fell http://www.pocketsoap.com/weblog/ 
// 
///////////////////////////////////////////////////////////////////////////////////////////////////
#endregion
using System;
using System.Text.RegularExpressions;
using CookComputing.XmlRpc;
using Subtext.Framework.Properties;

namespace Subtext.Framework.Tracking
{
	/// <summary>
	/// Summary description for WeblogsNotificatinProxy.
	/// </summary>
	
	public class PingBackNotificatinProxy : XmlRpcClientProtocol
	{
		public PingBackNotificatinProxy()
		{
			
		}

		private string errormessage = "No Error";
		public string ErrorMessage
		{
			get { return errormessage; }
		}

		public bool Ping(string pageText, Uri sourceURI, Uri targetURI)
		{
			if (sourceURI == null)
			{
				throw new ArgumentNullException("sourceURI", Resources.ArgumentNull_Uri);
			}

			if (targetURI == null)
			{
				throw new ArgumentNullException("targetURI", Resources.ArgumentNull_Uri);
			}

			string pingbackURL = GetPingBackURL(pageText, sourceURI);
			if (pingbackURL != null)
			{
				this.Url = pingbackURL;
				Notify(sourceURI.ToString(), targetURI.ToString());
				return true;
			}
			return false;
		}

		private static string GetPingBackURL(string pageText, Uri postUrl)
		{
			if (!Regex.IsMatch(pageText, postUrl.ToString(), RegexOptions.IgnoreCase | RegexOptions.Singleline))
			{
				if (pageText != null)
				{
					string pat = "<link rel=\"pingback\" href=\"([^\"]+)\" ?/?>";
					Regex reg = new Regex(pat, RegexOptions.IgnoreCase | RegexOptions.Singleline);
					Match m = reg.Match(pageText);
					if (m.Success)
					{
						return m.Result("$1");
					}
				}
			}
			return null;
		}

		[XmlRpcMethod("pingback.ping")]
		public void Notify(string sourceURI, string targetURI)
		{
			Invoke("Notifiy", new object[] { sourceURI, targetURI });
		}

	}
}

