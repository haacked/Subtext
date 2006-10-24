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
using System.Configuration;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using Subtext.Framework.Text;
using Subtext.Framework.Web;

namespace Subtext.Framework.Tracking
{
	/// <summary>
	/// Summary description for TrackBackNotificationProxy.
	/// </summary>
	public class TrackBackNotificationProxy
	{	
		/// <summary>
		/// Initializes a new instance of the <see cref="TrackBackNotificationProxy"/> class.
		/// </summary>
		public TrackBackNotificationProxy()
		{
		}

		public bool TrackBackPing(string pageText, Uri url, string title, Uri link, string blogname, string description)
		{
			string trackBackItem = GetTrackBackText(pageText, url, link);
			if(trackBackItem != null)
			{
				if(!trackBackItem.ToLower(System.Globalization.CultureInfo.InvariantCulture).StartsWith("http://"))
				{
					trackBackItem = "http://" + trackBackItem;
				}

                string parameters = "title=" + HttpUtility.HtmlEncode(title) + "&url=" + HttpUtility.HtmlEncode(link.ToString()) + "&blog_name=" + HttpUtility.HtmlEncode(blogname) + "&excerpt=" + HttpUtility.HtmlEncode(description);

				Uri trackBackUrl = HtmlHelper.ParseUri(trackBackItem);
				if(trackBackUrl != null)
				{
					return SendPing(trackBackUrl, parameters);
				}				
			}
			return true;
		}

		private bool SendPing(Uri trackBackItem, string parameters)
		{
			HttpWebRequest request = HttpHelper.CreateRequest(trackBackItem);
			request.Method = "POST";
			request.ContentLength = parameters.Length;
			request.ContentType = "application/x-www-form-urlencoded";
			request.KeepAlive = false;
			HttpHelper.SetProxy(request);

			using(StreamWriter myWriter = new StreamWriter(request.GetRequestStream()))
			{
				myWriter.Write(parameters);
			}

			HttpWebResponse response = (HttpWebResponse)request.GetResponse();
			
			return (response.StatusCode == HttpStatusCode.OK);
		}

		private string GetTrackBackText(string pageText, Uri url, Uri postUrl)
		{
			if(!Regex.IsMatch(pageText, postUrl.ToString(), RegexOptions.IgnoreCase|RegexOptions.Singleline))
			{
				string sPattern = @"<rdf:\w+\s[^>]*?>(</rdf:rdf>)?";
				Regex r = new Regex(sPattern,RegexOptions.IgnoreCase);
				Match m;
			
				for (m = r.Match(pageText); m.Success; m = m.NextMatch()) 
				{
					if(m.Groups.ToString().Length > 0)
					{
						string text = m.Groups[0].ToString();
						if(text.IndexOf(url.ToString()) > 0)
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

