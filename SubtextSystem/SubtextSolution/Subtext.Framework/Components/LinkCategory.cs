#region Disclaimer/Info
///////////////////////////////////////////////////////////////////////////////////////////////////
// .Text WebLog
// 
// .Text is an open source weblog system started by Scott Watermasysk. 
// Blog: http://ScottWater.com/blog 
// RSS: http://scottwater.com/blog/rss.aspx
// Email: Dottext@ScottWater.com
//
// For updated news and information please visit http://scottwater.com/dottext and subscribe to 
// the Rss feed @ http://scottwater.com/dottext/rss.aspx
//
// On its release (on or about August 1, 2003) this application is licensed under the BSD. However, I reserve the 
// right to change or modify this at any time. The most recent and up to date license can always be fount at:
// http://ScottWater.com/License.txt
// 
// Please direct all code related questions to:
// GotDotNet Workspace: http://www.gotdotnet.com/Community/Workspaces/workspace.aspx?id=e99fccb3-1a8c-42b5-90ee-348f6b77c407
// Yahoo Group http://groups.yahoo.com/group/DotText/
// 
///////////////////////////////////////////////////////////////////////////////////////////////////
#endregion

using System;
using System.Xml.Serialization;

namespace Subtext.Framework.Components
{
	/// <summary>
	/// Summary description for LinkCategory.
	/// </summary>
	[Serializable]
	public class LinkCategory : IBlogIdentifier
	{
		public LinkCategory()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		private int _blogID;
		public int BlogID
		{
			get {return this._blogID;}
			set {this._blogID = value;}
		}

		public LinkCategory(int catID, string title)
		{
			Title = title;
			CategoryID = catID;
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

		private int _catID;
		[XmlAttribute]
		public int CategoryID
		{
			get{return _catID;}
			set{_catID = value;}
		}

		private bool _isActive;
		public bool IsActive
		{
			get{return _isActive;}
			set{_isActive = value;}
		}

		private LinkCollection _links;
		public LinkCollection Links
		{
			get{return _links;}
			set{_links = value;}
		}

		private ImageCollection _images;
		public ImageCollection Images
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

