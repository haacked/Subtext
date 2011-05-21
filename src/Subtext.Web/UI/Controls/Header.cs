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
using System.Web.UI.WebControls;
using Subtext.Web.Controls;

namespace Subtext.Web.UI.Controls
{
    /// <summary>
    ///	Used to display the header.
    /// </summary>
    public class Header : BaseControl
    {
        protected Literal HeaderSubTitle;
        protected HyperLink HeaderTitle;

        protected string Title { get; set; }

        public string Subtitle { get; set; }

        protected string HomeUrl { get; set; }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            Title = Blog.Title;
            HomeUrl = Url.BlogUrl();
            Subtitle = Blog.SubTitle;

            if (null != FindControl("HeaderTitle"))
            {
                HeaderTitle.NavigateUrl = HomeUrl;
                HeaderTitle.Text = Title;
                ControlHelper.SetTitleIfNone(HeaderTitle, "The Title Of This Blog.");
            }

            if (null != FindControl("HeaderSubTitle"))
            {
                HeaderSubTitle.Text = Subtitle;
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            DataBind();
        }
    }
}