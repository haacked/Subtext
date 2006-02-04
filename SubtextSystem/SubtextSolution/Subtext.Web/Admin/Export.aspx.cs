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

