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
using Subtext.Framework.Data;
using Subtext.Framework.Providers;

namespace Subtext.Web.UI.Controls
{
    public class RelatedLinks : BaseControl
    {
        public RelatedLinks()
        {
            RowCount = 5;
        }

        public int RowCount { get; set; }

        protected override void OnLoad(EventArgs e)
        {
            var myRelLinks = new List<PositionItems>();
            int blogId = Blog.Id >= 1 ? Blog.Id : 0;
            var urlRelatedLinks = FindControl("Links") as Repeater;
            Entry entry = Cacher.GetEntryFromRequest(true, SubtextContext);
            ICollection<EntrySummary> relatedEntries = Repository.GetRelatedEntries(blogId, entry.Id,
                                                                                                   RowCount);

            foreach(EntrySummary relatedEntry in relatedEntries)
            {
                string myUrl = Url.EntryUrl(relatedEntry);
                myRelLinks.Add(new PositionItems(relatedEntry.Title, myUrl));
            }

            urlRelatedLinks.DataSource = myRelLinks;
            urlRelatedLinks.DataBind();
            base.OnLoad(e);
        }


        protected virtual void MoreReadingCreated(object sender, RepeaterItemEventArgs e)
        {
            if(e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                var pi = (PositionItems)e.Item.DataItem;
                BindLink(e, pi);
            }
        }

        private static void BindLink(RepeaterItemEventArgs e, PositionItems pi)
        {
            var relatedLink = (HyperLink)e.Item.FindControl("Link");
            if(relatedLink != null)
            {
                relatedLink.Text = pi.Title;
                relatedLink.NavigateUrl = pi.Url;
                relatedLink.Attributes.Add("rel", "me");
            }
        }
    }

    public class PositionItems
    {
        public PositionItems(string title, string url)
        {
            Title = title;
            Url = url;
        }

        public string Title { get; private set; }

        public string Url { get; private set; }
    }
}