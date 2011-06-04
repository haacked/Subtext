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
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using Subtext.Framework;
using Subtext.Framework.Components;
using Subtext.Framework.Data;
using Subtext.Framework.Providers;
using Subtext.Framework.Routing;

namespace Subtext.Web.UI
{
    /// <summary>
    /// Used to obtain configurable text displayed on the UI.  
    /// Uses application settings (bleh!).
    /// </summary>
    /// <remarks>
    /// This text ought to be configurable per-blog.
    /// </remarks>
    public static class UIHelpers
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
            if (text == null)
            {
                return defaultValue;
            }
            return text;
        }

        public static LinkCategory Links(this ObjectRepository repository, CategoryType catType, Blog blog, BlogUrlHelper urlHelper)
        {
            switch (catType)
            {
                case CategoryType.PostCollection:
                    return repository.BuildLinks(UIHelpers.PostCollection, CategoryType.PostCollection, blog, urlHelper);

                case CategoryType.ImageCollection:
                    return repository.BuildLinks(UIHelpers.ImageCollection, CategoryType.ImageCollection, blog, urlHelper);

                case CategoryType.StoryCollection:
                    return repository.BuildLinks(UIHelpers.ArticleCollection, CategoryType.StoryCollection, blog, urlHelper);

                default:
                    Debug.Fail(String.Format(CultureInfo.InvariantCulture,
                                             "Invalid CategoryType: {0} via Subtext.Web.UI.UIHelpers.Links", catType));
                    return null;
            }
        }

        /// <summary>
        /// Builds a <see cref="LinkCategory"/> using the specified url formats. 
        /// A LinkCategory is a common item to databind to a web control.
        /// </summary>
        /// <returns></returns>
        public static LinkCategory ArchiveMonth(this ObjectRepository repository, BlogUrlHelper urlHelper, Blog blog)
        {
            return repository.BuildMonthLinks(UIHelpers.Archives, urlHelper, blog);
        }

        /// <summary>
        /// Will convert ArchiveCountCollection method from Archives.GetPostsByMonthArchive()
        /// into a <see cref="LinkCategory"/>. LinkCategory is a common item to databind to a web control.
        /// </summary>
        public static LinkCategory BuildMonthLinks(this ObjectRepository repository, string title, BlogUrlHelper urlHelper, Blog blog)
        {
            ICollection<ArchiveCount> archiveCounts = repository.GetPostCountsByMonth();
            return archiveCounts.MergeIntoLinkCategory(title, urlHelper, blog);
        }

        public static LinkCategory MergeIntoLinkCategory(this IEnumerable<ArchiveCount> archiveCounts,
            string title,
            BlogUrlHelper urlHelper,
            Blog blog)
        {
            var linkCategory = new LinkCategory { Title = title };
            foreach (ArchiveCount archiveCount in archiveCounts)
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

    }
}