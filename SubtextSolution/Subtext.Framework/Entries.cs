#region Disclaimer/Info

///////////////////////////////////////////////////////////////////////////////////////////////////
// Subtext WebLog
// 
// Subtext is an open source weblog system that is a fork of the .TEXT
// weblog system.
//
// For updated news and information please visit http://subtextproject.com/
// Subtext is hosted at Google Code at http://code.google.com/p/subtext/
// The development mailing list is at subtext-devs@lists.sourceforge.net 
//
// This project is licensed under the BSD license.  See the License.txt file for more information.
///////////////////////////////////////////////////////////////////////////////////////////////////

#endregion

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Subtext.Configuration;
using Subtext.Extensibility;
using Subtext.Extensibility.Interfaces;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;
using Subtext.Framework.Emoticons;
using Subtext.Framework.Providers;
using Subtext.Framework.Services;
using Subtext.Framework.Text;

namespace Subtext.Framework
{
    /// <summary>
    /// Static class used to get entries (blog posts, comments, etc...) 
    /// from the data store.
    /// </summary>
    public static class Entries
    {
        /// <summary>
        /// Returns a collection of Posts for a give page and index size.
        /// </summary>
        /// <param name="postType"></param>
        /// <param name="categoryId">-1 means not to filter by a categoryID</param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public static IPagedCollection<EntryStatsView> GetPagedEntries(PostType postType, int? categoryId, int pageIndex,
                                                                       int pageSize)
        {
            return ObjectProvider.Instance().GetPagedEntries(postType, categoryId, pageIndex, pageSize);
        }

        public static EntryDay GetSingleDay(DateTime dt)
        {
            return ObjectProvider.Instance().GetEntryDay(dt);
        }

        /// <summary>
        /// Gets the entries to display on the home page.
        /// </summary>
        /// <param name="itemCount">Item count.</param>
        /// <returns></returns>
        public static ICollection<EntryDay> GetHomePageEntries(int itemCount)
        {
            return GetBlogPosts(itemCount, PostConfig.DisplayOnHomepage | PostConfig.IsActive);
        }

        /// <summary>
        /// Gets the specified number of entries using the <see cref="PostConfig"/> flags 
        /// specified.
        /// </summary>
        /// <remarks>
        /// This is used to get the posts displayed on the home page.
        /// </remarks>
        /// <param name="itemCount">Item count.</param>
        /// <param name="pc">Pc.</param>
        /// <returns></returns>
        public static ICollection<EntryDay> GetBlogPosts(int itemCount, PostConfig pc)
        {
            return ObjectProvider.Instance().GetBlogPosts(itemCount, pc);
        }

        public static ICollection<EntryDay> GetPostsByCategoryId(int itemCount, int categoryId)
        {
            return ObjectProvider.Instance().GetPostsByCategoryID(itemCount, categoryId);
        }

        public static IEnumerable<int> GetCategoryIdsFromCategoryTitles(Entry entry)
        {
            var categoryIds = new Collection<int>();
            //Ok, we have categories specified in the entry, but not the IDs.
            //We need to do something.
            foreach(string category in entry.Categories)
            {
                LinkCategory cat = ObjectProvider.Instance().GetLinkCategory(category, true);
                if(cat != null)
                {
                    categoryIds.Add(cat.Id);
                }
            }

            return categoryIds;
        }

        /// <summary>
        /// Updates the specified entry in the data provider.
        /// </summary>
        /// <param name="entry">Entry.</param>
        /// <param name="context"></param>
        /// <returns></returns>
        public static void Update(Entry entry, ISubtextContext context)
        {
            if(entry == null)
            {
                throw new ArgumentNullException("entry");
            }

            if(NullValue.IsNull(entry.DateSyndicated) && entry.IsActive && entry.IncludeInMainSyndication)
            {
                entry.DateSyndicated = Config.CurrentBlog.TimeZone.Now;
            }

            Update(entry, context, null /* categoryIds */);
        }

        /// <summary>
        /// Updates the specified entry in the data provider 
        /// and attaches the specified categories.
        /// </summary>
        /// <param name="entry">Entry.</param>
        /// <param name="context"></param>
        /// <param name="categoryIds">Category Ids this entry belongs to.</param>
        /// <returns></returns>
        public static void Update(Entry entry, ISubtextContext context, IEnumerable<int> categoryIds)
        {
            if(entry == null)
            {
                throw new ArgumentNullException("entry");
            }
            ObjectProvider repository = ObjectProvider.Instance();
            var transform = new CompositeTextTransformation
            {
                new XhtmlConverter(),
                new EmoticonsTransformation(context),
                new KeywordExpander(repository)
            };
            //TODO: Maybe use a INinjectParameter to control this.
            var publisher = new EntryPublisher(context, transform, new SlugGenerator(FriendlyUrlSettings.Settings));
            publisher.Publish(entry);
        }

