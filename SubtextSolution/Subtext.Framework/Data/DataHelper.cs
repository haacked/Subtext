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
using System.Collections.Specialized;
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
		#region Statisitics

		public static ViewStat LoadViewStat(IDataReader reader)
		{
			ViewStat vStat = new ViewStat();

			if (reader["Title"] != DBNull.Value)
			{
				vStat.PageTitle = (string) reader["Title"];
			}

			if (reader["Count"] != DBNull.Value)
			{
				vStat.ViewCount = (int) reader["Count"];
			}

			if (reader["Day"] != DBNull.Value)
			{
				vStat.ViewDate = (DateTime) reader["Day"];
			}

			if (reader["PageType"] != DBNull.Value)
			{
				vStat.PageType = (PageType)((byte)reader["PageType"]);
			}

            return vStat;
		}

		public static Referrer LoadReferrer(IDataReader reader)
		{
			Referrer refer = new Referrer();


			if (reader["URL"] != DBNull.Value)
			{
				refer.ReferrerURL = (string) reader["URL"];
			}

			if (reader["Title"] != DBNull.Value)
			{
				refer.PostTitle = (string) reader["Title"];
			}

			if (reader["EntryID"] != DBNull.Value)
			{
				refer.EntryID = (int) reader["EntryID"];
			}

			if (reader["LastUpdated"] != DBNull.Value)
			{
				refer.LastReferDate = (DateTime) reader["LastUpdated"];
			}

			if (reader["Count"] != DBNull.Value)
			{
				refer.Count = (int) reader["Count"];
			}

            refer.BlogId = Config.CurrentBlog.Id;

			return refer;
		}

		#endregion

		#region EntryDayCollection

		private static bool IsNewDay(DateTime dtCurrent, DateTime dtDay)
		{
			return !(dtCurrent.DayOfYear == dtDay.DayOfYear && dtCurrent.Year == dtDay.Year);
		}

        public static IList<EntryDay> LoadEntryDayCollection(IDataReader reader)
		{
			DateTime dt = new DateTime(1900, 1, 1);
			List<EntryDay> edc = new List<EntryDay>();
			EntryDay day = null;

			while(reader.Read())
			{
				if(IsNewDay(dt, (DateTime)reader["DateAdded"]))
				{
					dt = (DateTime)reader["DateAdded"];
					day = new EntryDay(dt);
					edc.Add(day);
				}
				day.Add(LoadEntry(reader));
			}
			return edc;
		}


		#endregion

		#region EntryCollection
        internal static IList<Entry> LoadEntryCollectionFromDataReader(IDataReader reader)
        {
            List<Entry> entries = new List<Entry>();
            while(reader.Read())
            {
                entries.Add(LoadEntry(reader));
            }

            if(entries.Count > 0 && reader.NextResult())
            {
                //Categories...
                Dictionary<int, StringCollection> categories = new Dictionary<int, StringCollection>();
                while(reader.Read())
                {
                    int postId = ReadInt32(reader, "Id");
                    string categoryTitle = ReadString(reader, "Title");
                    if(!categories.ContainsKey(postId))
                    {
                        categories.Add(postId, new StringCollection());
                    }
                    categories[postId].Add(categoryTitle);
                }
                
                foreach(Entry entry in entries)
                {
                    StringCollection categoryTitles;
                    if (categories.TryGetValue(entry.Id, out categoryTitles))
                    {
                        foreach (string category in categoryTitles)
                        {
                            entry.Categories.Add(category);
                        }
                    }
                }
            }
            return entries;
        }
		#endregion

		#region Blog Group
		public static BlogGroup LoadBlogGroup(IDataReader reader)
		{
			BlogGroup group = new BlogGroup();

			group.Id = ReadInt32(reader, "Id");
			group.Title = ReadString(reader, "Title");
			group.Description = ReadString(reader, "Description");
			group.DisplayOrder = ReadInt32(reader, "DisplayOrder");
			group.IsActive = ReadBoolean(reader, "Active");
			return group;
		}
		#endregion

		#region BlogAlias
		/// <summary>
		/// Loads the blog alias.
		/// </summary>
		/// <param name="reader">The reader.</param>
		/// <returns></returns>
		public static BlogAlias LoadBlogAlias(IDataReader reader)
		{
			BlogAlias alias = new BlogAlias();

			alias.Id = ReadInt32(reader, "Id");
			alias.BlogId = ReadInt32(reader, "BlogId");
			alias.Host = ReadString(reader, "Host");
			alias.Subfolder = ReadString(reader, "Application");
			alias.IsActive = ReadBoolean(reader, "IsActive");
			return alias;
		}
		#endregion
		#region Single Entry
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
			entry.DateCreated = (DateTime)reader["DateAdded"];
			
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

			SetUrlPattern(entry);

			return entry;
		}

		public static Entry LoadEntry(IDataReader reader)
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

		internal static FeedbackItem LoadFeedbackItem(IDataReader reader, Entry entry)
		{
			FeedbackItem feedbackItem = new FeedbackItem((FeedbackType)ReadInt32(reader, "FeedbackType"));
			LoadFeedbackItem(reader, feedbackItem);
			feedbackItem.Entry = entry;
			return feedbackItem;
		}

		internal static FeedbackItem LoadFeedbackItem(IDataReader reader)
		{
			FeedbackItem feedbackItem = new FeedbackItem((FeedbackType)ReadInt32(reader, "FeedbackType"));
			LoadFeedbackItem(reader, feedbackItem);
			return feedbackItem;
		}

		private static void LoadFeedbackItem(IDataReader reader, FeedbackItem feedbackItem)
		{
			feedbackItem.Id = ReadInt32(reader, "Id");
			feedbackItem.Title = ReadString(reader, "Title");
			feedbackItem.Body = ReadString(reader, "Body");
			feedbackItem.EntryId = ReadInt32(reader, "EntryId");
			feedbackItem.Author = ReadString(reader, "Author");
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

		internal static Entry LoadEntry(IDataReader reader, bool buildLinks)
		{
			Entry entry = new Entry((PostType)ReadInt32(reader, "PostType"));
			LoadEntry(reader, entry, buildLinks);
			return entry;
		}

		private static void LoadEntry(IDataReader reader, Entry entry, bool buildLinks)
		{
			entry.Author = ReadString(reader, "Author");
			entry.Email = ReadString(reader, "Email");
			entry.DateCreated = ReadDate(reader, "DateAdded");
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
            if(withEnclosure)
		        entry.Enclosure = LoadEnclosure(reader);
	
			if(buildLinks)
			{
				SetUrlPattern(entry);
			}
		}

		private static void SetUrlPattern(Entry entry)
		{
			switch(entry.PostType)
			{
				case PostType.BlogPost:
					entry.Url = Config.CurrentBlog.UrlFormats.EntryUrl(entry);
					break;

				case PostType.Story:
					entry.Url = Config.CurrentBlog.UrlFormats.ArticleUrl(entry);
					break;
			}
		}

		internal static int GetMaxItems(IDataReader reader)
		{
			reader.Read();
			return ReadInt32(reader, "TotalRecords");
		}

		#endregion

		#region Categories

		public static LinkCategory LoadLinkCategory(IDataReader reader)
		{
			LinkCategory lc = new LinkCategory(ReadInt32(reader, "CategoryID"), ReadString(reader, "Title"));
			lc.IsActive = (bool)reader["Active"];
			if(reader["CategoryType"] != DBNull.Value)
			{
				lc.CategoryType = (CategoryType)((byte)reader["CategoryType"]);
			}
			if(reader["Description"] != DBNull.Value)
			{
				lc.Description = ReadString(reader, "Description");
			}
            if (reader["BlogId"] != DBNull.Value)
            {
                lc.BlogId = ReadInt32(reader, "BlogId");
            }
            else 
            {
                lc.BlogId = Config.CurrentBlog.Id;
            }
			return lc;
		}

		public static LinkCategory LoadLinkCategory(DataRow dr)
		{
			LinkCategory lc = new LinkCategory((int)dr["CategoryID"], (string)dr["Title"]);
			
			// Active cannot be null.
			lc.IsActive = (bool)dr["Active"];

			if(dr["CategoryType"] != DBNull.Value)
			{
				lc.CategoryType = (CategoryType)((byte)dr["CategoryType"]);
			}
			if(dr["Description"] != DBNull.Value)
			{
				lc.Description = (string)dr["Description"];
			}
            if (dr["BlogId"] != DBNull.Value)
            {
                lc.BlogId = (int)dr["BlogId"];
            }
            else
            {
                lc.BlogId = Config.CurrentBlog.Id;
            }
			return lc;
		}

		#endregion

		#region Links

		public static Link LoadLink(IDataReader reader)
		{
			Link link = new Link();
			// Active cannot be null
			link.IsActive = (bool)reader["Active"];

			if(reader["NewWindow"] != DBNull.Value)
			{
				link.NewWindow = (bool)reader["NewWindow"];
			}

            if (reader["Rel"] != DBNull.Value)
            {
               link.Relation = ReadString(reader, "Rel");
            }   

			// LinkID cannot be null
			link.Id = ReadInt32(reader, "LinkID");
			
			if(reader["Rss"] != DBNull.Value)
			{
				link.Rss = ReadString(reader, "Rss");
			}
			
			if(reader["Url"] != DBNull.Value)
			{
				link.Url = ReadString(reader, "Url");
			}
			
			if(reader["Title"] != DBNull.Value)
			{
				link.Title = ReadString(reader, "Title");
			}

			if(reader["CategoryID"] != DBNull.Value)
			{
				link.CategoryID = ReadInt32(reader, "CategoryID");
			}
			
			if(reader["PostID"] != DBNull.Value)
			{
				link.PostID = ReadInt32(reader, "PostID");
			}

            if (reader["BlogId"] != DBNull.Value)
            {
                link.BlogId = ReadInt32(reader, "BlogId");
            }
			return link;
		}

		public static Link LoadLink(DataRow dr)
		{
			Link link = new Link();
			// Active cannot be null
			link.IsActive = (bool)dr["Active"];
			
			if(dr["NewWindow"] != DBNull.Value)
			{
				link.NewWindow = (bool)dr["NewWindow"];
			}

			//LinkID cannot be null.
			link.Id = (int)dr["LinkID"];
			
			if(dr["Rss"] != DBNull.Value)
			{
				link.Rss = (string)dr["Rss"];
			}
			
			if(dr["Url"] != DBNull.Value)
			{
				link.Url = (string)dr["Url"];
			}
			
			if(dr["Title"] != DBNull.Value)
			{
				link.Title = (string)dr["Title"];
			}
			
			if(dr["CategoryID"] != DBNull.Value)
			{
				link.CategoryID = (int)dr["CategoryID"];
			}

            if (dr["Rel"] != DBNull.Value)
            {
                link.Relation = (string)(dr["Rel"]);
            }  
			
			if(dr["PostID"] != DBNull.Value)
			{
				link.PostID = (int)dr["PostID"];
			}
			return link;
		}

		#endregion

		#region Config

		public static BlogInfo LoadConfigData(IDataReader reader)
		{
			BlogInfo info = new BlogInfo();
			info.Author = ReadString(reader, "Author");
			info.Id = ReadInt32(reader, "BlogId");
			info.Email = ReadString(reader, "Email");
			info.Password = ReadString(reader, "Password");
            info.OpenIDUrl = ReadString(reader, "OpenIDUrl");
            info.CardSpaceHash = ReadString(reader, "CardSpaceHash");

			info.SubTitle = ReadString(reader, "SubTitle");
			info.Title = ReadString(reader, "Title");
			info.UserName = ReadString(reader, "UserName");
			info.TimeZoneId = ReadInt32(reader, "TimeZone");
			info.ItemCount = ReadInt32(reader, "ItemCount");
			info.CategoryListPostCount = ReadInt32(reader, "CategoryListPostCount");
			info.Language = ReadString(reader, "Language");
			

			info.PostCount = ReadInt32(reader, "PostCount");
			info.CommentCount = ReadInt32(reader, "CommentCount");
			info.StoryCount = ReadInt32(reader, "StoryCount");
			info.PingTrackCount = ReadInt32(reader, "PingTrackCount");
			info.News = ReadString(reader, "News");
			info.TrackingCode = ReadString(reader, "TrackingCode");	
			
			info.LastUpdated = ReadDate(reader, "LastUpdated", new DateTime(2003, 1 , 1));
			info.Host = ReadString(reader, "Host");
			// The Subfolder property is stored in the Application column. 
			// This is a result of the legacy schema.
			info.Subfolder = ReadString(reader, "Application");

			info.Flag = (ConfigurationFlags)(ReadInt32(reader, "Flag"));

			info.Skin = new SkinConfig();
			info.Skin.TemplateFolder = ReadString(reader, "Skin");
			info.Skin.SkinStyleSheet = ReadString(reader, "SkinCssFile");
            info.Skin.CustomCssText = ReadString(reader, "SecondaryCss");
            info.MobileSkin = new SkinConfig();
            info.MobileSkin.TemplateFolder = ReadString(reader, "MobileSkin");
            info.MobileSkin.SkinStyleSheet = ReadString(reader, "MobileSkinCssFile");

            info.OpenIDUrl = ReadString(reader, "OpenIDUrl");
            info.OpenIDServer = ReadString(reader, "OpenIDServer");
            info.OpenIDDelegate = ReadString(reader, "OpenIDDelegate");
            info.CardSpaceHash = ReadString(reader, "CardSpaceHash");
			
			info.LicenseUrl = ReadString(reader, "LicenseUrl");
			
			info.DaysTillCommentsClose = ReadInt32(reader, "DaysTillCommentsClose", int.MaxValue);
			info.CommentDelayInMinutes = ReadInt32(reader, "CommentDelayInMinutes");
			info.NumberOfRecentComments = ReadInt32(reader, "NumberOfRecentComments");
			info.RecentCommentsLength = ReadInt32(reader, "RecentCommentsLength");
			info.FeedbackSpamServiceKey = ReadString(reader, "AkismetAPIKey");
			info.FeedBurnerName = ReadString(reader, "FeedBurnerName");

            info.BlogGroupId = ReadInt32(reader, "BlogGroupId");
            info.BlogGroupTitle = ReadString(reader, "BlogGroupTitle");
			return info;
		}

		#endregion

		#region Archive

        public static IList<ArchiveCount> LoadArchiveCount(IDataReader reader)
		{
			const string dateformat = "{0:00}/{1:00}/{2:0000}";
			string dt; //
            DateTime parsedDate;
			ArchiveCount ac;// new ArchiveCount();
            IList<ArchiveCount> acc = new Collection<ArchiveCount>();
			while(reader.Read())
			{
				ac = new ArchiveCount();
				dt = string.Format(CultureInfo.InvariantCulture, dateformat, ReadInt32(reader, "Month"),ReadInt32(reader, "Day"),ReadInt32(reader, "Year"));
				// FIX: BUG SF1423271 Archives Links
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

		//Needs to be handled else where!
		public static Link LoadArchiveLink(IDataReader reader)
		{
			Link link = new Link();
			int count = ReadInt32(reader, "Count");
			DateTime dt = new DateTime(ReadInt32(reader, "Year"),ReadInt32(reader, "Month"),1);
			link.NewWindow = false;
			link.Title = dt.ToString("y", CultureInfo.InvariantCulture) + " (" + count.ToString(CultureInfo.InvariantCulture) + ")";
			//link.Url = Globals.ArchiveUrl(dt,"MMyyyy");
			link.NewWindow = false;
			link.IsActive = true;
			return link;
		}

		#endregion

		#region Image

		public static Image LoadImage(IDataReader reader)
		{
			Image _image = new Image();
			_image.CategoryID = ReadInt32(reader, "CategoryID");
			_image.FileName = ReadString(reader, "File");
			_image.Height = ReadInt32(reader, "Height");
			_image.Width = ReadInt32(reader, "Width");
			_image.ImageID = ReadInt32(reader, "ImageID");
			_image.IsActive = (bool)reader["Active"];
			_image.Title = ReadString(reader, "Title");
            _image.BlogId = Config.CurrentBlog.Id;
			return _image;
		}

		#endregion

        #region Tags

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

        #endregion

		#region MetaTags

		public static MetaTag LoadMetaTag(IDataReader reader)
		{
			MetaTag tag = new MetaTag();

		    tag.Id = ReadInt32(reader, "Id");
			tag.Content = ReadString(reader, "Content");
			tag.Name = ReadString(reader, "Name");
			tag.HttpEquiv = ReadString(reader, "HttpEquiv");
		    tag.BlogId = ReadInt32(reader, "BlogId");
		    tag.EntryId = ReadNullableInt(reader, "EntryId");
            tag.DateCreated = ReadDate(reader, "DateCreated");

			return tag;
		}

		#endregion


        #region Enclosure

        public static Enclosure LoadEnclosure(IDataReader reader)
        {
            Enclosure enclosure = new Enclosure();

            enclosure.Id = ReadInt32(reader, "EnclosureId");
            enclosure.Title = ReadString(reader, "EnclosureTitle");
            enclosure.Url = ReadString(reader, "EnclosureUrl");
            enclosure.MimeType = ReadString(reader, "EnclosureMimeType");
            enclosure.Size = ReadInt64(reader, "EnclosureSize");
            enclosure.EntryId = ReadInt32(reader, "ID");
            enclosure.AddToFeed = ReadBoolean(reader, "AddToFeed");
            enclosure.ShowWithPost = ReadBoolean(reader, "ShowWithPost");

            return enclosure;
        }

        #endregion

		#region Keywords

		public static KeyWord LoadKeyWord(IDataReader reader)
		{
			KeyWord kw = new KeyWord();
			kw.Id = ReadInt32(reader, "KeyWordID");
			kw.BlogId = ReadInt32(reader, "BlogId");
			kw.OpenInNewWindow = (bool)reader["OpenInNewWindow"];
			kw.ReplaceFirstTimeOnly = (bool)reader["ReplaceFirstTimeOnly"];
			kw.CaseSensitive = (bool)reader["CaseSensitive"];
			kw.Text = ReadString(reader, "Text");
			kw.Rel = ReadString(reader, "Rel");
			if(reader["Title"] != DBNull.Value)
			{
				kw.Title = CheckNullString(reader["Title"]);
			}
			kw.Url = ReadString(reader, "Url");
			kw.Word = ReadString(reader, "Word");
			return kw;
		}

		/// <summary>
		/// If the string is empty or null, returns a 
		/// System.DBNull.Value.
		/// </summary>
		/// <param name="text"></param>
		/// <returns></returns>
		public static object CheckForNullString(string text)
		{
			if (String.IsNullOrEmpty(text))
			{
				return DBNull.Value;
			}
			else
			{
				return text;
			}
		}

		/// <summary>
		/// If the string is DBNull, returns null. Otherwise returns the string.
		/// </summary>
		/// <param name="obj">The obj.</param>
		/// <returns></returns>
		public static string CheckNullString(object obj)
		{
			if (obj is DBNull)
			{
				return null;
			}
			return (string)obj;
		}

		#endregion

		#region Host
		/// <summary>
		/// Loads the host from the data reader.
		/// </summary>
		/// <param name="reader">Reader.</param>
		/// <param name="info">HostInfo</param>
		/// <returns></returns>
		public static void LoadHost(IDataReader reader, HostInfo info)
		{
			info.HostUserName = ReadString(reader, "HostUserName");
			info.Password = ReadString(reader, "Password");
			info.Salt = ReadString(reader, "Salt");
			info.DateCreated = (DateTime)reader["DateCreated"];
		}
		#endregion

		#region Log Entries
		/// <summary>
		/// Loads the single log entry.
		/// </summary>
		/// <param name="reader">The reader.</param>
		/// <returns></returns>
		public static LogEntry LoadLogEntry(IDataReader reader)
		{
			LogEntry entry = new LogEntry();
			entry.Id = ReadInt32(reader, "Id");
			entry.BlogId = ReadInt32(reader, "BlogId");
			entry.Date = ReadDate(reader, "Date");
			entry.Thread = ReadString(reader, "Thread");
			entry.Level = ReadString(reader, "Level");
			entry.Context = ReadString(reader, "Context");
			entry.Logger = ReadString(reader, "Logger");
			entry.Message = ReadString(reader, "Message");
			entry.Exception = ReadString(reader, "Exception");
			entry.Url = ReadUri(reader, "Url");
			return entry;
		}
		#endregion

        /// <summary>
        /// Reads the long from the data reader.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <param name="columnName">Name of the column.</param>
        /// <returns></returns>
        public static long ReadInt64(IDataReader reader, string columnName)
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
        public static long ReadInt64(IDataReader reader, string columnName, long defaultValue)
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
		public static int ReadInt32(IDataReader reader, string columnName)
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
		public static int ReadInt32(IDataReader reader, string columnName, int defaultValue)
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
		public static bool ReadBoolean(IDataReader reader, string columnName)
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
		/// Reads the int from the data reader.
		/// </summary>
		/// <param name="reader">The reader.</param>
		/// <param name="columnName">Name of the column.</param>
		/// <returns></returns>
		public static int? ReadNullableInt(IDataReader reader, string columnName)
		{
			try
			{
				if (reader[columnName] != DBNull.Value)
					return (int)reader[columnName];
				else
					return null;
			}
			catch(IndexOutOfRangeException)
			{
				return null;
			}
		}

		/// <summary>
		/// Reads the string.
		/// </summary>
		/// <param name="reader">The reader.</param>
		/// <param name="columnName">Name of the coumn.</param>
		/// <returns></returns>
		public static string ReadString(IDataReader reader, string columnName)
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

		/// <summary>
		/// Reads the string.
		/// </summary>
		/// <param name="reader">The reader.</param>
		/// <param name="columnName">Name of the coumn.</param>
		/// <returns></returns>
		public static IPAddress ReadIpAddress(IDataReader reader, string columnName)
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
		public static Uri ReadUri(IDataReader reader, string columnName)
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
		public static DateTime ReadDate(IDataReader reader, string columnName)
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
		public static DateTime ReadDate(IDataReader reader, string columnName, DateTime defaultValue)
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

		public static SqlParameter MakeInParam(string ParamName, object Value)
		{
			return new SqlParameter(ParamName, Value);
		}

		/// <summary>
		/// Make input param.
		/// </summary>
		/// <param name="name">Name of param.</param>
		/// <param name="sqlType">Param type.</param>
		/// <param name="size">Param size.</param>
		/// <param name="value">Param value.</param>
		/// <returns>New parameter.</returns>
		public static SqlParameter MakeInParam(string name, SqlDbType sqlType, int size, object value)
		{
			return MakeParam(name, sqlType, size, ParameterDirection.Input, value);
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
		public static object CheckNull(int obj)
		{
			if(NullValue.IsNull(obj))
				return null;
			return obj;
		}

		/// <summary>
		/// Returns an empty string if the value is null.
		/// </summary>
		/// <param name="obj">The obj.</param>
		/// <returns></returns>
		public static string CheckNull(string obj)
		{
			if(obj == null)
				return string.Empty;
							 
			return obj;
		}

		/// <summary>
		/// Returns null if the value is an empty string.
		/// </summary>
		/// <param name="obj">The obj.</param>
		/// <returns></returns>
		public static string ReturnNullIfEmpty(string obj)
		{
			if (String.IsNullOrEmpty(obj))
				return null;

			return obj;
		}

		/// <summary>
		/// Returns a true null if the object is DBNull.
		/// </summary>
		/// <param name="obj">The obj.</param>
		/// <returns></returns>
		public static string CheckNull(DBNull obj)
		{
			return null;
		}

		/// <summary>
		/// Checks the value of the specified value type for a null value.  
		/// Returns null if the value represents a null value
		/// </summary>
		/// <param name="dateTime">Date time.</param>
		/// <returns></returns>
		public static object CheckNull(DateTime dateTime)
		{
			if(NullValue.IsNull(dateTime))
				return null;
			return dateTime;
		}

		internal static void DebugPrintCommand(SqlCommand command)
		{
			Console.Write(command.CommandText);
			foreach(SqlParameter parameter in command.Parameters)
			{
				Console.Write(" " + parameter.ParameterName + "=" + parameter.Value + ", ");
			}
			Console.Write(Environment.NewLine);
		}

		#region ExecuteDataTable

		/// <summary>
		/// Execute a SqlCommand (that returns a resultset and takes no parameters) against the database specified in 
		/// the connection string. 
		/// </summary>
		/// <remarks>
		/// e.g.:  
		///  DataTable dt = ExecuteDataTable(connString, CommandType.StoredProcedure, "GetOrders");
		/// </remarks>
		/// <param name="connectionString">a valid connection string for a SqlConnection</param>
		/// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
		/// <param name="commandText">the stored procedure name or T-SQL command</param>
		/// <returns>a DataTable containing the resultset generated by the command</returns>
		public static DataTable ExecuteDataTable(string connectionString, CommandType commandType, string commandText)
		{
			//pass through the call providing null for the set of SqlParameters
			return ExecuteDataTable(connectionString, commandType, commandText, null);
		}

		/// <summary>
		/// Execute a SqlCommand (that returns a resultset) against the database specified in the connection string 
		/// using the provided parameters.
		/// </summary>
		/// <remarks>
		/// e.g.:  
		///  DataTable dt = ExecuteDataTable(connString, CommandType.StoredProcedure, "GetOrders", new SqlParameter("@prodid", 24));
		/// </remarks>
		/// <param name="connectionString">a valid connection string for a SqlConnection</param>
		/// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
		/// <param name="commandText">the stored procedure name or T-SQL command</param>
		/// <param name="commandParameters">an array of SqlParamters used to execute the command</param>
		/// <returns>a DataTable containing the resultset generated by the command</returns>
		public static DataTable ExecuteDataTable(string connectionString, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
		{
			//create & open a SqlConnection, and dispose of it after we are done.
			using (SqlConnection cn = new SqlConnection(connectionString))
			{
				cn.Open();

				//call the overload that takes a connection in place of the connection string
				return ExecuteDataTable(cn, commandType, commandText, commandParameters);
			}
		}



		/// <summary>
		/// Execute a SqlCommand (that returns a resultset and takes no parameters) against the provided SqlConnection. 
		/// </summary>
		/// <remarks>
		/// e.g.:  
		///  DataTable dt = ExecuteDataTable(conn, CommandType.StoredProcedure, "GetOrders");
		/// </remarks>
		/// <param name="connection">a valid SqlConnection</param>
		/// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
		/// <param name="commandText">the stored procedure name or T-SQL command</param>
		/// <returns>a DataTable containing the resultset generated by the command</returns>
		public static DataTable ExecuteDataTable(SqlConnection connection, CommandType commandType, string commandText)
		{
			//pass through the call providing null for the set of SqlParameters
			return ExecuteDataTable(connection, commandType, commandText, null);
		}

		/// <summary>
		/// Execute a SqlCommand (that returns a resultset) against the specified SqlConnection 
		/// using the provided parameters.
		/// </summary>
		/// <remarks>
		/// e.g.:  
		///  DataTable dt = ExecuteDataTable(conn, CommandType.StoredProcedure, "GetOrders", new SqlParameter("@prodid", 24));
		/// </remarks>
		/// <param name="connection">a valid SqlConnection</param>
		/// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
		/// <param name="commandText">the stored procedure name or T-SQL command</param>
		/// <param name="commandParameters">an array of SqlParamters used to execute the command</param>
		/// <returns>a DataTable containing the resultset generated by the command</returns>
		public static DataTable ExecuteDataTable(SqlConnection connection, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
		{
			//create a command and prepare it for execution
			SqlCommand cmd = new SqlCommand();
			PrepareCommand(cmd, connection, null, commandType, commandText, commandParameters);

			//create the DataAdapter & DataTable
			SqlDataAdapter da = new SqlDataAdapter(cmd);
			DataTable dt = new DataTable();

			//fill the DataTable using default values for DataTable names, etc.
			da.Fill(dt);

			// detach the SqlParameters from the command object, so they can be used again.			
			cmd.Parameters.Clear();

			//return the DataTable
			return dt;
		}


		/// <summary>
		/// Execute a SqlCommand (that returns a resultset and takes no parameters) against the provided SqlTransaction. 
		/// </summary>
		/// <remarks>
		/// e.g.:  
		///  DataTable dt = ExecuteDataTable(trans, CommandType.StoredProcedure, "GetOrders");
		/// </remarks>
		/// <param name="transaction">a valid SqlTransaction</param>
		/// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
		/// <param name="commandText">the stored procedure name or T-SQL command</param>
		/// <returns>a DataTable containing the resultset generated by the command</returns>
		public static DataTable ExecuteDataTable(SqlTransaction transaction, CommandType commandType, string commandText)
		{
			//pass through the call providing null for the set of SqlParameters
			return ExecuteDataTable(transaction, commandType, commandText, null);
		}

		/// <summary>
		/// Execute a SqlCommand (that returns a resultset) against the specified SqlTransaction
		/// using the provided parameters.
		/// </summary>
		/// <remarks>
		/// e.g.:  
		///  DataTable dt = ExecuteDataTable(trans, CommandType.StoredProcedure, "GetOrders", new SqlParameter("@prodid", 24));
		/// </remarks>
		/// <param name="transaction">a valid SqlTransaction</param>
		/// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
		/// <param name="commandText">the stored procedure name or T-SQL command</param>
		/// <param name="commandParameters">an array of SqlParamters used to execute the command</param>
		/// <returns>a DataTable containing the resultset generated by the command</returns>
		public static DataTable ExecuteDataTable(SqlTransaction transaction, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
		{
			//create a command and prepare it for execution
			SqlCommand cmd = new SqlCommand();
			PrepareCommand(cmd, transaction.Connection, transaction, commandType, commandText, commandParameters);

			//create the DataAdapter & DataTable
			SqlDataAdapter da = new SqlDataAdapter(cmd);
			DataTable dt = new DataTable();

			//fill the DataTable using default values for DataTable names, etc.
			da.Fill(dt);

			// detach the SqlParameters from the command object, so they can be used again.
			cmd.Parameters.Clear();

			//return the DataTable
			return dt;
		}

		/// <summary>
		/// This method opens (if necessary) and assigns a connection, transaction, command type and parameters 
		/// to the provided command.
		/// </summary>
		/// <param name="command">the SqlCommand to be prepared</param>
		/// <param name="connection">a valid SqlConnection, on which to execute this command</param>
		/// <param name="transaction">a valid SqlTransaction, or 'null'</param>
		/// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
		/// <param name="commandText">the stored procedure name or T-SQL command</param>
		/// <param name="commandParameters">an array of SqlParameters to be associated with the command or 'null' if no parameters are required</param>
		private static void PrepareCommand(SqlCommand command, SqlConnection connection, SqlTransaction transaction, CommandType commandType, string commandText, SqlParameter[] commandParameters)
		{
			//if the provided connection is not open, we will open it
			if (connection.State != ConnectionState.Open)
			{
				connection.Open();
			}

			//associate the connection with the command
			command.Connection = connection;

			//set the command text (stored procedure name or SQL statement)
			command.CommandText = commandText;

			//if we were provided a transaction, assign it.
			if (transaction != null)
			{
				command.Transaction = transaction;
			}

			//set the command type
			command.CommandType = commandType;

			//attach the command parameters if they are provided
			if (commandParameters != null)
			{
				AttachParameters(command, commandParameters);
			}

			return;
		}

		/// <summary>
		/// This method is used to attach array of SqlParameters to a SqlCommand.
		/// 
		/// This method will assign a value of DbNull to any parameter with a direction of
		/// InputOutput and a value of null.  
		/// 
		/// This behavior will prevent default values from being used, but
		/// this will be the less common case than an intended pure output parameter (derived as InputOutput)
		/// where the user provided no input value.
		/// </summary>
		/// <param name="command">The command to which the parameters will be added</param>
		/// <param name="commandParameters">an array of SqlParameters tho be added to command</param>
		private static void AttachParameters(SqlCommand command, SqlParameter[] commandParameters)
		{
			foreach (SqlParameter p in commandParameters)
			{
				//check for derived output value with no value assigned
				if ((p.Direction == ParameterDirection.InputOutput) && (p.Value == null))
				{
					p.Value = DBNull.Value;
				}

				command.Parameters.Add(p);
			}
		}
		#endregion ExecuteDataTable
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
