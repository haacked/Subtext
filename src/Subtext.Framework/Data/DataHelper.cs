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
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Net;
using Subtext.Extensibility;
using Subtext.Extensibility.Interfaces;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;

//Need to remove Global.X calls ...just seems unclean
//Maybe create a another class formatter ...Format.Entry(ref Entry entry) 
//or, Instead of Globals.PostUrl(int id) --> Globals.PostUrl(ref Entry entry)
//...

namespace Subtext.Framework.Data
{
    /// <summary>
    /// Contains helper methods for getting blog entries from the database 
    /// into objects such as <see cref="Entry" />
    /// </summary>
    public static class DataHelper
    {
        public static IEnumerable<T> ReadEnumerable<T>(this IDataReader reader, Func<IDataReader, T> map)
        {
            while(reader.Read())
            {
                yield return map(reader);
            }
        }

        public static IPagedCollection<T> ReadPagedCollection<T>(this IDataReader reader, Func<IDataReader, T> map)
        {
            var collection = new PagedCollection<T>(reader.ReadEnumerable(map).ToList());
            reader.NextResult();
            reader.Read();
            collection.MaxItems = reader.ReadValue<int>("TotalRecords");
            return collection;
        }

        public static T ReadObject<T>(this IDataReader reader, params string[] exclusionList) where T : new()
        {
            var item = new T();
            reader.ReadObject(item, exclusionList);
            return item;
        }

        public static T ReadObject<T>(this IDataReader reader, T item, params string[] exclusionList)
        {
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(item);
            foreach(PropertyDescriptor property in properties)
            {
                if(property.IsReadOnly)
                {
                    continue;
                }

                if(!property.PropertyType.IsReadablePropertyType())
                {
                    continue;
                }

                if(exclusionList != null && exclusionList.IsExcluded(property.Name))
                {
                    continue;
                }

                // We need to catch this exception in cases when we're upgrading and the column might not exist yet.
                // It'd be nice to have a cleaner way of doing this.
                try
                {
                    object value = reader[property.Name];
                    if(value != DBNull.Value)
                    {
                        if(property.PropertyType != typeof(Uri))
                        {
                            property.SetValue(item, value);    
                        }
                        else
                        {
                            var url = value as string;
                            if(!String.IsNullOrEmpty(url))
                            {
                                Uri uri;
                                if(Uri.TryCreate(url, UriKind.RelativeOrAbsolute, out uri))
                                {
                                    property.SetValue(item, uri);
                                }
                            }
                        }
                    }
                }
                catch(IndexOutOfRangeException)
                {
                    if(typeof(T) != typeof(HostInfo))
                    {
                        throw;
                    }
                }
            }
            return item;
        }

        public static T ReadValue<T>(this IDataReader reader, string columnName)
        {
            return reader.ReadValue(columnName, default(T));
        }

        public static T ReadValue<T>(this IDataReader reader, string columnName, T defaultValue)
        {
            return reader.ReadValue(columnName, value => (T)value, defaultValue);
        }

