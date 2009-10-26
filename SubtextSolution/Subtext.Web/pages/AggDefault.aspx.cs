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
using System.Web;
using System.Web.UI.WebControls;
using Subtext.Web.UI.Controls;

namespace Subtext.Web
{
    /// <summary>
    /// When AggregateBlog is enabled in Web.config, this page renders contents from 
    /// all blogs which have set their blog posts to be included in the main blog.
    /// </summary>
    public partial class AggDefault : AggregatePage
    {
        protected HyperLink Hyperlink6;
        protected HyperLink Hyperlink7;

        /// <summary>
        /// Url to the aggregate page.
        /// </summary>
        protected string AggregateUrl
        {
            get { return Url.AppRoot(); }
        }

        protected override void OnLoad(EventArgs e)
        {
            //No postbacks on this page. It is output cached.
            SetStyle();
            DataBind();
            base.OnLoad(e);
        }

        protected string ImageUrl(string imageName)
        {
            return VirtualPathUtility.ToAbsolute("~/images/" + imageName);
        }

        private void SetStyle()
        {
            const string style = "<link href=\"{0}{1}\" type=\"text/css\" rel=\"stylesheet\">";
            string apppath = HttpContext.Current.Request.ApplicationPath.EndsWith("/")
                                 ? HttpContext.Current.Request.ApplicationPath
                                 : HttpContext.Current.Request.ApplicationPath + "/";
            //TODO: This is hard-coded to look in the simple skin for aggregate blogs. We should change this later.
            Style.Text = string.Format(style, apppath, "Skins/Aggregate/Simple/Style.css") + "\n" +
                         string.Format(style, apppath, "Skins/Aggregate/Simple/blue.css") + "\n" +
                         string.Format(style, apppath, "Scripts/jquery.lightbox-0.5.css");
        }
    }
}