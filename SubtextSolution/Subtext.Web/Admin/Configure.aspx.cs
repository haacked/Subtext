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
using System.Collections.Generic;
using System.Globalization;
using System.Web.UI.WebControls;
using Subtext.Framework;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;
using Subtext.Framework.UI.Skinning;
using Subtext.Framework.Util;

namespace Subtext.Web.Admin.Pages
{
	public partial class Configure : AdminOptionsPage
	{
		// abstract out at a future point for i18n
		private const string RES_SUCCESS = "Your configuration was successfully updated.";
		private const string RES_FAILURE = "Configuration update failed.";
	
		public CategoryType CategoryType
		{
			get { return (CategoryType)ViewState["CategoryType"]; }
			set { ViewState["CategoryType"] = value; }
		}

        protected IList<SkinTemplate> Skins
        {
            get
            {
                return this.skins;
            }
        }

        IList<SkinTemplate> skins = new SkinTemplateCollection();

        protected IList<SkinTemplate> MobileSkins
        {
            get
            {
                if (this.mobileSkins == null)
                {
                    this.mobileSkins = new List<SkinTemplate>(new SkinTemplateCollection(true));
                    this.mobileSkins.Insert(0, SkinTemplate.Empty);
                }
                return this.mobileSkins;
            }
        }
        IList<SkinTemplate> mobileSkins = null;

		protected override void BindLocalUI()
		{
			BlogInfo info = Config.CurrentBlog;
			txbTitle.Text = info.Title;
			txbSubtitle.Text = info.SubTitle;
			txbAuthor.Text = info.Author;
			txbAuthorEmail.Text = info.Email;
			txbUser.Text = info.UserName;
			txbNews.Text = info.News;
			txbGenericTrackingCode.Text = info.TrackingCode;
			ckbAllowServiceAccess.Checked = info.AllowServiceAccess;
			ddlTimezone.DataSource = WindowsTimeZone.TimeZones;
			ddlTimezone.DataTextField = "DisplayName";
			ddlTimezone.DataValueField = "Id";
			ddlTimezone.DataBind();
			ListItem selectedItem = ddlTimezone.Items.FindByValue(info.TimeZoneId.ToString(CultureInfo.InvariantCulture));
			if (selectedItem != null)
				selectedItem.Selected = true;

			ListItem languageItem = ddlLangLocale.Items.FindByValue(info.Language);
			if(languageItem != null)
			{
				languageItem.Selected = true;
			}		
			
			if(info.Skin.HasCustomCssText)
			{
				txbSecondaryCss.Text = info.Skin.CustomCssText;
			}

            //TODO: Move to a general DataBind() call.
            ddlSkin.DataBind();
            mobileSkinDropDown.DataBind();

            SetSelectedSkin(ddlSkin, info.Skin.SkinKey);
            SetSelectedSkin(mobileSkinDropDown, info.MobileSkin.SkinKey);
			
			int count = Config.Settings.ItemCount;
			int increment = 1;
			for (int i = 1; i <= count; i = i + increment)//starting with 25, the list items increment by 5. Example: 1,2,3,...24,25,30,35,...,45,50.
			{
				ddlItemCount.Items.Add(new ListItem(i.ToString(CultureInfo.InvariantCulture), i.ToString(CultureInfo.InvariantCulture)));
				if (i == 25) { increment = 5; }
			}

			if (info.ItemCount <= count)
			{
				ddlItemCount.Items.FindByValue(info.ItemCount.ToString(CultureInfo.InvariantCulture)).Selected = true;
			}

			//int 0 = "All" items
			int categoryListPostCount = Config.Settings.CategoryListPostCount;
			int maxDropDownItems = categoryListPostCount;
			if (maxDropDownItems <= 0)
			{
				maxDropDownItems = 50;//since 0 represents "All", this provides some other options in the ddl.
			}			
			ddlCategoryListPostCount.Items.Add(new ListItem("All".ToString(CultureInfo.InvariantCulture), 0.ToString(CultureInfo.InvariantCulture)));
			increment = 1;
			for (int j = 1; j <= maxDropDownItems; j = j + increment)//starting with 25, the list items increment by 5. Example: 1,2,3,...24,25,30,35,...,45,50.
			{
				ddlCategoryListPostCount.Items.Add(new ListItem(j.ToString(CultureInfo.InvariantCulture), j.ToString(CultureInfo.InvariantCulture)));
				if (j == 25) { increment = 5; }
			}

			if (info.CategoryListPostCount <= maxDropDownItems)
			{
				ddlCategoryListPostCount.Items.FindByValue(info.CategoryListPostCount.ToString(CultureInfo.InvariantCulture)).Selected = true;
			}

			UpdateTime();
			base.BindLocalUI();
		}

