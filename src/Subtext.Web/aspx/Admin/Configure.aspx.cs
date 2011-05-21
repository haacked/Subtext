#region Disclaimer/Info

///////////////////////////////////////////////////////////////////////////////////////////////////
// Subtext WebLog
// 
// Subtext is an open source weblog system that is a fork of the .TEXT
// weblog system.
//
// For updated news and information please visit http://subtextproject.com/
// Subtext is hosted at Google Code at http://code.google.com/p/subtext/
// The development mailing list is at subtext@googlegroups.com 
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
using Subtext.Infrastructure;

namespace Subtext.Web.Admin.Pages
{
    public partial class Configure : AdminOptionsPage
    {
        // abstract out at a future point for i18n
        private const string FailureMessage = "Configuration update failed.";
        private const string SuccessMessage = "Your configuration was successfully updated.";
        private IList<SkinTemplate> _mobileSkins;
        private ICollection<SkinTemplate> _skins;

        public CategoryType CategoryType
        {
            get { return (CategoryType)ViewState["CategoryType"]; }
            set { ViewState["CategoryType"] = value; }
        }

        protected ICollection<SkinTemplate> Skins
        {
            get
            {
                if (_skins == null)
                {
                    var engine = new SkinEngine();
                    IDictionary<string, SkinTemplate> templates = engine.GetSkinTemplates(false /* mobile */);
                    _skins = templates.Values;
                    foreach (SkinTemplate template in _skins)
                    {
                        if (template.MobileSupport == MobileSupport.Supported)
                        {
                            template.Name += " (mobile ready)";
                        }
                    }
                }
                return _skins;
            }
        }

        protected ICollection<SkinTemplate> MobileSkins
        {
            get
            {
                if (_mobileSkins == null)
                {
                    var engine = new SkinEngine();
                    IDictionary<string, SkinTemplate> templates = engine.GetSkinTemplates(true /* mobile */);
                    _mobileSkins = new List<SkinTemplate>(templates.Values);
                    _mobileSkins.Insert(0, SkinTemplate.Empty);
                }
                return _mobileSkins;
            }
        }

        private ITimeZone SelectedTimeZone
        {
            get
            {
                string timeZoneId = String.IsNullOrEmpty(ddlTimezone.SelectedValue)
                                        ? TimeZone.CurrentTimeZone.StandardName
                                        : ddlTimezone.SelectedValue;

                return new TimeZoneWrapper(TimeZones.GetTimeZones().GetById(timeZoneId));
            }
        }

        protected override void BindLocalUI()
        {
            txbTitle.Text = Blog.Title;
            txbSubtitle.Text = Blog.SubTitle;
            txbAuthor.Text = Blog.Author;
            txbAuthorEmail.Text = Blog.Email;
            txbUser.Text = Blog.UserName;
            txbNews.Text = Blog.News;
            ckbShowEmailonRssFeed.Checked = Blog.ShowEmailAddressInRss;
            txbGenericTrackingCode.Text = Blog.TrackingCode;
            ckbAllowServiceAccess.Checked = Blog.AllowServiceAccess;
            chkAutoGenerate.Checked = Blog.AutoFriendlyUrlEnabled;
            ddlTimezone.DataSource = TimeZones.GetTimeZones();
            ddlTimezone.DataTextField = "DisplayName";
            ddlTimezone.DataValueField = "Id";
            ddlTimezone.DataBind();
            ListItem selectedItem = ddlTimezone.Items.FindByValue(Blog.TimeZoneId.ToString(CultureInfo.InvariantCulture));
            if (selectedItem != null)
            {
                selectedItem.Selected = true;
            }

            ListItem languageItem = ddlLangLocale.Items.FindByValue(Blog.Language);
            if (languageItem != null)
            {
                languageItem.Selected = true;
            }

            if (Blog.Skin.HasCustomCssText)
            {
                txbSecondaryCss.Text = Blog.Skin.CustomCssText;
            }

            //TODO: Move to a general DataBind() call.
            int count = Config.Settings.ItemCount;
            int increment = 1;
            for (int i = 1; i <= count; i = i + increment)
            //starting with 25, the list items increment by 5. Example: 1,2,3,...24,25,30,35,...,45,50.
            {
                ddlItemCount.Items.Add(new ListItem(i.ToString(CultureInfo.InvariantCulture),
                                                    i.ToString(CultureInfo.InvariantCulture)));
                if (i == 25)
                {
                    increment = 5;
                }
            }

            if (Blog.ItemCount <= count)
            {
                ddlItemCount.Items.FindByValue(Blog.ItemCount.ToString(CultureInfo.InvariantCulture)).Selected = true;
            }

            //int 0 = "All" items
            int categoryListPostCount = Config.Settings.CategoryListPostCount;
            int maxDropDownItems = categoryListPostCount;
            if (maxDropDownItems <= 0)
            {
                maxDropDownItems = 50; //since 0 represents "All", this provides some other options in the ddl.
            }
            ddlCategoryListPostCount.Items.Add(new ListItem("All".ToString(CultureInfo.InvariantCulture),
                                                            0.ToString(CultureInfo.InvariantCulture)));
            increment = 1;
            for (int j = 1; j <= maxDropDownItems; j = j + increment)
            //starting with 25, the list items increment by 5. Example: 1,2,3,...24,25,30,35,...,45,50.
            {
                ddlCategoryListPostCount.Items.Add(new ListItem(j.ToString(CultureInfo.InvariantCulture),
                                                                j.ToString(CultureInfo.InvariantCulture)));
                if (j == 25)
                {
                    increment = 5;
                }
            }

            if (Blog.CategoryListPostCount <= maxDropDownItems)
            {
                ddlCategoryListPostCount.Items.FindByValue(
                    Blog.CategoryListPostCount.ToString(CultureInfo.InvariantCulture)).Selected = true;
            }

            UpdateTime();

            base.BindLocalUI();
        }

