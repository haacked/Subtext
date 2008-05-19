using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Subtext.Framework;
using Subtext.Framework.Format;
using Subtext.Framework.Configuration;

namespace Subtext.Web.Admin.WebUI {
    public class AdminUrlHelper 
    {
        UrlFormats formats;

        public AdminUrlHelper() : this(Config.CurrentBlog) { }

        public AdminUrlHelper(BlogInfo blog) 
        {
            formats = blog.UrlFormats;
        }

        public string PostsList 
        {
            get 
            {
                return formats.AdminUrl("Posts/");
            }
        }

        public string ArticlesList {
            get {
                return formats.AdminUrl("Articles/");
            }
        }

        public string AdminUrl(string page) 
        {
            return formats.AdminUrl(page);
        }
    }
}