        private void SetSelectedSkin(DropDownList skinDropDown, string skinKey)
        {
            ListItem skinItem = skinDropDown.Items.FindByValue(skinKey.ToUpper(CultureInfo.InvariantCulture));
            if (skinItem != null)
            {
                skinItem.Selected = true;
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
				info.UserName = txbUser.Text;

				info.TimeZoneId = Int32.Parse(ddlTimezone.SelectedItem.Value);
				info.Subfolder = Config.CurrentBlog.Subfolder;
				info.Host = Config.CurrentBlog.Host;
				info.Id = Config.CurrentBlog.Id;

				info.ItemCount = Int32.Parse(ddlItemCount.SelectedItem.Value);
				info.CategoryListPostCount = Int32.Parse(ddlCategoryListPostCount.SelectedItem.Value);				
				info.Language = ddlLangLocale.SelectedItem.Value;
				
				info.AllowServiceAccess = ckbAllowServiceAccess.Checked;

				info.Skin.CustomCssText = txbSecondaryCss.Text.Trim();

				info.News = NormalizeString(txbNews.Text);
				info.TrackingCode = NormalizeString(txbGenericTrackingCode.Text);

				SkinTemplate skinTemplate = new SkinTemplateCollection().GetTemplate(ddlSkin.SelectedItem.Value);
				info.Skin.TemplateFolder = skinTemplate.TemplateFolder;
				info.Skin.SkinStyleSheet = skinTemplate.StyleSheet;

                SkinTemplate mobileSkinTemplate = new SkinTemplateCollection(true).GetTemplate(mobileSkinDropDown.SelectedItem.Value) ?? SkinTemplate.Empty;
                info.MobileSkin.TemplateFolder = mobileSkinTemplate.TemplateFolder;
                info.MobileSkin.SkinStyleSheet = mobileSkinTemplate.StyleSheet;
				Config.UpdateConfigData(info);

				Messages.ShowMessage(RES_SUCCESS);
			}
			catch(Exception ex)
			{
				Messages.ShowError(String.Format(Constants.RES_EXCEPTION, RES_FAILURE, ex.Message));
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
			this.ddlTimezone.SelectedIndexChanged += new EventHandler(ddlTimezone_SelectedIndexChanged);
			this.btnPost.Click += new EventHandler(btnPost_Click);
		}
		#endregion

		private static string NormalizeString(string text)
		{
			string tmp = text.Trim();
			return tmp.Length == 0 ? null : tmp;
		}

		protected void btnPost_Click(object sender, EventArgs e)
		{
			BindPost();
		}

		protected void ddlTimezone_SelectedIndexChanged(object sender, EventArgs e)
		{
			UpdateTime();
		}
		
		void UpdateTime()
		{
			lblServerTimeZone.Text = string.Format("{0} ({1})", TimeZone.CurrentTimeZone.StandardName, TimeZone.CurrentTimeZone.GetUtcOffset(DateTime.Now));
			lblServerTime.Text = DateTime.Now.ToString("yyyy/MM/dd hh:mm tt");
			lblUtcTime.Text = DateTime.UtcNow.ToString("yyyy/MM/dd hh:mm tt");
			lblCurrentTime.Text = SelectedTimeZone.Now.ToString("yyyy/MM/dd hh:mm tt");
		}

		WindowsTimeZone SelectedTimeZone
		{
			get
			{
				int timeZoneId;
				string timeZoneText = ddlTimezone.SelectedValue;
				if (String.IsNullOrEmpty(timeZoneText))
					timeZoneId = TimeZone.CurrentTimeZone.StandardName.GetHashCode();
				else
					timeZoneId = int.Parse(timeZoneText);

				return WindowsTimeZone.GetById(timeZoneId);
			}
		}
	}
}
