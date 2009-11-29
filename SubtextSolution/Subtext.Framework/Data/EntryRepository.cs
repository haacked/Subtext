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
using System.Data;
using System.Globalization;
using System.Linq;
using Subtext.Extensibility;
using Subtext.Extensibility.Interfaces;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;
using Subtext.Framework.Text;

namespace Subtext.Framework.Data
{
    public partial class DatabaseObjectProvider
    {
        /// <summary>
        /// Returns a pageable collection of entries ordered by the id descending.
        /// This is used in the admin section.
        /// </summary>
        public override IPagedCollection<EntryStatsView> GetEntries(PostType postType, int? categoryId, int pageIndex, int pageSize)
        {
            using(IDataReader reader = _procedures.GetEntries(BlogId, categoryId, pageIndex, pageSize, (int)postType))
            {
                return reader.ReadPagedCollection(r => reader.ReadEntryStatsView());
            }
        }

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
            using(IDataReader reader = _procedures.GetConditionalEntries(itemCount,
                (int)postType,
                (int)postConfig,
                BlogId,
                includeCategories,
                CurrentDateTime))
            {
                return reader.ReadEntryCollection();
            }
        }

        public override ICollection<Entry> GetEntriesByCategory(int itemCount, int categoryId, bool activeOnly)
        {
            using(IDataReader reader = _procedures.GetPostsByCategoryID(itemCount, categoryId, activeOnly, BlogId, CurrentDateTime))
            {
                return reader.ReadEntryCollection();
            }
        }

        public override ICollection<Entry> GetEntriesByTag(int itemCount, string tagName)
        {
            using(IDataReader reader = _procedures.GetPostsByTag(itemCount, tagName, BlogId, true, CurrentDateTime))
            {
                return reader.ReadEntryCollection();
            }
        }

        public override ICollection<EntryStatsView> GetPopularEntries(int blogId, DateFilter filter)
        {
            DateTime? minDate = null;
            if(filter == DateFilter.LastMonth)
            {
                minDate = CurrentDateTime.AddMonths(-1);
            }
            else if(filter == DateFilter.LastWeek)
            {
                minDate = CurrentDateTime.AddDays(-7);
            }

            using(IDataReader reader = _procedures.GetPopularPosts(BlogId, minDate))
            {
                return reader.ReadCollection(r =>
                    {
                        var entry = r.ReadEntryStatsView();
                        entry.PostType = PostType.BlogPost;
                        return entry;
                    });
            }
        }

        public override IPagedCollection<EntryStatsView> GetEntriesForExport(int pageIndex, int pageSize)
        {
            using(IDataReader reader = _procedures.GetEntriesForExport(BlogId, pageIndex, pageSize))
            {
                var entries = reader.ReadEntryCollection<EntryStatsView, IPagedCollection<EntryStatsView>>(r => r.ReadPagedCollection(innerReader => innerReader.ReadEntryStatsView()));
                if(reader.NextResult())
                {
                    var comments = reader.ReadEnumerable(r => r.ReadFeedbackItem());
                    entries.Accumulate(comments, entry => entry.Id, comment => comment.EntryId, 
                        (entry, comment) => { entry.Comments.Add(comment); comment.Entry = entry;});

                    if(reader.NextResult())
                    {
                        var trackBacks = reader.ReadEnumerable(r => r.ReadFeedbackItem());
                        entries.Accumulate(trackBacks, entry => entry.Id, trackback => trackback.EntryId,
                            (entry, trackback) => { entry.Comments.Add(trackback); trackback.Entry = entry; });
                    }
                }
                return entries;
            }
        }

        public override EntryDay GetEntryDay(DateTime dateTime)
        {
            using(IDataReader reader = _procedures.GetEntriesByDayRange(dateTime.Date, dateTime.Date.AddDays(1), (int)PostType.BlogPost, true, BlogId, CurrentDateTime))
            {
                var entryDay = new EntryDay(dateTime);
                while(reader.Read())
                {
                    entryDay.Add(reader.ReadEntry());
                }
                return entryDay;
            }
        }

