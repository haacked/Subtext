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

using System.Configuration;

namespace Subtext.Web.UI
{
    /// <summary>
    /// Used to obtain configurable text displayed on the UI.  
    /// Uses application settings (bleh!).
    /// </summary>
    /// <remarks>
    /// This text ought to be configurable per-blog.
    /// </remarks>
    public static class UIText
    {
        //TODO: Refactor this to use blog settings, not app settings.
        /// <summary>
        /// Gets the titel for the post categories.
        /// </summary>
        /// <value>The post collection.</value>
        public static string PostCollection
        {
            get { return GetSafeConfig("PostCollection", "Post Categories"); }
        }

        /// <summary>
        /// Gets the title for the article categories.
        /// </summary>
        /// <value>The article collection.</value>
        public static string ArticleCollection
        {
            get { return GetSafeConfig("ArticleCollection", "Article Categories"); }
        }

        /// <summary>
        /// Gets the title for the image galleries.
        /// </summary>
        /// <value>The image collection.</value>
        public static string ImageCollection
        {
            get { return GetSafeConfig("ImageCollection", "Image Galleries"); }
        }

        /// <summary>
        /// Gets the title for the Archives links.
        /// </summary>
        /// <value>The archives.</value>
        public static string Archives
        {
            get { return GetSafeConfig("Archives", "Archives"); }
        }

        private static string GetSafeConfig(string name, string defaultValue)
        {
            string text = ConfigurationManager.AppSettings[name];
            if(text == null)
            {
                return defaultValue;
            }
            return text;
        }
    }
}