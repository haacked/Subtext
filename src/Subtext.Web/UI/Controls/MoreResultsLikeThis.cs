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
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Subtext.Framework.Components;
using Subtext.Framework.Data;
using Subtext.Framework.Services.SearchEngine;
using Subtext.Web.UI.Pages;

namespace Subtext.Web.UI.Controls
{
    public class MoreResultsLikeThis : BaseControl
    {
        public MoreResultsLikeThis()
        {
            RowCount = 5;
        }

        private int _rowCount;
        public int RowCount
        {
            get { return _rowCount + 1; }
            set { _rowCount = value; }
        }

        public string Query
        {
            get
            {
                var page = Page as SubtextMasterPage;
                return page != null ? page.Query : null;
            }
        }

        public IEnumerable<SearchEngineResult> SearchResults
        {
            get;
            set;
        }

        protected override void OnLoad(EventArgs e)
        {
            int blogId = Blog.Id >= 1 ? Blog.Id : 0;
            var urlRelatedLinks = FindControl("Links") as Repeater;

            if (urlRelatedLinks != null)
            {
                if (SearchResults == null)
                {
                    int entryId = -1;
                    Entry entry = Cacher.GetEntryFromRequest(true, SubtextContext);
                    if (entry != null) entryId = entry.Id;
                    SearchResults = SearchEngineService.Search(Query, RowCount, blogId, entryId);
                }
                urlRelatedLinks.DataSource = SearchResults;
                urlRelatedLinks.DataBind();
            }
            var keywords = FindControl("keywords") as Literal;
            if (keywords != null)
            {
                keywords.Text = HttpUtility.HtmlEncode(Query);
            }

            base.OnLoad(e);
        }

        protected virtual void MoreReadingCreated(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                var pi = (SearchEngineResult)e.Item.DataItem;
                BindLink(e, pi);
            }
            if (e.Item.ItemType == ListItemType.Footer)
            {
                var searchMoreLink = e.Item.FindControl("searchMore") as HtmlAnchor;
                if (searchMoreLink != null)
                {
                    searchMoreLink.InnerText = searchMoreLink.InnerText + Query;
                    searchMoreLink.HRef = Url.SearchPageUrl(Query);
                }
            }
        }

        private void BindLink(RepeaterItemEventArgs e, SearchEngineResult searchResult)
        {
            var relatedLink = (HyperLink)e.Item.FindControl("Link");
            var datePublished = (Literal)e.Item.FindControl("DatePublished");
            var score = (Literal)e.Item.FindControl("Score");
            if (relatedLink != null)
            {
                relatedLink.Text = searchResult.Title;
                relatedLink.NavigateUrl = Url.EntryUrl(searchResult);
                if (datePublished != null) datePublished.Text = searchResult.DateSyndicated.ToShortDateString();
                if (score != null) score.Text = searchResult.Score.ToString();
            }
        }
    }
}
