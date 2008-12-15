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
		public int BlogId
		{
			get;
			set;
		}

        public BlogInfo Blog
        {
            get;
            set;
        }

		[XmlAttribute]
		public int ImageID
		{
			get;
			set;
		}

		public int CategoryID
		{
			get;
			set;
		}

        //TODO: This is pure laziness and a band-aid for 
        //      aggregate blogs. Will fix later.
        public string CategoryTitle { 
            get; 
            set; 
        }

		public bool IsActive
		{
			get;
			set;
		}

        public string FileName
        {
            get;
            set;
        }

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

		public string Title
		{
			get;
			set;
		}

		public int Width
		{
			get;
			set;
		}
	
		public int Height
		{
			get;
			set;
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