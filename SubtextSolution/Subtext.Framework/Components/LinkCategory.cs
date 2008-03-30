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
using System.Xml.Serialization;

namespace Subtext.Framework.Components
{
	/// <summary>
	/// Summary description for LinkCategory.
	/// </summary>
	[Serializable]
	public class LinkCategory
	{
		/// <summary>
		/// Creates a new <see cref="LinkCategory"/> instance.
		/// </summary>
		public LinkCategory()
		{}

		/// <summary>
		/// Creates a new <see cref="LinkCategory"/> instance.
		/// </summary>
		/// <param name="catID">Cat ID.</param>
		/// <param name="title">Title.</param>
		public LinkCategory(int catID, string title)
		{
			Title = title;
			this.Id = catID;
		}

		private int _blogID;
		public int BlogId
		{
			get {return this._blogID;}
			set {this._blogID = value;}
		}

		private string _sorttext;
		public string SortText
		{
			get
			{
				if(_sorttext == null)
				{
					_sorttext = Title;
				}
				return _sorttext;
			}
			set{_sorttext = value;}
		}

		private string _title;
		public string Title
		{
			get{return _title;}
			set{_title = value;}
		}

		public bool HasDescription
		{
			get
			{
				return Description != null && Description.Trim().Length > 0;
			}
		}

		private string _description;
		public string Description
		{
			get{return _description;}
			set{_description = value;}
		}

		
		private CategoryType _categoryType = CategoryType.LinkCollection;
		public CategoryType CategoryType
		{
			get{return _categoryType;}
			set{_categoryType = value;}
		}

		private int id;
		[XmlAttribute("CategoryID")]
		public int Id
		{
			get{return this.id;}
			set{this.id = value;}
		}

		private bool _isActive;
		public bool IsActive
		{
			get{return _isActive;}
			set{_isActive = value;}
		}

		private ICollection<Link> _links;
		public ICollection<Link> Links
		{
			get{return _links;}
			set{_links = value;}
		}

        private ICollection<Image> _images;
        public ICollection<Image> Images
		{
			get{return _images;}
			set{_images = value;}
		}

		public bool HasLinks
		{
			get
			{
				return Links != null;
			}
		}

		public bool HasImages
		{
			get
			{
				return Images != null;
			}
		}

	}
}