        /// <summary>
        /// Returns the previous and next entry to the specified entry.
        /// </summary>
        /// <param name="entryId"></param>
        /// <returns></returns>
        /// <param name="postType"></param>
        public override ICollection<EntrySummary> GetPreviousAndNextEntries(int entryId, PostType postType)
        {
            using(IDataReader reader = _procedures.GetEntryPreviousNext(entryId, (int)postType, BlogId, CurrentDateTime))
            {
                return reader.ReadCollection<EntrySummary>();
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
            using(IDataReader reader = _procedures.GetPostsByMonth(month, year, BlogId, CurrentDateTime))
            {
                return reader.ReadEntryCollection();
            }
        }

        public override ICollection<Entry> GetPostsByDayRange(DateTime start, DateTime stop, PostType postType, bool activeOnly)
        {
            DateTime min = start;
            DateTime max = stop;

            if(stop < start)
            {
                min = stop;
                max = start;
            }

            using(IDataReader reader = _procedures.GetEntriesByDayRange(min, max, (int)postType, activeOnly, BlogId, CurrentDateTime))
            {
                return reader.ReadEntryCollection();
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
            using(IDataReader reader = _procedures.GetEntryReader(BlogId, id, activeOnly, includeCategories))
            {
                if(reader.Read())
                {
                    return DataHelper.ReadEntryWithCategories(reader);
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
            using(IDataReader reader = _procedures.GetEntryReader(BlogId,
                entryName,
                activeOnly,
                includeCategories))
            {
                if(reader.Read())
                {
                    return DataHelper.ReadEntryWithCategories(reader);
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
            ValidateEntry(entry);

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

            if(categoryIds != null)
            {
                SetEntryCategoryList(entry.Id, categoryIds);
            }

            if(entry.Id > -1)
            {
                Config.CurrentBlog.LastUpdated = entry.DateCreated;
            }

            return entry.Id;
        }

        /// <summary>
        /// Saves the categories for the specified post.
        /// </summary>
        public override bool SetEntryCategoryList(int entryId, IEnumerable<int> categoryIds)
        {
            if(categoryIds == null)
            {
                return _procedures.InsertLinkCategoryList(string.Empty, entryId, BlogId);
            }

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
            if(tags == null)
                throw new ArgumentNullException("tags");

            string tagList = "";
            foreach(string tag in tags)
            {
                tagList += tag + ",";
            }
            if(tagList.Length > 0)
                tagList = tagList.Substring(0, tagList.Length - 1);

            return _procedures.InsertEntryTagList(postId, BlogId, tagList);
        }

        /// <summary>
        /// Saves changes to the specified entry attaching the specified categories.
        /// </summary>
        /// <param name="entry">Entry.</param>
        /// <param name="categoryIds">Category Ids.</param>
        /// <returns></returns>
        public override bool Update(Entry entry, IEnumerable<int> categoryIds)
        {
            ValidateEntry(entry);

            if(entry.IsActive && NullValue.IsNull(entry.DateSyndicated))
            {
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

            if(!updated)
            {
                return false;
            }

            if(categoryIds != null && categoryIds.Count() > 0)
            {
                SetEntryCategoryList(entry.Id, categoryIds);
            }

            if(Config.Settings.Tracking.UseTrackingServices)
            {
                if(entry.Id > -1)
                {
                    Config.CurrentBlog.LastUpdated = entry.DateModified;
                }
            }
            return true;
        }

        public override ICollection<ArchiveCount> GetPostCountsByMonth()
        {
            using(IDataReader reader = _procedures.GetPostsByMonthArchive(BlogId, CurrentDateTime))
            {
                ICollection<ArchiveCount> acc = DataHelper.ReadArchiveCount(reader);
                return acc;
            }
        }

        public override ICollection<ArchiveCount> GetPostCountsByYear()
        {
            using(IDataReader reader = _procedures.GetPostsByYearArchive(BlogId, CurrentDateTime))
            {
                ICollection<ArchiveCount> acc = DataHelper.ReadArchiveCount(reader);
                return acc;
            }
        }

        public override ICollection<ArchiveCount> GetPostCountsByCategory()
        {
            using(IDataReader reader = _procedures.GetPostsByCategoriesArchive(BlogId))
            {
                ICollection<ArchiveCount> acc = DataHelper.ReadArchiveCount(reader);
                return acc;
            }
        }
    }
}
