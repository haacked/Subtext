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
using System.Xml.Serialization;

namespace Subtext.Framework.Components
{
    /// <summary>
    /// Summary description for BlogGroup.
    /// </summary>
    [Serializable]
    public class BlogGroup
    {
        /// <summary>
        /// Creates a new <see cref="BlogGroup"/> instance.
        /// </summary>
        public BlogGroup()
        {
        }

        /// <summary>
        /// Creates a new <see cref="BlogGroup"/> instance.
        /// </summary>
        /// <param name="id">Blog Group ID.</param>
        /// <param name="title">Title.</param>
        public BlogGroup(int id, string title)
        {
            Title = title;
            Id = id;
        }

        public string Title { get; set; }

        public bool HasDescription
        {
            get { return Description != null && Description.Trim().Length > 0; }
        }

        public string Description { get; set; }


        public int DisplayOrder { get; set; }

        [XmlAttribute("BlogGroupID")]
        public int Id { get; set; }

        public bool IsActive { get; set; }

        public ICollection<Blog> Blogs { get; set; }
    }
}