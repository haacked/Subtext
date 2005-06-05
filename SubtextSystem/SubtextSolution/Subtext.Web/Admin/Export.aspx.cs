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

using System;

namespace Subtext.Web.Admin.Pages
{
	public class Export : AdminPage
	{	
		private void Page_Load(object sender, System.EventArgs e)
		{
			string command = Request.QueryString["command"].ToLower(System.Globalization.CultureInfo.InvariantCulture);

			switch (command) 
			{
				case "opml" :
					ExportLinksToOpml();
					break;
				default :
					break;
			}
		}

		public void ExportLinksToOpml()
		{
//			PagedLinkCollection pagedAllLinks = Links.GetPagedLinks(1, 1);
//			LinkCollection allLinks = Links.GetPagedLinks(1, pagedAllLinks.MaxItems);	
//			XmlDocument doc = OpmlProvider.Export(allLinks);
//
//			Response.Clear();
//			Response.ContentEncoding = System.Text.Encoding.UTF8;
//			Response.AppendHeader("Content-Disposition", "attachment; filename=links.opml");
////			Response.AppendHeader("Content-Length", doc.OuterXml.Length.ToString());
//			Response.ContentType = "application/octet-stream";
//
//			XmlTextWriter writer = new XmlTextWriter(Response.OutputStream, Response.ContentEncoding);
//			writer.Formatting = Formatting.Indented;
//			writer.Indentation = 4;
//			writer.IndentChar = ' ';
//			doc.Save(writer);
//			writer.Flush();
//			
//			Response.End();
//			writer.Close();
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

