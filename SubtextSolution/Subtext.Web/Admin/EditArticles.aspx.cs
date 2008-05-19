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
using System.Web.UI.WebControls;

namespace Subtext.Web.Admin.Pages
{
	public partial class EditArticles : ConfirmationPage
	{
        public EditArticles() : base()
	    {
            this.TabSectionId = "Articles";
	    }
	    
		protected void Page_Load(object sender, System.EventArgs e)
		{
			BindLocalUI();
		}

		private void BindLocalUI()
		{
			// REFACTOR: we're duplicating this in both Articles and Posts for the sake
			// of wireup. There's a structural issue btw Page and Container.
			LinkButton lkbNewPost = Utilities.CreateLinkButton("New Article");
			lkbNewPost.CausesValidation = false;
			lkbNewPost.Click += new System.EventHandler(lkbNewPost_Click);
            base.AdminMasterPage.AddToActions(lkbNewPost);

			// REFACTOR: Structural issue btw Page and Container.
			HyperLink lnkEditCategories = Utilities.CreateHyperLink("Edit Categories",
                string.Format(System.Globalization.CultureInfo.InvariantCulture, "{0}?{1}={2}", Constants.URL_EDITCATEGORIES, Keys.QRYSTR_CATEGORYTYPE, categoryLinks.CategoryType));
            base.AdminMasterPage.AddToActions(lnkEditCategories);
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

		}
		#endregion

		private void lkbNewPost_Click(object sender, System.EventArgs e)
		{
			Editor.EditNewEntry();
		}
	}
}