        public static T ReadValue<T>(this IDataReader reader, string columnName, Func<object, T> map, T defaultValue)
        {
            try
            {
                object value = reader[columnName];
                if(value != null && value != DBNull.Value)
                {
                    return map(value);
                }
                return defaultValue;
            }
            catch(FormatException)
            {
                return defaultValue;
            }
            catch(IndexOutOfRangeException)
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// Checks the value type and returns null if the 
        /// value is "null-equivalent".
        /// </summary>
        public static int? NullIfMinValue(this int value)
        {
            if(NullValue.IsNull(value))
            {
                return null;
            }
            return value;
        }

        public static DateTime? NullIfEmpty(this DateTime dateTime)
        {
            if(NullValue.IsNull(dateTime))
            {
                return null;
            }
            return dateTime;
        }

        public static Referrer ReadReferrer(IDataReader reader, Blog blog)
        {
            var refer = reader.ReadObject<Referrer>();
            refer.BlogId = blog.Id;
            return refer;
        }

        internal static ICollection<Entry> ReadEntryCollection(this IDataReader reader)
        {
            return reader.ReadEntryCollection(true /* buildLinks */);
        }

        internal static ICollection<Entry> ReadEntryCollection(this IDataReader reader, bool buildLinks)
        {
            return reader.ReadEntryCollection<Entry, List<Entry>>(r => r.ReadEnumerable(innerReader => innerReader.ReadEntry(buildLinks)).ToList());
        }

        internal static TCollection ReadEntryCollection<TEntry, TCollection>(this IDataReader reader, Func<IDataReader, TCollection> collectionFunc) where TCollection : ICollection<TEntry> where TEntry : Entry
        {
            var entries = collectionFunc(reader);
            if(entries.Count > 0 && reader.NextResult())
            {
                var categories = reader.ReadEnumerable(r => new { EntryId = r.ReadValue<int>("PostId"), Title = r.ReadValue<string>("Title") });
                entries.Accumulate(categories, entry => entry.Id, category => category.EntryId, (entry, category) => entry.Categories.Add(category.Title));
            }
            return entries;
        }

        internal static ICollection<TItem> ReadCollection<TItem>(this IDataReader reader, Func<IDataReader, TItem> map)
        {
            return reader.ReadEnumerable(map).ToList();
        }

        internal static ICollection<TItem> ReadCollection<TItem>(this IDataReader reader) where TItem : new()
        {
            return reader.ReadCollection(r => r.ReadObject<TItem>());
        }

        //Crappy. Need to clean up all of the entry references
        public static EntryStatsView ReadEntryStatsView(this IDataReader reader)
        {
            var entry = new EntryStatsView
            {
                BlogId = reader.ReadValue("BlogId",0),
                PostType = ((PostType)reader.ReadValue<int>("PostType")),
                WebCount = reader.ReadValue("WebCount", 0),
                AggCount = reader.ReadValue("AggCount", 0),
                WebLastUpdated = reader.ReadValue<DateTime>("WebLastUpdated"),
                AggLastUpdated = reader.ReadValue<DateTime>("AggLastUpdated"),
                Author = reader.ReadValue<string>("Author"),
                Email = reader.ReadValue<string>("Email"),
                DateCreated = reader.ReadValue<DateTime>("DateCreated"),
                DateModified = reader.ReadValue<DateTime>("DateUpdated"),
                Id = reader.ReadValue<int>("ID"),
                Description = reader.ReadValue<string>("Description"),
                EntryName = reader.ReadValue<string>("EntryName"),
                FeedBackCount = reader.ReadValue<int>("FeedBackCount"),
                Body = reader.ReadValue<string>("Text"),
                Title = reader.ReadValue<string>("Title"),
                PostConfig = (PostConfig)(reader.ReadValue<int>("PostConfig")),
                DateSyndicated = reader.ReadValue<DateTime>("DateSyndicated")
            };

            return entry;
        }

        public static Entry ReadEntry(this IDataReader reader)
        {
            return ReadEntry(reader, true);
        }

        /// <summary>
        /// Only use this when loading a SINGLE entry from a reader.
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static Entry ReadEntryWithCategories(IDataReader reader)
        {
            Entry entry = reader.ReadEntry();
            if(reader.NextResult())
            {
                entry.Categories.AddRange(reader.ReadEnumerable(r => r.ReadValue<string>("Title")).Distinct(StringComparer.Ordinal));
            }
            return entry;
        }

        internal static FeedbackItem ReadFeedbackItem(this IDataReader reader, Entry entry)
        {
            var feedbackItem = new FeedbackItem((FeedbackType)reader.ReadValue<int>("FeedbackType"));
            ReadFeedbackItem(reader, feedbackItem);
            feedbackItem.Entry = entry;
            return feedbackItem;
        }

        internal static FeedbackItem ReadFeedbackItem(this IDataReader reader)
        {
            var feedbackItem = new FeedbackItem((FeedbackType)reader.ReadValue<int>("FeedbackType"));
            ReadFeedbackItem(reader, feedbackItem);
            return feedbackItem;
        }

        private static void ReadFeedbackItem(this IDataReader reader, FeedbackItem feedbackItem)
        {
            feedbackItem.Id = reader.ReadValue<int>("Id");
            feedbackItem.Title = reader.ReadValue<string>("Title");
            feedbackItem.Body = reader.ReadValue<string>("Body");
            feedbackItem.EntryId = reader.ReadValue<int>("EntryId");
            feedbackItem.Author = reader.ReadValue<string>("Author") ?? string.Empty;
            feedbackItem.IsBlogAuthor = reader.ReadValue<bool>("IsBlogAuthor");
            feedbackItem.Email = reader.ReadValue<string>("Email");
            feedbackItem.SourceUrl = ReadUri(reader, "Url");
            feedbackItem.FeedbackType = (FeedbackType)reader.ReadValue<int>("FeedbackType");
            feedbackItem.Status = (FeedbackStatusFlag)reader.ReadValue<int>("StatusFlag");
            feedbackItem.CreatedViaCommentApi = reader.ReadValue<bool>("CommentAPI");
            feedbackItem.Referrer = reader.ReadValue<string>("Referrer");
            feedbackItem.IpAddress = reader.ReadIpAddress("IpAddress");
            feedbackItem.UserAgent = reader.ReadValue<string>("UserAgent");
            feedbackItem.ChecksumHash = reader.ReadValue<string>("FeedbackChecksumHash");
            feedbackItem.DateCreated = reader.ReadValue<DateTime>("DateCreated");
            feedbackItem.DateModified = reader.ReadValue<DateTime>("DateModified");
            feedbackItem.ParentEntryName = reader.ReadValue<string>("ParentEntryName");
            feedbackItem.ParentDateCreated = reader.ReadValue<DateTime>("ParentEntryCreateDate");
            feedbackItem.ParentDateSyndicated = reader.ReadValue<DateTime>("ParentEntryDateSyndicated");
        }

        public static Entry ReadEntry(this IDataReader reader, bool buildLinks)
        {
            var entry = new Entry((PostType)reader.ReadValue<int>("PostType"), null);
            reader.ReadEntry(entry, buildLinks);
            return entry;
        }

        public static Entry ReadEntry(this IDataReader reader, Entry entry, bool buildLinks)
        {
            return reader.ReadEntry(entry, buildLinks, false);
        }

        public static Entry ReadEntry(this IDataReader reader, Entry entry, bool buildLinks, bool includeBlog)
        {
            entry.Author = reader.ReadValue<string>("Author");
            entry.Email = reader.ReadValue<string>("Email");
            entry.DateCreated = reader.ReadValue<DateTime>("DateCreated");
            entry.DateModified = reader.ReadValue<DateTime>("DateUpdated");

            entry.Id = reader.ReadValue<int>("ID");
            entry.Description = reader.ReadValue<string>("Description");
            entry.EntryName = reader.ReadValue<string>("EntryName");

            entry.FeedBackCount = reader.ReadValue("FeedBackCount", 0);
            entry.Body = reader.ReadValue<string>("Text");
            entry.Title = reader.ReadValue<string>("Title");
            entry.PostConfig = (PostConfig)(reader.ReadValue("PostConfig", (int)PostConfig.None));
            entry.DateSyndicated = reader.ReadValue<DateTime>("DateSyndicated");

            var withEnclosure = reader.ReadValue<bool>("EnclosureEnabled");
            if(withEnclosure)
            {
                entry.Enclosure = ReadEnclosure(reader);
            }

            if(includeBlog)
            {
                entry.Blog = ReadBlog(reader);
            }
            return entry;
        }

        public static Blog ReadBlog(this IDataReader reader)
        {
            return reader.ReadBlog(string.Empty);
        }

        public static Blog ReadBlog(this IDataReader reader, string prefix)
        {
            var info = new Blog
            {
                Author = reader.ReadValue<string>(prefix + "Author"),
                Id = reader.ReadValue<int>(prefix + "BlogId"),
                Email = reader.ReadValue<string>(prefix + "Email"),
                Password = reader.ReadValue<string>(prefix + "Password"),
                SubTitle = reader.ReadValue<string>(prefix + "SubTitle"),
                Title = reader.ReadValue<string>(prefix + "Title"),
                UserName = reader.ReadValue<string>(prefix + "UserName"),
                TimeZoneId = reader.ReadValue<string>(prefix + "TimeZoneId"),
                ItemCount = reader.ReadValue<int>(prefix + "ItemCount"),
                CategoryListPostCount = reader.ReadValue<int>(prefix + "CategoryListPostCount"),
                Language = reader.ReadValue<string>(prefix + "Language"),
                PostCount = reader.ReadValue<int>(prefix + "PostCount"),
                CommentCount = reader.ReadValue<int>(prefix + "CommentCount"),
                StoryCount = reader.ReadValue<int>(prefix + "StoryCount"),
                PingTrackCount = reader.ReadValue<int>(prefix + "PingTrackCount"),
                News = reader.ReadValue<string>(prefix + "News"),
                TrackingCode = reader.ReadValue<string>(prefix + "TrackingCode"),
                LastUpdated = reader.ReadValue(prefix + "LastUpdated", new DateTime(2003, 1, 1)),
                Host = reader.ReadValue<string>(prefix + "Host"),
                Subfolder = reader.ReadValue<string>(prefix + "Application"),
                Flag = (ConfigurationFlags)(reader.ReadValue<int>(prefix + "Flag")),
                Skin = new SkinConfig
                {
                    TemplateFolder = reader.ReadValue<string>(prefix + "Skin"),
                    SkinStyleSheet = reader.ReadValue<string>(prefix + "SkinCssFile"),
                    CustomCssText = reader.ReadValue<string>(prefix + "SecondaryCss")
                },
                MobileSkin = new SkinConfig
                {
                    TemplateFolder = reader.ReadValue<string>(prefix + "MobileSkin"),
                    SkinStyleSheet = reader.ReadValue<string>(prefix + "MobileSkinCssFile")
                },
                OpenIdUrl = reader.ReadValue<string>(prefix + "OpenIdUrl"),
                OpenIdServer = reader.ReadValue<string>(prefix + "OpenIdServer"),
                OpenIdDelegate = reader.ReadValue<string>(prefix + "OpenIdDelegate"),
                CardSpaceHash = reader.ReadValue<string>(prefix + "CardSpaceHash"),
                LicenseUrl = reader.ReadValue<string>(prefix + "LicenseUrl"),
                DaysTillCommentsClose = reader.ReadValue(prefix + "DaysTillCommentsClose", int.MaxValue),
                CommentDelayInMinutes = reader.ReadValue<int>(prefix + "CommentDelayInMinutes"),
                NumberOfRecentComments = reader.ReadValue<int>(prefix + "NumberOfRecentComments"),
                RecentCommentsLength = reader.ReadValue<int>(prefix + "RecentCommentsLength"),
                FeedbackSpamServiceKey = reader.ReadValue<string>(prefix + "AkismetAPIKey"),
                RssProxyUrl = reader.ReadValue<string>(prefix + "FeedBurnerName"),
                BlogGroupId = reader.ReadValue<int>(prefix + "BlogGroupId"),
                BlogGroupTitle = reader.ReadValue<string>(prefix + "BlogGroupTitle")
            };
            return info;
        }

        public static ICollection<ArchiveCount> ReadArchiveCount(IDataReader reader)
        {
            const string dateformat = "{0:00}/{1:00}/{2:0000}";
            var acc = new Collection<ArchiveCount>();
            while(reader.Read())
            {
                var ac = new ArchiveCount();
                string dt = string.Format(CultureInfo.InvariantCulture,
                                          dateformat,
                                          reader.ReadValue<int>("Month"),
                                          reader.ReadValue<int>("Day"),
                                          reader.ReadValue<int>("Year"));
                // FIX: BUG SF1423271 Archives Links
                DateTime parsedDate;
                if(
                    !DateTime.TryParseExact(dt, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal,
                                            out parsedDate))
                {
                    break;
                }

                ac.Date = parsedDate;
                ac.Count = reader.ReadValue<int>("Count");
                //TODO: This broke the unit tests: ac.Title = reader.ReadValue<string>("Title");
                //TODO: This broke the unit tests: ac.Id = reader.ReadValue<int>("Id");
                acc.Add(ac);
            }
            return acc;
        }

        public static Image ReadImage(this IDataReader reader)
        {
            return ReadImage(reader, false, false);
        }

        public static Image ReadImage(this IDataReader reader, bool includeBlog, bool includeCategory)
        {
            var image = reader.ReadObject<Image>("CategoryTitle", "LocalDirectoryPath");

            if(includeBlog)
            {
                image.Blog = reader.ReadBlog("Blog.");
            }
            if(includeCategory)
            {
                image.CategoryTitle = reader.ReadValue<string>("Category.Title");
            }
            return image;
        }

        public static IDictionary<string, int> ReadTags(IDataReader reader)
        {
            var tags = new SortedDictionary<string, int>();
            while(reader.Read())
            {
                tags.Add(
                    reader.ReadValue<string>("Name"),
                    reader.ReadValue<int>("TagCount"));
            }
            return tags;
        }

        private static bool IsExcluded(this IEnumerable<string> exclusionList, string propertyName)
        {
            return exclusionList.Any(excludedProperty => excludedProperty == propertyName);
        }

        private static bool IsReadablePropertyType(this Type t)
        {
            bool isReadable = t.IsPrimitive ||
                              t == typeof(DateTime) ||
                              t == typeof(string) ||
                              t == typeof(Uri);

            if(!isReadable)
            {
                //Maybe it's a nullable.
                Type underlyingType = Nullable.GetUnderlyingType(t);
                if(underlyingType != null)
                {
                    return IsReadablePropertyType(underlyingType);
                }
            }
            return isReadable;
        }

        /// <summary>
        /// Reads the string.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <param name="columnName">Name of the coumn.</param>
        /// <returns></returns>
        public static IPAddress ReadIpAddress(this IDataReader reader, string columnName)
        {
            return reader.ReadValue(columnName, value => IPAddress.Parse((string)value), IPAddress.None);
        }

        /// <summary>
        /// Reads an URI from the database.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="columnName"></param>
        /// <returns></returns>
        public static Uri ReadUri(this IDataReader reader, string columnName)
        {
            return reader.ReadValue(columnName, value => new Uri((string)value), null);
        }

        public static SqlParameter MakeInParam(string paramName, object value)
        {
            return new SqlParameter(paramName, value);
        }

        /// <summary>
        /// Make input param.
        /// </summary>
        public static SqlParameter MakeOutParam(string paramName, SqlDbType dbType, int size)
        {
            return MakeParam(paramName, dbType, size, ParameterDirection.Output, null);
        }

        /// <summary>
        /// Make stored procedure param.
        /// </summary>
        public static SqlParameter MakeParam(string paramName, SqlDbType dbType, Int32 size,
                                             ParameterDirection direction, object value)
        {
            SqlParameter param = size > 0 ? new SqlParameter(paramName, dbType, size) : new SqlParameter(paramName, dbType);

            param.Direction = direction;
            if(!(direction == ParameterDirection.Output && value == null))
            {
                param.Value = value;
            }

            return param;
        }

        // Expects that the caller will dispose of the reader.
        public static ICollection<LinkCategory> ReadLinkCategories(this IDataReader reader, bool includeLinks)
        {
            var categories = reader.ReadEnumerable(r => r.ReadLinkCategory()).ToList();
            
            if(includeLinks && reader.NextResult())
            {
                var links = reader.ReadEnumerable(r => r.ReadObject<Link>());
                categories.Accumulate(links, category => category.Id, link => link.CategoryId, (category, link) => category.Links.Add(link));
            }

            return categories;
        }

        public static LinkCategory ReadLinkCategory(this IDataReader reader)
        {
            var lc = new LinkCategory(reader.ReadValue<int>("CategoryId"), reader.ReadValue<string>("Title")) { IsActive = (bool)reader["Active"] };
            if(reader["CategoryType"] != DBNull.Value)
            {
                lc.CategoryType = (CategoryType)((byte)reader["CategoryType"]);
            }
            if(reader["Description"] != DBNull.Value)
            {
                lc.Description = reader.ReadValue<string>("Description");
            }
            lc.BlogId = reader["BlogId"] != DBNull.Value ? reader.ReadValue<int>("BlogId") : Config.CurrentBlog.Id;
            return lc;
        }

        public static Enclosure ReadEnclosure(this IDataReader reader)
        {
            var enclosure = new Enclosure
            {
                Id = reader.ReadValue<int>("EnclosureId"),
                Title = reader.ReadValue<string>("EnclosureTitle"),
                Url = reader.ReadValue<string>("EnclosureUrl"),
                MimeType = reader.ReadValue<string>("EnclosureMimeType"),
                Size = reader.ReadValue<long>("EnclosureSize"),
                EntryId = reader.ReadValue<int>("ID"),
                AddToFeed = reader.ReadValue<bool>("AddToFeed"),
                ShowWithPost = reader.ReadValue<bool>("ShowWithPost")
            };

            return enclosure;
        }
    }

    /// <summary>
    /// Sort direction.
    /// </summary>
    public enum SortDirection
    {
        None = 0,
        /// <summary>Sort ascending</summary>
        Ascending,
        /// <summary>Sort descending</summary>
        Descending
    }
}