        private void BindPost()
        {
            try
            {
                Blog.Title = txbTitle.Text;
                Blog.SubTitle = txbSubtitle.Text;
                Blog.Author = txbAuthor.Text;
                Blog.Email = txbAuthorEmail.Text;
                Blog.UserName = txbUser.Text;
                Blog.ShowEmailAddressInRss = ckbShowEmailonRssFeed.Checked;
                Blog.TimeZoneId = ddlTimezone.SelectedItem.Value;
                Blog.Subfolder = Blog.Subfolder;
                Blog.Host = Blog.Host;
                Blog.Id = Blog.Id;

                Blog.ItemCount = Int32.Parse(ddlItemCount.SelectedItem.Value);
                Blog.CategoryListPostCount = Int32.Parse(ddlCategoryListPostCount.SelectedItem.Value);
                Blog.Language = ddlLangLocale.SelectedItem.Value;

                Blog.AllowServiceAccess = ckbAllowServiceAccess.Checked;

                Blog.Skin.CustomCssText = txbSecondaryCss.Text.Trim();

                Blog.News = NormalizeString(txbNews.Text);
                Blog.TrackingCode = NormalizeString(txbGenericTrackingCode.Text);

                Blog.AutoFriendlyUrlEnabled = chkAutoGenerate.Checked;
                Repository.UpdateConfigData(Blog);

                Messages.ShowMessage(SuccessMessage);
            }
            catch (Exception ex)
            {
                Messages.ShowError(String.Format(Constants.RES_EXCEPTION, FailureMessage, ex.Message));
            }
        }

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

        private void UpdateTime()
        {
            lblServerTimeZone.Text = string.Format(CultureInfo.InvariantCulture, "{0} ({1})",
                                                   TimeZone.CurrentTimeZone.StandardName,
                                                   TimeZone.CurrentTimeZone.GetUtcOffset(DateTime.Now));
            lblServerTime.Text = DateTime.Now.ToString("yyyy/MM/dd hh:mm tt");
            lblUtcTime.Text = DateTime.UtcNow.ToString("yyyy/MM/dd hh:mm tt");
            lblCurrentTime.Text = SelectedTimeZone.Now.ToString("yyyy/MM/dd hh:mm tt");
        }

        #region Web Form Designer generated code

        protected override void OnInit(EventArgs e)
        {
            //
            // CODEGEN: This call is required by the ASP.NET Web Form Designer.
            //
            InitializeComponent();
            base.OnInit(e);

            ViewState["CategoryId"] = NullValue.NullInt32;
            ViewState["CategoryType"] = Constants.DEFAULT_CATEGORYTYPE;
        }

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.ddlTimezone.SelectedIndexChanged += ddlTimezone_SelectedIndexChanged;
            this.btnPost.Click += btnPost_Click;
        }

        #endregion
    }
}