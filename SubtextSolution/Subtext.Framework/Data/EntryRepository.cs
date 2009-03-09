using System.Collections.Generic;
using System.Data;
using Subtext.Extensibility;
using Subtext.Framework.Components;
using Subtext.Extensibility.Interfaces;
using System;
using Subtext.Framework.Text;
using Subtext.Framework.Configuration;
using System.Globalization;
using Subtext.Framework;
using System.Linq;
using BlogML.Xml;
using Subtext.BlogML.Interfaces;
using Subtext.ImportExport;

namespace Subtext.Framework.Data
{
    public partial class DatabaseObjectProvider
    {
        /// <summary>
        /// Gets the entries that meet the specific <see cref="PostType"/> 
        /// and the <see cref="PostConfig"/> flags.
        /// </summary>
        /// <remarks>
        /// This is called to get the main syndicated entries and supports MetaWeblog API.
        /// </remarks>
        /// <param name="itemCount">Item count.</param>
        /// <param name="postType">The type of post to retrieve.</param>
        /// <param name="postConfig">Post configuration options.</param>
        /// <param name="includeCategories">Whether or not to include categories</param>
        /// <returns></returns>
        public override ICollection<Entry> GetEntries(int itemCount, PostType postType, PostConfig postConfig, bool includeCategories)
        {
            using (IDataReader reader = _procedures.GetConditionalEntries(itemCount, 
                (int)postType, 
                (int)postConfig, 
                (int?)BlogId,
                includeCategories, 
                CurrentDateTime))
            {
                return reader.LoadEntryCollectionFromDataReader();
            }
        }

        /// <summary>
        /// Returns a pageable collection of entries ordered by the id descending.
        /// This is used in the admin section.
        /// </summary>
        /// <param name="postType">Type of the post.</param>
        /// <param name="categoryID">The category ID.</param>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <returns></returns>
        public override IPagedCollection<Entry> GetPagedEntries(PostType postType, int categoryID, int pageIndex, int pageSize)
        {
            using (IDataReader reader = GetPagedEntriesReader(postType, categoryID, pageIndex, pageSize))
            {
                return reader.GetPagedCollection<Entry>(r => DataHelper.LoadEntryStatsView(reader));
            }
        }

        private IDataReader GetPagedEntriesReader(PostType postType, int categoryID, int pageIndex, int pageSize)
        {
            if (categoryID > 0)
            {
                return _procedures.GetPageableEntriesByCategoryID(BlogId, categoryID, pageIndex, pageSize, (int)postType);
            }
            else
            {
                return _procedures.GetPageableEntries(BlogId, pageIndex, pageSize, (int)postType);
            }
        }

        public override EntryDay GetEntryDay(DateTime dateTime)
        {
            using (IDataReader reader = _procedures.GetSingleDay(dateTime, BlogId, CurrentDateTime))
            {
                EntryDay ed = new EntryDay(dateTime);
                while (reader.Read())
                {
                    ed.Add(DataHelper.LoadEntry(reader));
                }
                return ed;
            }
        }

        /// <summary>
        /// Returns blog posts that meet the criteria specified in the <see cref="PostConfig"/> flags.
        /// </summary>
        /// <param name="itemCount">Item count.</param>
        /// <param name="pc">Pc.</param>
        /// <remarks>
        /// This is used to get the posts displayed on the home page.
        /// </remarks>
        /// <returns></returns>
        public override ICollection<EntryDay> GetBlogPosts(int itemCount, PostConfig pc)
        {
            using (IDataReader reader = _procedures.GetConditionalEntries(itemCount, (int)PostType.BlogPost, (int)pc, BlogId, false, CurrentDateTime))
            {
                ICollection<EntryDay> edc = DataHelper.LoadEntryDayCollection(reader);
                return edc;
            }
        }

        public override ICollection<EntryDay> GetPostsByCategoryID(int itemCount, int categoryId)
        {
            using (IDataReader reader = _procedures.GetPostsByCategoryID(itemCount, categoryId, true, BlogId, CurrentDateTime))
            {
                ICollection<EntryDay> edc = DataHelper.LoadEntryDayCollection(reader);
                return edc;
            }
        }

        /// <summary>
        /// Returns the previous and next entry to the specified entry.
        /// </summary>
        /// <param name="entryId"></param>
        /// <returns></returns>
        /// <param name="postType"></param>
        public override ICollection<Entry> GetPreviousAndNextEntries(int entryId, PostType postType)
        {
            using (IDataReader reader = _procedures.GetEntryPreviousNext(entryId, (int)postType, BlogId, CurrentDateTime))
            {
                return DataHelper.LoadEntryCollectionFromDataReader(reader);
            }
        }

