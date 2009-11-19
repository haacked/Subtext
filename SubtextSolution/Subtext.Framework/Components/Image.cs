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
using System.Globalization;
using System.IO;
using System.Xml.Serialization;
using Subtext.Framework.Properties;

namespace Subtext.Framework.Components
{
    /// <summary>
    /// Summary description for Link.
    /// </summary>
    [Serializable]
    public class Image
    {
        private string _localDirectoryPath;
        public int BlogId { get; set; }

        public Blog Blog { get; set; }

        [XmlAttribute]
        public int ImageID { get; set; }

        public int CategoryID { get; set; }

        //TODO: This is pure laziness and a band-aid for 
        //      aggregate blogs. Will fix later.
        public string CategoryTitle { get; set; }

        public bool IsActive { get; set; }

        public string FileName { get; set; }

        /// <summary>
        /// Gets the filepath on the local server.
        /// </summary>
        public virtual string FilePath
        {
            get { return Path.Combine(LocalDirectoryPath, FileName); }
        }

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
                if(_localDirectoryPath == null)
                {
                    throw new InvalidOperationException(Resources.InvalidOperation_LocalDirectoryPathNotSet);
                }

                return _localDirectoryPath;
            }
            set
            {
                if(value != null)
                {
                    value = Path.GetFullPath(value);
                }
                _localDirectoryPath = value;
            }
        }

        public string Title { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }

        public string Url { get; set; }

        public string OriginalFile
        {
            get { return string.Format("o_{0}", FileName); }
        }

        public string ThumbNailFile
        {
            get { return string.Format("t_{0}", FileName); }
        }

        public string ResizedFile
        {
            get { return string.Format("r_{0}", FileName); }
        }

        public string OriginalFilePath
        {
            get { return Path.Combine(LocalDirectoryPath, OriginalFile); }
        }

        public string ThumbNailFilePath
        {
            get { return Path.Combine(LocalDirectoryPath, ThumbNailFile); }
        }

        public string ResizedFilePath
        {
            get { return Path.Combine(LocalDirectoryPath, ResizedFile); }
        }
    }
}