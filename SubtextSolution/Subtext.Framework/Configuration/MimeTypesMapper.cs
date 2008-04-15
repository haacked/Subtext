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

        public static MimeTypesMapper Mappings
        {
            get { return _mappings; }
        }

        private int _count=0;

        public int Count
        {
            get { return _count; }
        }

        public MimeTypesMapper(NameValueCollection config)
        {
            if (config == null)
                throw new ArgumentNullException("config");
            _config = config;
            _count = _config.Keys.Count;
        }

        public string GetMimeType(string ext)
        {
            if(ext==null)
                throw new ArgumentNullException("ext","The file extension cannot be null.");
            if (_config[ext] != null)
                return _config[ext];
            return null;
        }

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
