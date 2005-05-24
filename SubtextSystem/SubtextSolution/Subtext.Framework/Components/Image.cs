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
	public class Image
	{
		private int _blogID;
		public int BlogID
		{
			get {return this._blogID;}
			set {this._blogID = value;}
		}

		private int _imageID;
		[XmlAttribute]
		public  virtual int ImageID
		{
			get{return _imageID;}
			set{_imageID = value;}
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

		private string _file;
		public  virtual string File
		{
			get{return _file;}
			set{_file= value;}
		}

		
		public  virtual string Path
		{
			get{return LocalFilePath + File;}
		
		}

		private string _localfile;
		public  virtual string LocalFilePath
		{
			get
			{
				if(_localfile == null)
				{
					throw new Exception("Image.LocalFilePath has not been set yet.");
				}
				return _localfile;
			}
			set
			{
				if(value == null)
				{
					_localfile = null;
				}
					_localfile= value.Replace("/","\\");}
		}

		private string _title;
		public  virtual string Title
		{
			get{return _title;}
			set{_title= value;}
		}

		private int _width;
		public  virtual int Width
		{
			get{return _width;}
			set{_width = value;}
		}
	
		private int _height;
		public  virtual int Height
		{
			get{return _height;}
			set{_height = value;}
		}

		public string OriginalFile
		{
			get
			{
				return "o_" + File;
			}
		}

		public string ThumbNailFile
		{
			get
			{
				return "t_" + File;
			}
		}

		public string ResizedFile
		{
			get
			{
				return "r_" + File;
			}
		}

		public string OriginalFilePath
		{
			get
			{
				return LocalFilePath + OriginalFile;
			}
		}

		public string ThumbNailFilePath
		{
			get
			{
				return LocalFilePath + ThumbNailFile;
			}
		}

		public string ResizedFilePath
		{
			get
			{
				return LocalFilePath + ResizedFile;
			}
		}

	}
}

