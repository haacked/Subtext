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
using Subtext.Framework;
using Subtext.Framework.Components;
using Subtext.Framework.Data;
using Subtext.Framework.Routing;

namespace Subtext.Web.UI
{
    /// <summary>
    /// Summary description for UIData.
    /// </summary>
    public static class UIData
    {
        public static LinkCategory Links(CategoryType catType, Blog blog, UrlHelper urlHelper)
        {
            switch(catType)
            {
                case CategoryType.PostCollection:
                    return Transformer.BuildLinks(UIText.PostCollection, CategoryType.PostCollection, blog, urlHelper);

                case CategoryType.ImageCollection:
                    return Transformer.BuildLinks(UIText.ImageCollection, CategoryType.ImageCollection, blog, urlHelper);

                case CategoryType.StoryCollection:
                    return Transformer.BuildLinks(UIText.ArticleCollection, CategoryType.StoryCollection, blog, urlHelper);

                default:
                    Debug.Fail(String.Format(CultureInfo.InvariantCulture,
                                             "Invalid CategoryType: {0} via Subtext.Web.UI.UIData.Links", catType));
                    return null;
            }
        }

        /// <summary>
        /// Builds a <see cref="LinkCategory"/> using the specified url formats. 
        /// A LinkCategory is a common item to databind to a web control.
        /// </summary>
        /// <returns></returns>
        public static LinkCategory ArchiveMonth(UrlHelper urlHelper, Blog blog)
        {
            return Transformer.BuildMonthLinks(UIText.Archives, urlHelper, blog);
        }
    }
}