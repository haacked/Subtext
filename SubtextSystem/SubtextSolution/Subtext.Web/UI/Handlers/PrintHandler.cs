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

using System;
using System.Text;
using System.Web;
using Subtext.Framework;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;
using Subtext.Framework.Format;

namespace Subtext.Web.UI
{
	/// <summary>
	/// Class that implements <see cref="IHttpHandler"/> to render a 
	/// page for printing purposes.  Not currently in use.
	/// </summary>
	public class PrintHandler : IHttpHandler
	{
		//TODO: Test and find a way to integrate this class.
		public void ProcessRequest(HttpContext context)
		{
			int PostID = UrlFormats.GetPostIDFromUrl(context.Request.Path);
			if(PostID > -1)
			{
				Entry entry = Entries.GetEntry(PostID, EntryGetOption.ActiveOnly);
				if(entry != null)
				{
					StringBuilder sb = new StringBuilder();
					sb.Append("<html><head><title>" + entry.Title + "</title>");
					sb.Append("<link rel=\"stylesheet\" href=\"" + Config.CurrentBlog.RootUrl + "Skins/" + Globals.Skin(context) + "/style.css\" type=\"text/css\" /></head><body>");
					sb.Append("<h1>" + entry.Title + "</h1>");
					sb.Append("by: " + entry.Author + "<br />");
					sb.Append("posted on: " + entry.DateCreated.ToString("f") + "<br />");
					if(entry.IsUpdated)
					{
						sb.Append("updated on: " + entry.DateUpdated.ToString("f") + "<br />");
					}
					sb.Append(entry.Body);
					sb.Append("<hr>originally published at: " + entry.Link);
					sb.Append("</body></html>");
					context.Response.Write(sb.ToString());
				}
				else
				{
					context.Response.Write("<H1>Could not find the story</H1>");
				}
				
			}
			else
			{
				context.Response.Write("<H1>Could not find the story</H1>");
			}
		}
	
		public bool IsReusable
		{
			get { return true; }
		}
	}
}

