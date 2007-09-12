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

using System.Collections.Generic;
using Subtext.Framework;
using Subtext.Framework.Components;

namespace Subtext.Framework.Syndication
{
	/// <summary>
	/// Summary description for CategoryWriter.
	/// </summary>
	public class CategoryWriter : RssWriter
	{
		private LinkCategory _lc;
		public LinkCategory Category
		{
			get {return _lc;}
			set {_lc = value;}
		}

		private string _url;
		public string Url
		{
			get {return _url;}
			set {_url = value;}
		}

		//TODO: implement dateLastViewedFeedItemPublished
		//TODO: Implement useDeltaEncoding
		/// <summary>
		/// Creates a new <see cref="CategoryWriter"/> instance.
		/// </summary>
		/// <param name="ec">Ec.</param>
		/// <param name="lc">Lc.</param>
		/// <param name="url">URL.</param>
		public CategoryWriter(IList<Entry> ec, LinkCategory lc, string url) : base(ec, NullValue.NullDateTime, false)
		{
			Category = lc;
			Url = url;
		}

		protected override void WriteChannel()
		{
			if(Category == null)
			{
				base.WriteChannel();
			}
			else
			{
				BuildChannel(Category.Title,  Url,  info.Owner.Email,  Category.HasDescription ? Category.Description : Category.Title, info.Language, info.Author, Subtext.Framework.Configuration.Config.CurrentBlog.LicenseUrl);
			}
		}
	}
}
