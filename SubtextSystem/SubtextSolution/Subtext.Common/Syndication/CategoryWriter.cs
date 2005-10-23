using System;
using Subtext.Framework;
using Subtext.Framework.Components;

namespace Subtext.Common.Syndication
{
	/// <summary>
	/// Summary description for CategoryWriter.
	/// </summary>
	public class CategoryWriter : RssWriter
	{
		private LinkCategory _lc;
		public LinkCategory Category
		{
			get {return this._lc;}
			set {this._lc = value;}
		}

		private string _url;
		public string Url
		{
			get {return this._url;}
			set {this._url = value;}
		}

		//TODO: implement dateLastViewedFeedItemPublished
		//TODO: Implement useDeltaEncoding
		/// <summary>
		/// Creates a new <see cref="CategoryWriter"/> instance.
		/// </summary>
		/// <param name="ec">Ec.</param>
		/// <param name="lc">Lc.</param>
		/// <param name="Url">URL.</param>
		public CategoryWriter(EntryCollection ec, LinkCategory lc, string Url) : base(ec, NullValue.NullDateTime, false)
		{
			this.Category = lc;
			this.Url = Url;
		}

		protected override void WriteChannel()
		{
			if(this.Category == null)
			{
				base.WriteChannel();
			}
			else
			{
				this.BuildChannel(Category.Title,  Url,  info.Email,  Category.HasDescription ? Category.Description : Category.Title, info.Language, info.Author, Subtext.Framework.Configuration.Config.CurrentBlog.LicenseUrl);
			}
		}


		


	}
}
