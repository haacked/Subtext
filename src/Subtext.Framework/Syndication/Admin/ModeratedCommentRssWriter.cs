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
using System.IO;
using Subtext.Framework.Components;
using Subtext.Framework.Routing;

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

            Uri url = UrlHelper.AdminUrl("Feedback.aspx", new { status = 2 }).ToFullyQualifiedUrl(Blog);
            BuildChannel(CommentEntry.Title, url, CommentEntry.Email,
                         CommentEntry.HasDescription ? CommentEntry.Description : CommentEntry.Body, Blog.Language,
                         Blog.Author, Blog.LicenseUrl, image);
        }
    }
}