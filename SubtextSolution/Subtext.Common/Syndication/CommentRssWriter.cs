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
using Subtext.Framework;
using Subtext.Framework.Components;

namespace Subtext.Common.Syndication
{
	//Same code as RssWriter. Need to refactor these two to use 1  generic base.

	/// <summary>
	/// Generates CommentRssWriter 
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

		protected override void WriteChannel()
		{
			this.BuildChannel(CommentEntry.Title, CommentEntry.Link, CommentEntry.Email, CommentEntry.HasDescription ? CommentEntry.Description : CommentEntry.Body,info.Language, info.Author, Subtext.Framework.Configuration.Config.CurrentBlog.LicenseUrl);
		}

	}
}

