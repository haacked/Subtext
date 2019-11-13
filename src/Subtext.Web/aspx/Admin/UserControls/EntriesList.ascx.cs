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
using System.Diagnostics;
using System.Globalization;
using System.Web.UI.WebControls;
using Subtext.Extensibility;
using Subtext.Extensibility.Interfaces;
using Subtext.Framework.Components;
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

        public string HeaderText
        {
            get { return (string)ViewState["HeaderText"] ?? string.Empty; }
            set { ViewState["HeaderText"] = value; }
        }

        public string ResultsUrlFormat
        {
            get { return resultsPager.UrlFormat; }
            set { resultsPager.UrlFormat = value; }
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
                {
                    return (PostType)ViewState["PostType"];
                }
                return PostType.None;
            }
            set { ViewState["PostType"] = value; }
        }

        protected override void OnInit(EventArgs e)
        {
            rprSelectionList.ItemCommand += OnItemCommand;
            base.OnInit(e);
        }

        protected override void OnLoad(EventArgs e)
        {
            if (Request.QueryString[Keys.QRYSTR_PAGEINDEX] != null)
            {
                pageIndex = Convert.ToInt32(Request.QueryString[Keys.QRYSTR_PAGEINDEX]);
            }

            if (Request.QueryString[Keys.QRYSTR_CATEGORYID] != null)
            {
                categoryId = Convert.ToInt32(Request.QueryString[Keys.QRYSTR_CATEGORYID]);
            }

            resultsPager.PageSize = Preferences.ListingItemCount;
            resultsPager.PageIndex = pageIndex;

            if (categoryId != null)
            {
                string catIdQueryString = string.Format(CultureInfo.InvariantCulture, "&{0}={1}", Keys.QRYSTR_CATEGORYID,
                                                        categoryId);
                if (!resultsPager.UrlFormat.EndsWith(catIdQueryString))
                {
                    resultsPager.UrlFormat += catIdQueryString;
                }
            }

            if (!IsPostBack)
            {
                BindList();
            }

            base.OnLoad(e);
        }

        void OnItemCommand(object source, RepeaterCommandEventArgs e)
        {
            ConfirmDelete(Convert.ToInt32(e.CommandArgument));
        }

        private void ConfirmDelete(int postID)
        {
            var page = (AdminPage)Page;
            if (page != null)
            {
                var command = new DeletePostCommand(Repository, postID, page.SearchEngine);
                command.Execute();
                BindList();
            }
        }

        public string PostsEditUrl(object item)
        {
            var entry = (Entry)item;
            return AdminUrl.PostsEdit(entry.Id);
        }

        public string ArticlesEditUrl(object item)
        {
            var entry = (Entry)item;
            return AdminUrl.ArticlesEdit(entry.Id);
        }

        public string ContentEditUrl(object item)
        {
            if (((AdminPage)this.Page).TabSectionId == "Articles")
            {
                return ArticlesEditUrl(item);
            }
            else
            {
                return PostsEditUrl(item);
            }
        }

        public string WindowsLiveWriterEditUrl(object item)
        {
            var entry = (Entry)item;
            return Url.WindowsLiveWriterEditUrl(entry.Id, this.Blog);
        }

        public string ReferrersUrl(object item)
        {
            var entry = (Entry)item;
            return AdminUrl.Referrers(entry.Id);
        }

        protected string IsActiveText(object entryObject)
        {
            var entry = entryObject as Entry;

            Debug.Assert(entry != null, "Entry should never be null here");

            string active = "False";
            if (entry.IsActive)
            {
                active = "True";
                if (entry.DatePublishedUtc > DateTime.UtcNow)
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

            IPagedCollection<EntryStatsView> selectionList = Repository.GetEntries(EntryType, categoryId, pageIndex: pageIndex,
                pageSize: resultsPager.PageSize);

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
            title.InnerText = HeaderText;
            base.OnPreRender(e);
        }

        protected EntryStatsView GetEntry(object o)
        {
            return o as EntryStatsView;
        }
    }
}