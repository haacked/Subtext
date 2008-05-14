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
using System.Collections.Specialized;
using System.Configuration;
using System.Globalization;
using System.IO;
using log4net;
using Subtext.Framework.Logging;

namespace Subtext.Framework.Configuration
{
    public class MimeTypesMapper
    {
        NameValueCollection _config = null;
        static MimeTypesMapper _mappings = new MimeTypesMapper((NameValueCollection)ConfigurationManager.GetSection("EnclosureMimetypes"));

        /// <summary>
        /// Returns the MimeTypesMapper instance.
        /// </summary>
        public static MimeTypesMapper Mappings
        {
            get { return _mappings; }
        }

        private int _count=0;

        public int Count
        {
            get { return _count; }
        }

        public NameValueCollection List
        {
            get { return _config; }
        }

        public MimeTypesMapper(NameValueCollection config)
        {
            if (config == null)
                throw new ArgumentNullException("config");
            _config = config;
            _count = _config.Keys.Count;
        }

        /// <summary>
        /// Returns the mimetype that corresponds to a file extension.
        /// </summary>
        /// <param name="ext">Extension of a file.</param>
        /// <returns>The MimeType</returns>
        public string GetMimeType(string ext)
        {
            if(ext==null)
                throw new ArgumentNullException("ext","The file extension cannot be null.");
            if (_config[ext] != null)
                return _config[ext];
            return null;
        }

        /// <summary>
        /// Detect the mimetype of the url of a file.
        /// </summary>
        /// <param name="url">Url of the file</param>
        /// <returns>The MimeType.</returns>
        public string ParseUrl(string url)
        {
            if (url == null)
                throw new ArgumentNullException("url", "The url cannot be null.");
            Uri uri;

            if (!Uri.TryCreate(url,UriKind.Absolute,out uri))
                throw new ArgumentException("Url not valid.","url");

            string path = uri.GetComponents(UriComponents.Path,UriFormat.SafeUnescaped);

            string ext = Path.GetExtension(path);

            return GetMimeType(ext);
            
        }
    }
}
