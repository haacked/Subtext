using System;
using System.Collections.Generic;
using System.ComponentModel;
using Subtext.Framework;
using Subtext.Framework.Components;

namespace Subtext.Web.Admin.UserControls
{
    public partial class CategoryLinkList : System.Web.UI.UserControl
    {
        public CategoryLinkList() {
            CategoryType = CategoryType.None;
        }

        private const string QRYSTR_CATEGORYFILTER = "catid";
        private const string QRYSTR_CATEGORYTYPE = "catType";
        protected ICollection<LinkCategoryLink> categoryLinks = new List<LinkCategoryLink>();

        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                //Viewstate access is lost on postback for this control, so catType defaults to PostCollection.
                //So check if the catType is available in the query string and set this.catType's value to 
                //the querystring's category type enumeration
                if (!String.IsNullOrEmpty(Request.QueryString[Keys.QRYSTR_CATEGORYTYPE]))
                {
                    CategoryType = (CategoryType)Enum.Parse(typeof(CategoryType), Request.QueryString[Keys.QRYSTR_CATEGORYTYPE]);
                }
                BindCategoriesRepeater();
            }
        }

        private void BindCategoriesRepeater()
        {
            string baseUrl = "Default.aspx";

            if (this.CategoryType != CategoryType.None)
            {
                if (this.CategoryType == CategoryType.ImageCollection)
                {
                    baseUrl = "EditGalleries.aspx";
                    this.categoryLinks.Add(new LinkCategoryLink("All Galleries", "EditGalleries.aspx"));
                }
                else 
                {
                    this.categoryLinks.Add(new LinkCategoryLink("All Categories", "EditLinks.aspx"));
                }

                if (this.CategoryType != CategoryType.None)
                {
                    ICollection<LinkCategory> categories = Links.GetCategories(CategoryType, ActiveFilter.None);
                    foreach (LinkCategory current in categories)
                    {
                        this.categoryLinks.Add(new LinkCategoryLink(current.Title, string.Format(System.Globalization.CultureInfo.InvariantCulture, "{4}?{0}={1}&{2}={3}", QRYSTR_CATEGORYFILTER, current.Id, QRYSTR_CATEGORYTYPE, this.CategoryType, baseUrl)));
                    }
                }
            }
            rptCategories.DataSource = this.categoryLinks;
            rptCategories.DataBind();
        }

        [Browsable(true)]
        [Description("Sets the type of categories to load.")]
        public CategoryType CategoryType
        {
            get;
            set;
        }
    }

    public struct LinkCategoryLink
    {
        public LinkCategoryLink(string title, string url)
        {
            this.title = title;
            this.url = url;
        }

        public string Url
        {
            get { return this.url; }
            set { this.url = value; }
        }

        string url;

        public string Title
        {
            get { return this.title; }
            set { this.title = value; }
        }

        string title;
    }
}
