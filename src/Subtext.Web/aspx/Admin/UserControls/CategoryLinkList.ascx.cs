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
using System.ComponentModel;
using System.Globalization;
using Subtext.Framework;
using Subtext.Framework.Components;
using Subtext.Web.Admin.WebUI.Controls;
using Subtext.Web.UI.ViewModels;

namespace Subtext.Web.Admin.UserControls
{
    public partial class CategoryLinkList : BaseUserControl
    {
        protected ICollection<LinkCategoryLink> categoryLinks = new List<LinkCategoryLink>();

        public CategoryLinkList()
        {
            CategoryType = CategoryType.None;
        }

        [Browsable(true)]
        [Description("Sets the type of categories to load.")]
        public CategoryType CategoryType { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //Viewstate access is lost on postback for this control, so catType defaults to PostCollection.
                //So check if the catType is available in the query string and set this.catType's value to 
                //the querystring's category type enumeration
                if (!String.IsNullOrEmpty(Request.QueryString[Keys.QRYSTR_CATEGORYTYPE]))
                {
                    CategoryType =
                        (CategoryType)Enum.Parse(typeof(CategoryType), Request.QueryString[Keys.QRYSTR_CATEGORYTYPE]);
                }
                BindCategoriesRepeater();
            }
        }

        private void BindCategoriesRepeater()
        {
            //NEED TO USE ROUTING
            string baseUrl = null;

            if (CategoryType != CategoryType.None)
            {
                if (CategoryType == CategoryType.ImageCollection)
                {
                    categoryLinks.Add(new LinkCategoryLink("All Galleries", AdminUrl.EditGalleries()));
                    baseUrl = "EditGalleries.aspx";
                }
                else if (CategoryType == CategoryType.LinkCollection)
                {
                    categoryLinks.Add(new LinkCategoryLink("All Categories", AdminUrl.EditLinks()));
                    baseUrl = "EditLinks.aspx";
                }
                else if (CategoryType == CategoryType.PostCollection)
                {
                    categoryLinks.Add(new LinkCategoryLink("All Categories", AdminUrl.PostsList()));
                    baseUrl = "posts/default.aspx";
                }
                else if (CategoryType == CategoryType.StoryCollection)
                {
                    categoryLinks.Add(new LinkCategoryLink("All Categories", AdminUrl.ArticlesList()));
                    baseUrl = "articles/default.aspx";
                }

                ICollection<LinkCategory> categories = Repository.GetCategories(CategoryType, ActiveFilter.None);
                foreach (LinkCategory current in categories)
                {
                    string url = string.Format(CultureInfo.InvariantCulture, "{0}?{1}={2}&{3}={4}",
                                               Url.AdminUrl(baseUrl),
                                               Keys.QRYSTR_CATEGORYID,
                                               current.Id,
                                               Keys.QRYSTR_CATEGORYTYPE,
                                               CategoryType
                                               );
                    categoryLinks.Add(new LinkCategoryLink(current.Title, url));
                }
            }
            rptCategories.DataSource = categoryLinks;
            rptCategories.DataBind();
        }
    }
}