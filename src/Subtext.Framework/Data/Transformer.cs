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
using System.Globalization;
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
        public static LinkCategory BuildLinks(string title, CategoryType catType, Blog blog, UrlHelper urlHelper)
        {
            ICollection<LinkCategory> links = ObjectProvider.Instance().GetCategories(catType, true /* activeOnly */);
            return MergeLinkCategoriesIntoSingleLinkCategory(title, catType, links, urlHelper, blog);
        }

        /// <summary>
        /// Converts a LinkCategoryCollection into a single LinkCategory with its own LinkCollection.
        /// </summary>
        public static LinkCategory MergeLinkCategoriesIntoSingleLinkCategory(string title, CategoryType catType,
                                                                             IEnumerable<LinkCategory> links,
                                                                             UrlHelper urlHelper, Blog blog)
        {
            if(!links.IsNullOrEmpty())
            {
                var mergedLinkCategory = new LinkCategory {Title = title};

                var merged = from linkCategory in links
                            select GetLinkFromLinkCategory(linkCategory, catType, urlHelper, blog);
                mergedLinkCategory.Links.AddRange(merged);
                return mergedLinkCategory;
            }

            return null;
        }

        private static Link GetLinkFromLinkCategory(LinkCategory linkCategory, CategoryType catType, UrlHelper urlHelper, Blog blog)
        {
            var link = new Link {Title = linkCategory.Title};

            switch(catType)
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

        /// <summary>
        /// Will convert ArchiveCountCollection method from Archives.GetPostsByMonthArchive()
        /// into a <see cref="LinkCategory"/>. LinkCategory is a common item to databind to a web control.
        /// </summary>
        public static LinkCategory BuildMonthLinks(string title, UrlHelper urlHelper, Blog blog)
        {
            ICollection<ArchiveCount> archiveCounts = ObjectProvider.Instance().GetPostCountsByMonth();
            return MergeArchiveCountsIntoLinkCategory(title, archiveCounts, urlHelper, blog);
        }

        public static LinkCategory MergeArchiveCountsIntoLinkCategory(string title,
                                                                      IEnumerable<ArchiveCount> archiveCounts,
                                                                      UrlHelper urlHelper, Blog blog)
        {
            var linkCategory = new LinkCategory {Title = title};
            foreach(ArchiveCount archiveCount in archiveCounts)
            {
                var link = new Link
                {
                    NewWindow = false,
                    IsActive = true,
                    Title = archiveCount.Date.ToString("y") + " (" +
                            archiveCount.Count.ToString(CultureInfo.InvariantCulture) + ")",
                    Url = urlHelper.MonthUrl(archiveCount.Date)
                };

                linkCategory.Links.Add(link);
            }
            return linkCategory;
        }

        /// <summary>
        /// Will convert ArchiveCountCollection method from Archives.GetPostsByCategoryArchive()
        /// into a <see cref="LinkCategory"/>. LinkCategory is a common item to databind to a web control.
        /// </summary>
        public static LinkCategory BuildCategoriesArchiveLinks(string title, UrlHelper urlHelper)
        {
            ICollection<ArchiveCount> acc = Archives.GetPostCountByCategory();

            var category = new LinkCategory {Title = title};
            foreach(ArchiveCount ac in acc)
            {
                var link = new Link
                {
                    IsActive = true,
                    NewWindow = false,
                    Title = string.Format("{0} ({1})", ac.Title, ac.Count.ToString(CultureInfo.InvariantCulture)),
                    Url = urlHelper.CategoryUrl(new Category {Id = ac.Id, Title = ac.Title})
                };
                //Ugh, I hate how categories work in Subtext. So intertwined with links.

                category.Links.Add(link);
            }
            return category;
        }
    }
}