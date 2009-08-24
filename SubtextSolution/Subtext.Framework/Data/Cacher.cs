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
using System.Globalization;
using System.IO;
using System.Web.Caching;
using Subtext.Configuration;
using Subtext.Framework.Components;
using Subtext.Framework.Properties;
using Subtext.Framework.Providers;
using Subtext.Framework.Text;
using Subtext.Framework.Util;
using Subtext.Infrastructure;

namespace Subtext.Framework.Data
{
    //TODO: Refactor. Static classes like this are a pain!
    /// <summary>
    /// Encapsulates obtaining content from the cache.
    /// </summary>
    public static class Cacher
    {
        public const int ShortDuration = 10;
        public const int MediumDuration = 20;
        public const int LongDuration = 30;
        private static readonly string EntryMonthKey = "EntryMonth:Date{0:yyyyMM}Blog{1}";

        /// <summary>
        /// Gets the entries for the specified month.
        /// </summary>
        /// <param name="dt">The dt.</param>
        /// <param name="cacheDuration">The cache duration.</param>
        /// <returns></returns>
        public static ICollection<Entry> GetMonth(DateTime dt, ISubtextContext context)
        {
            string key = string.Format(CultureInfo.InvariantCulture, EntryMonthKey, dt, context.Blog.Id);
            ICache cache = context.Cache;
            ICollection<Entry> month = (ICollection<Entry>)cache[key];
            if (month == null)
            {
                month = context.Repository.GetPostsByMonth(dt.Month, dt.Year);
                if (month != null)
                {
                    cache.InsertDuration(key, month, LongDuration);
                }
            }
            return month;
        }

        private static readonly string EntryDayKey = "EntryDay:Date{0:yyyyMMdd}Blog{1}";
        public static EntryDay GetDay(DateTime day, ISubtextContext context)
        {
            string key = string.Format(CultureInfo.InvariantCulture, EntryDayKey, day, context.Blog.Id);
            ICache cache = context.Cache;

            EntryDay entryDay = (EntryDay)cache[key];
            if (entryDay == null)
            {
                entryDay = context.Repository.GetEntryDay(day);
                if (entryDay != null)
                {
                    cache.InsertDuration(key, entryDay, LongDuration);
                }
            }
            return entryDay;

        }

        private const string ECKey = "EC:Count{0}Category{1}BlogId{2}";
        public static ICollection<Entry> GetEntriesByCategory(int count, int categoryID, ISubtextContext context)
        {
            string key = string.Format(ECKey, count, categoryID, context.Blog.Id);
            var cache = context.Cache;
            var entryCollection = (ICollection<Entry>)cache[key];
            if (entryCollection == null)
            {
                entryCollection = context.Repository.GetEntriesByCategory(count, categoryID, true /* activeOnly */);

                if (entryCollection != null)
                {
                    cache.InsertDuration(key, entryCollection, ShortDuration);
                }
            }
            return entryCollection;
        }

        private static readonly string ETKey = "ET:Count{0}Tag{1}BlogId{2}";
        public static ICollection<Entry> GetEntriesByTag(int count, string tag, ISubtextContext context)
        {
            string key = string.Format(ETKey, count, tag, context.Blog.Id);
            ICache cache = context.Cache;
            var entries = (ICollection<Entry>)cache[key];
            if (entries == null)
            {
                entries = context.Repository.GetEntriesByTag(count, tag);

                if (entries != null)
                {
                    cache.InsertDuration(key, entries, ShortDuration);
                }
            }
            return entries;
        }

        /// <summary>
        /// Returns a LinkCategory for a single category based on the request url.
        /// </summary>
        /// <param name="cacheDuration">The cache duration.</param>
        /// <returns></returns>
        public static LinkCategory SingleCategory(ISubtextContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");

            string categorySlug = context.RequestContext.GetSlugFromRequest();
            if (categorySlug.IsNumeric())
            {
                int categoryID = Int32.Parse(categorySlug);
                return SingleCategory(categoryID, true, context);
            }
            else
            {
                return SingleCategory(categorySlug, true, context);
            }
        }

