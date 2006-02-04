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
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Subtext.Framework.Configuration;
using Subtext.Framework.Util;
using Subtext.Web.Admin.WebUI;
using PlaceHolder = Subtext.Web.Admin.WebUI.PlaceHolder;
using ScriptTag = Subtext.Web.Controls.ScriptTag;

namespace Subtext.Web.Admin
{
	public abstract class PageTemplate : UserControl
	{
		protected PlaceHolder PageTitle;
		protected PlaceHolder PageContent;
		protected PlaceHolder LabelCategories;
		protected PlaceHolder LabelActions;
		protected HyperLink BlogTitle;
		protected HeaderLink Css1;
		protected LinkList LinksCategories;
		protected LinkList LinksActions;
		protected LinkButton LogoutLink;
		protected HyperLink BlogTitleLink;
		protected BreadCrumbs BreadCrumbs;		
		protected HeaderBase Base1;
		protected Literal LoggedInUser;
		protected HtmlGenericControl GalleryTab;
		protected ScriptTag HelptipJs;
		protected ScriptTag AdminJs;
		protected HeaderLink HelptipCss;
		protected ScriptTag NiceForms;
		protected HeaderLink NiceFormsStyle;

		#region Accessors

		private string _resourcePath;
		
		public string ResourcePath
		{
			get
			{
				if(this._resourcePath == null)
				{
					this._resourcePath = Globals.WebPathCombine(HttpContext.Current.Request.ApplicationPath,  "/admin/");
				}
				return this._resourcePath;
			}
			set
			{
			}
		}

		#endregion

		private void Page_Load(object sender, EventArgs e)
		{
			GalleryTab.Visible = Config.Settings.AllowImages;
			LoggedInUser.Text = Config.CurrentBlog.Author;
		}

		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e) 
		{
			InitializeComponent();
			base.OnInit(e);
		}
		
		private void InitializeComponent() 
		{
			this.Load += new EventHandler(this.Page_Load);

		}
		#endregion
	}
}

