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

namespace Subtext.Web.UI.Controls
{
    /// <summary>
    ///	Displays news...
    /// </summary>
    public class News : BaseControl
    {
        protected Literal NewsItem;

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (Context != null)
            {
                if (Blog.HasNews)
                {
                    NewsItem.Text = Blog.News;
                }
                else
                {
                    //this.Controls.Clear();
                    Visible = false;
                }
            }
        }
    }
}