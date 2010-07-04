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
using System.Collections.Specialized;
using System.Configuration;
using System.Xml.Serialization;

namespace Subtext.Framework.Configuration
{
    /// <summary>
    /// Contains various configuration settings stored in the 
    /// web.config file.
    /// </summary>
    [Serializable]
    public class BlogConfigurationSettings
    {
        private Tracking _tracking;
        private NameValueCollection _allowedHtmlTags;

        public BlogConfigurationSettings()
        {
            QueuedThreads = 5;
            ItemCount = 15;
            CategoryListPostCount = 10;
            ServerTimeZone = -2037797565; //PST
            GalleryImageMaxWidth = 640;
            GalleryImageMaxHeight = 480;
            GalleryImageThumbnailHeight = 120;
            GalleryImageThumbnailWidth = 120;
        }

        public Tracking Tracking
        {
            get
            {
                _tracking = _tracking ?? new Tracking();
                return _tracking;
            }
            set { _tracking = value; }
        }

        public bool UseWww { get; set; }

        public int QueuedThreads { get; set; }

        public bool AllowServiceAccess { get; set; }

        public bool AllowScriptsInPosts { get; set; }

        public bool UseHashedPasswords { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether or not to allow images.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if [allow images]; otherwise, <c>false</c>.
        /// </value>
        public bool AllowImages { get; set; }

        /// <summary>
        /// Gets or sets the default number of items to display 
        /// for syndication feeds.
        /// </summary>
        /// <value></value>
        public int ItemCount { get; set; }

        /// <summary>
        /// Gets or sets the number of posts to display 
        /// on the category list pages.
        /// </summary>
        /// <value></value>
        public int CategoryListPostCount { get; set; }

        /// <summary>
        /// Gets or sets the server time zone.
        /// </summary>
        /// <value></value>
        public int ServerTimeZone { get; set; }

        public int GalleryImageMaxWidth { get; set; }
        public int GalleryImageMaxHeight { get; set; }
        public int GalleryImageThumbnailWidth { get; set; }
        public int GalleryImageThumbnailHeight { get; set; }

        /// <summary>
        /// Gets a value indicating whether invisible captcha enabled.  This is 
        /// configured within the "InvisibleCaptchaEnabled" app setting. It is not 
        /// set per blog, but system-wide. This gives hosters a way to opt-out of 
        /// this feature in case it ends up being problematci.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if [invisible captcha enabled]; otherwise, <c>false</c>.
        /// </value>
        public bool InvisibleCaptchaEnabled
        {
            get
            {
                if(String.IsNullOrEmpty(ConfigurationManager.AppSettings["InvisibleCaptchaEnabled"]))
                {
                    return true;
                }

                bool enabled;
                if(bool.TryParse(ConfigurationManager.AppSettings["InvisibleCaptchaEnabled"], out enabled))
                {
                    return enabled;
                }
                return true;
            }
        }

        /// <summary>
        /// Returns a <see cref="NameValueCollection"/> containing the allowed 
        /// HTML tags within a user comment.  The value contains a comma 
        /// separated list of allowed attributes.
        /// </summary>
        /// <value>The allowed HTML tags.</value>
        [XmlIgnore]
        public NameValueCollection AllowedHtmlTags
        {
            get
            {
                if(_allowedHtmlTags == null)
                {
                    _allowedHtmlTags = ((NameValueCollection)(ConfigurationManager.GetSection("AllowableCommentHtml")));
                }
                return _allowedHtmlTags;
            }
        }
    }
}