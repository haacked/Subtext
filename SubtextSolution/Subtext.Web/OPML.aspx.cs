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
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Xml;
using Subtext.Framework;
using Subtext.Framework.Data;
using Subtext.Framework.Providers;
using Subtext.Web.UI.Controls.Aggregate;
using System.Collections.Generic;

namespace Subtext.Web
{
	/// <summary>
	/// Summary description for OPML.
	/// </summary>
	public class OPML : AggregatePage
	{
		private void Page_Load(object sender, EventArgs e)
		{
            int? groupId = GetGroupIdFromQueryString();
            var blogStats = ObjectProvider.Instance().GetBlogsByGroup(Blog.AggregateBlog.Host, groupId);
			Response.ContentType = "text/xml";
			Response.Write(Write(blogStats, Request.ApplicationPath));
		}
		
		private static string Write(IEnumerable<Blog> blogStats, string appPath)
		{
			if(!appPath.EndsWith("/"))
			{
				appPath += "/";
			}
			StringWriter sw = new StringWriter();
				
			XmlTextWriter writer = new XmlTextWriter(sw);
			writer.Formatting = Formatting.Indented;
			writer.WriteStartDocument();

			//OPML ROOT
			writer.WriteStartElement("opml");

			//Body
			writer.WriteStartElement("body");

			string baseUrl = "http://{0}" + appPath + "{1}";
            foreach (var blog in blogStats)
			{
				writer.WriteStartElement("outline");

				string title = blog.Title;
				string htmlUrl = string.Format(baseUrl, blog.Host, blog.Subfolder);
				string xmlUrl= htmlUrl + "/rss.aspx";

				writer.WriteAttributeString("title", title);
				writer.WriteAttributeString("htmlUrl", htmlUrl);
				writer.WriteAttributeString("xmlUrl", xmlUrl);

				writer.WriteEndElement();									
			}
			writer.WriteEndElement();
			writer.WriteEndElement();
			writer.Flush();
			writer.Close();
			sw.Close();
			return sw.ToString();
		}

		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{    
			this.Load += new System.EventHandler(this.Page_Load);
		}
		#endregion
	}
}