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
using System.Diagnostics;
using System.Globalization;
using System.Text.RegularExpressions;
using Subtext.Framework.Web;

namespace Subtext.Framework.Format
{
    /// <summary>
    /// Default Implemenation of UrlFormats
    /// </summary>
    public class UrlFormats
    {
        /// <summary>
        /// Parses out the subfolder of the blog from the requested URL.  It 
        /// simply searches for the first "folder" after the host and 
        /// Request.ApplicationPath.
        /// </summary>
        /// <remarks>
        /// <p>
        /// For example, if a blog is hosted at the virtual directory http://localhost/Subtext.Web/ and 
        /// request is made for http://localhost/Subtext.Web/, the subfolder name is "" (empty string). 
        /// Howver, a request for http://localhost/Subtext.Web/MyBlog/ would return "MyBlog" as the 
        /// subfolder.
        /// </p>
        /// <p>
        /// Likewise, if a blog is hosted at http://localhost/, a request for http://localhost/MyBlog/ 
        /// would return "MyBlog" as the subfolder.
        /// </p>
        /// </remarks>
        /// <param name="rawUrl">The raw url.</param>
        /// <param name="applicationPath">The virtual application name as found in the Request.ApplicationName property.</param>
        /// <returns></returns>
        public static string GetBlogSubfolderFromRequest(string rawUrl, string applicationPath)
        {
            if(rawUrl == null)
            {
                throw new ArgumentNullException("rawUrl");
            }

            if(applicationPath == null)
            {
                throw new ArgumentNullException("applicationPath");
            }

            Debug.Assert(applicationPath.StartsWith("/"), "ApplicationPaths always start with a slash");

            if(!rawUrl.StartsWith(applicationPath, StringComparison.OrdinalIgnoreCase))
            {
                return string.Empty;
            }
            int appPathLength = applicationPath.Length;
            int startIndex = appPathLength; 
            if(appPathLength > 1)
            {
                startIndex++;
            } 
            if(startIndex > rawUrl.Length)
            {
                return string.Empty;
            }
            int endIndex = rawUrl.IndexOf('/', startIndex);
            if(endIndex < 0)
            {
                string path = rawUrl.Substring(startIndex);
                // Don't want to return default.aspx etc.
                return path.Contains(".") ? string.Empty : path;
            }
            return rawUrl.Substring(startIndex, endIndex - startIndex);
        }
    }
}