        #region Entry Category List

        /// <summary>
        /// Sets the categories for this entry.
        /// </summary>
        /// <param name="entryId">The entry id.</param>
        /// <param name="categories">The categories.</param>
        public static void SetEntryCategoryList(int entryId, IEnumerable<int> categories)
        {
            ObjectProvider.Instance().SetEntryCategoryList(entryId, categories);
        }

        #endregion

        #region Tag Utility Functions

        public static bool RebuildAllTags()
        {
            foreach(EntryDay day in GetBlogPosts(0, PostConfig.None))
            {
                foreach(Entry e in day)
                {
                    ObjectProvider.Instance().SetEntryTagList(e.Id, e.Body.ParseTags());
                }
            }
            return true;
        }

        #endregion

        #region EntryCollections

        /// <summary>
        /// Gets the main syndicated entries.
        /// </summary>
        /// <param name="itemCount">Item count.</param>
        /// <returns></returns>
        public static ICollection<Entry> GetMainSyndicationEntries(int itemCount)
        {
            return GetRecentPosts(itemCount, PostType.BlogPost,
                                  PostConfig.IncludeInMainSyndication | PostConfig.IsActive, true
                /* includeCategories */);
        }

        /// <summary>
        /// Gets the comments (including trackback, etc...) for the specified entry.
        /// </summary>
        /// <param name="parentEntry">Parent entry.</param>
        /// <returns></returns>
        public static ICollection<FeedbackItem> GetFeedBack(Entry parentEntry)
        {
            return ObjectProvider.Instance().GetFeedbackForEntry(parentEntry);
        }

        /// <summary>
        /// Returns the itemCount most recent posts.  
        /// This is used to support MetaBlogAPI...
        /// </summary>
        /// <param name="itemCount"></param>
        /// <param name="postType"></param>
        /// <param name="postConfig"></param>
        /// <param name="includeCategories"></param>
        /// <returns></returns>
        public static ICollection<Entry> GetRecentPosts(int itemCount, PostType postType, PostConfig postConfig,
                                                        bool includeCategories)
        {
            return ObjectProvider.Instance().GetEntries(itemCount, postType, postConfig, includeCategories);
        }

        /// <summary>
        /// Returns the posts for the specified month for the Month Archive section.
        /// </summary>
        /// <param name="month"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        public static ICollection<Entry> GetPostsByMonth(int month, int year)
        {
            return ObjectProvider.Instance().GetPostsByMonth(month, year);
        }

        public static ICollection<Entry> GetPostsByDayRange(DateTime start, DateTime stop, PostType postType,
                                                            bool activeOnly)
        {
            return ObjectProvider.Instance().GetPostsByDayRange(start, stop, postType, activeOnly);
        }

        public static ICollection<Entry> GetEntriesByCategory(int itemCount, int categoryId, bool activeOnly)
        {
            return ObjectProvider.Instance().GetEntriesByCategory(itemCount, categoryId, activeOnly);
        }

        public static ICollection<Entry> GetEntriesByTag(int itemCount, string tagName)
        {
            return ObjectProvider.Instance().GetEntriesByTag(itemCount, tagName);
        }

        #endregion

        #region Single Entry

        /// <summary>
        /// Searches the data store for the first comment with a 
        /// matching checksum hash.
        /// </summary>
        /// <param name="checksumHash">Checksum hash.</param>
        /// <returns></returns>
        public static FeedbackItem GetFeedbackByChecksumHash(string checksumHash)
        {
            return ObjectProvider.Instance().GetFeedbackByChecksumHash(checksumHash);
        }

        /// <summary>
        /// Gets the entry from the data store by id. Only returns an entry if it is 
        /// within the current blog (Config.CurrentBlog).
        /// </summary>
        /// <param name="entryId">The ID of the entry.</param>
        /// <param name="postConfig">The entry option used to constrain the search.</param>
        /// <param name="includeCategories">Whether the returned entry should have its categories collection populated.</param>
        /// <returns></returns>
        public static Entry GetEntry(int entryId, PostConfig postConfig, bool includeCategories)
        {
            bool isActive = ((postConfig & PostConfig.IsActive) == PostConfig.IsActive);
            return ObjectProvider.Instance().GetEntry(entryId, isActive, includeCategories);
        }

        #endregion
    }
}