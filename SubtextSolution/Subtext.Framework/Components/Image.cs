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
using System.IO;
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
		public int BlogId
		{
			get {return this._blogID;}
			set {this._blogID = value;}
		}

		private int _imageId;
		[XmlAttribute]
		public  virtual int ImageID
		{
			get{return _imageId;}
			set{_imageId = value;}
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

		public  virtual string FileName
		{
			get{return _file;}
			set{_file= value;}
		}
		private string _file;

		/// <summary>
		/// Gets the filepath on the local server.
		/// </summary>
		public virtual string FilePath
		{
			get
			{
				return Path.Combine(LocalDirectoryPath, FileName);
			}
		}

		private string localDirectoryPath;
		/// <summary>
		/// The directory on the local server where the image will be saved.
		/// </summary>
		/// <remarks>
		/// Assumes the specified path is a directory path!
		/// </remarks>
        public virtual string LocalDirectoryPath
        {
            get
            {
                if (this.localDirectoryPath == null)
                    throw new InvalidOperationException("Image.LocalFilePath has not been set yet.");

                return this.localDirectoryPath;
            }
            set
            {
				if (value != null)
					value = Path.GetFullPath(value);
				this.localDirectoryPath = value;
            }
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
				return "o_" + FileName;
			}
		}

		public string ThumbNailFile
		{
			get
			{
				return "t_" + FileName;
			}
		}

		public string ResizedFile
		{
			get
			{
				return "r_" + FileName;
			}
		}

		public string OriginalFilePath
		{
			get
			{
				return Path.Combine(LocalDirectoryPath, OriginalFile);
			}
		}

		public string ThumbNailFilePath
		{
			get
			{
				return Path.Combine(LocalDirectoryPath, ThumbNailFile);
			}
		}

		public string ResizedFilePath
		{
			get
			{
				return Path.Combine(LocalDirectoryPath, ResizedFile);
			}
		}
	}
}