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
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using Subtext.Framework.Services.SearchEngine;
using Subtext.Framework.Util;

namespace Subtext.Web.UI.Controls
{
    public class Search : BaseControl
    {
        public Search()
        {
            MaxResultsCount = 100;
        }

        public int MaxResultsCount { get; set; }

        public void btnSearch_Click(object sender, EventArgs e)
        {
            var txtSearch = FindControl("txtSearch") as TextBox;
            if (!String.IsNullOrEmpty(txtSearch.Text))
                Response.Redirect(Url.SearchPageUrl(txtSearch.Text), true);
        }

        protected override void OnLoad(EventArgs e)
        {
            if (IsPostBack)
            {
                return;
            }

            var searchResults = FindControl("results") as Repeater;
            var noResults = FindControl("noresults") as PlaceHolder;
            var terms = FindControl("terms") as Literal;
            var txtSearch = FindControl("txtSearch") as TextBox;


            string queryString = SubtextContext.RequestContext.GetQueryFromRequest();
            if (!String.IsNullOrEmpty(queryString))
            {
                if (txtSearch != null) txtSearch.Text = queryString;
                var results = SearchEngineService.Search(queryString, MaxResultsCount, Blog.Id);
                if (results.Count() > 0)
                {
                    searchResults.DataSource = results;
                    searchResults.DataBind();
                    searchResults.Visible = true;
                }
                else
                {
                    terms.Text = HttpUtility.HtmlEncode(queryString);
                    noResults.Visible = true;
                }
            }
            base.OnLoad(e);
        }

        protected virtual void SearchResultsCreated(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                SearchResult = (SearchEngineResult)e.Item.DataItem;
                BindLink(e, SearchResult);
            }
        }

        public SearchEngineResult SearchResult
        {
            get;
            private set;
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
                if (datePublished != null)
                {
                    datePublished.Text = searchResult.DateSyndicated.ToShortDateString();
                }
                if (score != null)
                {
                    score.Text = searchResult.Score.ToString();
                }
            }
        }
    }
}
