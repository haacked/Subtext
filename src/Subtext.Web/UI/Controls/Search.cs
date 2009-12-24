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
using System.Linq;
using System.Web.UI.WebControls;
using Subtext.Framework.Data;
using Subtext.Framework.Services.SearchEngine;
using Subtext.Framework.Util;

namespace Subtext.Web.UI.Controls
{
    public class Search : BaseControl
    {
        public Search()
        {
            MaxResults = 100;
        }

        public ISearchEngineService SearchEngineService
        {
            get
            {
                return SubtextPage.SearchEngineService;
            }
        }

        protected TextBox txtSearch;

        public int MaxResults { get; set; }

        protected override void OnLoad(EventArgs e)
        {
            var searchResults = FindControl("results") as Repeater;
            var noResults = FindControl("noresults") as PlaceHolder;
            var terms = FindControl("terms") as Literal;


            if (!String.IsNullOrEmpty(txtSearch.Text))
                Response.Redirect(Request.Url.AbsolutePath + "?q=" + Server.UrlEncode(txtSearch.Text),true);

            string queryString = SubtextContext.RequestContext.GetQueryFromRequest();
            if (!String.IsNullOrEmpty(queryString))
            {
                txtSearch.Text = queryString;
                var results = SearchEngineService.Search(queryString, MaxResults, Blog.Id);
                if (results.Count()>0)
                {
                    searchResults.DataSource = results;
                    searchResults.DataBind();
                    searchResults.Visible = true;
                }
                else
                {
                    terms.Text = queryString;
                    noResults.Visible = true;
                }
            }
            base.OnLoad(e);
        }

        protected virtual void SearchResultsCreated(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                var pi = (SearchEngineResult)e.Item.DataItem;
                BindLink(e, pi);
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
                relatedLink.Attributes.Add("rel", "me");
                if (datePublished != null) datePublished.Text = searchResult.DateSyndicated.ToShortDateString();
                if (score != null) score.Text = searchResult.Score.ToString();
            }
        }
    }
}
