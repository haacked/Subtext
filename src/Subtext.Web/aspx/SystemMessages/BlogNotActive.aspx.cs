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
using Subtext.Framework.Web.Handlers;

namespace Subtext.Web
{
    /// <summary>
    /// Displays the blog not active message.
    /// </summary>
    public partial class BlogNotActive : SubtextPage
    {
        protected override void OnLoad(EventArgs e)
        {
            if (!Blog.IsActive)
            {
                plcInactiveBlogMessage.Visible = true;
                plcNothingToSeeHere.Visible = false;
            }
            else
            {
                lnkBlog.HRef = Url.BlogUrl();
            }
            base.OnLoad(e);
        }
    }
}