        /// <summary>
        /// Returns the posts for the specified month for the Month Archive section.
        /// </summary>
        /// <param name="month"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        public override ICollection<Entry> GetPostsByMonth(int month, int year)
        {
            using (IDataReader reader = _procedures.GetPostsByMonth(month, year, BlogId, CurrentDateTime))
            {
                return DataHelper.LoadEntryCollectionFromDataReader(reader);
            }
        }

        public override ICollection<Entry> GetPostsByDayRange(DateTime start, DateTime stop, PostType postType, bool activeOnly)
        {
            DateTime min = start;
            DateTime max = stop;

            if (stop < start)
            {
                min = stop;
                max = start;
            }

            using (IDataReader reader = _procedures.GetEntriesByDayRange(min, max, (int)postType, activeOnly, BlogId, CurrentDateTime))
            {
                return DataHelper.LoadEntryCollectionFromDataReader(reader);
            }
        }

        public override ICollection<Entry> GetEntriesByCategory(int itemCount, int categoryId, bool activeOnly)
        {
            using (IDataReader reader = _procedures.GetPostsByCategoryID(itemCount, categoryId, activeOnly, BlogId, CurrentDateTime))
            {
                return DataHelper.LoadEntryCollectionFromDataReader(reader);
            }
        }

        public override ICollection<Entry> GetEntriesByTag(int itemCount, string tagName)
        {
            using (IDataReader reader = _procedures.GetPostsByTag(itemCount, tagName, BlogId, true, CurrentDateTime))
            {
                return DataHelper.LoadEntryCollectionFromDataReader(reader);
            }
        }

        /// <summary>
        /// Returns an active <see cref="Entry" /> by the id regardless of which blog it is 
        /// located in.
        /// </summary>
        /// <param name="id">Id of the entry</param>
        /// <param name="includeCategories">Whether the entry should have its Categories property populated</param>
        /// <returns></returns>
        public override Entry GetEntry(int id, bool includeCategories)
        {
            using (IDataReader reader = _procedures.GetEntryReader(id, includeCategories))
            {
                if (reader.Read())
                {
                    return DataHelper.LoadEntry(reader);
                }
                return null;
            }
        }

        /// <summary>
        /// Returns an <see cref="Entry" /> with the specified id.
        /// </summary>
        /// <param name="id">Id of the entry</param>
        /// <param name="activeOnly">Whether or not to only return the entry if it is active.</param>
        /// <param name="includeCategories">Whether the entry should have its Categories property populated</param>
        /// <returns></returns>
        public override Entry GetEntry(int id, bool activeOnly, bool includeCategories)
        {
            using (IDataReader reader = _procedures.GetEntryReader(BlogId, id, activeOnly, includeCategories))
            {
                if (reader.Read())
                {
                    return DataHelper.LoadEntryWithCategories(reader);
                }
                return null;
            }
        }

        /// <summary>
        /// Returns an <see cref="Entry" /> with the specified entry name.
        /// </summary>
        /// <param name="entryName">Url friendly entry name.</param>
        /// <param name="activeOnly">Whether or not to only return the entry if it is active.</param>
        /// <param name="includeCategories">Whether the entry should have its Categories property populated</param>
        /// <returns></returns>
        public override Entry GetEntry(string entryName, bool activeOnly, bool includeCategories)
        {
            using (IDataReader reader = _procedures.GetEntryReader(BlogId, 
                entryName, 
                activeOnly, 
                includeCategories))
            {
                if (reader.Read())
                {
                    return DataHelper.LoadEntryWithCategories(reader);
                }
                return null;
            }
        }

        /// <summary>
        /// Deletes the specified entry.
        /// </summary>
        /// <param name="entryId">The entry id.</param>
        /// <returns></returns>
        public override bool DeleteEntry(int entryId)
        {
            return _procedures.DeletePost(entryId, CurrentDateTime);
        }

