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
		{
            CategoryType = CategoryType.LinkCollection;
        }

		/// <summary>
		/// Creates a new <see cref="LinkCategory"/> instance.
		/// </summary>
		/// <param name="catID">Cat ID.</param>
		/// <param name="title">Title.</param>
		public LinkCategory(int catID, string title) : this()
		{
			Title = title;
			this.Id = catID;
		}

		public int BlogId
		{
			get;
			set;
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

		public string Title
		{
			get;
			set;
		}

		public bool HasDescription
		{
			get
			{
				return Description != null && Description.Trim().Length > 0;
			}
		}

		public string Description
		{
			get;
			set;
		}

		
		public CategoryType CategoryType
		{
			get;
			set;
		}

		[XmlAttribute("CategoryID")]
		public int Id
		{
			get;
			set;
		}

		public bool IsActive
		{
			get;
			set;
		}

		public ICollection<Link> Links
		{
			get {
                _links = _links ?? new List<Link>();
                return _links;
            }
			
		}
        ICollection<Link> _links;

        public ICollection<Image> Images
		{
			get;
			set;
		}

		public bool HasLinks
		{
			get
			{
				return Links.Count > 0;
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

