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
using Subtext.Framework.Components;

namespace Subtext.Web.UI.Controls
{
    /// <summary>
    /// Summary description for Top10Module.
    /// </summary>
    public class Top10Module : BaseControl
    {
        protected Repeater Top10Entries;

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            int blogId = Blog.Id >= 1 ? Blog.Id : 0;
            var myLastItems = new List<PositionTopItems>();

            ICollection<EntrySummary> entrySummaries = Repository.GetTopEntrySummaries(blogId, 10);

            foreach (EntrySummary entrySummary in entrySummaries)
            {
                entrySummary.Blog = Blog;
                string title = entrySummary.Title;
                string myUrl = Url.EntryUrl(entrySummary);

                myLastItems.Add(new PositionTopItems(title, myUrl));
            }

            Top10Entries.DataSource = myLastItems;
            Top10Entries.DataBind();
        }
    }

    public class PositionTopItems
    {
        public PositionTopItems(string title, string url)
        {
            Title = title;
            this.url = url;
        }

        public string Title { get; private set; }

        public string url { get; private set; }
    }
}