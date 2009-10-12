#region Disclaimer/Info

///////////////////////////////////////////////////////////////////////////////////////////////////
// Subtext WebLog
// 
// Subtext is an open source weblog system that is a fork of the .TEXT
// weblog system.
//
// For updated news and information please visit http://subtextproject.com/
// Subtext is hosted at Google Code at http://code.google.com/p/subtext/
// The development mailing list is at subtext-devs@lists.sourceforge.net 
//
// This project is licensed under the BSD license.  See the License.txt file for more information.
///////////////////////////////////////////////////////////////////////////////////////////////////

#endregion

using System;
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

            // The {0} represents a potential virtual directory
            const string urlPatternFormat = "{0}/(?<app>.*?)/";

            //Remove any / from App.
            string cleanApp = "/" + HttpHelper.StripSurroundingSlashes(applicationPath);
            if(cleanApp == "/")
            {
                cleanApp = string.Empty;
            }
            string appRegex = Regex.Escape(cleanApp);

            string urlRegexPattern = string.Format(CultureInfo.InvariantCulture, urlPatternFormat, appRegex);

            var urlRegex = new Regex(urlRegexPattern, RegexOptions.IgnoreCase | RegexOptions.Compiled);
            Match match = urlRegex.Match(rawUrl);
            if(match.Success)
            {
                return match.Groups["app"].Value;
            }
            return string.Empty;
        }
    }
}