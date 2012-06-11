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
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Web;
using Subtext.Framework.Web;

namespace Subtext.Framework.UI.Skinning
{
    /// <summary>
    /// Provides rendering facilities for stylesheet elements in the head element of the page
    /// </summary>
    public class StyleSheetElementCollectionRenderer
    {
        private readonly SkinEngine _skinEngine;

        public StyleSheetElementCollectionRenderer(SkinEngine skinEngine)
        {
            _skinEngine = skinEngine;
        }

        private IDictionary<string, SkinTemplate> Templates
        {
            get
            {
                var templates = _templates;
                if (templates == null)
                {
                    templates = _skinEngine.GetSkinTemplates(false /* mobile */);
                    _templates = templates;
                }
                return templates;
            }
        }
        IDictionary<string, SkinTemplate> _templates;


        private static string RenderStyleAttribute(string attributeName, string attributeValue)
        {
            return attributeValue != null ? string.Format(" {0}=\"{1}\"", attributeName, attributeValue) : String.Empty;
        }

        private static string RenderStyleElement(string skinPath, Style style)
        {
            return RenderStyleElement(skinPath, style, string.Empty, string.Empty);
        }

        private static string RenderStyleElement(string skinPath, string cssFilename)
        {
            string element = string.Empty;
            element += "<link";
            element +=
                string.Format("{0}{1}{2} />{3}", RenderStyleAttribute("type", "text/css"), RenderStyleAttribute("rel", "stylesheet"), RenderStyleAttribute("href", skinPath + cssFilename), Environment.NewLine);

            return element;
        }

        private static string RenderStyleElement(string skinPath, Style style, string skinName, string cssRequestParam)
        {
            string element = string.Empty;

            if (!String.IsNullOrEmpty(style.Conditional))
            {
                element = string.Format(CultureInfo.InvariantCulture, "<!--[{0}]>{1}", style.Conditional,
                                        Environment.NewLine);
            }

            element += "<link";
            if (!string.IsNullOrEmpty(style.Media) &&
               !style.Media.Equals("all", StringComparison.OrdinalIgnoreCase))
            {
                element += RenderStyleAttribute("media", style.Media);
            }

            element +=
                RenderStyleAttribute("type", "text/css") +
                RenderStyleAttribute("rel", "stylesheet") +
                RenderStyleAttribute("title", style.Title);

            if (string.IsNullOrEmpty(skinName))
            {
                element +=
                    string.Format("{0} />{1}", RenderStyleAttribute("href", GetStylesheetHrefPath(skinPath, style)), Environment.NewLine);
            }
            else
            {
                element +=
                    string.Format("{0} />{1}", RenderStyleAttribute("href", GetStylesheetHrefPath(skinPath, style, skinName, cssRequestParam)), Environment.NewLine);
            }

            if (!String.IsNullOrEmpty(style.Conditional))
            {
                element += string.Format("<![endif]-->{0}", Environment.NewLine);
            }

            return element;
        }


        /// <summary>
        /// Gets the stylesheet href path.
        /// </summary>
        /// <param name="skinPath">The skin path.</param>
        /// <param name="style">The style.</param>
        /// <returns></returns>
        public static string GetStylesheetHrefPath(string skinPath, Style style)
        {
            if (style.Href.StartsWith("~"))
            {
                return HttpHelper.ExpandTildePath(style.Href);
            }
            return style.Href.StartsWith("/") || style.Href.StartsWith("http://") || style.Href.StartsWith("https://")
                       ? style.Href
                       : skinPath + style.Href;
        }

        /// <summary>
        /// Gets the stylesheet href path.
        /// </summary>
        /// <param name="skinName">The skin name as in the key.</param>
        /// <param name="skinPath">The skin path.</param>
        /// <param name="style">The style.</param>
        /// <param name="cssRequestParam">The parameters used to request the css via the css handler.</param>
        /// <returns></returns>
        public static string GetStylesheetHrefPath(string skinPath, Style style, string skinName, string cssRequestParam)
        {
            if (IsStyleRemote(style))
            {
                return style.Href;
            }
            return string.Format("{0}css.axd?name={1}&{2}", skinPath, skinName, cssRequestParam);
        }

        private static string CreateStylePath(string skinTemplateFolder)
        {
            string applicationPath = HttpContext.Current.Request.ApplicationPath;
            string path = string.Format("{0}/Skins/{1}/", (applicationPath == "/" ? String.Empty : applicationPath), skinTemplateFolder);
            return path;
        }


        public string RenderStyleElementCollection(string skinName)
        {
            SkinTemplate skinTemplate = Templates.GetValueOrDefault(skinName);
            return RenderStyleElementCollection(skinName, skinTemplate);
        }

