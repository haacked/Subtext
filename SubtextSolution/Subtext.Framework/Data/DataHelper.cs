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

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Net;
using Subtext.Extensibility;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;
using Subtext.Framework.Logging;

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
        public static DateTime? NullIfEmpty(this DateTime dateTime)
        {
            if (NullValue.IsNull(dateTime))
            {
                return null;
            }
            return dateTime;
        }

		public static Referrer LoadReferrer(IDataReader reader)
		{
            Referrer refer = reader.LoadObject<Referrer>();
            refer.BlogId = Config.CurrentBlog.Id;
			return refer;
		}

		private static bool IsNewDay(DateTime dtCurrent, DateTime dtDay)
		{
			return !(dtCurrent.DayOfYear == dtDay.DayOfYear && dtCurrent.Year == dtDay.Year);
		}

        public static ICollection<EntryDay> LoadEntryDayCollection(IDataReader reader)
		{
			DateTime dt = new DateTime(1900, 1, 1);
			List<EntryDay> edc = new List<EntryDay>();
			EntryDay day = null;

			while(reader.Read())
			{
				if(IsNewDay(dt, (DateTime)reader["DateCreated"]))
				{
					dt = (DateTime)reader["DateCreated"];
					day = new EntryDay(dt);
					edc.Add(day);
				}
				day.Add(LoadEntry(reader));
			}
			return edc;
		}

        internal static ICollection<Entry> LoadEntryCollectionFromDataReader(this IDataReader reader) {
            return reader.LoadEntryCollectionFromDataReader(true /* buildLinks */);
        }

        internal static ICollection<Entry> LoadEntryCollectionFromDataReader(this IDataReader reader, bool buildLinks)
        {
            var entries = new Dictionary<int, Entry>();
            while(reader.Read())
            {
                var entry = reader.LoadEntry(buildLinks);
                entries.Add(entry.Id, entry);
            }

            if(entries.Count > 0 && reader.NextResult())
            {
                //Categories...
                while(reader.Read())
                {
                    int entryId = ReadInt32(reader, "Id");
                    string categoryTitle = ReadString(reader, "Title");
                    Entry entry;
                    if (entries.TryGetValue(entryId, out entry)) {
                        entry.Categories.Add(categoryTitle);
                    }
                }
            }
            return entries.Values;
        }

		//Crappy. Need to clean up all of the entry references
		public static EntryStatsView LoadEntryStatsView(IDataReader reader)
		{
			EntryStatsView entry = new EntryStatsView();

			entry.PostType = ((PostType)ReadInt32(reader, "PostType"));

			if(reader["WebCount"] != DBNull.Value)
			{
				entry.WebCount = ReadInt32(reader, "WebCount");	
			}

			if(reader["AggCount"] != DBNull.Value)
			{
				entry.AggCount = ReadInt32(reader, "AggCount");	
			}

			if(reader["WebLastUpdated"] != DBNull.Value)
			{
				entry.WebLastUpdated = (DateTime)reader["WebLastUpdated"];	
			}
			
			if(reader["AggLastUpdated"] != DBNull.Value)
			{
				entry.AggLastUpdated = (DateTime)reader["AggLastUpdated"];	
			}

			if(reader["Author"] != DBNull.Value)
			{
				entry.Author = ReadString(reader, "Author");
			}
			if(reader["Email"] != DBNull.Value)
			{
				entry.Email = ReadString(reader, "Email");
			}
			entry.DateCreated = (DateTime)reader["DateCreated"];
			
			if(reader["DateUpdated"] != DBNull.Value)
			{
				entry.DateModified = (DateTime)reader["DateUpdated"];
			}

			entry.Id = ReadInt32(reader, "ID");

			if(reader["Description"] != DBNull.Value)
			{
				entry.Description = ReadString(reader, "Description");
			}

			if(reader["EntryName"] != DBNull.Value)
			{
				entry.EntryName = ReadString(reader, "EntryName");
			}

			if(reader["FeedBackCount"] != DBNull.Value)
			{
				entry.FeedBackCount = ReadInt32(reader, "FeedBackCount");
			}

			if(reader["Text"] != DBNull.Value)
			{
				entry.Body = ReadString(reader, "Text");
			}

			if(reader["Title"] != DBNull.Value)
			{
				entry.Title =ReadString(reader, "Title");
			}

			if(reader["PostConfig"] != DBNull.Value)
			{
				entry.PostConfig = (PostConfig)(ReadInt32(reader, "PostConfig"));
			}

			if(reader["DateSyndicated"] != DBNull.Value)
			{
				entry.DateSyndicated = (DateTime)reader["DateSyndicated"];
			}

			return entry;
		}

		public static Entry LoadEntry(this IDataReader reader)
		{
			return LoadEntry(reader, true);
		}

		/// <summary>
		/// Only use this when loading a SINGLE entry from a reader.
		/// </summary>
		/// <param name="reader"></param>
		/// <returns></returns>
		public static Entry LoadEntryWithCategories(IDataReader reader)
		{
			Entry entry = LoadEntry(reader);
			if(reader.NextResult())
			{
				while(reader.Read())
				{
					string categoryTitle = ReadString(reader, "Title");
					if(!entry.Categories.Contains(categoryTitle))
					{
						entry.Categories.Add(categoryTitle);
					}
				}
			}
			return entry;
		}

		internal static FeedbackItem LoadFeedbackItem(this IDataReader reader, Entry entry)
		{
			FeedbackItem feedbackItem = new FeedbackItem((FeedbackType)ReadInt32(reader, "FeedbackType"));
			LoadFeedbackItem(reader, feedbackItem);
			feedbackItem.Entry = entry;
			return feedbackItem;
		}

		internal static FeedbackItem LoadFeedbackItem(this IDataReader reader)
		{
			FeedbackItem feedbackItem = new FeedbackItem((FeedbackType)ReadInt32(reader, "FeedbackType"));
			LoadFeedbackItem(reader, feedbackItem);
			return feedbackItem;
		}

		private static void LoadFeedbackItem(this IDataReader reader, FeedbackItem feedbackItem)
		{
			feedbackItem.Id = ReadInt32(reader, "Id");
			feedbackItem.Title = ReadString(reader, "Title");
			feedbackItem.Body = ReadString(reader, "Body");
			feedbackItem.EntryId = ReadInt32(reader, "EntryId");
			feedbackItem.Author = ReadString(reader, "Author") ?? string.Empty;
			feedbackItem.IsBlogAuthor = ReadBoolean(reader, "IsBlogAuthor");
			feedbackItem.Email = ReadString(reader, "Email");
			feedbackItem.SourceUrl = ReadUri(reader, "Url");
			feedbackItem.FeedbackType = (FeedbackType)ReadInt32(reader, "FeedbackType");
			feedbackItem.Status = (FeedbackStatusFlag)ReadInt32(reader, "StatusFlag");
			feedbackItem.CreatedViaCommentAPI = ReadBoolean(reader, "CommentAPI");
			feedbackItem.Referrer = ReadString(reader, "Referrer");
			feedbackItem.IpAddress = ReadIpAddress(reader, "IpAddress");
			feedbackItem.UserAgent = ReadString(reader, "UserAgent");
			feedbackItem.ChecksumHash = ReadString(reader, "FeedbackChecksumHash");
			
			feedbackItem.DateCreated = ReadDate(reader, "DateCreated");
			feedbackItem.DateModified = ReadDate(reader, "DateModified");
			feedbackItem.ParentEntryName = ReadString(reader, "ParentEntryName");
			feedbackItem.ParentDateCreated = ReadDate(reader, "ParentEntryCreateDate");
		}

		public static Entry LoadEntry(this IDataReader reader, bool buildLinks)
		{
			Entry entry = new Entry((PostType)ReadInt32(reader, "PostType"));
			LoadEntry(reader, entry, buildLinks);
			return entry;
		}

        public static Entry LoadEntry(this IDataReader reader, Entry entry, bool buildLinks)
        {
            return LoadEntry(reader, entry, buildLinks, false);
        }

		public static Entry LoadEntry(this IDataReader reader, Entry entry, bool buildLinks, bool includeBlog)
		{
			entry.Author = ReadString(reader, "Author");
			entry.Email = ReadString(reader, "Email");
            entry.DateCreated = ReadDate(reader, "DateCreated");
			entry.DateModified = ReadDate(reader, "DateUpdated");
			
			entry.Id = ReadInt32(reader, "ID");
			entry.Description = ReadString(reader, "Description");
			entry.EntryName = ReadString(reader, "EntryName");
	
			entry.FeedBackCount = ReadInt32(reader, "FeedBackCount", 0);
			entry.Body = ReadString(reader, "Text");
			entry.Title = ReadString(reader, "Title");
			entry.PostConfig = (PostConfig)(ReadInt32(reader, "PostConfig", (int)PostConfig.None));			
			entry.DateSyndicated = ReadDate(reader, "DateSyndicated");

		    bool withEnclosure = ReadBoolean(reader, "EnclosureEnabled");
            if (withEnclosure)
            {
                entry.Enclosure = LoadEnclosure(reader);
            }
	
            if (includeBlog) {
                entry.Blog = LoadBlog(reader);
            }
            return entry;
		}

		internal static int GetMaxItems(IDataReader reader)
		{
			reader.Read();
			return ReadInt32(reader, "TotalRecords");
		}

		public static LinkCategory LoadLinkCategory(this IDataReader reader)
		{
			LinkCategory lc = new LinkCategory(reader.ReadInt32("CategoryID"), reader.ReadString("Title"));
			lc.IsActive = (bool)reader["Active"];
			if(reader["CategoryType"] != DBNull.Value) {
				lc.CategoryType = (CategoryType)((byte)reader["CategoryType"]);
			}
			if(reader["Description"] != DBNull.Value) {
				lc.Description = reader.ReadString("Description");
			}
            if (reader["BlogId"] != DBNull.Value) {
                lc.BlogId = reader.ReadInt32("BlogId");
            }
            else {
                lc.BlogId = Config.CurrentBlog.Id;
            }
			return lc;
		}

        public static Blog LoadBlog(this IDataReader reader) {
            return reader.LoadBlog(string.Empty);
        }

		public static Blog LoadBlog(this IDataReader reader, string prefix)
		{
			Blog info = new Blog();
			info.Author = reader.ReadString(prefix + "Author");
			info.Id = reader.ReadInt32(prefix + "BlogId");
			info.Email = reader.ReadString(prefix + "Email");
			info.Password = reader.ReadString(prefix + "Password");
            info.OpenIDUrl = reader.ReadString(prefix + "OpenIDUrl");
            info.CardSpaceHash = reader.ReadString(prefix + "CardSpaceHash");

			info.SubTitle = reader.ReadString(prefix + "SubTitle");
			info.Title = reader.ReadString(prefix + "Title");
            info.UserName = reader.ReadString(prefix + "UserName");
            info.TimeZoneId = reader.ReadInt32(prefix + "TimeZone");
			info.ItemCount = reader.ReadInt32(prefix + "ItemCount");
			info.CategoryListPostCount = reader.ReadInt32(prefix + "CategoryListPostCount");
			info.Language = reader.ReadString(prefix + "Language");

			info.PostCount = reader.ReadInt32(prefix + "PostCount");
            info.CommentCount = reader.ReadInt32(prefix + "CommentCount");
            info.StoryCount = reader.ReadInt32(prefix + "StoryCount");
            info.PingTrackCount = reader.ReadInt32(prefix + "PingTrackCount");
            info.News = reader.ReadString(prefix + "News");
            info.TrackingCode = reader.ReadString(prefix + "TrackingCode");

            info.LastUpdated = reader.ReadDate(prefix + "LastUpdated", new DateTime(2003, 1, 1));
			info.Host = reader.ReadString(prefix + "Host");
			// The Subfolder property is stored in the Application column. 
			// This is a result of the legacy schema.
            info.Subfolder = reader.ReadString(prefix + "Application");

            info.Flag = (ConfigurationFlags)(reader.ReadInt32(prefix + "Flag"));

			info.Skin = new SkinConfig();
			info.Skin.TemplateFolder = reader.ReadString(prefix + "Skin");
			info.Skin.SkinStyleSheet = reader.ReadString(prefix + "SkinCssFile");
            info.Skin.CustomCssText = reader.ReadString(prefix + "SecondaryCss");
            info.MobileSkin = new SkinConfig();
            info.MobileSkin.TemplateFolder = reader.ReadString(prefix + "MobileSkin");
            info.MobileSkin.SkinStyleSheet = reader.ReadString(prefix + "MobileSkinCssFile");

            info.OpenIDUrl = reader.ReadString(prefix + "OpenIDUrl");
            info.OpenIDServer = reader.ReadString(prefix + "OpenIDServer");
            info.OpenIDDelegate = reader.ReadString(prefix + "OpenIDDelegate");
            info.CardSpaceHash = reader.ReadString(prefix + "CardSpaceHash");
			
			info.LicenseUrl = reader.ReadString(prefix + "LicenseUrl");
			
			info.DaysTillCommentsClose = reader.ReadInt32(prefix + "DaysTillCommentsClose", int.MaxValue);
			info.CommentDelayInMinutes = reader.ReadInt32(prefix + "CommentDelayInMinutes");
			info.NumberOfRecentComments = reader.ReadInt32(prefix + "NumberOfRecentComments");
			info.RecentCommentsLength = reader.ReadInt32(prefix + "RecentCommentsLength");
			info.FeedbackSpamServiceKey = reader.ReadString(prefix + "AkismetAPIKey");
			info.FeedBurnerName = reader.ReadString(prefix + "FeedBurnerName");

            info.BlogGroupId = reader.ReadInt32(prefix + "BlogGroupId");
            info.BlogGroupTitle = reader.ReadString(prefix + "BlogGroupTitle");
			return info;
		}

        public static ICollection<ArchiveCount> LoadArchiveCount(IDataReader reader)
		{
			const string dateformat = "{0:00}/{1:00}/{2:0000}";
            var acc = new Collection<ArchiveCount>();
			while(reader.Read())
			{
				var ac = new ArchiveCount();
				string dt = string.Format(CultureInfo.InvariantCulture, 
                    dateformat, 
                    ReadInt32(reader, "Month"), 
                    ReadInt32(reader, "Day"), 
                    ReadInt32(reader, "Year"));
				// FIX: BUG SF1423271 Archives Links
                DateTime parsedDate;
                if (!DateTime.TryParseExact(dt, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal, out parsedDate))
                    break;

                ac.Date = parsedDate;
				ac.Count = ReadInt32(reader, "Count");
                //TODO: This broke the unit tests: ac.Title = ReadString(reader, "Title");
				//TODO: This broke the unit tests: ac.Id = ReadInt32(reader, "Id");
				acc.Add(ac);
			}
			return acc;
	
		}

        public static Image LoadImage(this IDataReader reader) {
            return LoadImage(reader, false, false);
        }
		
        public static Image LoadImage(this IDataReader reader, bool includeBlog, bool includeCategory)
		{
            Image image = reader.LoadObject<Image>("CategoryTitle", "LocalDirectoryPath");

            if (includeBlog) {
                image.Blog = reader.LoadBlog("Blog.");
            }
            if (includeCategory) {
                image.CategoryTitle = reader.ReadString("Category.Title");
            }
			return image;
		}

        public static IDictionary<string, int> LoadTags(IDataReader reader)
        {
            SortedDictionary<string, int> tags = new SortedDictionary<string, int>();
            while (reader.Read())
            {
                tags.Add(
                    ReadString(reader, "Name"),
                    ReadInt32(reader, "TagCount"));
            }
            return tags;
        }

        #region Enclosure

        public static Enclosure LoadEnclosure(IDataReader reader)
        {
            Enclosure enclosure = new Enclosure();

            enclosure.Id = ReadInt32(reader, "EnclosureId");
            enclosure.Title = ReadString(reader, "EnclosureTitle");
            enclosure.Url = ReadString(reader, "EnclosureUrl");
            enclosure.MimeType = ReadString(reader, "EnclosureMimeType");
            enclosure.Size = reader.ReadInt64("EnclosureSize");
            enclosure.EntryId = ReadInt32(reader, "ID");
            enclosure.AddToFeed = ReadBoolean(reader, "AddToFeed");
            enclosure.ShowWithPost = ReadBoolean(reader, "ShowWithPost");

            return enclosure;
        }

        #endregion

        /// <summary>
        /// Reads the long from the data reader.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <param name="columnName">Name of the column.</param>
        /// <returns></returns>
        public static long ReadInt64(this IDataReader reader, string columnName)
        {
            return ReadInt64(reader, columnName, 0);
        }

        /// <summary>
        /// Reads the long from the data reader. If the value is null, 
        /// returns the default value.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <param name="columnName">Name of the column.</param>
        /// <param name="defaultValue">devault value for the field</param>
        /// <returns></returns>
        public static long ReadInt64(this IDataReader reader, string columnName, long defaultValue)
        {
            try
            {
                if (reader[columnName] != DBNull.Value)
                    return (long)reader[columnName];
                else
                    return defaultValue;
            }
            catch (IndexOutOfRangeException)
            {
                return defaultValue;
            }
        }

		/// <summary>
		/// Reads the int from the data reader.
		/// </summary>
		/// <param name="reader">The reader.</param>
		/// <param name="columnName">Name of the column.</param>
		/// <returns></returns>
		public static int ReadInt32(this IDataReader reader, string columnName)
		{
			return ReadInt32(reader, columnName, NullValue.NullInt32);
		}

		/// <summary>
		/// Reads the int from the data reader. If the value is null, 
		/// returns the default value.
		/// </summary>
		/// <param name="reader">The reader.</param>
		/// <param name="columnName">Name of the column.</param>
		/// <param name="defaultValue">devault value for the field</param>
		/// <returns></returns>
		public static int ReadInt32(this IDataReader reader, string columnName, int defaultValue)
		{
			try
			{
				if (reader[columnName] != DBNull.Value)
					return (int)reader[columnName];
				else
					return defaultValue;
			}
			catch(IndexOutOfRangeException)
			{
				return defaultValue;
			}
		}

		/// <summary>
		/// Reads a boolean from the data reader. If the value is null, 
		/// returns false.
		/// </summary>
		/// <param name="reader">The reader.</param>
		/// <param name="columnName">Name of the column.</param>
		/// <returns></returns>
		public static bool ReadBoolean(this IDataReader reader, string columnName)
		{
			try
			{
				if (reader[columnName] != DBNull.Value)
					return (bool)reader[columnName];
				else
					return false;
			}
			catch (IndexOutOfRangeException)
			{
				return false;
			}
		}

		/// <summary>
		/// Reads the string.
		/// </summary>
		/// <param name="reader">The reader.</param>
		/// <param name="columnName">Name of the coumn.</param>
		/// <returns></returns>
		public static string ReadString(this IDataReader reader, string columnName)
		{
			try
			{
				if (reader[columnName] != DBNull.Value)
					return (string)reader[columnName];
				else
					return null;
			}
			catch(IndexOutOfRangeException)
			{
				return null;
			}
		}

        public static T LoadObject<T>(this IDataReader reader, params string[] exclusionList) where T : new() {
            T item = new T();
            reader.LoadObject<T>(item, exclusionList);
            return item;
        }

        public static T LoadObject<T>(this IDataReader reader, T item, params string[] exclusionList)
        {
            var properties = TypeDescriptor.GetProperties(item);
            foreach(PropertyDescriptor property in properties) 
            {
                if (property.IsReadOnly) {
                    continue;
                }

                if (!property.PropertyType.IsLoadablePropertyType()) {
                    continue;
                }

                if (exclusionList != null && IsExcluded(property.Name, exclusionList)) {
                    continue;
                }

                var value = reader[property.Name];
                if (value != DBNull.Value)
                {
                    property.SetValue(item, value);
                }
            }
            return item;
        }

        private static bool IsExcluded(string propertyName, string[] exclusionList) {
            foreach (string excludedProperty in exclusionList) {
                if (propertyName == excludedProperty) {
                    return true;
                }
            }
            return false;
        }

        private static bool IsLoadablePropertyType(this Type t) {
            
            bool isLoadable = t.IsPrimitive || 
                t == typeof(DateTime) || 
                t == typeof(string);

            if (!isLoadable) { 
                //Maybe it's a nullable.
                Type underlyingType = Nullable.GetUnderlyingType(t);
                if (underlyingType != null) {
                    return IsLoadablePropertyType(underlyingType);
                }
            }
            return isLoadable;
        }

		/// <summary>
		/// Reads the string.
		/// </summary>
		/// <param name="reader">The reader.</param>
		/// <param name="columnName">Name of the coumn.</param>
		/// <returns></returns>
		public static IPAddress ReadIpAddress(this IDataReader reader, string columnName)
		{
			try
			{
				if (reader[columnName] != DBNull.Value)
					return IPAddress.Parse((string)reader[columnName]);
				else
					return IPAddress.None;
			}
			catch(FormatException)
			{
				return IPAddress.None;
			}
			catch (IndexOutOfRangeException)
			{
				return IPAddress.None;
			}
		}

		/// <summary>
		/// Reads an URI from the database.
		/// </summary>
		/// <param name="reader"></param>
		/// <param name="columnName"></param>
		/// <returns></returns>
		public static Uri ReadUri(this IDataReader reader, string columnName)
		{
			try
			{
				if(reader[columnName] != DBNull.Value)
					return new Uri((string) reader[columnName]);
				else
					return null;
			}
			catch(IndexOutOfRangeException)
			{
				return null;
			}
			catch(FormatException)
			{
				return null;
			}
		}

		/// <summary>
		/// Reads the date.
		/// </summary>
		/// <param name="reader">The reader.</param>
		/// <param name="columnName">Name of the column.</param>
		/// <returns></returns>
		public static DateTime ReadDate(this IDataReader reader, string columnName)
		{
			return ReadDate(reader, columnName, NullValue.NullDateTime);
		}

		/// <summary>
		/// Reads the date.
		/// </summary>
		/// <param name="reader">The reader.</param>
		/// <param name="columnName">Name of the column.</param>
		/// <param name="defaultValue">The default value.</param>
		/// <returns></returns>
		public static DateTime ReadDate(this IDataReader reader, string columnName, DateTime defaultValue)
		{
			try
			{
				if (reader[columnName] != DBNull.Value)
					return (DateTime)reader[columnName];
				else
					return defaultValue;
			}
			catch(IndexOutOfRangeException)
			{
				return defaultValue;
			}
		}

		public static SqlParameter MakeInParam(string paramName, object value)
		{
			return new SqlParameter(paramName, value);
		}

		/// <summary>
		/// Make input param.
		/// </summary>
		/// <param name="ParamName">Name of param.</param>
		/// <param name="DbType">Param type.</param>
		/// <param name="Size">Param size.</param>
		/// <returns>New parameter.</returns>
		public static SqlParameter MakeOutParam(string ParamName, SqlDbType DbType, int Size)
		{
			return MakeParam(ParamName, DbType, Size, ParameterDirection.Output, null);
		}

		/// <summary>
		/// Make stored procedure param.
		/// </summary>
		/// <param name="ParamName">Name of param.</param>
		/// <param name="DbType">Param type.</param>
		/// <param name="Size">Param size.</param>
		/// <param name="Direction">Parm direction.</param>
		/// <param name="Value">Param value.</param>
		/// <returns>New parameter.</returns>
		public static SqlParameter MakeParam(string ParamName, SqlDbType DbType, Int32 Size, ParameterDirection Direction, object Value)
		{
			SqlParameter param;

			if (Size > 0)
				param = new SqlParameter(ParamName, DbType, Size);
			else
				param = new SqlParameter(ParamName, DbType);

			param.Direction = Direction;
			if (!(Direction == ParameterDirection.Output && Value == null))
				param.Value = Value;

			return param;
		}

		/// <summary>
        /// Checks the value type and returns null if the 
        /// value is "null-equivalent".
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <returns></returns>
        public static int? NullIfMinValue(this int value)
        {
            if (NullValue.IsNull(value))
                return null;
            return (int?)value;
        }

        public static PagedCollection<T> GetPagedCollection<T>(this IDataReader reader, Func<IDataReader, T> loadIndividualFunc) {
            PagedCollection<T> collection = new PagedCollection<T>();
            while (reader.Read())
            {
                collection.Add(loadIndividualFunc(reader));
            }
            reader.NextResult();
            collection.MaxItems = DataHelper.GetMaxItems(reader);
            return collection;
        }

        // Expects that the caller will dispose of the reader.
        public static ICollection<LinkCategory> LoadLinkCategories(this IDataReader reader, bool includeLinks) {
            var categories = new Dictionary<int, LinkCategory>();
            
            while (reader.Read()) {
                var category = reader.LoadLinkCategory();
                categories.Add(category.Id, category);
            }

            if (includeLinks && reader.NextResult()) {
                while (reader.Read()) {
                    var link = reader.LoadObject<Link>();
                    LinkCategory category;
                    if(categories.TryGetValue(link.CategoryID, out category)) {
                        category.Links.Add(link);
                    }
                }
            }

            return categories.Values;
        }
        
        internal static LinkCategory LoadLinkCategoryFromReader(this IDataReader reader)
        {
            if (reader.Read())
            {
                LinkCategory lc = DataHelper.LoadLinkCategory(reader);
                return lc;
            }
            return null;
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
