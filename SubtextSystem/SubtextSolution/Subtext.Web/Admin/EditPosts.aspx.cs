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
using System.Web.UI.WebControls;

namespace Subtext.Web.Admin.Pages
{
	public class EditPosts :  ConfirmationPage
	{
		protected Subtext.Web.Admin.WebUI.Page PageContainer;
		protected Subtext.Web.Admin.UserControls.EntryEditor Editor;
	
		private void Page_Load(object sender, System.EventArgs e)
		{
			BindLocalUI();
		}

		private void BindLocalUI()
		{
			LinkButton lkbNewPost = Utilities.CreateLinkButton("New Post");
			lkbNewPost.CausesValidation = false;
			lkbNewPost.Click += new System.EventHandler(lkbNewPost_Click);
			PageContainer.AddToActions(lkbNewPost);

			HyperLink lnkEditCategories = Utilities.CreateHyperLink("Edit Categories", 
				string.Format(System.Globalization.CultureInfo.InvariantCulture, "{0}?{1}={2}", Constants.URL_EDITCATEGORIES, Keys.QRYSTR_CATEGORYID, 
				(int)PageContainer.CategoryType));
			PageContainer.AddToActions(lnkEditCategories);
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

		private void lkbNewPost_Click(object sender, System.EventArgs e)
		{
			Editor.EditNewEntry();
		}
	}
}

