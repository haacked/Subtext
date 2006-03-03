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
using System.IO;
using System.Net;
using System.Text;

namespace Subtext.Framework.Util
{
	public class BlogRequest 
	{
		private const int defaultTimeout_ = 60000;
		private static string referer_ = @"http://Subtext.com/Services/default.htm";
		private static readonly string userAgent_ = VersionInfo.UserAgent
			+ " (" + Environment.OSVersion.ToString() + "; .NET CLR " + Environment.Version.ToString() + ")";

		
		public static HttpWebRequest CreateRequest(string url) 
		{
			WebRequest req = WebRequest.Create(url);
			
			HttpWebRequest wreq = req as HttpWebRequest;
			if (null != wreq) 
			{
				wreq.UserAgent = userAgent_;
				wreq.Referer =  referer_;
				wreq.Timeout = defaultTimeout_;
			}
			return wreq;
		}	

		public static HttpWebResponse GetResponse(string url)
		{
			HttpWebRequest request = CreateRequest(url);
			return (HttpWebResponse)request.GetResponse() ;
		}		

		public static string GetPageText(string url)
		{
			HttpWebResponse response = GetResponse(url);
			using (Stream s = response.GetResponseStream())
			{
				string enc = response.ContentEncoding.Trim() ;
				if ( enc == "" )
					enc = "us-ascii" ;
				Encoding encode = System.Text.Encoding.GetEncoding(enc);
				using ( StreamReader sr = new StreamReader( s, encode ) )
				{
					return sr.ReadToEnd() ;
				}
			}
		}
		
		
	}
}

