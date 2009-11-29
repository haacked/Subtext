#region Disclaimer/Info

///////////////////////////////////////////////////////////////////////////////////////////////////
// Subtext WebLog
// 
// Subtext is an open source weblog system that is a fork of the .TEXT
// weblog system.
//
// For updated news and information please visit http://subtextproject.com/
// Subtext is hosted at Google Code at http://code.google.com/p/subtext/
// The development mailing list is at subtext@googlegroups.com 
//
// This project is licensed under the BSD license.  See the License.txt file for more information.
///////////////////////////////////////////////////////////////////////////////////////////////////

#endregion

using System;
using System.Collections.Generic;

namespace Subtext.Framework.Components
{
    /// <summary>
    /// Summary description for LinkCategory.
    /// </summary>
    [Serializable]
    public class LinkCategory : Category
    {
        ICollection<Link> _links;

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
        /// <param name="categoryId"></param>
        /// <param name="title">Title.</param>
        public LinkCategory(int categoryId, string title) : this()
        {
            Title = title;
            Id = categoryId;
        }

        public bool HasDescription
        {
            get { return !String.IsNullOrEmpty(Description); }
        }

        public string Description { get; set; }


        public CategoryType CategoryType { get; set; }

        public bool IsActive { get; set; }

        public ICollection<Link> Links
        {
            get
            {
                _links = _links ?? new List<Link>();
                return _links;
            }
        }

        public ICollection<Image> Images { get; set; }

        public bool HasLinks
        {
            get { return Links.Count > 0; }
        }

        public bool HasImages
        {
            get { return Images != null; }
        }
    }
}