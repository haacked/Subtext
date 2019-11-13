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
using System.Web.UI.WebControls;
using Subtext.Framework;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;

namespace Subtext.Web.UI.Controls
{
    /// <summary>
    /// Displays the titles of the most recent entries on the skin.
    /// </summary>
    public class PopularPosts : BaseControl
    {
        protected Repeater postList;

        public EntryStatsView CurrentEntry { get; private set; }

        public DateFilter FilterType { get; set; }

        /// <summary>
        /// Binds the posts <see cref="List{T}"/> to the post list repeater.
        /// Raises the <see cref="E:System.Web.UI.Control.Load"/>
        /// event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.EventArgs"/> object that contains the event data.</param>
        protected override void OnLoad(EventArgs e)
        {
            FilterType = DateFilter.None;
            string filterTypeText = Request.QueryString["popular-posts"];
            string strCookieName = Config.CurrentBlog.BlogGroupId + "popular-posts";
            if (filterTypeText == null)
            {
                if (Request.Cookies[strCookieName] != null)
                {
                    filterTypeText = Request.Cookies[strCookieName].Value;
                }
            }

            Response.Cookies[strCookieName].Value = filterTypeText;
            Response.Cookies[strCookieName].Expires = DateTime.UtcNow.AddYears(1);

            if (!string.IsNullOrEmpty(filterTypeText))
            {
                try
                {
                    FilterType = (DateFilter)Enum.Parse(typeof(DateFilter), filterTypeText, true);
                }
                catch
                {
                }
            }

            ICollection<EntryStatsView> posts = Repository.GetPopularEntries(Blog.Id, FilterType);

            base.OnLoad(e);

            if (posts != null)
            {
                postList.DataSource = posts;
                postList.DataBind();
            }
            else
            {
                Controls.Clear();
                Visible = false;
            }
        }

        protected void PostCreated(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                CurrentEntry = (EntryStatsView)e.Item.DataItem;
            }
        }
    }
}