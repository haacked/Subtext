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
using Subtext.Framework.Components;

namespace Subtext.Web.UI.Controls
{
    public class AggBlogStats : AggregateUserControl
    {
        protected Literal BlogCount;
        protected Literal CommentCount;
        protected Literal PingtrackCount;
        protected Literal PostCount;
        protected Literal StoryCount;

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            int? groupId = GetGroupIdFromQueryString();
            HostStats stats = Repository.GetTotalBlogStats(Page.HostInfo.AggregateBlog.Host, groupId);
            if (stats != null)
            {
                BlogCount.Text = stats.BlogCount.ToString();
                PostCount.Text = stats.PostCount.ToString();
                StoryCount.Text = stats.StoryCount.ToString();
                CommentCount.Text = stats.CommentCount.ToString();
                PingtrackCount.Text = stats.PingTrackCount.ToString();
            }
        }
    }
}