        private static readonly string LCKey = "LC{0}BlogId{1}";

        public static LinkCategory SingleCategory(int categoryId, bool isActive, ISubtextContext context)
        {
            LinkCategoryRetrieval retrieval = delegate { return context.Repository.GetLinkCategory(categoryId, isActive); };
            return SingleCategory(retrieval, categoryId, context);
        }

        public static LinkCategory SingleCategory(string categoryName, bool isActive, ISubtextContext context)
        {
            LinkCategoryRetrieval retrieval = delegate { return context.Repository.GetLinkCategory(categoryName, isActive); };
            LinkCategory category = SingleCategory(retrieval, categoryName, context);
            if (category != null)
                return category;

            if (context.Blog.AutoFriendlyUrlEnabled)
            {
                categoryName = categoryName.Replace(FriendlyUrlSettings.Settings.SeparatingCharacter, " ");
                retrieval = delegate { return context.Repository.GetLinkCategory(categoryName, isActive); };
                return SingleCategory(retrieval, categoryName, context);
            }

            return null; //couldn't find category
        }

        private static LinkCategory SingleCategory<T>(LinkCategoryRetrieval retrievalDelegate, T categoryKey, ISubtextContext context)
        {
            ICache cache = context.Cache;
            string key = string.Format(LCKey, categoryKey, context.Blog.Id);
            LinkCategory lc = (LinkCategory)cache[key];
            if (lc == null)
            {
                lc = retrievalDelegate();
                if (lc != null)
                    cache.InsertDuration(key, lc, ShortDuration);
            }
            return lc;
        }

        delegate LinkCategory LinkCategoryRetrieval();

        //TODO: This should only be called in one place total. And it needs to be tested.
        public static Entry GetEntryFromRequest(bool allowRedirectToEntryName, ISubtextContext context)
        {
            string slug = context.RequestContext.GetSlugFromRequest();
            if (!String.IsNullOrEmpty(slug))
            {
                return GetEntry(slug, context);
            }

            int? id = context.RequestContext.GetIdFromRequest();
            if (id != null)
            {
                Entry entry = GetEntry(id.Value, context);
                if (entry == null)
                {
                    return null;
                }

                //TODO: Violation of SRP here!
                //Second condition avoids infinite redirect loop. Should never happen.
                if (allowRedirectToEntryName && entry.HasEntryName && !entry.EntryName.IsNumeric())
                {
                    var response = context.RequestContext.HttpContext.Response;
                    response.StatusCode = 301;
                    response.Status = "301 Moved Permanently";
                    response.RedirectLocation = context.UrlHelper.EntryUrl(entry).ToFullyQualifiedUrl(context.Blog).ToString();
                    response.End();
                }
                return entry;
            }

            return null;
        }

        private const string EntryKeyID = "Entry{0}BlogId{1}";
        private const string EntryKeyName = "EntryName{0}BlogId{1}";

        /// <summary>
        /// Retrieves a single entry from the cache by the entry name.  
        /// If it is not in the cache, gets it from the database and 
        /// inserts it into the cache.
        /// </summary>
        /// <param name="EntryName">Name of the entry.</param>
        /// <param name="cacheDuration">The cache duration.</param>
        /// <returns></returns>
        public static Entry GetEntry(string entryName, ISubtextContext context)
        {
            Blog blog = context.Blog;
            ObjectProvider repository = context.Repository;
            int blogId = blog.Id;

            ICache cache = context.Cache;
            string key = string.Format(EntryKeyName, entryName, blogId);

            Entry entry = (Entry)cache[key];
            if (entry == null)
            {
                entry = repository.GetEntry(entryName, true /* activeOnly */, true /* includeCategories */);

                if (entry != null)
                {
                    if (entry.DateSyndicated > blog.TimeZone.Now)
                    {
                        return null;
                    }

                    cache.InsertDuration(key, entry, MediumDuration);

                    //Most other page items will use the entryID. Add entry to cache for id key as well.
                    //Bind them together with a cache dependency.
                    string entryIDKey = string.Format(EntryKeyID, entry.Id, blogId);
                    CacheDependency cd = new CacheDependency(null, new string[] { key });
                    cache.Insert(entryIDKey, entry, cd);
                }
            }
            return entry;
        }

