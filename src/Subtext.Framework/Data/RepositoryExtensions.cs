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
using System.Linq;
using Subtext.Extensibility;
using Subtext.Extensibility.Interfaces;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;
using Subtext.Framework.Providers;

namespace Subtext.Framework
{
    public static class RepositoryExtensions
    {
        public static IEnumerable<EntryDay> GetBlogPostsByCategoryGroupedByDay(this ObjectProvider repository, int itemCount, int categoryId)
        {
            return repository.GetEntriesByCategory(itemCount, categoryId, true /*activeOnly*/).GroupByDayUsingDateSyndicated();
        }

        public static IEnumerable<EntryDay> GetBlogPostsForHomePage(this ObjectProvider repository, int itemCount, PostConfig postConfig)
        {
            return repository.GetEntries(itemCount, PostType.BlogPost, postConfig, false /*includeCategories*/).GroupByDayUsingDateSyndicated();
        }

        public static IEnumerable<EntryDay> GroupByDayUsingDateSyndicated(this IEnumerable<Entry> entries)
        {
            var groupedEntries =
                    from entry in entries
                    group entry by entry.DateSyndicated.Date
                        into entriesGroupedByDay
                        select entriesGroupedByDay;
            foreach (var group in groupedEntries)
            {
                yield return new EntryDay(group.Key, group.ToList());
            }
        }

        /// <summary>
        /// Gets the active blog count by host.
        /// </summary>
        /// <param name="host">The host.</param>
        /// <returns></returns>
        /// <param name="pageIndex">Zero based index of the page to retrieve.</param>
        /// <param name="pageSize">Number of records to display on the page.</param>
        /// <param name="flags">Configuration flags to filter blogs retrieved.</param>
        public static IPagedCollection<Blog> GetBlogsByHost(this ObjectProvider repository, string host, int pageIndex, int pageSize,
                                                            ConfigurationFlags flags)
        {
            if (String.IsNullOrEmpty(host))
            {
                throw new ArgumentNullException("host");
            }

            return repository.GetPagedBlogs(host, pageIndex, pageSize, flags);
        }

        /// <summary>
        /// Returns a <see cref="IList{T}"/> containing ACTIVE the <see cref="Blog"/> 
        /// instances within the specified range.
        /// </summary>
        /// <param name="pageIndex">Page index.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="flags"></param>
        /// <returns></returns>
        public static IPagedCollection<Blog> GetBlogs(this ObjectProvider repository, int pageIndex, int pageSize, ConfigurationFlags flags)
        {
            return ObjectProvider.Instance().GetPagedBlogs(null, pageIndex, pageSize, flags);
        }

    }
}
