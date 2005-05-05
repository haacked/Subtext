#region Disclaimer/Info
///////////////////////////////////////////////////////////////////////////////////////////////////
// .Text WebLog
// 
// .Text is an open source weblog system started by Scott Watermasysk. 
// Blog: http://ScottWater.com/blog 
// RSS: http://scottwater.com/blog/rss.aspx
// Email: Dottext@ScottWater.com
//
// For updated news and information please visit http://scottwater.com/dottext and subscribe to 
// the Rss feed @ http://scottwater.com/dottext/rss.aspx
//
// On its release (on or about August 1, 2003) this application is licensed under the BSD. However, I reserve the 
// right to change or modify this at any time. The most recent and up to date license can always be fount at:
// http://ScottWater.com/License.txt
// 
// Please direct all code related questions to:
// GotDotNet Workspace: http://www.gotdotnet.com/Community/Workspaces/workspace.aspx?id=e99fccb3-1a8c-42b5-90ee-348f6b77c407
// Yahoo Group http://groups.yahoo.com/group/DotText/
// 
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
	public  class BlogRequest 
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

