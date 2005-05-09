#region Disclaimer/Info
///////////////////////////////////////////////////////////////////////////////////////////////////
// .Text WebLog
// 
// .Text is an open source weblog system started by Scott Watermasysk. 
// Blog: http://ScottWater.com/blog 
// RSS: http://scottwater.com/blog/rss.aspx
// Email: Dottext@ScottWater.com
//
// For updated news and information please visit http://scottwater.com/dottext and subscribe to 
// the Rss feed @ http://scottwater.com/dottext/rss.aspx
//
// On its release (on or about August 1, 2003) this application is licensed under the BSD. However, I reserve the 
// right to change or modify this at any time. The most recent and up to date license can always be fount at:
// http://ScottWater.com/License.txt
// 
// Please direct all code related questions to:
// GotDotNet Workspace: http://www.gotdotnet.com/Community/Workspaces/workspace.aspx?id=e99fccb3-1a8c-42b5-90ee-348f6b77c407
// Yahoo Group http://groups.yahoo.com/group/DotText/
// 
///////////////////////////////////////////////////////////////////////////////////////////////////
#endregion

using System;
using System.Data;
using System.Globalization;
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
	/// into objects such as <see cref="EntryDayCollection"/> and <see cref="EntryCollection"/>.
	/// </summary>
	public class DataHelper
	{
		#region Statisitics

		public static ViewStat LoadSingleViewStat(IDataReader reader)
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

		public static Referrer LoadSingleReferrer(IDataReader reader)
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

			return refer;
		}

		#endregion

		#region EntryDayCollection

		private static bool IsNewDay(DateTime dtCurrent, DateTime dtDay)
		{
			return !(dtCurrent.DayOfYear == dtDay.DayOfYear && dtCurrent.Year == dtDay.Year);
		}

		public static EntryDayCollection LoadEntryDayCollection(IDataReader reader)
		{
			DateTime dt = new DateTime(1900,1,1);
			EntryDayCollection edc = new EntryDayCollection();
			EntryDay day = null;

			while(reader.Read())
			{
				if(IsNewDay(dt,(DateTime)reader["DateAdded"]))
				{
					dt = (DateTime)reader["DateAdded"];
					day = new EntryDay(dt);
					edc.Add(day);
				}
				day.Add(DataHelper.LoadSingleEntry(reader));
			}
			return edc;

		}


		#endregion

		#region EntryCollection

		public static EntryCollection LoadEntryCollection(IDataReader reader)
		{
			EntryCollection ec = new EntryCollection();
			while(reader.Read())
			{
				ec.Add(LoadSingleEntry(reader));
			}
			return ec;	
		}

		#endregion

		#region Single Entry
		//Crappy. Need to clean up all of the entry references
		public static EntryStatsView LoadSingleEntryStatsView(IDataReader reader)
		{
			EntryStatsView entry = new EntryStatsView();

			entry.PostType = ((PostType)(int)reader["PostType"]);

			if(reader["WebCount"] != DBNull.Value)
			{
				entry.WebCount = (int)reader["WebCount"];	
			}

			if(reader["AggCount"] != DBNull.Value)
			{
				entry.AggCount = (int)reader["AggCount"];	
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
				entry.Author = (string)reader["Author"];
			}
			if(reader["Email"] != DBNull.Value)
			{
				entry.Email = (string)reader["Email"];
			}
			entry.DateCreated = (DateTime)reader["DateAdded"];
			
			if(reader["DateUpdated"] != DBNull.Value)
			{
				entry.DateUpdated = (DateTime)reader["DateUpdated"];
			}

			entry.EntryID = (int)reader["ID"];

			if(reader["TitleUrl"] != DBNull.Value)
			{
				entry.TitleUrl = (string)reader["TitleUrl"];
			}
			
			if(reader["SourceName"] != DBNull.Value)
			{
				entry.SourceName = (string)reader["SourceName"];
			}
			if(reader["SourceUrl"] != DBNull.Value)
			{
				entry.SourceUrl = (string)reader["SourceUrl"];
			}

			if(reader["Description"] != DBNull.Value)
			{
				entry.Description = (string)reader["Description"];
			}

			if(reader["EntryName"] != DBNull.Value)
			{
				entry.EntryName = (string)reader["EntryName"];
			}

			if(reader["FeedBackCount"] != DBNull.Value)
			{
				entry.FeedBackCount = (int)reader["FeedBackCount"];
			}

			if(reader["Text"] != DBNull.Value)
			{
				entry.Body = (string)reader["Text"];
			}

			if(reader["Title"] != DBNull.Value)
			{
				entry.Title =(string)reader["Title"];
			}

			if(reader["PostConfig"] != DBNull.Value)
			{
				entry.PostConfig = (PostConfig)((int)reader["PostConfig"]);
			}

			if(reader["ParentID"] != DBNull.Value)
			{
				entry.ParentID = (int)reader["ParentID"];
			}

			SetUrlPattern(entry);

			return entry;
		}


		public static Entry LoadSingleEntry(IDataReader reader)
		{
			return LoadSingleEntry(reader, true);
		}

		public static Entry LoadSingleEntry(IDataReader reader, bool buildLinks)
		{
			Entry entry = new Entry((PostType)(int)reader["PostType"]);
			LoadSingleEntry(reader, entry, buildLinks);
			return entry;
		}

		public static void LoadSingleEntry(IDataReader reader, Entry entry)
		{
			LoadSingleEntry(reader, entry, true);
		}

		private static void LoadSingleEntry(IDataReader reader, Entry entry, bool buildLinks)
		{
			if(reader["Author"] != DBNull.Value)
			{
				entry.Author = (string)reader["Author"];
			}
			if(reader["Email"] != DBNull.Value)
			{
				entry.Email = (string)reader["Email"];
			}
			entry.DateCreated = (DateTime)reader["DateAdded"];
			if(reader["DateUpdated"] != DBNull.Value)
			{
				entry.DateUpdated = (DateTime)reader["DateUpdated"];
			}
	
			entry.EntryID = (int)reader["ID"];
	
			if(reader["TitleUrl"] != DBNull.Value)
			{
				entry.TitleUrl = (string)reader["TitleUrl"];
			}
	
			if(reader["SourceName"] != DBNull.Value)
			{
				entry.SourceName = (string)reader["SourceName"];
			}
			if(reader["SourceUrl"] != DBNull.Value)
			{
				entry.SourceUrl = (string)reader["SourceUrl"];
			}
	
			if(reader["Description"] != DBNull.Value)
			{
				entry.Description = (string)reader["Description"];
			}
	
			if(reader["EntryName"] != DBNull.Value)
			{
				entry.EntryName = (string)reader["EntryName"];
			}
	
			if(reader["FeedBackCount"] != DBNull.Value)
			{
				entry.FeedBackCount = (int)reader["FeedBackCount"];
			}
			if(reader["Text"] != DBNull.Value)
			{
				entry.Body = (string)reader["Text"];
			}
	
			if(reader["Title"] != DBNull.Value)
			{
				entry.Title =(string)reader["Title"];
			}
	
			if(reader["PostConfig"] != DBNull.Value)
			{
				entry.PostConfig = (PostConfig)((int)reader["PostConfig"]);
			}
	
			if(reader["ParentID"] != DBNull.Value)
			{
				entry.ParentID = (int)reader["ParentID"];
			}
	
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
					entry.Link = Config.CurrentBlog().UrlFormats.EntryUrl(entry);
					break;
				case PostType.Story:
					entry.Link = Config.CurrentBlog().UrlFormats.ArticleUrl(entry);
					break;

				case PostType.Comment:
				case PostType.PingTrack:
					entry.Link = Config.CurrentBlog().UrlFormats.CommentUrl(entry);
					break;
			}
		}



		public static Entry LoadSingleEntry(DataRow dr)
		{
			Entry entry = new Entry((PostType)(int)dr["PostType"]);

			if(dr["Author"] != DBNull.Value)
			{
				entry.Author = (string)dr["Author"];
			}
			if(dr["Email"] != DBNull.Value)
			{
				entry.Email = (string)dr["Email"];
			}

			// Not null.
			entry.DateCreated = (DateTime)dr["DateAdded"];
			
			if(dr["DateUpdated"] != DBNull.Value)
			{
				entry.DateUpdated = (DateTime)dr["DateUpdated"];
			}
			entry.EntryID = (int)dr["ID"];

			if(dr["TitleUrl"] != DBNull.Value)
			{
				entry.TitleUrl = (string)dr["TitleUrl"];
			}
			
			if(dr["SourceName"] != DBNull.Value)
			{
				entry.SourceName = (string)dr["SourceName"];
			}
			if(dr["SourceUrl"] != DBNull.Value)
			{
				entry.SourceUrl = (string)dr["SourceUrl"];
			}

			if(dr["Description"] != DBNull.Value)
			{
				entry.Description = (string)dr["Description"];
			}

			if(dr["EntryName"] != DBNull.Value)
			{
				entry.EntryName = (string)dr["EntryName"];
			}

			if(dr["FeedBackCount"] != DBNull.Value)
			{
				entry.FeedBackCount = (int)dr["FeedBackCount"];
			}

			if(dr["Text"] != DBNull.Value)
			{
				entry.Body = (string)dr["Text"];
			}

			// Title cannot be null.
			entry.Title =(string)dr["Title"];

			if(dr["PostConfig"] != DBNull.Value)
			{
				entry.PostConfig = (PostConfig)((int)dr["PostConfig"]);
			}

			if(dr["ParentID"] != DBNull.Value)
			{
				entry.ParentID = (int)dr["ParentID"];
			}

			SetUrlPattern(entry);
			return entry;
		}

		public static void LoadSingleEntry(ref Entry entry, DataRow dr)
		{
			entry.PostType = ((PostType)(int)dr["PostType"]);

			if(dr["Author"] != DBNull.Value)
			{
				entry.Author = (string)dr["Author"];
			}
			if(dr["Email"] != DBNull.Value)
			{
				entry.Email = (string)dr["Email"];
			}

			
			entry.DateCreated = (DateTime)dr["DateAdded"];
			if(dr["DateUpdated"] != DBNull.Value)
			{
				entry.DateUpdated = (DateTime)dr["DateUpdated"];
			}
			entry.EntryID = (int)dr["ID"];

			if(dr["TitleUrl"] != DBNull.Value)
			{
				entry.TitleUrl = (string)dr["TitleUrl"];
			}
			
			if(dr["SourceName"] != DBNull.Value)
			{
				entry.SourceName = (string)dr["SourceName"];
			}
			if(dr["SourceUrl"] != DBNull.Value)
			{
				entry.SourceUrl = (string)dr["SourceUrl"];
			}

			if(dr["Description"] != DBNull.Value)
			{
				entry.Description = (string)dr["Description"];
			}

			if(dr["EntryName"] != DBNull.Value)
			{
				entry.EntryName = (string)dr["EntryName"];
			}

			if(dr["FeedBackCount"] != DBNull.Value)
			{
				entry.FeedBackCount = (int)dr["FeedBackCount"];
			}

			if(dr["Text"] != DBNull.Value)
			{
				entry.Body = (string)dr["Text"];
			}

			// Title cannot be null.
			entry.Title = (string)dr["Title"];
			
			if(dr["PostConfig"] != DBNull.Value)
			{
				entry.PostConfig = (PostConfig)((int)dr["PostConfig"]);
			}

			if(dr["ParentID"] != DBNull.Value)
			{
				entry.ParentID = (int)dr["ParentID"];
			}

			SetUrlPattern(entry);


		}		

		/// <summary>
		/// Returns a single CategoryEntry from a DataReader. Expects the data reader to have
		/// two sets of results. Should only be used to load 1 ENTRY
		/// </summary>
		/// <param name="reader"></param>
		/// <returns></returns>
		public static CategoryEntry LoadSingleCategoryEntry(IDataReader reader)
		{
			CategoryEntry entry = new CategoryEntry();
			LoadSingleEntry(reader, entry);

			reader.NextResult();

			System.Collections.ArrayList al = new System.Collections.ArrayList();
			while(reader.Read())
			{
				al.Add((string)reader["Title"]);
			}

			if(al.Count > 0)
			{
				entry.Categories = (string[])al.ToArray(typeof(string));
			}

			return entry;
		}


		public static CategoryEntry LoadSingleCategoryEntry(DataRow dr)
		{
			Entry entry = new CategoryEntry();
			LoadSingleEntry(ref entry, dr);

			DataRow[] child = dr.GetChildRows("cats");
			if(child != null && child.Length > 0)
			{
				int count = child.Length;
				string[] cats = new string[count];
				for(int i=0;i<count;i++)
				{
					cats[i] = (string)child[i]["Title"];
				}
				((CategoryEntry)entry).Categories = cats;	
			}

			return (CategoryEntry)entry;
		}

		public static int GetMaxItems(IDataReader reader)
		{
			reader.Read();
			return (int)reader["TotalRecords"];
		}

		#endregion

		#region Categories

		public static LinkCategory LoadSingleLinkCategory(IDataReader reader)
		{
			LinkCategory lc = new LinkCategory();
			lc.CategoryID = (int)reader["CategoryID"];
			lc.Title = (string)reader["Title"];
			lc.IsActive = (bool)reader["Active"];
			if(reader["CategoryType"] != DBNull.Value)
			{
				lc.CategoryType = (CategoryType)((byte)reader["CategoryType"]);
			}
			if(reader["Description"] != DBNull.Value)
			{
				lc.Description = (string)reader["Description"];
			}
			return lc;
		}

		public static LinkCategory LoadSingleLinkCategory(DataRow dr)
		{
			LinkCategory lc = new LinkCategory();
			// CategoryID cannot be null.
			lc.CategoryID = (int)dr["CategoryID"];

			// Title cannot be null.
			lc.Title = (string)dr["Title"];
			
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
			return lc;
		}

		#endregion

		#region Links

		public static Link LoadSingleLink(IDataReader reader)
		{
			Link link = new Link();
			// Active cannot be null
			link.IsActive = (bool)reader["Active"];

			if(reader["NewWindow"] != DBNull.Value)
			{
				link.NewWindow = (bool)reader["NewWindow"];
			}

			// LinkID cannot be null
			link.LinkID = (int)reader["LinkID"];
			
			if(reader["Rss"] != DBNull.Value)
			{
				link.Rss = (string)reader["Rss"];
			}
			
			if(reader["Url"] != DBNull.Value)
			{
				link.Url = (string)reader["Url"];
			}
			
			if(reader["Title"] != DBNull.Value)
			{
				link.Title = (string)reader["Title"];
			}

			if(reader["CategoryID"] != DBNull.Value)
			{
				link.CategoryID = (int)reader["CategoryID"];
			}
			
			if(reader["PostID"] != DBNull.Value)
			{
				link.PostID = (int)reader["PostID"];
			}
			return link;
		}

		public static Link LoadSingleLink(DataRow dr)
		{
			Link link = new Link();
			// Active cannot be null
			link.IsActive = (bool)dr["Active"];
			
			if(dr["NewWindow"] != DBNull.Value)
			{
				link.NewWindow = (bool)dr["NewWindow"];
			}

			//LinkID cannot be null.
			link.LinkID = (int)dr["LinkID"];
			
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
			
			if(dr["PostID"] != DBNull.Value)
			{
				link.PostID = (int)dr["PostID"];
			}
			return link;
		}

		#endregion

		#region Config

		public static BlogConfig LoadConfigData(IDataReader reader)
		{
			BlogConfig config = new BlogConfig();
			config.Author = (string)reader["Author"];
			config.BlogID = (int)reader["BlogID"];
			config.Email = (string)reader["Email"];
			config.Password = (string)reader["Password"];

			config.SubTitle = (string)reader["SubTitle"];
			config.Title = (string)reader["Title"];
			config.UserName = (string)reader["UserName"];
			config.TimeZone = (int)reader["TimeZone"];
			config.ItemCount = (int)reader["ItemCount"];
			config.Language = (string)reader["Language"];
			

			config.PostCount = (int)reader["PostCount"];
			config.CommentCount = (int)reader["CommentCount"];
			config.StoryCount = (int)reader["StoryCount"];
			config.PingTrackCount = (int)reader["PingTrackCount"];

			if(reader["News"] != DBNull.Value)
			{
				config.News = (string)reader["News"];
			}

			if(reader["LastUpdated"] != DBNull.Value)
			{
				config.LastUpdated = (DateTime)reader["LastUpdated"];
			}
			else
			{
				config.LastUpdated = new DateTime(2003,1,1);
			}
			config.Host = (string)reader["Host"];
			config.Application = (string)reader["Application"];

			config.Flag = (ConfigurationFlag)((int)reader["Flag"]);

			config.Skin = new SkinConfig();
			config.Skin.SkinName = (string)reader["Skin"];

			if(reader["SkinCssFile"] != DBNull.Value)
			{
				config.Skin.SkinCssFile = (string)reader["SkinCssFile"];
			}
		
			if(reader["SecondaryCss"] != DBNull.Value)
			{
				config.Skin.SkinCssText = (string)reader["SecondaryCss"];
			}

			//Wait till v2.0
			//config.ExtendedProperties = new ExtendedProperties((byte[])reader["NVC"]);

			return config;
		}

		#endregion

		#region Archive

		public static ArchiveCountCollection LoadArchiveCount(IDataReader reader)
		{
			const string dateformat = "{0}/{1}/{2}";
			string dt = null; //
			ArchiveCount ac =null;// new ArchiveCount();
			ArchiveCountCollection acc = new ArchiveCountCollection();
			
			while(reader.Read())
			{
				ac = new ArchiveCount();
				dt = string.Format(CultureInfo.CurrentCulture, dateformat, (int)reader["Month"],(int)reader["Day"],(int)reader["Year"]);
				ac.Date = DateTime.Parse(dt);
				//ac.Date = DateTime.ParseExact(dt,"MM/dd/yyyy",en);

				ac.Count = (int)reader["Count"];
				acc.Add(ac);
			}
			return acc;
			
		}

		//Needs to be handled else where!
		public static Link LoadArchiveLink(IDataReader reader)
		{
			Link link = new Link();
			int count = (int)reader["Count"];
			DateTime dt = new DateTime((int)reader["Year"],(int)reader["Month"],1);
			link.NewWindow = false;
			link.Title = dt.ToString("y") + " (" + count.ToString() + ")";
			//link.Url = Globals.ArchiveUrl(dt,"MMyyyy");
			link.NewWindow = false;
			link.IsActive = true;
			return link;
		}

		#endregion

		#region Image

		public static Image LoadSingleImage(IDataReader reader)
		{
			Image _image = new Image();
			_image.CategoryID = (int)reader["CategoryID"];
			_image.File = (string)reader["File"];
			_image.Height = (int)reader["Height"];
			_image.Width = (int)reader["Width"];
			_image.ImageID = (int)reader["ImageID"];
			_image.IsActive = (bool)reader["Active"];
			_image.Title = (string)reader["Title"];
			return _image;
		}

		#endregion

		#region Keywords

		public static KeyWord LoadSingleKeyWord(IDataReader reader)
		{
			KeyWord kw = new KeyWord();
			kw.KeyWordID = (int)reader["KeyWordID"];
			kw.BlogID = (int)reader["BlogID"];
			kw.OpenInNewWindow = (bool)reader["OpenInNewWindow"];
			kw.ReplaceFirstTimeOnly = (bool)reader["ReplaceFirstTimeOnly"];
			kw.CaseSensitive = (bool)reader["CaseSensitive"];
			kw.Text = (string)reader["Text"];
			if(reader["Title"] != DBNull.Value)
			{
				kw.Title = CheckNullString(reader["Title"]);
			}
			kw.Url = (string)reader["Url"];
			kw.Word = (string)reader["Word"];
			return kw;
		}



		#endregion

		#region Helpers

		public static string CheckNullString(object obj)
		{
			if(obj is DBNull)
			{
				return null;
			}
			return (string)obj;
		}

		public static object CheckNull(string text)
		{
			if(text != null && text.Trim().Length > 0)
			{
				return text;
			}
			return DBNull.Value;
		}

		public static string CheckNull(object obj)
		{
			return (string) obj;
		}

		public static string CheckNull(DBNull obj)
		{
			return null;
		}

		#endregion
	}
}

