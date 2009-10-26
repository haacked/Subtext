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
using Subtext.Extensibility;
using Subtext.Framework.Components;
using Subtext.Framework.Data;
using Subtext.Framework.Providers;
using Subtext.Framework.Text;

namespace Subtext.Framework
{
    /// <summary>
    /// Static class used to get entries (blog posts, comments, etc...) 
    /// from the data store.
    /// </summary>
    public static class Entries
    {
        public static void RebuildAllTags(this ObjectProvider repository)
        {
            foreach(var day in repository.GetBlogPostsForHomePage(0, PostConfig.None))
            {
                foreach(var entry in day)
                {
                    repository.SetEntryTagList(entry.Id, entry.Body.ParseTags());
                }
            }
        }

        /// <summary>
        /// Gets the main syndicated entries.
        /// </summary>
        public static ICollection<Entry> GetMainSyndicationEntries(this ObjectProvider repository, int itemCount)
        {
            return repository.GetEntries(itemCount, PostType.BlogPost, PostConfig.IncludeInMainSyndication | PostConfig.IsActive, true /* includeCategories */);
        }
    }
}