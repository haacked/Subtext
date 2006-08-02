#region Disclaimer/Info
///////////////////////////////////////////////////////////////////////////////////////////////////
// Subtext WebLog
// 
// Subtext is an open source weblog system that is a fork of the .TEXT
// weblog system.
//
// For updated news and information please visit http://subtextproject.com/
// Subtext is hosted at SourceForge at http://sourceforge.net/projects/subtext
// The development mailing list is at subtext-devs@lists.sourceforge.net 
//
// This project is licensed under the BSD license.  See the License.txt file for more information.
///////////////////////////////////////////////////////////////////////////////////////////////////
#endregion

using System;
using System.Collections.Generic;
using System.Xml;
using Subtext.Framework;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;
using Subtext.Framework.Syndication;

namespace Subtext.Common.Syndication
{
	/// <summary>
	/// Writes a CommentRSS feed to the underlying stream.  Inherits from 
	/// <see cref="BaseRssWriter"/> which ultimately inherits from <see cref="XmlTextWriter"/>.
	/// </summary>
	public class CommentRssWriter : Subtext.Framework.Syndication.BaseRssWriter
	{
		private Entry CommentEntry = null;

		/// <summary>
		/// Creates a new <see cref="CommentRssWriter"/> instance.
		/// </summary>
		/// <param name="commentEntries">Ec.</param>
		/// <param name="entry">Ce.</param>
        public CommentRssWriter(IList<Entry> commentEntries, Entry entry) : base(NullValue.NullDateTime, false)
		{
			if(commentEntries == null)
				throw new ArgumentNullException("commentEntries", "Cannot generate a comment rss feed for a null collection of entries.");
			
			if(entry == null)
				throw new ArgumentNullException("entry", "Comment RSS feed must be associated to an entry.");
			
			this.Entries = commentEntries;
			this.CommentEntry = entry;
			this.UseAggBugs = false;
			this.AllowComments = false;
		}

		/// <summary>
		/// Writes the RSS channel to the underlying stream.
		/// </summary>
		protected override void WriteChannel()
		{
			RssImageElement image = new RssImageElement(new Uri(info.HostFullyQualifiedUrl, "RSS2Image.gif"), CommentEntry.Title, CommentEntry.FullyQualifiedUrl, 77, 60, null);
			this.BuildChannel(CommentEntry.Title, CommentEntry.FullyQualifiedUrl.ToString(), CommentEntry.Email, CommentEntry.HasDescription ? CommentEntry.Description : CommentEntry.Body, info.Language, info.Author, Config.CurrentBlog.LicenseUrl, image);
		}
	}
}

