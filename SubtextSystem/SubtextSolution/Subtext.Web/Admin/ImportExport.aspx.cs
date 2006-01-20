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
using System.IO;
using System.Web.UI.WebControls;
using log4net;
using Subtext.Framework.Configuration;
using Subtext.Framework.Exceptions;
using Subtext.Framework.Import;
using Subtext.Framework.Logging;
using Subtext.Web.Admin.WebUI;

namespace Subtext.Web.Admin.Pages
{
	/// <summary>
	/// Renders the page used to import and export blog data using 
	/// the BlogML format proposed in 
	/// <see href="http://markitup.com/Posts/PostsByCategory.aspx?categoryId=5751cee9-5b20-4db1-93bd-7e7c66208236">this blog</see>
	/// </summary>
	public class ImportExportPage : AdminPage
	{
		protected Page PageContainer;
		protected HyperLink hypBlogMLFile;
		protected CheckBox chkEmbedAttach;
		protected Button btnSave;
		protected AdvancedPanel Action;
		protected MessagePanel Messages;

		protected System.Web.UI.WebControls.Button btnLoad;
		protected System.Web.UI.WebControls.RequiredFieldValidator blogMLFileRequired;
		protected System.Web.UI.HtmlControls.HtmlInputFile importBlogMLFile;
		private readonly static ILog log = new Log();
	
		private void Page_Load(object sender, EventArgs e)
		{
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
			this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
			this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		private void btnSave_Click(object sender, EventArgs e)
		{
			Response.Redirect("Handlers/BlogMLExport.ashx?embed=" + this.chkEmbedAttach.Checked);
		}

		private void btnLoad_Click(object sender, System.EventArgs e)
		{
			if(Page.IsValid)
				LoadBlogML();
		}

		private void LoadBlogML()
		{
			SubtextBlogMLReader bmlReader = new SubtextBlogMLReader();
			bool errOccured = false;

			StreamReader sReader = new StreamReader(this.importBlogMLFile.PostedFile.InputStream);
			try
			{
				bmlReader.ReadBlog(sReader.ReadToEnd(), false);
			}
			catch(BlogImportException bie)
			{
				log.Error("Import of BlogML file failed.", bie);
				Messages.ShowError(bie.Message, true);
			}
			finally
			{
				sReader.Close();
			}

			if(!errOccured)
				Messages.ShowMessage("The BlogML file was successfully imported!");
		}
	}
}