        public string RenderStyleElementCollection(string skinName, SkinTemplate skinTemplate)
        {
            var templateDefinedStyles = new StringBuilder();
            string finalStyleDefinition = string.Empty;

            var addedStyle = new List<string>();

            if (skinTemplate != null)
            {
                string skinPath = CreateStylePath(skinTemplate.TemplateFolder);

                // If skin doesn't want to be merged, just write plain css
                if (skinTemplate.StyleMergeMode == StyleMergeMode.None)
                {
                    if (skinTemplate.Styles != null)
                    {
                        foreach (Style style in skinTemplate.Styles)
                        {
                            templateDefinedStyles.Append(RenderStyleElement(skinPath, style));
                        }
                    }

                    if (!skinTemplate.ExcludeDefaultStyle)
                    {
                        templateDefinedStyles.Append(RenderStyleElement(skinPath, "style.css"));
                    }

                    if (skinTemplate.HasSkinStylesheet)
                    {
                        templateDefinedStyles.Append(RenderStyleElement(skinPath, skinTemplate.StyleSheet));
                    }

                    finalStyleDefinition = templateDefinedStyles.ToString();
                }
                else if (skinTemplate.StyleMergeMode == StyleMergeMode.MergedAfter ||
                        skinTemplate.StyleMergeMode == StyleMergeMode.MergedFirst)
                {
                    foreach (Style style in skinTemplate.Styles)
                    {
                        if (!CanStyleBeMerged(style))
                        {
                            string styleKey = BuildStyleKey(style);
                            if (!addedStyle.Contains(styleKey) || IsStyleRemote(style))
                            {
                                templateDefinedStyles.Append(RenderStyleElement(skinPath, style, skinName, styleKey));
                                addedStyle.Add(styleKey);
                            }
                        }
                    }

                    string mergedStyleLink = RenderStyleElement(skinPath, string.Format("css.axd?name={0}", skinName));
                    if (skinTemplate.StyleMergeMode == StyleMergeMode.MergedAfter)
                    {
                        finalStyleDefinition = templateDefinedStyles + mergedStyleLink;
                    }
                    else if (skinTemplate.StyleMergeMode == StyleMergeMode.MergedFirst)
                    {
                        finalStyleDefinition = mergedStyleLink + templateDefinedStyles;
                    }
                }
            }
            return Environment.NewLine + finalStyleDefinition;
        }


        private static string BuildStyleKey(Style style)
        {
            var keyBuilder = new StringBuilder();
            if (!String.IsNullOrEmpty(style.Media) && !style.Media.Equals("all", StringComparison.OrdinalIgnoreCase))
            {
                keyBuilder.AppendFormat("media={0}&", style.Media);
            }
            if (!String.IsNullOrEmpty(style.Title))
            {
                keyBuilder.AppendFormat("title={0}&", style.Title);
            }
            if (!String.IsNullOrEmpty(style.Conditional))
            {
                keyBuilder.AppendFormat("conditional={0}&", HttpUtility.UrlEncode(style.Conditional));
            }

            string key = keyBuilder.ToString();
            if (key.Length > 0)
            {
                return key.Substring(0, key.Length - 1);
            }
            return
                string.Empty;
        }

        public ICollection<StyleDefinition> GetStylesToBeMerged(string name)
        {
            return GetStylesToBeMerged(name, null, null, null);
        }

        public ICollection<StyleDefinition> GetStylesToBeMerged(string skinName, string media, string title,
                                                                string conditional)
        {
            bool normalCss = false;
            var styles = new List<StyleDefinition>();

            SkinTemplate skinTemplate = Templates.GetValueOrDefault(skinName);

            if ((string.IsNullOrEmpty(media)) && string.IsNullOrEmpty(title) && string.IsNullOrEmpty(conditional))
            {
                normalCss = true;
            }

            if (skinTemplate != null)
            {
                string skinPath = CreateStylePath(skinTemplate.TemplateFolder);

                if (skinTemplate.Styles != null)
                {
                    foreach (Style style in skinTemplate.Styles)
                    {
                        if (normalCss)
                        {
                            if (CanStyleBeMerged(style))
                            {
                                string tmpHref;
                                if (style.Href.StartsWith("~"))
                                {
                                    tmpHref = HttpHelper.ExpandTildePath(style.Href);
                                }
                                else
                                {
                                    tmpHref = skinPath + style.Href;
                                }
                                styles.Add(new StyleDefinition(tmpHref, style.Media));
                            }
                        }
                        else
                        {
                            string tmpMedia = style.Media;
                            if (tmpMedia != null && tmpMedia.Equals("all"))
                            {
                                tmpMedia = null;
                            }
                            if (string.Compare(media, tmpMedia, StringComparison.OrdinalIgnoreCase) == 0 &&
                               string.Compare(title, style.Title, StringComparison.OrdinalIgnoreCase) == 0 &&
                               string.Compare(conditional, style.Conditional,
                                              StringComparison.OrdinalIgnoreCase) == 0)
                            {
                                string tmpHref;
                                if (style.Href.StartsWith("~"))
                                {
                                    tmpHref = HttpHelper.ExpandTildePath(style.Href);
                                }
                                else
                                {
                                    tmpHref = skinPath + style.Href;
                                }
                                styles.Add(new StyleDefinition(tmpHref, style.Media));
                            }
                        }
                    }
                }

                if (normalCss)
                {
                    //Main style
                    if (!skinTemplate.ExcludeDefaultStyle)
                    {
                        styles.Add(new StyleDefinition(skinPath + "style.css"));
                    }

                    //Secondary Style
                    if (skinTemplate.HasSkinStylesheet)
                    {
                        styles.Add(new StyleDefinition(skinPath + skinTemplate.StyleSheet));
                    }
                }
            }
            return styles;
        }

        public static bool CanStyleBeMerged(Style style)
        {
            if (!String.IsNullOrEmpty(style.Conditional))
            {
                return false;
            }
            if (!string.IsNullOrEmpty(style.Title))
            {
                return false;
            }
            if (IsStyleRemote(style))
            {
                return false;
            }
            return true;
        }

        private static bool IsStyleRemote(Style style)
        {
            if (style.Href.StartsWith("http://", StringComparison.OrdinalIgnoreCase) ||
               style.Href.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
            return false;
        }
    }
}