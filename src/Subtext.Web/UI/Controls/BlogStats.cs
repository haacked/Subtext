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
using System.Globalization;
using System.Web.UI.WebControls;
using Subtext.Framework;

namespace Subtext.Web.UI.Controls
{
    /// <summary>
    ///		Summary description for Header.
    /// </summary>
    public class BlogStats : CachedColumnControl
    {
        protected Literal CommentCount;
        protected Literal PingTrackCount;
        protected Literal PostCount;
        protected Literal StoryCount;


        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (Context != null)
            {
                Blog info = Blog;
                PostCount.Text = info.PostCount.ToString(CultureInfo.InvariantCulture);
                StoryCount.Text = info.StoryCount.ToString(CultureInfo.InvariantCulture);
                CommentCount.Text = info.CommentCount.ToString(CultureInfo.InvariantCulture);
                PingTrackCount.Text = info.PingTrackCount.ToString(CultureInfo.InvariantCulture);
            }
        }
    }
}