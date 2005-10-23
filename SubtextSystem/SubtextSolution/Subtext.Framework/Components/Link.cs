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
	/// Summary description for Link.
	/// </summary>
	[Serializable]
	public class Link
	{
		private int _blogID;
		public int BlogID
		{
			get {return this._blogID;}
			set {this._blogID = value;}
		}

		private int _linkid;
		[XmlAttribute]
		public  virtual int LinkID
		{
			get{return _linkid;}
			set{_linkid = value;}
		}

		private int _postID = NullValue.NullInt32;
		public  virtual int PostID
		{
			get{return _postID;}
			set{_postID = value;}
		}

		private int _catID;
		public  virtual int CategoryID
		{
			get{return _catID;}
			set{_catID = value;}
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

