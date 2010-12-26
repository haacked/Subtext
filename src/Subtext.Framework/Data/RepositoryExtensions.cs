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
using Subtext.Extensibility;
using Subtext.Framework.Components;
using Subtext.Framework.Providers;

namespace Subtext.Framework.Data
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
    }
}
