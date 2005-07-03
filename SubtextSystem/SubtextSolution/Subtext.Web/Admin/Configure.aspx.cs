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
using System.Globalization;
using System.Web.Caching;
using System.Web.UI.WebControls;
using System.Xml;
using Subtext.Framework;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;

namespace Subtext.Web.Admin.Pages
{
	public class Configure : AdminOptionsPage
	{
		// abstract out at a future point for i18n
		private const string RES_SUCCESS = "Your configuration was successfully updated.";
		private const string RES_FAILURE = "Configuration update failed.";

		protected Subtext.Web.Admin.WebUI.AdvancedPanel Edit;
		protected System.Web.UI.WebControls.LinkButton lkbPost;
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

			try
			{
				ddlLangLocale.Items.FindByValue(info.Language).Selected = true;
			}
			catch{}
			
			
			if(info.Skin.HasSecondaryText)
			{
				txbSecondaryCss.Text = info.Skin.SkinCssText;
			}

			XmlDocument doc = (XmlDocument)Cache["SkinsDoc"];
			if(doc == null)
			{
				doc = new XmlDocument();
				string filename = Request.MapPath("~/Admin/Skins.config");
				doc.Load(filename);
				CacheDependency dep = new CacheDependency(filename);
				Cache.Insert("SkinsDoc",doc,dep);				
			}

			XmlNodeList nodes = doc.SelectNodes("//SkinTemplates/Skins/SkinTemplate");

			foreach(XmlNode node in nodes)
			{
				string css = node.Attributes["SecondaryCss"] != null ? "-" + node.Attributes["SecondaryCss"].Value : string.Empty;
				string name = node.Attributes["Skin"].Value +  css;
				//string id = node.Attributes["SkinID"].Value;
				ddlSkin.Items.Add(new ListItem(name, name));
			}
		
			try
			{
				ddlSkin.Items.FindByValue(info.Skin.SkinID).Selected = true;
			}
			catch
			{
				
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
				info.Application = Config.CurrentBlog.Application;
				info.Host = Config.CurrentBlog.Host;
				info.BlogID = Config.CurrentBlog.BlogID;

				info.ItemCount = Int32.Parse(ddlItemCount.SelectedItem.Value);
				info.Language = ddlLangLocale.SelectedItem.Value;
				
				info.AllowServiceAccess = ckbAllowServiceAccess.Checked;

				info.Skin.SkinCssText = txbSecondaryCss.Text.Trim();
			
				
				string news = txbNews.Text.Trim();
				info.News = news.Length == 0 ? null : news;


				string[] skins = ddlSkin.SelectedItem.Text.Split('-');

				//Need to add logic for a skin name that might include "-"
				info.Skin.SkinName = skins[0].Trim();
				

				if(skins.Length > 1)
				{
					info.Skin.SkinCssFile = skins[skins.Length-1].Trim();
				}
				else
				{
					info.Skin.SkinCssFile = null;
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

			ViewState["CategoryID"] = Constants.NULL_CATEGORYID;
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

