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
using System.Xml.Serialization;

namespace Subtext.Framework.Components
{
	/// <summary>
	/// Summary description for Link.
	/// </summary>
	[Serializable]
	public class Link
	{
		private int _blogID;
		public int BlogId
		{
			get {return this._blogID;}
			set {this._blogID = value;}
		}

		private int _linkid;
		[XmlAttribute("LinkID")]
		public  virtual int Id
		{
			get{return _linkid;}
			set{_linkid = value;}
		}

		private int _postId = NullValue.NullInt32;
		public  virtual int PostID
		{
			get{return _postId;}
			set{_postId = value;}
		}

		private int _catId;
		public  virtual int CategoryID
		{
			get{return _catId;}
			set{_catId = value;}
		}

		private bool _isActive;
		public  virtual bool IsActive
		{
			get{return _isActive;}
			set{_isActive = value;}
		}

		private bool _newtarget;
		public  virtual bool NewWindow
		{
			get{return _newtarget;}
			set{_newtarget = value;}
		}

		private string _url;
		public  virtual string Url
		{
			get{return _url;}
			set{_url= value;}
		}

		private string _rss;
		public  virtual string Rss
		{
			get{return _rss;}
			set{_rss= value;}
		}

		private string _title;
		public  virtual string Title
		{
			get{return _title;}
			set{_title= value;}
		}



		public bool HasRss
		{
			get{return (Rss != null && Rss.Trim().Length > 0); }
		}
	}
}

