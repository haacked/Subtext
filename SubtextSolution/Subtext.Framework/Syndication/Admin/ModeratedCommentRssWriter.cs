using System.Collections.Generic;
using System.IO;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;

namespace Subtext.Framework.Syndication.Admin
{
	public class ModeratedCommentRssWriter : CommentRssWriter
	{
		public ModeratedCommentRssWriter(TextWriter writer, ICollection<FeedbackItem> commentEntries, Entry entry, ISubtextContext context)
			: base(writer, commentEntries, entry, context)
		{
		}

		protected override void WriteChannel()
		{
			RssImageElement image = new RssImageElement(GetRssImage(), CommentEntry.Title, CommentEntry.FullyQualifiedUrl, 77, 60, null);
			this.BuildChannel(CommentEntry.Title, Blog.UrlFormats.AdminUrl("Feedback.aspx?status=2"), CommentEntry.Email, CommentEntry.HasDescription ? CommentEntry.Description : CommentEntry.Body, Blog.Language, Blog.Author, Blog.LicenseUrl, image);			
		}


		//protected override string GetLinkFromItem(FeedbackItem item)
		//{
		//    return Blog.UrlFormats.AdminUrl("Feedback.aspx?status=2");
		//}
		protected override void EntryXml(FeedbackItem item, BlogConfigurationSettings settings)
		{
			base.EntryXml(item, settings);
		}
	}
}
