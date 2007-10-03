using System;
using System.Collections.Generic;
using System.ComponentModel;
using Subtext.Framework;
using Subtext.Framework.Components;
using CategoryTypeEnum = Subtext.Framework.Components.CategoryType;

namespace Subtext.Web.Admin.UserControls
{
    public partial class CategoryLinkList : System.Web.UI.UserControl
    {
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
                    this.catType = (CategoryType)Enum.Parse(typeof(CategoryType), Request.QueryString[Keys.QRYSTR_CATEGORYTYPE]);
                }
                BindCategoriesRepeater();
            }
        }

        private void BindCategoriesRepeater()
        {
            if (this.catType != CategoryTypeEnum.None)
            {
                if(this.catType == CategoryTypeEnum.ImageCollection)
                {
                    this.categoryLinks.Add(new LinkCategoryLink("All Galleries", "EditGalleries.aspx"));
                }
                else 
                {
                    this.categoryLinks.Add(new LinkCategoryLink("All Categories", Page.Request.Url.LocalPath));
                }

                if (this.catType != CategoryType.None)
                {
                    ICollection<LinkCategory> categories = Links.GetCategories(this.catType, ActiveFilter.None);
                    foreach (LinkCategory current in categories)
                    {
                        this.categoryLinks.Add(new LinkCategoryLink(current.Title, string.Format(System.Globalization.CultureInfo.InvariantCulture, "{0}?{1}={2}&{3}={4}", Page.Request.Url.LocalPath, QRYSTR_CATEGORYFILTER, current.Id, QRYSTR_CATEGORYTYPE, this.catType)));
                    }
                }
            }
            rptCategories.DataSource = this.categoryLinks;
            rptCategories.DataBind();
        }

        [Browsable(true)]
        [Description("Sets the type of categories to load.")]
        public CategoryTypeEnum CategoryType
        {
            get { return catType; }
            set { catType = value; }
        }

        CategoryTypeEnum catType = CategoryTypeEnum.None;
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
