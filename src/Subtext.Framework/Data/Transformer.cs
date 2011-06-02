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

using System.Collections.Generic;
using System.Linq;
using Subtext.Framework.Components;
using Subtext.Framework.Providers;
using Subtext.Framework.Routing;

namespace Subtext.Framework.Data
{
    /// <summary>
    /// Common Subtext object transformations
    /// </summary>
    public static class Transformer
    {
        /// <summary>
        /// Converts a LinkCategoryCollection into a single LinkCategory with its own LinkCollection.
        /// </summary>
        public static LinkCategory BuildLinks(string title, CategoryType catType, Blog blog, BlogUrlHelper urlHelper)
        {
            ICollection<LinkCategory> links = ObjectProvider.Instance().GetCategories(catType, true /* activeOnly */);
            return MergeLinkCategoriesIntoSingleLinkCategory(title, catType, links, urlHelper, blog);
        }

        /// <summary>
        /// Converts a LinkCategoryCollection into a single LinkCategory with its own LinkCollection.
        /// </summary>
        public static LinkCategory MergeLinkCategoriesIntoSingleLinkCategory(string title, CategoryType catType,
                                                                             IEnumerable<LinkCategory> links,
                                                                             BlogUrlHelper urlHelper, Blog blog)
        {
            if (!links.IsNullOrEmpty())
            {
                var mergedLinkCategory = new LinkCategory { Title = title };

                var merged = from linkCategory in links
                             select GetLinkFromLinkCategory(linkCategory, catType, urlHelper, blog);
                mergedLinkCategory.Links.AddRange(merged);
                return mergedLinkCategory;
            }

            return null;
        }

        private static Link GetLinkFromLinkCategory(LinkCategory linkCategory, CategoryType catType, BlogUrlHelper urlHelper, Blog blog)
        {
            var link = new Link { Title = linkCategory.Title };

            switch (catType)
            {
                case CategoryType.StoryCollection:
                    link.Url = urlHelper.CategoryUrl(linkCategory).ToFullyQualifiedUrl(blog).ToString();
                    break;

                case CategoryType.PostCollection:
                    link.Url = urlHelper.CategoryUrl(linkCategory).ToFullyQualifiedUrl(blog).ToString();
                    link.Rss = urlHelper.CategoryRssUrl(linkCategory);
                    break;

                case CategoryType.ImageCollection:
                    link.Url = urlHelper.GalleryUrl(linkCategory.Id).ToFullyQualifiedUrl(blog).ToString();
                    break;
            }
            link.NewWindow = false;
            return link;
        }


    }
}