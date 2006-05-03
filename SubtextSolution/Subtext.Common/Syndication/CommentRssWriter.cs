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
using System.Xml;
using Subtext.Framework;
using Subtext.Framework.Components;
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
		/// <param name="ec">Ec.</param>
		/// <param name="ce">Ce.</param>
		public CommentRssWriter(EntryCollection ec, Entry ce) : base(NullValue.NullDateTime, false)
		{
			this.Entries = ec;
			this.CommentEntry = ce;
			this.UseAggBugs = false;
			this.AllowComments = false;
		}

		/// <summary>
		/// Writes the RSS channel to the underlying stream.
		/// </summary>
		protected override void WriteChannel()
		{
			this.BuildChannel(CommentEntry.Title, CommentEntry.Link, CommentEntry.Email, CommentEntry.HasDescription ? CommentEntry.Description : CommentEntry.Body,info.Language, info.Author, Subtext.Framework.Configuration.Config.CurrentBlog.LicenseUrl);
		}
	}
}

