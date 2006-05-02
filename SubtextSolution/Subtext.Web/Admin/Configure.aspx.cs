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
using System.Globalization;
using System.Web.Caching;
using System.Web.UI.WebControls;
using System.Xml;
using Subtext.Framework;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;
using Subtext.Framework.UI.Skinning;

namespace Subtext.Web.Admin.Pages
{
	public class Configure : AdminOptionsPage
	{
		// abstract out at a future point for i18n
		private const string RES_SUCCESS = "Your configuration was successfully updated.";
		private const string RES_FAILURE = "Configuration update failed.";

		protected Subtext.Web.Admin.WebUI.AdvancedPanel Edit;
		protected System.Web.UI.WebControls.Button lkbPost;
		protected System.Web.UI.WebControls.TextBox txbTitle;
		protected System.Web.UI.WebControls.TextBox txbSubtitle;
		protected System.Web.UI.WebControls.TextBox txbAuthor;
		protected System.Web.UI.WebControls.TextBox txbAuthorEmail;
		protected System.Web.UI.WebControls.DropDownList ddlSkin;
		protected System.Web.UI.WebControls.DropDownList ddlItemCount;
		protected System.Web.UI.WebControls.DropDownList ddlTimezone;
		protected System.Web.UI.WebControls.DropDownList ddlLangLocale;
		protected System.Web.UI.WebControls.CheckBox ckbAllowServiceAccess;
		protected System.Web.UI.WebControls.TextBox txbNews;
		protected System.Web.UI.WebControls.TextBox txbUser;
		protected System.Web.UI.WebControls.TextBox txbSecondaryCss;
		protected Subtext.Web.Admin.WebUI.MessagePanel Messages;
		protected Subtext.Web.Controls.HelpToolTip HelpToolTip1;
		protected Subtext.Web.Controls.HelpToolTip HelpToolTip2;
		//protected Subtext.Web.Admin.WebUI.Page PageContainer;
	
		#region Accessors
		public CategoryType CategoryType
		{
			get { return (CategoryType)ViewState["CategoryType"]; }
			set { ViewState["CategoryType"] = value; }
		}
		
		#endregion

		private void Page_Load(object sender, System.EventArgs e)
		{
			#if WANRelease
			this.txbUser.Enabled = false;
			#endif
			
			if (!IsPostBack)
			{
				BindForm();
			}
		}

		private void BindForm()
		{
			BlogInfo info = Config.CurrentBlog;
			txbTitle.Text = info.Title;
			txbSubtitle.Text = info.SubTitle;
			txbAuthor.Text = info.Author;
			txbAuthorEmail.Text = info.Email;
			txbUser.Text = info.UserName;
			txbNews.Text = info.News;
			ckbAllowServiceAccess.Checked = info.AllowServiceAccess;
			ddlTimezone.Items.FindByValue(info.TimeZone.ToString(CultureInfo.InvariantCulture)).Selected = true;

			ListItem languageItem = ddlLangLocale.Items.FindByValue(info.Language);
			if(languageItem != null)
			{
				languageItem.Selected = true;
			}		
			
			if(info.Skin.HasSecondaryText)
			{
				txbSecondaryCss.Text = info.Skin.SkinCssText;
			}

			SkinTemplate[] templates = SkinTemplates.Instance().Templates;
			foreach(SkinTemplate template in templates)
			{
				ddlSkin.Items.Add(new ListItem(template.SkinID, template.SkinID));
			}

			ListItem skinItem = ddlSkin.Items.FindByValue(info.Skin.SkinName);
			if(skinItem != null)
			{
				skinItem.Selected = true;
			}
			
			int count = Config.Settings.ItemCount;
			for (int i = 1; i <=count; i++)
			{
				ddlItemCount.Items.Add(new ListItem(i.ToString(CultureInfo.InvariantCulture), i.ToString(CultureInfo.InvariantCulture)));
			}

			if (info.ItemCount <= count)
			{
				ddlItemCount.Items.FindByValue(info.ItemCount.ToString(CultureInfo.InvariantCulture)).Selected = true;
			}

		}

		private void BindPost()
		{
			try
			{
				BlogInfo info = Config.CurrentBlog;
				info.Title = txbTitle.Text;
				info.SubTitle = txbSubtitle.Text;
				info.Author = txbAuthor.Text;
				info.Email = txbAuthorEmail.Text;

				#if WANRelease
				info.UserName = txbUser.Text;
				#endif

				info.TimeZone = Int32.Parse(ddlTimezone.SelectedItem.Value);
				info.Subfolder = Config.CurrentBlog.Subfolder;
				info.Host = Config.CurrentBlog.Host;
				info.BlogId = Config.CurrentBlog.BlogId;

				info.ItemCount = Int32.Parse(ddlItemCount.SelectedItem.Value);
				info.Language = ddlLangLocale.SelectedItem.Value;
				
				info.AllowServiceAccess = ckbAllowServiceAccess.Checked;

				info.Skin.SkinCssText = txbSecondaryCss.Text.Trim();
			
				
				string news = txbNews.Text.Trim();
				info.News = news.Length == 0 ? null : news;

				SkinTemplate skinTemplate = SkinTemplates.Instance().GetTemplate(ddlSkin.SelectedItem.Value);
				info.Skin.SkinName = skinTemplate.SkinID;
				if(skinTemplate.UseSecondaryCss)
				{
					info.Skin.SkinCssFile = skinTemplate.SecondaryCss;
				}
				Config.UpdateConfigData(info);

				this.Messages.ShowMessage(RES_SUCCESS);
			}
			catch(Exception ex)
			{
				this.Messages.ShowError(String.Format(Constants.RES_EXCEPTION, RES_FAILURE, ex.Message));
			}
		}
		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
			base.OnInit(e);

			ViewState["CategoryID"] = NullValue.NullInt32;
			ViewState["CategoryType"] = Constants.DEFAULT_CATEGORYTYPE;
		}
		
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{    
			this.lkbPost.Click += new System.EventHandler(this.lkbPost_Click);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		private void lkbPost_Click(object sender, System.EventArgs e)
		{
			BindPost();
		}
	}
}

