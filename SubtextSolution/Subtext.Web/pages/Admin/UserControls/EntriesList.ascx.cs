#region Disclaimer/Info
///////////////////////////////////////////////////////////////////////////////////////////////////
// Subtext WebLog
// 
// Subtext is an open source weblog system that is a fork of the .TEXT
// weblog system.
//
// For updated news and information please visit http://subtextproject.com/
// Subtext is hosted at Google Code at http://code.google.com/p/subtext/
// The development mailing list is at subtext-devs@lists.sourceforge.net 
//
// This project is licensed under the BSD license.  See the License.txt file for more information.
///////////////////////////////////////////////////////////////////////////////////////////////////
#endregion

using System;
using System.Diagnostics;
using System.Globalization;
using Subtext.Extensibility;
using Subtext.Extensibility.Interfaces;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;
using Subtext.Web.Admin.Commands;
using Subtext.Web.Admin.Pages;
using Subtext.Web.Properties;
using Subtext.Web.UI.Controls;

namespace Subtext.Web.Admin.UserControls
{
    public partial class EntriesList : BaseControl
    {
        private int? categoryId = null;
        private int pageIndex = 0;

        protected override void OnInit(EventArgs e)
        {
            this.rprSelectionList.ItemCommand += OnItemCommand;
            base.OnInit(e);
        }

        protected override void OnLoad(EventArgs e)
        {
            if (Request.QueryString[Keys.QRYSTR_PAGEINDEX] != null)
            {
                this.pageIndex = Convert.ToInt32(Request.QueryString[Keys.QRYSTR_PAGEINDEX]);
            }

            if (Request.QueryString[Keys.QRYSTR_CATEGORYID] != null)
            {
                this.categoryId = Convert.ToInt32(Request.QueryString[Keys.QRYSTR_CATEGORYID]);
            }

            this.resultsPager.PageSize = Preferences.ListingItemCount;
            this.resultsPager.PageIndex = this.pageIndex;

            if (this.categoryId != null)
            {
                string catIdQueryString = string.Format(CultureInfo.InvariantCulture, "&{0}={1}", Keys.QRYSTR_CATEGORYID, this.categoryId);
                if (!this.resultsPager.UrlFormat.EndsWith(catIdQueryString))
                    this.resultsPager.UrlFormat += catIdQueryString;
            }

            if (!IsPostBack)
            {
                BindList();
            }

            base.OnLoad(e);
        }

        void OnItemCommand(object source, System.Web.UI.WebControls.RepeaterCommandEventArgs e)
        {
            ConfirmDelete(Convert.ToInt32(e.CommandArgument));
        }

        private void ConfirmDelete(int postID)
        {
            AdminPage page = (AdminPage)Page;
            if (page != null)
            {
                var command = new DeletePostCommand(Repository, postID);
                command.Execute();
                BindList();
            }
        }

        public string PostsEditUrl(object item)
        {
            var entry = (Entry)item;
            return AdminUrl.PostsEdit(entry.Id);
        }

        public string ReferrersUrl(object item)
        {
            var entry = (Entry)item;
            return AdminUrl.Referrers(entry.Id);
        }

        protected string IsActiveText(object entryObject)
        {
            Entry entry = entryObject as Entry;

            Debug.Assert(entry != null, "Entry should never be null here");

            string active = "False";
            if (entry.IsActive)
            {
                active = "True";
                if (entry.DateSyndicated > Config.CurrentBlog.TimeZone.Now)
                {
                    active += "<em> on " + entry.DateSyndicated.ToShortDateString() + "</em>";
                }
            }
            return active;
        }

        private void BindList()
        {
            if (categoryId != null)
            {
                LinkCategory category = Repository.GetLinkCategory(categoryId, false);
                if (category != null)
                {
                    HeaderText = Resources.Label_Posts.ToUpper(CultureInfo.CurrentCulture) + " (" + category.Title + ")";
                }
            }

            IPagedCollection<EntryStatsView> selectionList = Repository.GetPagedEntries(this.EntryType, this.categoryId, this.pageIndex, this.resultsPager.PageSize);

            if (selectionList.Count > 0)
            {
                resultsPager.ItemCount = selectionList.MaxItems;
                rprSelectionList.DataSource = selectionList;
                rprSelectionList.DataBind();
                NoMessagesLabel.Visible = false;
            }

            NoMessagesLabel.Visible = selectionList.Count <= 0;
            resultsPager.Visible = rprSelectionList.Visible = selectionList.Count > 0;
        }

        protected override void OnPreRender(EventArgs e)
        {
            this.title.InnerText = HeaderText;
            base.OnPreRender(e);
        }

        public string HeaderText
        {
            get { return (string)ViewState["HeaderText"] ?? string.Empty; }
            set { ViewState["HeaderText"] = value; }
        }

        public string ResultsUrlFormat
        {
            get
            {
                return this.resultsPager.UrlFormat;
            }
            set
            {
                this.resultsPager.UrlFormat = value;
            }
        }

        /// <summary>
        /// Gets or sets the type of the entry.
        /// </summary>
        /// <value>The type of the entry.</value>
        public PostType EntryType
        {
            get
            {
                if (ViewState["PostType"] != null)
                    return (PostType)ViewState["PostType"];
                return PostType.None;
            }
            set
            {
                ViewState["PostType"] = value;
            }
        }

        protected EntryStatsView GetEntry(object o) 
        {
            return o as EntryStatsView;
        }
    }
}