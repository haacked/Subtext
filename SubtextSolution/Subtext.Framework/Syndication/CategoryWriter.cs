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
using Subtext.Framework;
using Subtext.Framework.Components;
using System.IO;

namespace Subtext.Framework.Syndication
{
	/// <summary>
	/// Summary description for CategoryWriter.
	/// </summary>
	public class CategoryWriter : RssWriter
	{
		public LinkCategory Category
		{
			get;
			set;
		}

		public string Url
		{
			get;
			set;
		}

		//TODO: implement dateLastViewedFeedItemPublished
		//TODO: Implement useDeltaEncoding
		/// <summary>
		/// Creates a new <see cref="CategoryWriter"/> instance.
		/// </summary>
		/// <param name="ec">Ec.</param>
		/// <param name="lc">Lc.</param>
		/// <param name="url">URL.</param>
		public CategoryWriter(TextWriter writer, ICollection<Entry> ec, LinkCategory lc, string url, ISubtextContext context) : base(writer, ec, NullValue.NullDateTime, false, context)
		{
			this.Category = lc;
			this.Url = url;
		}

		protected override void WriteChannel()
		{
			if(this.Category == null)
			{
				base.WriteChannel();
			}
			else
			{
				this.BuildChannel(Category.Title,  Url,  Blog.Email,  Category.HasDescription ? Category.Description : Category.Title, Blog.Language, Blog.Author, Subtext.Framework.Configuration.Config.CurrentBlog.LicenseUrl);
			}
		}
	}
}
