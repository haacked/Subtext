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
using System.Web;
using Subtext.Framework.Services;

namespace Subtext.Framework.Configuration
{
    /// <summary>
    /// Summary description for SkinConfig.
    /// </summary>
    [Serializable]
    public class SkinConfig
    {
        public static readonly SkinConfig DefaultSkin = CreateDefaultSkin();

        /// <summary>
        /// This is the skin template folder. Note that multiple "Skins" can 
        /// share the same template folder. The template folder contains the 
        /// *.ascx files for the skins.
        /// </summary>
        public string TemplateFolder { get; set; }

        /// <summary>
        /// Gets or sets the skin's primary CSS file, if any.  
        /// Some Skins have multiple flavors based on different CSS files.  
        /// For example, Redbook, Bluebook, and Greenbook are all variations 
        /// of the skin Redbook.  They vary by the skin css file.
        /// </summary>
        /// <value>The skin CSS file.</value>
        public string SkinStyleSheet { get; set; }

        /// <summary>
        /// This is CSS text that is entered within the admin section.
        /// </summary>
        public string CustomCssText { get; set; }

        /// <summary>
        /// Returns true if the skin has a skin specific css file 
        /// that is applied after style.css (there is one style.css 
        /// per template folder).
        /// </summary>
        public bool HasStyleSheet
        {
            get { return SkinStyleSheet != null && SkinStyleSheet.Trim().Length > 0; }
        }

        /// <summary>
        /// Returns true if the user specified some custom CSS in the admin section.
        /// </summary>
        public bool HasCustomCssText
        {
            get { return CustomCssText != null && CustomCssText.Trim().Length > 0; }
        }

        /// <summary>
        /// A lookup key for a skin.
        /// </summary>
        public string SkinKey
        {
            get
            {
                if(HasStyleSheet)
                {
                    return string.Format("{0}-{1}", TemplateFolder, SkinStyleSheet);
                }
                return TemplateFolder;
            }
        }

        /// <summary>
        /// Creates the default skin to be used if none is specified.
        /// </summary>
        /// <returns></returns>
        static SkinConfig CreateDefaultSkin()
        {
            var defaultSkin = new SkinConfig {TemplateFolder = "RedBook", SkinStyleSheet = "Blue.css"};
            return defaultSkin;
        }

        /// <summary>
        /// Returns the current skin for the current context.
        /// </summary>
        /// <returns></returns>
        public static SkinConfig GetCurrentSkin(Blog blog, HttpContextBase context)
        {
            var service = new BrowserDetectionService();
            BrowserInfo capabilities = service.DetectBrowserCapabilities(context.Request);

            bool isMobile = capabilities.Mobile;

            SkinConfig skin;
            if(isMobile)
            {
                skin = blog.MobileSkin;
                if(skin.TemplateFolder != null)
                {
                    return skin;
                }
            }

            skin = blog.Skin;

            if(skin.TemplateFolder == null)
            {
                skin = DefaultSkin;
            }
            return skin;
        }
    }
}