        /// <summary>
        /// Retrieves a single entry from the cache by the id.
        /// If it is not in the cache, gets it from the database and
        /// inserts it into the cache.
        /// </summary>
        /// <param name="entryID">The entry ID.</param>
        /// <param name="cacheDuration">The cache duration.</param>
        /// <returns></returns>
        public static Entry GetEntry(int entryId, ISubtextContext context)
        {
            Blog blog = context.Blog;
            ObjectProvider repository = context.Repository;
            int blogId = blog.Id;

            ICache cache = context.Cache;
            string key = string.Format(EntryKeyID, entryId, blog.Id);

            Entry entry = (Entry)cache[key];
            if (entry == null)
            {
                entry = repository.GetEntry(entryId, true /* activeOnly */, true /* includeCategories */);
                if (entry != null)
                {
                    cache.InsertDuration(key, entry, MediumDuration);
                }
            }
            return entry;
        }

        private static readonly string TagsKey = "TagsCount{0}BlogId{1}";
        /// <summary>
        /// Retrieves the current tags from the cache based on the ItemCount and
        /// Blog Id. If it is not in the cache, it gets it from the database and 
        /// inserts it into the cache.
        /// </summary>
        /// <param name="ItemCount">The item count</param>
        /// <param name="cacheDuration">The cache duration.</param>
        /// <returns></returns>
        public static IEnumerable<Tag> GetTopTags(int ItemCount, ISubtextContext context)
        {
            ICache cache = context.Cache;
            string key = string.Format(TagsKey, ItemCount, context.Blog.Id);

            IEnumerable<Tag> tags = (IEnumerable<Tag>)cache[key];
            if (tags == null)
            {
                tags = Tags.GetTopTags(ItemCount);
                if (tags != null)
                {
                    cache.InsertDuration(key, tags, LongDuration);
                }
            }
            return tags;
        }

        /// <summary>
        /// Clears the comment cache.
        /// </summary>
        /// <param name="entryID">The entry ID.</param>
        public static void ClearCommentCache(int entryId, ISubtextContext context)
        {
            string key = string.Format(ParentCommentEntryKey, entryId, context.Blog.Id);
            context.Cache.Remove(key);
        }

        private static readonly string ParentCommentEntryKey = "ParentEntry:Comments:EntryID{0}:BlogId{1}";

        /// <summary>
        /// Returns all the feedback for the specified entry. Checks the cache first.
        /// </summary>
        /// <param name="parentEntry"></param>
        /// <param name="cacheDuration"></param>
        /// <returns></returns>
        /// <param name="fromCache"></param>
        public static ICollection<FeedbackItem> GetFeedback(Entry parentEntry, bool fromCache, ISubtextContext context)
        {
            ICollection<FeedbackItem> comments = null;
            ICache cache = context.Cache;
            string key = null;
            if (fromCache)
            {
                key = string.Format(ParentCommentEntryKey, parentEntry.Id, context.Blog.Id);
                comments = (ICollection<FeedbackItem>)cache[key];
            }
            if (comments == null)
            {
                comments = context.Repository.GetFeedbackForEntry(parentEntry);
                if (comments != null && fromCache)
                {
                    cache.InsertDuration(key, comments, ShortDuration);
                }
            }
            return comments;
        }

        public static void InsertDuration(this ICache cache, string key, object value, int duration)
        {
            cache.Insert(key, value, null, DateTime.Now.AddSeconds(duration), TimeSpan.Zero, CacheItemPriority.Normal, null);
        }
    }
}
