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
using log4net;
using Subtext.BlogML;
using Subtext.Framework.Exceptions;
using Subtext.Framework.Logging;

namespace Subtext.Web.Admin.Pages
{
	/// <summary>
	/// Renders the page used to import and export blog data using 
	/// the BlogML format proposed in 
	/// <see href="http://markitup.com/Posts/PostsByCategory.aspx?categoryId=5751cee9-5b20-4db1-93bd-7e7c66208236">this blog</see>
	/// </summary>
	public partial class ImportExportPage : AdminPage
	{
		private readonly static ILog log = new Log();
	
	    public ImportExportPage() : base()
	    {
            this.TabSectionId = "ImportExport";
	    }
	    
		protected void Page_Load(object sender, EventArgs e)
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
			this.btnSave.Click += btnSave_Click;
			this.btnLoad.Click += btnLoad_Click;
		}
		#endregion

		protected void btnSave_Click(object sender, EventArgs e)
		{
			Response.Redirect("Handlers/BlogMLExport.ashx?embed=" + this.chkEmbedAttach.Checked);
		}

		protected void btnLoad_Click(object sender, System.EventArgs e)
		{
			if(Page.IsValid)
				LoadBlogML();
		}

		private void LoadBlogML()
		{
			BlogMLReader bmlReader = BlogMLReader.Create(BlogMLProvider.Instance());
			bool errOccured = false;
			
			try
			{
                bmlReader.ReadBlog(this.importBlogMLFile.PostedFile.InputStream);
			}
			catch(BlogImportException bie)
			{
				log.Error("Import of BlogML file failed.", bie);
				Messages.ShowError(bie.Message, true);
			}
		    finally
			{
			    this.importBlogMLFile.PostedFile.InputStream.Close();
			}

			if(!errOccured)
				Messages.ShowMessage("The BlogML file was successfully imported!");
		}
	}
}