        /// <summary>
        /// Creates the specified entry in the back end data store attaching 
        /// the specified category ids.
        /// </summary>
        /// <param name="entry">Entry.</param>
        /// <param name="categoryIds">Category I ds.</param>
        /// <returns></returns>
        public override int Create(Entry entry, IEnumerable<int> categoryIds)
        {
            if (!ValidateEntry(entry)) {
                throw new BlogFailedPostException("Failed post exception");
            }

            entry.Id = _procedures.InsertEntry(entry.Title
                , entry.Body.NullIfEmpty()
                , (int)entry.PostType
                , entry.Author.NullIfEmpty()
                , entry.Email.NullIfEmpty()
                , entry.Description.NullIfEmpty()
                , BlogId
                , entry.DateCreated
                , (int)entry.PostConfig
                , entry.EntryName.NullIfEmpty()
                , entry.DateSyndicated.NullIfEmpty()
                , CurrentDateTime);

            if (categoryIds != null) {
                SetEntryCategoryList(entry.Id, categoryIds);
            }

            if (entry.Id > -1) {
                Config.CurrentBlog.LastUpdated = entry.DateCreated;
            }

            return entry.Id;
        }

        /// <summary>
        /// Saves the categories for the specified post.
        /// </summary>
        /// <param name="postId"></param>
        /// <param name="categoryIds"></param>
        /// <returns></returns>
        public override bool SetEntryCategoryList(int entryId, IEnumerable<int> categoryIds)
        {
            if (categoryIds == null)
                return _procedures.InsertLinkCategoryList(string.Empty, entryId, BlogId); ;

            var idsAsStrings = categoryIds.Select(id => id.ToString(CultureInfo.InvariantCulture));
            string catList = string.Join(",", idsAsStrings.ToArray());

            return _procedures.InsertLinkCategoryList(catList, entryId, BlogId);
        }

        /// <summary>
        /// Saves the tags for the specified post
        /// </summary>
        /// <param name="postId">The EntryId for the post to update</param>
        /// <param name="tags">
        /// An array of tag strings for the associated post. If there are no tags
        /// associated with the post, pass tags with length zero to remove post tags
        /// if present.
        /// </param>
        /// <returns></returns>
        public override bool SetEntryTagList(int postId, IEnumerable<string> tags)
        {
            if (tags == null)
                throw new ArgumentNullException("tags", "Tags cannot be null.");

            string tagList = "";
            foreach (string tag in tags)
            {
                tagList += tag + ",";
            }
            if (tagList.Length > 0)
                tagList = tagList.Substring(0, tagList.Length - 1);

            return _procedures.InsertEntryTagList(postId, BlogId, tagList);
        }

        /// <summary>
        /// Saves changes to the specified entry attaching the specified categories.
        /// </summary>
        /// <param name="entry">Entry.</param>
        /// <param name="categoryIds">Category Ids.</param>
        /// <returns></returns>
        public override bool Update(Entry entry, params int[] categoryIds)
        {
            if (!ValidateEntry(entry)) {
                throw new BlogFailedPostException("Failed post exception");
            }

            if (entry.IsActive && NullValue.IsNull(entry.DateSyndicated)) {
                entry.DateSyndicated = CurrentDateTime;
            }

            bool updated = _procedures.UpdateEntry(
                entry.Id
                , entry.Title ?? string.Empty
                , entry.Body.NullIfEmpty()
                , (int)entry.PostType
                , entry.Author.NullIfEmpty()
                , entry.Email.NullIfEmpty()
                , entry.Description.NullIfEmpty()
                , entry.DateModified
                , (int)entry.PostConfig
                , entry.EntryName.NullIfEmpty()
                , entry.DateSyndicated.NullIfEmpty()
                , BlogId
                , CurrentDateTime);

            if (!updated)
            {
                return false;
            }

            if (categoryIds != null && categoryIds.Length > 0)
            {
                SetEntryCategoryList(entry.Id, categoryIds);
            }

            if (Config.Settings.Tracking.UseTrackingServices) {
                if (entry.Id > -1) {
                    Config.CurrentBlog.LastUpdated = entry.DateModified;
                }
            }
            return true;
        }

        public override ICollection<ArchiveCount> GetPostCountsByMonth()
        {
            using (IDataReader reader = _procedures.GetPostsByMonthArchive(BlogId, CurrentDateTime))
            {
                ICollection<ArchiveCount> acc = DataHelper.LoadArchiveCount(reader);
                return acc;
            }
        }

        public override ICollection<ArchiveCount> GetPostCountsByYear()
        {
            using (IDataReader reader = _procedures.GetPostsByYearArchive(BlogId, CurrentDateTime))
            {
                ICollection<ArchiveCount> acc = DataHelper.LoadArchiveCount(reader);
                return acc;
            }
        }

        public override ICollection<ArchiveCount> GetPostCountsByCategory()
        {
            using (IDataReader reader = _procedures.GetPostsByCategoriesArchive(BlogId))
            {
                ICollection<ArchiveCount> acc = DataHelper.LoadArchiveCount(reader);
                return acc;
            }
        }
    }
}
