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
	/// Summary description for BlogGroup.
	/// </summary>
	[Serializable]
	public class BlogGroup
	{
		/// <summary>
		/// Creates a new <see cref="BlogGroup"/> instance.
		/// </summary>
		public BlogGroup()
		{}

		/// <summary>
		/// Creates a new <see cref="BlogGroup"/> instance.
		/// </summary>
		/// <param name="id">Blog Group ID.</param>
		/// <param name="title">Title.</param>
        public BlogGroup(int id, string title)
		{
			Title = title;
            this.Id = id;
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


        private int _displayOrder;
		public int DisplayOrder
		{
			get{return _displayOrder;}
			set{_displayOrder = value;}
		}

		private int id;
        [XmlAttribute("BlogGroupID")]
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

		private ICollection<BlogInfo> _blogs;
        public ICollection<BlogInfo> Blogs
		{
            get { return _blogs; }
            set { _blogs = value; }
		}

	}
}

