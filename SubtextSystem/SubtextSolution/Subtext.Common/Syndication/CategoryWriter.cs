using System;
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

		//TODO: implement lastViewedId
		public CategoryWriter(EntryCollection ec, LinkCategory lc, string Url) : base(ec, int.MinValue)
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
				this.BuildChannel(Category.Title,  Url,  info.Author,  Category.HasDescription ? Category.Description : Category.Title, info.Language, info.Author, Subtext.Framework.Configuration.Config.CurrentBlog.LicenseUrl);
			}
		}


		


	}
}
