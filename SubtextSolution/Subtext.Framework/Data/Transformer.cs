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

using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;
using Subtext.Framework.Format;
using Subtext.Framework.Routing;
using Subtext.Framework.Providers;

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
		/// <param name="title">title for the LinkCategory</param>
		/// <param name="catType">Type of Categories to transform</param>
		/// <param name="formats">Determines how the Urls are formated</param>
		/// <returns></returns>
		public static LinkCategory BuildLinks(string title, CategoryType catType, UrlFormats formats)
		{
            ICollection<LinkCategory> links = Links.GetCategories(catType, ActiveFilter.ActiveOnly);
            return MergeLinkCategoriesIntoSingleLinkCategory(title, catType, links, new UrlHelper(null, null), Config.CurrentBlog);
		}

        /// <summary>
        /// Converts a LinkCategoryCollection into a single LinkCategory with its own LinkCollection.
        /// </summary>
        /// <param name="title">title for the LinkCategory</param>
        /// <param name="catType">Type of Categories to transform</param>
        /// <param name="formats">Determines how the Urls are formated</param>
        /// <returns></returns>
        public static LinkCategory MergeLinkCategoriesIntoSingleLinkCategory(string title, CategoryType catType, IEnumerable<LinkCategory> links, UrlHelper urlHelper, Blog blog)
        {
            if (links != null && links.Count() > 0)
            {
                LinkCategory mergedLinkCategory = new LinkCategory();
                mergedLinkCategory.Title = title;
                
                foreach (LinkCategory linkCategory in links)
                {
                    Link link = new Link();

                    link.Title = linkCategory.Title;
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
                    mergedLinkCategory.Links.Add(link);
                }
            }

            return null;
        }

		/// <summary>
		/// Will convert ArchiveCountCollection method from Archives.GetPostsByMonthArchive()
		/// into a <see cref="LinkCategory"/>. LinkCategory is a common item to databind to a web control.
		/// </summary>
		/// <param name="title">title for the Category</param>
		/// <param name="formats">Determines how the Urls are formated</param>
		/// <returns>A LinkCategory object with a Link (via LinkCollection) for each month in ArchiveCountCollection</returns>
		public static LinkCategory BuildMonthLinks(string title, UrlHelper urlHelper)
		{
            ICollection<ArchiveCount> archiveCounts = ObjectProvider.Instance().GetPostCountsByMonth();
            return MergeArchiveCountsIntoLinkCategory(title, archiveCounts, urlHelper, Config.CurrentBlog);
		}

        public static LinkCategory MergeArchiveCountsIntoLinkCategory(string title, IEnumerable<ArchiveCount> archiveCounts, UrlHelper urlHelper, Blog blog)
        {
            LinkCategory linkCategory = new LinkCategory();
            linkCategory.Title = title;
            foreach (ArchiveCount archiveCount in archiveCounts)
            {
                Link link = new Link();
                link.NewWindow = false;
                link.Title = archiveCount.Date.ToString("y") + " (" + archiveCount.Count.ToString(CultureInfo.InvariantCulture) + ")";
                link.Url = urlHelper.MonthUrl(archiveCount.Date);
                link.NewWindow = false;
                link.IsActive = true;

                linkCategory.Links.Add(link);
            }
            return linkCategory;
        }

        /// <summary>
        /// Will convert ArchiveCountCollection method from Archives.GetPostsByCategoryArchive()
        /// into a <see cref="LinkCategory"/>. LinkCategory is a common item to databind to a web control.
        /// </summary>
        /// <param name="title">title for the Category</param>
        /// <param name="formats">Determines how the Urls are formated</param>
        /// <returns>A LinkCategory object with a Link (via LinkCollection) for each month in ArchiveCountCollection</returns>
        public static LinkCategory BuildCategoriesArchiveLinks(string title, UrlHelper urlHelper)
        {
            ICollection<ArchiveCount> acc = Archives.GetPostCountByCategory();

            LinkCategory lc = new LinkCategory();
            lc.Title = title;
            foreach (ArchiveCount ac in acc)
            {
                Link link = new Link();
                link.NewWindow = false;
                link.Title = ac.Title + " (" + ac.Count.ToString(CultureInfo.InvariantCulture) + ")";
                //Ugh, I hate how categories work in Subtext. So intertwined with links.
                link.Url = urlHelper.CategoryUrl(new Category { Id = ac.Id, Title = ac.Title });
                link.NewWindow = false;
                link.IsActive = true;

                lc.Links.Add(link);
            }
            return lc;
        }
	}
}
