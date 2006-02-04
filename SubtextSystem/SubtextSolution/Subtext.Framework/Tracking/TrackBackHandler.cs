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
 // ASPNetWeblog is not responsible for, shall have no liability for 
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
 // Based off of code by Simon Fell http://www.pocketsoap.com/weblog/ 
 // 
 ///////////////////////////////////////////////////////////////////////////////////////////////////
#endregion
using System;
using System.Globalization;
using System.Web;
using System.Xml;
using Subtext.Extensibility;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;
using Subtext.Framework.Text;
using Subtext.Framework.Util;

namespace Subtext.Framework.Tracking
{
	/// <summary>
	/// Summary description for TrackBackHandler.
	/// </summary>
	public class TrackBackHandler : IHttpHandler
	{
		public TrackBackHandler() 
		{
		}
	
		public void ProcessRequest(HttpContext context)
		{
			context.Response.ContentType="text/xml" ;

			int postId = 0 ;
			try 
			{
				postId = WebPathStripper.GetEntryIDFromUrl(context.Request.Path);//int.Parse(context.Request.QueryString["postid"]) ;
			}
			catch 
			{
				trackbackResponse (context, 1, "EntryID is invalid or missing" ) ;
			}


			if ( context.Request.HttpMethod == "POST" )
			{
				string title     = safeParam(context,"title") ;
				string excerpt   = safeParam(context,"excerpt") ;
				string url       = safeParam(context,"url");
				string blog_name = safeParam(context,"blog_name") ;

				// is the url valid ?
				if ( url == "" )
				{
					trackbackResponse (context, 1, "no url parameter found, please try harder!") ;
				}
	
				string pageTitle = null;
				Entry trackedEntry = Entries.GetEntry(postId, EntryGetOption.ActiveOnly);
				if (trackedEntry != null &&  ! Verifier.SourceContainsTarget(url, trackedEntry.Link, out pageTitle))
				{
					trackbackResponse (context, 2, "Sorry couldn't find a relevant link in " + url ) ;
				}

				Entry entry = new Entry(PostType.PingTrack);
				entry.ParentID = postId;
				entry.Title = title;
				entry.TitleUrl = url;
				entry.Author = blog_name;
				entry.Body = excerpt;
				entry.IsActive = true;
				
				entry.DateCreated = entry.DateUpdated = DateTime.Now;

				Entries.Create(entry);
			}
			else
			{
				Entry entry = Entries.GetEntry(postId,EntryGetOption.ActiveOnly);

				XmlTextWriter w = new XmlTextWriter(context.Response.Output) ;
				w.Formatting = Formatting.Indented;


				w.WriteStartDocument() ;
				w.WriteStartElement("response") ;
				w.WriteElementString("error", "0") ;
				w.WriteStartElement("rss") ;
				w.WriteAttributeString("version", "0.91") ;
				w.WriteStartElement("channel") ;
				w.WriteElementString("title", entry.Title ) ;
				w.WriteElementString("link", Config.CurrentBlog.UrlFormats.TrackBackUrl(postId));
				w.WriteElementString("description", "" ) ;
				w.WriteElementString("language", "en-us" ) ;

				w.WriteEndElement() ; // channel
				w.WriteEndElement() ; // rss 
				w.WriteEndElement() ; // response
				w.WriteEndDocument() ;
		
			}

		}

		public bool IsReusable
		{
			get { return true; }
		}

		private void trackbackResponse(HttpContext context, int errNum, string errText)
		{
			XmlDocument d = new XmlDocument() ;
			XmlElement root = d.CreateElement("response") ;
			d.AppendChild(root) ;
			XmlElement er = d.CreateElement("error") ;
			root.AppendChild(er) ;
			er.AppendChild(d.CreateTextNode(errNum.ToString(CultureInfo.InvariantCulture))) ;
			if ( errText != "" )
			{
				XmlElement msg = d.CreateElement("message") ;
				root.AppendChild(msg) ;
				msg.AppendChild(d.CreateTextNode(errText)) ;
			}
			d.Save ( context.Response.Output ) ;
			context.Response.End() ;
		}

		private string safeParam(HttpContext context,string pName)
		{
			if ( context.Request.Form[pName] != null )
				return HtmlHelper.SafeFormat(context.Request.Form[pName]);
			return  string.Empty;
		}
	}
}

