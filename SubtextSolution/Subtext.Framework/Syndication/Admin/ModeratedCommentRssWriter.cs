using System;
using System.Collections.Generic;
using System.IO;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;

namespace Subtext.Framework.Syndication.Admin
{
    public class ModeratedCommentRssWriter : CommentRssWriter
    {
        public ModeratedCommentRssWriter(TextWriter writer, ICollection<FeedbackItem> commentEntries, Entry entry,
                                         ISubtextContext context)
            : base(writer, commentEntries, entry, context)
        {
        }

        protected override void WriteChannel()
        {
            var image = new RssImageElement(GetRssImage(),
                                            CommentEntry.Title,
                                            UrlHelper.EntryUrl(CommentEntry).ToFullyQualifiedUrl(Blog),
                                            77,
                                            60,
                                            null);

            Uri url = UrlHelper.AdminUrl("Feedback.aspx", new {status = 2}).ToFullyQualifiedUrl(Blog);
            BuildChannel(CommentEntry.Title, url, CommentEntry.Email,
                         CommentEntry.HasDescription ? CommentEntry.Description : CommentEntry.Body, Blog.Language,
                         Blog.Author, Blog.LicenseUrl, image);
        }


        //protected override string GetLinkFromItem(FeedbackItem item)
        //{
        //    return UrlHelper.AdminUrl("Feedback.aspx?status=2");
        //}
        protected override void EntryXml(FeedbackItem item, BlogConfigurationSettings settings)
        {
            base.EntryXml(item, settings);
        }
    }
}