using System;
using System.Collections.Generic;
using System.Text;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;

namespace Subtext.Framework.Syndication.Admin
{
	public class ModeratedCommentRssWriter:CommentRssWriter
	{
		public ModeratedCommentRssWriter(IList<FeedbackItem> commentEntries, Entry entry)
			:base(commentEntries,entry)
		{
		}

		protected override void WriteChannel()
		{
			RssImageElement image = new RssImageElement(GetRssImage(), CommentEntry.Title, CommentEntry.FullyQualifiedUrl, 77, 60, null);
			this.BuildChannel(CommentEntry.Title, Config.CurrentBlog.UrlFormats.AdminUrl("Feedback.aspx?status=2"), CommentEntry.Author.Email, CommentEntry.HasDescription ? CommentEntry.Description : CommentEntry.Body, info.Language, info.Author, Config.CurrentBlog.LicenseUrl, image);			
		}
	}
}
