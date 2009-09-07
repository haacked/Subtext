using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using Subtext.Framework;
using Subtext.Framework.Components;
using Subtext.Framework.Data;
using Subtext.Framework.Providers;

namespace Subtext.Web.UI.Controls
{
    public class RelatedLinks : BaseControl
    {
        public RelatedLinks()
            : base()
        {
            RowCount = 5;
        }

        public int RowCount
        {
            get;
            set;
        }

        protected override void OnLoad(EventArgs e)
        {
            var myRelLinks = new List<PositionItems>();
            int blogId = Blog.Id >= 1 ? Blog.Id : 0;
            Repeater urlRelatedLinks = this.FindControl("Links") as Repeater;
            Entry entry = Cacher.GetEntryFromRequest(true, SubtextContext);
            var relatedEntries = ObjectProvider.Instance().GetRelatedEntries(blogId, entry.Id, RowCount);

            foreach (var relatedEntry in relatedEntries)
            {
                string myURL = Url.EntryUrl(relatedEntry);
                myRelLinks.Add(new PositionItems(relatedEntry.Title, myURL));
            }

            urlRelatedLinks.DataSource = myRelLinks;
            urlRelatedLinks.DataBind(); 
            base.OnLoad(e);
        }
        

        protected virtual void MoreReadingCreated(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                PositionItems pi = (PositionItems)e.Item.DataItem;
                BindLink(e, pi);

            }
        }

        private void BindLink(RepeaterItemEventArgs e, PositionItems pi)
        {
            HyperLink relatedLink = (HyperLink)e.Item.FindControl("Link");
            if (relatedLink != null)
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

        public string Title
        {
            get;
            private set;
        }

        public string Url
        {
            get;
            private set;
        }
    }
}
