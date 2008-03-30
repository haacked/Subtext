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
using System.Collections.Generic;
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
        readonly SkinTemplates templates;
		
        public StyleSheetElementCollectionRenderer(SkinTemplates templates)
        {
            this.templates = templates;
        }
		
        private static string RenderStyleAttribute(string attributeName, string attributeValue)
        {
            return attributeValue != null ? " " + attributeName + "=\"" + attributeValue + "\"" : String.Empty;
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
                RenderStyleAttribute("type", "text/css") +
                RenderStyleAttribute("rel", "stylesheet") +
                RenderStyleAttribute("href", skinPath + cssFilename) +
                " />" + Environment.NewLine;

            return element;
        }

        private static string RenderStyleElement(string skinPath, Style style, string skinName, string cssRequestParam)
        {
            string element = string.Empty;

            if (!String.IsNullOrEmpty(style.Conditional))
            {
                element = string.Format("<!--[{0}]>{1}", style.Conditional, Environment.NewLine);
            }
		    
            element += "<link";
            if (style.Media != null && style.Media.Length > 0 && !style.Media.ToLower().Equals("all"))
                element += RenderStyleAttribute("media", style.Media);

            element +=
                RenderStyleAttribute("type", "text/css") +
                RenderStyleAttribute("rel", "stylesheet") +
                RenderStyleAttribute("title", style.Title);

            if(string.IsNullOrEmpty(skinName))
            {
                element += 
                    RenderStyleAttribute("href", GetStylesheetHrefPath(skinPath, style)) + //TODO: Look at this line again.
                    " />" + Environment.NewLine;
            }
            else
            {
                element +=
                    RenderStyleAttribute("href", GetStylesheetHrefPath(skinPath, style, skinName, cssRequestParam)) + //TODO: Look at this line again.
                    " />" + Environment.NewLine;                
            }

            if (!String.IsNullOrEmpty(style.Conditional))
            {
                element += "<![endif]-->" + Environment.NewLine;
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
            else if (style.Href.StartsWith("/") || style.Href.StartsWith("http://") || style.Href.StartsWith("https://"))
            {
                return style.Href;
            }
            else
            {
                return skinPath + style.Href;
            }
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
            if(IsStyleRemote(style))
                return style.Href;
            else
            {
                return skinPath + "css.axd?name=" + skinName + "&" + cssRequestParam;
            }
        }

        private static string CreateStylePath(string skinTemplateFolder)
        {
            string applicationPath = HttpContext.Current.Request.ApplicationPath;
            string path = (applicationPath == "/" ? String.Empty : applicationPath) + "/Skins/" + skinTemplateFolder + "/";
            return path;
        }


        public string RenderStyleElementCollection(string skinName)
        {
            StringBuilder templateDefinedStyles = new StringBuilder();
            string finalStyleDefinition = string.Empty;

            SkinTemplate skinTemplate = templates.GetTemplate(skinName);
			
            List<string> addedStyle = new List<string>();

            if (skinTemplate != null && skinTemplate.Styles != null)
            {
                string skinPath = CreateStylePath(skinTemplate.TemplateFolder);

                // If skin doesn't want to be merged, just write plain css
                if (skinTemplate.StyleMergeMode == StyleMergeMode.None)
                {
                    foreach (Style style in skinTemplate.Styles)
                    {
                        templateDefinedStyles.Append(RenderStyleElement(skinPath, style));
                    }

                    if (!skinTemplate.ExcludeDefaultStyle)
                        templateDefinedStyles.Append(RenderStyleElement(skinPath,"style.css"));

                    if(skinTemplate.HasSkinStylesheet)
                        templateDefinedStyles.Append(RenderStyleElement(skinPath, skinTemplate.StyleSheet));

                    finalStyleDefinition = templateDefinedStyles.ToString();
                }
                else if (skinTemplate.StyleMergeMode == StyleMergeMode.MergedAfter || skinTemplate.StyleMergeMode == StyleMergeMode.MergedFirst)
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

                    string mergedStyleLink = RenderStyleElement(skinPath, "css.axd?name=" + skinName);
                    if(skinTemplate.StyleMergeMode == StyleMergeMode.MergedAfter)
                    {
                        finalStyleDefinition = templateDefinedStyles + mergedStyleLink;
                    }
                    else if (skinTemplate.StyleMergeMode== StyleMergeMode.MergedFirst)
                    {
                        finalStyleDefinition = mergedStyleLink + templateDefinedStyles;
                    }
                }
            }
            return Environment.NewLine + finalStyleDefinition;
        }



        private static string BuildStyleKey(Style style)
        {
            StringBuilder keyBuilder = new StringBuilder();
            if (!String.IsNullOrEmpty(style.Media) && !style.Media.ToLower().Equals("all"))
                keyBuilder.Append("media=" + style.Media + "&");
            if (!String.IsNullOrEmpty(style.Title))
                keyBuilder.Append("title=" + style.Title + "&");
            if (!String.IsNullOrEmpty(style.Conditional))
                keyBuilder.Append("conditional=" + HttpUtility.UrlEncode(style.Conditional) + "&");

            string key = keyBuilder.ToString();
            if(key.Length>0)
                return key.Substring(0, key.Length - 1);
            return
                string.Empty;
        }

        public IList<StyleDefinition> GetStylesToBeMerged(string name)
        {
            return GetStylesToBeMerged(name, null, null, null);
        }

        public IList<StyleDefinition> GetStylesToBeMerged(string skinName, string media, string title, string conditional)
        {
            bool normalCss = false;
            List<StyleDefinition> styles = new List<StyleDefinition>();

            SkinTemplate skinTemplate = templates.GetTemplate(skinName);

            if((string.IsNullOrEmpty(media)) && string.IsNullOrEmpty(title) && string.IsNullOrEmpty(conditional))
                normalCss = true;
            
            if (skinTemplate != null)
            {
                string skinPath = CreateStylePath(skinTemplate.TemplateFolder);

                if (skinTemplate.Styles != null)
                {
                    foreach (Style style in skinTemplate.Styles)
                    {
                        if(normalCss)
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
                            if (tmpMedia!=null && tmpMedia.Equals("all"))
                                tmpMedia = null;
                            if (string.Compare(media, tmpMedia, StringComparison.InvariantCultureIgnoreCase) == 0 && string.Compare(title, style.Title, StringComparison.InvariantCultureIgnoreCase) == 0 && string.Compare(conditional, style.Conditional, StringComparison.InvariantCultureIgnoreCase) == 0)
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

                if(normalCss)
                {
                    //Main style
                    if(!skinTemplate.ExcludeDefaultStyle)
                        styles.Add(new StyleDefinition(skinPath + "style.css"));

                    //Secondary Style
                    if (skinTemplate.HasSkinStylesheet)
                        styles.Add(new StyleDefinition(skinPath + skinTemplate.StyleSheet));
                }

            }
            return styles;
        }

        public static bool CanStyleBeMerged(Style style)
        {
            if(!String.IsNullOrEmpty(style.Conditional))
                return false;
            if(!string.IsNullOrEmpty(style.Title))
                return false;
            if (IsStyleRemote(style))
                return false;
            return true;
        }

        private static bool IsStyleRemote(Style style)
        {
            if (style.Href.StartsWith("http://") || style.Href.StartsWith("https://"))
                return true;
            else
                return false;
        }
    }

    public class StyleDefinition : IEquatable<StyleDefinition>
    {
        private string href;
        private string media = string.Empty;

        public string Href
        {
            get { return href; }
            set { href = value; }
        }

        public string Media
        {
            get { return media; }
            set { media = value; }
        }

        public StyleDefinition(string href)
        {
            this.href = href;
        }

        public StyleDefinition(string href, string media)
        {
            this.href = href;
            this.media = media;
        }

        public override string ToString()
        {
            return "Href: " + href + ", Media: " + media;
        }

        public bool Equals(StyleDefinition styleDefinition)
        {
            if (styleDefinition == null) return false;
            return Equals(href, styleDefinition.href) && Equals(media, styleDefinition.media);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj)) return true;
            return Equals(obj as StyleDefinition);
        }

        public override int GetHashCode()
        {
            return href.GetHashCode() + 29*media.GetHashCode();
        }
    }
}
