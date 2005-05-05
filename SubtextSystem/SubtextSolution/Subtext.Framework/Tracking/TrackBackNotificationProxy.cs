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
using System.Text.RegularExpressions;
using Subtext.Framework.Util;

namespace Subtext.Framework.Tracking
{
	/// <summary>
	/// Summary description for TrackBackNotificationProxy.
	/// </summary>
	public class TrackBackNotificationProxy
	{
		public TrackBackNotificationProxy()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		private string HtmlEncode(string text)
		{
			return System.Web.HttpUtility.HtmlEncode(text);
			
		}

		public bool TrackBackPing(string pageText,string url, string title, string link, string blogname, string description)
		{
			string trackBackItem = GetTrackBackText(pageText,url,link);
			if(trackBackItem != null)
			{
				if(!trackBackItem.ToLower().StartsWith("http://"))
				{
					trackBackItem = "http://" + trackBackItem;
				}

				string parameters = "title=" + HtmlEncode(title) + "&url=" + HtmlEncode(link) + "&blog_name=" + HtmlEncode(blogname) + "&excerpt=" + HtmlEncode(description);

				
				SendPing(trackBackItem, parameters);


			}
			return true;
 
		}

		private void SendPing(string trackBackItem, string parameters)
		{
			StreamWriter myWriter =null;

			HttpWebRequest request = BlogRequest.CreateRequest(trackBackItem);
			request.Method = "POST";
			request.ContentLength = parameters.Length;
			request.ContentType = "application/x-www-form-urlencoded";
			request.KeepAlive = false;

			try
			{
				myWriter = new StreamWriter(request.GetRequestStream());
				myWriter.Write(parameters);
			}
			catch{}
			finally
			{
				myWriter.Close();
			}
		}

		private string GetTrackBackText(string pageText, string url, string PostUrl)
		{
			if(!Regex.IsMatch(pageText,PostUrl,RegexOptions.IgnoreCase|RegexOptions.Singleline))
			{

				string sPattern = @"<rdf:\w+\s[^>]*?>(</rdf:rdf>)?";
				Regex r = new Regex(sPattern,RegexOptions.IgnoreCase);
				Match m;
			
				for (m = r.Match(pageText); m.Success; m = m.NextMatch()) 
				{
					if(m.Groups.ToString().Length > 0)
					{
					
						string text = m.Groups[0].ToString();
						if(text.IndexOf(url) > 0)
						{
							string tbPattern = "trackback:ping=\"([^\"]+)\"";
							Regex reg = new Regex(tbPattern, RegexOptions.IgnoreCase) ;
							Match m2 = reg.Match(text) ;
							if ( m2.Success )
							{
								return m2.Result("$1") ;
							}


							return text;
						}
					}
				}
			}

			return null;
	
		}
	}
}

