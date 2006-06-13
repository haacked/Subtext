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
using System.Collections.Specialized;
using System.Data;
using Subtext.Extensibility;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;
using Subtext.Framework.Format;
using Subtext.Framework.Providers;
using Subtext.Framework.Text;
using Subtext.Framework.Util;

namespace Subtext.Framework.Data
{
	/// <summary>
	/// Concrete implementation of <see cref="ObjectProvider"/>. This 
	/// provides objects persisted to a database.  The specific database 
	/// is configured via a <see cref="DbProvider"/>.
	/// </summary>
	public class DatabaseObjectProvider : ObjectProvider
	{	
		string _name;

		#region Host
		/// <summary>
		/// Returns the <see cref="HostInfo"/> for the Subtext installation.
		/// </summary>
		/// <returns>A <see cref="HostInfo"/> instance.</returns>
		public override HostInfo LoadHostInfo(HostInfo hostInfo)
		{
			using(IDataReader reader = DbProvider.Instance().GetHost())
			{
				if(reader.Read())
				{
					DataHelper.LoadHost(reader, hostInfo);
					reader.Close();
					return hostInfo;
				}
				reader.Close();
			}
			return null;
		}

		/// <summary>
		/// Updates the <see cref="HostInfo"/> instance.  If the host record is not in the 
		/// database, one is created. There should only be one host record.
		/// </summary>
		/// <param name="host">The host information.</param>
		public override bool UpdateHost(HostInfo host)
		{
			return DbProvider.Instance().UpdateHost(host);
		}

		#endregion Host

		#region Blogs
		/// <summary>
		/// Gets a pageable <see cref="IList"/> of <see cref="BlogInfo"/> instances.
		/// </summary>
		/// <param name="pageIndex">Page index.</param>
		/// <param name="pageSize">Size of the page.</param>
		/// <param name="sortDescending">Sort descending.</param>
		/// <returns></returns>
        public override PagedCollection<BlogInfo> GetPagedBlogs(int pageIndex, int pageSize, bool sortDescending)
		{
			IDataReader reader = DbProvider.Instance().GetPagedBlogs(pageIndex, pageSize, sortDescending);
			try
			{
                PagedCollection<BlogInfo> pec = new PagedCollection<BlogInfo>();
				while(reader.Read())
				{
					pec.Add(DataHelper.LoadConfigData(reader));
				}
				reader.NextResult();
				pec.MaxItems = DataHelper.GetMaxItems(reader);
				return pec;
				
			}
			finally
			{
				reader.Close();
			}
		}

		/// <summary>
		/// Gets the blog by id.
		/// </summary>
		/// <param name="blogId">Blog id.</param>
		/// <returns></returns>
		public override BlogInfo GetBlogById(int blogId)
		{
			using(IDataReader reader = DbProvider.Instance().GetBlogById(blogId))
			{
				if(reader.Read())
				{
					BlogInfo info = DataHelper.LoadConfigData(reader);
					reader.Close();
					return info;
				}
				reader.Close();
			}
			return null;
		}

		/// <summary>
        /// Returns <see cref="IPagedCollection"/> with the blogs that 
		/// have the specified host.
		/// </summary>
		/// <param name="host">Host.</param>
		/// <returns></returns>
        public override IPagedCollection<BlogInfo> GetBlogsByHost(string host)
		{
			IDataReader reader = DbProvider.Instance().GetBlogsByHost(host);
			try
			{
                IPagedCollection<BlogInfo> pec = new PagedCollection<BlogInfo>();
				while(reader.Read())
				{
					pec.Add(DataHelper.LoadConfigData(reader));
				}
				reader.NextResult();
				pec.MaxItems = DataHelper.GetMaxItems(reader);
				return pec;
				
			}
			finally
			{
				reader.Close();
			}
		}
		#endregion

		#region Entries

		#region Paged Posts

		/// <summary>
		/// Gets the paged entries.
		/// </summary>
		/// <param name="postType">Type of the post.</param>
		/// <param name="categoryID">The category ID.</param>
		/// <param name="pageIndex">Index of the page.</param>
		/// <param name="pageSize">Size of the page.</param>
		/// <param name="sortDescending">if set to <c>true</c> [sort descending].</param>
		/// <returns></returns>
        public override IPagedCollection<Entry> GetPagedEntries(PostType postType, int categoryID, int pageIndex, int pageSize, bool sortDescending)
		{
			IDataReader reader = DbProvider.Instance().GetPagedEntries(postType,categoryID,pageIndex,pageSize,sortDescending);
			try
			{
                PagedCollection<Entry> pec = new PagedCollection<Entry>();
				while(reader.Read())
				{
					pec.Add(DataHelper.LoadSingleEntryStatsView(reader));
				}
				reader.NextResult();
				pec.MaxItems = DataHelper.GetMaxItems(reader);
				return pec;
				
			}
			finally
			{
				reader.Close();
			}
		}

		/// <summary>
		/// Gets the paged feedback.
		/// </summary>
		/// <param name="pageIndex">Index of the page.</param>
		/// <param name="pageSize">Size of the page.</param>
		/// <param name="sortDescending">if set to <c>true</c> [sort descending].</param>
		/// <returns></returns>
        public override IPagedCollection<Entry> GetPagedFeedback(int pageIndex, int pageSize, bool sortDescending)
		{
			IDataReader reader = DbProvider.Instance().GetPagedFeedback(pageIndex, pageSize, sortDescending);
            IPagedCollection<Entry> pec = new PagedCollection<Entry>();
			while(reader.Read())
			{
				pec.Add(DataHelper.LoadSingleEntry(reader));
			}
			reader.NextResult();
			pec.MaxItems = DataHelper.GetMaxItems(reader);
			return pec;
		}

		#endregion

		#region EntryDays

		public override EntryDay GetSingleDay(DateTime dt)
		{
			IDataReader reader = DbProvider.Instance().GetSingleDay(dt);
			try
			{
				EntryDay ed = new EntryDay(dt);
				while(reader.Read())
				{
					ed.Add(DataHelper.LoadSingleEntry(reader));
				}
				return ed;
			}
			finally
			{
				reader.Close();
			}
		}

		/// <summary>
		/// Returns blog posts that meet the criteria specified in the <see cref="PostConfig"/> flags.
		/// </summary>
		/// <param name="itemCount">Item count.</param>
		/// <param name="pc">Pc.</param>
		/// <returns></returns>
        public override ICollection<EntryDay> GetBlogPosts(int itemCount, PostConfig pc)
		{
			IDataReader reader = DbProvider.Instance().GetConditionalEntries(itemCount, PostType.BlogPost, pc);
			try
			{
                ICollection<EntryDay> edc = DataHelper.LoadEntryDayCollection(reader);
				return edc;
			}
			finally
			{
				reader.Close();
			}
		}

        public override ICollection<EntryDay> GetRecentDayPosts(int itemCount, bool activeOnly)
		{
			IDataReader reader = DbProvider.Instance().GetRecentDayPosts(itemCount, activeOnly);
			try
			{
                ICollection<EntryDay> edc = DataHelper.LoadEntryDayCollection(reader);
				return edc;
			}
			finally
			{
				reader.Close();
			}
		}

        public override ICollection<EntryDay> GetPostsByMonth(int month, int year)
		{
			IDataReader reader = DbProvider.Instance().GetPostCollectionByMonth(month,year);
			try
			{
                ICollection<EntryDay> edc = DataHelper.LoadEntryDayCollection(reader);
				return edc;
			}
			finally
			{
				reader.Close();
			}
		}

        public override ICollection<EntryDay> GetPostsByCategoryID(int itemCount, int catID)
		{
			IDataReader reader = DbProvider.Instance().GetPostsByCategoryID(itemCount,catID);
			try
			{
                ICollection<EntryDay> edc = DataHelper.LoadEntryDayCollection(reader);
				return edc;
			}
			finally
			{
				reader.Close();
			}
		}

		#endregion

		#region EntryCollections

		/// <summary>
		/// Gets the entries that meet the specific <see cref="PostType"/> 
		/// and the <see cref="PostConfig"/> flags.
		/// </summary>
		/// <remarks>
		/// This is called to get the main syndicated entries.
		/// </remarks>
		/// <param name="itemCount">Item count.</param>
		/// <param name="pt">Pt.</param>
		/// <param name="pc">Pc.</param>
		/// <returns></returns>
		public override IList<Entry> GetConditionalEntries(int itemCount, PostType pt, PostConfig pc)
		{
			IDataReader reader = DbProvider.Instance().GetConditionalEntries(itemCount, pt, pc);
			return LoadEntryCollectionFromDataReader(reader);
		}

		public override IList<Entry> GetFeedBack(Entry parentEntry)
		{
			IDataReader reader = DbProvider.Instance().GetFeedBack(parentEntry.EntryID);
			UrlFormats formats = Config.CurrentBlog.UrlFormats;
			const bool buildLinks = true;
			try
			{
				List<Entry> ec = new List<Entry>();
				Entry entry;
				while(reader.Read())
				{
					//Don't build links.
					entry = DataHelper.LoadSingleEntry(reader, !buildLinks);
					entry.Url = formats.CommentUrl(parentEntry, entry);
					ec.Add(entry);
				}
				return ec;
			}
			finally
			{
				reader.Close();
			}
		}

        public override ICollection<CategoryEntry> GetRecentPostsWithCategories(int itemCount, bool activeOnly)
		{
			DataSet ds = DbProvider.Instance().GetRecentPostsWithCategories(itemCount, activeOnly);
            List<CategoryEntry> ec = new List<CategoryEntry>();
			int count = ds.Tables[0].Rows.Count;
			for(int i =0; i<count; i++)
			{
				DataRow row = ds.Tables[0].Rows[i];
				CategoryEntry ce = DataHelper.LoadSingleCategoryEntry(row);
				ec.Add(ce);
			}
			return ec;
		}

		public override IList<Entry> GetRecentPosts(int itemCount, PostType postType, bool activeOnly)
		{
			IDataReader reader = DbProvider.Instance().GetRecentPosts(itemCount, postType, activeOnly);
			return LoadEntryCollectionFromDataReader(reader);
		}

		public override IList<Entry> GetRecentPosts(int itemCount, PostType postType, bool activeOnly, DateTime dateUpdated)
		{
			IDataReader reader = DbProvider.Instance().GetRecentPosts(itemCount, postType, activeOnly, dateUpdated);
			return LoadEntryCollectionFromDataReader(reader);
		}

		public override IList<Entry> GetPostCollectionByMonth(int month, int year)
		{
			IDataReader reader = DbProvider.Instance().GetPostCollectionByMonth(month,year);
			return LoadEntryCollectionFromDataReader(reader);
		}

		public override IList<Entry> GetPostsByDayRange(DateTime start, DateTime stop, PostType postType, bool activeOnly)
		{
			IDataReader reader;
			if(stop > start)
			{
				reader = DbProvider.Instance().GetEntriesByDateRangle(start, stop, postType, activeOnly);
			}
			else
			{
				reader = DbProvider.Instance().GetEntriesByDateRangle(stop, start, postType, activeOnly);
			}

			try
			{
				IList<Entry> ec = DataHelper.LoadEntryCollection(reader);
				return ec;
			}
			finally
			{
				reader.Close();
			}
		}

		public override IList<Entry> GetEntriesByCategory(int itemCount, int catID, bool activeOnly)
		{
			IDataReader reader = DbProvider.Instance().GetEntriesByCategory(itemCount,catID, activeOnly);
			return LoadEntryCollectionFromDataReader(reader);
		}

		IList<Entry> LoadEntryCollectionFromDataReader(IDataReader reader)
		{
			try
			{
				IList<Entry> ec = DataHelper.LoadEntryCollection(reader);
				return ec;
			}
			finally
			{
				reader.Close();
			}
		}

		#endregion

		#region Single Entry
		Entry LoadEntryFromReader(IDataReader reader)
		{
			try
			{
				Entry entry = null;
				while(reader.Read())
				{
					entry = DataHelper.LoadSingleEntry(reader);
					break;
				}
				return entry;
			}
			finally
			{
				reader.Close();
			}
		}

		/// <summary>
		/// Searches the data store for the first comment with a 
		/// matching checksum hash.
		/// </summary>
		/// <param name="checksumHash">Checksum hash.</param>
		/// <returns></returns>
		public override Entry GetCommentByChecksumHash(string checksumHash)
		{
			IDataReader reader = DbProvider.Instance().GetCommentByChecksumHash(checksumHash);
			return LoadEntryFromReader(reader);
		}

		public override Entry GetEntry(int postID, bool activeOnly)
		{
			IDataReader reader = DbProvider.Instance().GetEntry(postID, activeOnly);
			return LoadEntryFromReader(reader);
		}


		public override Entry GetEntry(string EntryName, bool activeOnly)
		{
			IDataReader reader = DbProvider.Instance().GetEntry(EntryName, activeOnly);
			return LoadEntryFromReader(reader);
		}

		public override CategoryEntry GetCategoryEntry(int postid, bool activeOnly)
		{
			IDataReader reader = DbProvider.Instance().GetCategoryEntry(postid, activeOnly);
			try
			{
				CategoryEntry entry = null;
				while(reader.Read())
				{
					entry = DataHelper.LoadSingleCategoryEntry(reader);
					break;
				}
				return entry;
			}
			finally
			{
				reader.Close();
			}
		}
		#endregion

		#region Delete

		public override bool Delete(int PostID)
		{
			return DbProvider.Instance().DeleteEntry(PostID);
		}

		#endregion

		#region Create Entry

		/// <summary>
		/// Creates the specified entry in the back end data store attaching 
		/// the specified category ids.
		/// </summary>
		/// <param name="entry">Entry.</param>
		/// <param name="CategoryIDs">Category I ds.</param>
		/// <returns></returns>
		public override int Create(Entry entry, int[] CategoryIDs)
		{
			if(entry.PostType == PostType.PingTrack)
			{
				return DbProvider.Instance().InsertPingTrackEntry(entry);
			}

			if(!FormatEntry(entry,true))
			{
				throw new BlogFailedPostException("Failed post exception");
			}		

			if(entry is CategoryEntry)
			{
				entry.EntryID = DbProvider.Instance().InsertCategoryEntry(((CategoryEntry)entry));
			}
			else
			{
				entry.EntryID = DbProvider.Instance().InsertEntry(entry);	
		
				if(CategoryIDs != null)
				{
					DbProvider.Instance().SetEntryCategoryList(entry.EntryID,CategoryIDs);
				}
			}

			if(entry.EntryID > -1 && Config.Settings.Tracking.UseTrackingServices)
			{
				entry.Url = Subtext.Framework.Configuration.Config.CurrentBlog.UrlFormats.EntryUrl(entry);
			}

			if(entry.EntryID > -1)
			{
				Config.CurrentBlog.LastUpdated = entry.DateCreated;
			}

			return entry.EntryID;
		}

		#endregion

		#region Update

		public override bool Update(Entry entry)
		{
			return Update(entry,null);
		}

		public override bool Update(Entry entry, int[] CategoryIDs)
		{
			if(!FormatEntry(entry,false))
			{
				throw new BlogFailedPostException("Failed post exception");
			}

			if(entry is CategoryEntry)
			{
				if(!DbProvider.Instance().UpdateCategoryEntry(((CategoryEntry)entry)))
				{
					return false;
				}
			}
			else
			{
				if(!DbProvider.Instance().UpdateEntry(entry))
				{
					return false;
				}
		
				if(CategoryIDs != null)
				{
					DbProvider.Instance().SetEntryCategoryList(entry.EntryID,CategoryIDs);
				}
			}

			if(Config.Settings.Tracking.UseTrackingServices)
			{
				if(entry.PostType == PostType.BlogPost)
				{
					entry.Url = Config.CurrentBlog.UrlFormats.EntryUrl(entry);
				}
				else
				{
					entry.Url = Config.CurrentBlog.UrlFormats.ArticleUrl(entry);
				}

				if(entry.EntryID > -1)
				{
					Config.CurrentBlog.LastUpdated = entry.DateUpdated;
				}
			}
			return true;
		}

		#endregion

		#region SetCategoriesList

		public override bool SetEntryCategoryList(int EntryID, int[] Categories)
		{
			return DbProvider.Instance().SetEntryCategoryList(EntryID,Categories);
		}

		#endregion
        
		#region Format Helper
		
		private bool FormatEntry(Entry e, bool UseKeyWords)
		{
			//Do this before we validate the text
			if(UseKeyWords)
			{
				KeyWords.Format(e);
			}

			//TODO: Make this a configuration option.
			e.Body = Transform.EmoticonTransforms(e.Body);

			if(Text.HtmlHelper.HasIllegalContent(e.Body))
			{
				return false;
			}

			if(Text.HtmlHelper.HasIllegalContent(e.Title))
			{
				return false;
			}

			if(Text.HtmlHelper.HasIllegalContent(e.Description))
			{
				return false;
			}

			if(Text.HtmlHelper.HasIllegalContent(e.TitleUrl))
			{
				return false;
			}

			if(Config.Settings.UseXHTML)
			{
				if(!HtmlHelper.ConvertHtmlToXHtml(e))
				{
					return false;
				}
			}

			return true;
		}

		#endregion

		#endregion

		#region Links/Categories

		#region Paged Links

        public override IPagedCollection<Link> GetPagedLinks(int categoryTypeID, int pageIndex, int pageSize, bool sortDescending)
		{
			IDataReader reader = DbProvider.Instance().GetPagedLinks(categoryTypeID, pageIndex, pageSize, sortDescending);
			try
			{
                IPagedCollection<Link> plc = new PagedCollection<Link>();
				while(reader.Read())
				{
					plc.Add(DataHelper.LoadSingleLink(reader));
				}
				reader.NextResult();
				plc.MaxItems = DataHelper.GetMaxItems(reader);
				return plc;
			}
			finally
			{
				reader.Close();
			}

		}

		#endregion

		#region LinkCollection

		public override ICollection<Link> GetLinkCollectionByPostID(int PostID)
		{
			IDataReader reader = DbProvider.Instance().GetLinkCollectionByPostID(PostID);
			try
			{
				ICollection<Link> lc = new List<Link>();
				while(reader.Read())
				{
					lc.Add(DataHelper.LoadSingleLink(reader));
				}
				return lc;
			}
			finally
			{
				reader.Close();
			}
		}

		public override ICollection<Link> GetLinksByCategoryID(int catID, bool activeOnly)
		{
			IDataReader reader = DbProvider.Instance().GetLinksByCategoryID(catID, activeOnly);
            List<Link> lc = new List<Link>();
			try
			{
				while(reader.Read())
				{
					lc.Add(DataHelper.LoadSingleLink(reader));
				}
				return lc;
			}
			finally
			{
				reader.Close();
			}
		}

		#endregion

		#region Single Link

		public override Link GetSingleLink(int linkID)
		{
			IDataReader reader = DbProvider.Instance().GetSingleLink(linkID);
			try
			{
				Link link = null;
				while(reader.Read())
				{
					link = DataHelper.LoadSingleLink(reader);
					break;
				}
				return link;
			}
			finally
			{
				reader.Close();
			}			
		}

		#endregion

        #region ICollection<LinkCategory>

        public override ICollection<LinkCategory> GetCategories(CategoryType catType, bool activeOnly)
		{
			IDataReader reader = DbProvider.Instance().GetCategories(catType, activeOnly);
            ICollection<LinkCategory> lcc = new List<LinkCategory>();
			try
			{
				while(reader.Read())
				{
					lcc.Add(DataHelper.LoadSingleLinkCategory(reader));
				}
				return lcc;
			}
			finally
			{
				reader.Close();
			}
		}

        public override ICollection<LinkCategory> GetActiveCategories()
		{
			DataSet ds = DbProvider.Instance().GetActiveCategories();
            ICollection<LinkCategory> lcc = new List<LinkCategory>();
			foreach(DataRow dr in ds.Tables[0].Rows)
			{
				LinkCategory lc = DataHelper.LoadSingleLinkCategory(dr);
				lc.Links = new List<Link>();
				foreach(DataRow drLink in dr.GetChildRows("CategoryID"))
				{
					lc.Links.Add(DataHelper.LoadSingleLink(drLink));
				}
				lcc.Add(lc);				
			}
			return lcc;
		}

		#endregion

		#region LinkCategory

		public override LinkCategory GetLinkCategory(int CategoryID, bool IsActive)
		{
			IDataReader reader = DbProvider.Instance().GetLinkCategory(CategoryID,IsActive);
			
			try
			{
				reader.Read();
				LinkCategory lc = DataHelper.LoadSingleLinkCategory(reader);
				return lc;
			}
			finally
			{
				reader.Close();
			}
		}

		public override LinkCategory GetLinkCategory(string categoryName,bool IsActive)
		{
			IDataReader reader = DbProvider.Instance().GetLinkCategory(categoryName,IsActive);
			
			try
			{
				reader.Read();
				LinkCategory lc = DataHelper.LoadSingleLinkCategory(reader);
				return lc;
			}
			finally
			{
				reader.Close();
			}
		}

		#endregion

		#region Edit Links/Categories

		public override bool UpdateLink(Link link)
		{
			return DbProvider.Instance().UpdateLink(link);
		}

		public override int CreateLink(Link link)
		{
			return DbProvider.Instance().InsertLink(link);
		}

		public override bool UpdateLinkCategory(LinkCategory lc)
		{
			return DbProvider.Instance().UpdateCategory(lc);
		}
		
		public override int CreateLinkCategory(LinkCategory lc)
		{
			return DbProvider.Instance().InsertCategory(lc);
		}

		public override bool DeleteLinkCategory(int CategoryID)
		{
			return DbProvider.Instance().DeleteCategory(CategoryID);
		}

		public override bool DeleteLink(int LinkID)
		{
			return DbProvider.Instance().DeleteLink(LinkID);
		}

		#endregion

		#endregion

		#region Stats

        public override IPagedCollection<ViewStat> GetPagedViewStats(int pageIndex, int pageSize, DateTime beginDate, DateTime endDate)
		{
			IDataReader reader = DbProvider.Instance().GetPagedViewStats(pageIndex,pageSize,beginDate,endDate);
			try
			{
                IPagedCollection<ViewStat> vs = new PagedCollection<ViewStat>();
				while(reader.Read())
				{
					vs.Add(DataHelper.LoadSingleViewStat(reader));
				}
				reader.NextResult();
				vs.MaxItems = DataHelper.GetMaxItems(reader);
				return vs;
			}
			finally
			{
				reader.Close();
			}	
		}

        public override IPagedCollection<Referrer> GetPagedReferrers(int pageIndex, int pageSize)
		{
			IDataReader reader = DbProvider.Instance().GetPagedReferrers(pageIndex, pageSize);
            return LoadPagedReferrersCollection(reader);
        }

        public override IPagedCollection<Referrer> GetPagedReferrers(int pageIndex, int pageSize, int entryId)
		{
			IDataReader reader = DbProvider.Instance().GetPagedReferrers(pageIndex, pageSize, entryId);
            return LoadPagedReferrersCollection(reader);
		}
	    
	    private IPagedCollection<Referrer> LoadPagedReferrersCollection(IDataReader reader)
	    {
            try
            {
                IPagedCollection<Referrer> prc = new PagedCollection<Referrer>();
                while (reader.Read())
                {
                    prc.Add(DataHelper.LoadSingleReferrer(reader));
                }
                reader.NextResult();
                prc.MaxItems = DataHelper.GetMaxItems(reader);
                return prc;
            }
            finally
            {
                reader.Close();
            }
	    }

		public override bool TrackEntry(EntryView ev)
		{
			return DbProvider.Instance().TrackEntry(ev);
		}

		public override bool TrackEntry(IEnumerable<EntryView> evc)
		{
			return DbProvider.Instance().TrackEntry(evc);
		}

		#endregion

		#region  Configuration

		/// <summary>
		/// Adds the initial blog configuration.  This is a convenience method for 
		/// allowing a user with a freshly installed blog to immediately gain access 
		/// to the admin section to edit the blog.
		/// </summary>
		/// <param name="title"></param>
		/// <param name="host"></param>
		/// <param name="subfolder"></param>
		/// <param name="userName">Name of the user.</param>
		/// <param name="password">Password.</param>
		/// <returns></returns>
		public override bool CreateBlog(string title, string userName, string password, string host, string subfolder)
		{
			return DbProvider.Instance().AddBlogConfiguration(title, userName, password, host, subfolder);
		}
		
		public override bool UpdateBlog(BlogInfo info)
		{
			return DbProvider.Instance().UpdateBlog(info);
		}

		/// <summary>
		/// Returns a <see cref="BlogInfo"/> instance containing 
		/// the configuration settings for the blog specified by the 
		/// Hostname and Application.
		/// </summary>
		/// <remarks>
		/// Until Subtext supports multiple blogs again (if ever), 
		/// this will always return the same instance.
		/// </remarks>
		/// <param name="hostname">Hostname.</param>
		/// <param name="subfolder">Subfolder.</param>
		/// <param name="strict">If false, then this will return a blog record if 
		/// there is only one blog record, regardless if the subfolder and hostname match.</param>
		/// <returns></returns>
		public override BlogInfo GetBlogInfo(string hostname, string subfolder, bool strict)
		{
			IDataReader reader = DbProvider.Instance().GetBlogInfo(hostname, subfolder, strict);
			try
			{
				BlogInfo info = null;
				while(reader.Read())
				{
					info = DataHelper.LoadConfigData(reader);
					break;
				}
				return info;
			}
			finally
			{
				reader.Close();
			}
		}
		#endregion

		#region KeyWords

		public override KeyWord GetKeyWord(int KeyWordID)
		{
			IDataReader reader = DbProvider.Instance().GetKeyWord(KeyWordID);
			try
			{
				KeyWord kw = null;
				while(reader.Read())
				{
					kw = DataHelper.LoadSingleKeyWord(reader);
					break;
				}
				return kw;
			}
			finally
			{
				reader.Close();
			}
		}
		
		public override ICollection<KeyWord> GetKeyWords()
		{
			IDataReader reader = DbProvider.Instance().GetKeyWords();
			try
			{
				List<KeyWord> kwc = new List<KeyWord>();
				while(reader.Read())
				{
					kwc.Add(DataHelper.LoadSingleKeyWord(reader));
				}
				return kwc;
			}
			finally
			{
				reader.Close();
			}
		}

        public override IPagedCollection<KeyWord> GetPagedKeyWords(int pageIndex, int pageSize, bool sortDescending)
		{
			IDataReader reader = DbProvider.Instance().GetPagedKeyWords(pageIndex,pageSize,sortDescending);
			try
			{
                IPagedCollection<KeyWord> pkwc = new PagedCollection<KeyWord>();
				while(reader.Read())
				{
					pkwc.Add(DataHelper.LoadSingleKeyWord(reader));
				}
				reader.NextResult();
				pkwc.MaxItems = DataHelper.GetMaxItems(reader);

				return pkwc;
			}
			finally
			{
				reader.Close();
			}
		}
		
		public override bool UpdateKeyWord(KeyWord kw)
		{
			return DbProvider.Instance().UpdateKeyWord(kw);
		}

		public override int InsertKeyWord(KeyWord kw)
		{
			return DbProvider.Instance().InsertKeyWord(kw);
		}

		public override bool DeleteKeyWord(int KeyWordID)
		{
			return DbProvider.Instance().DeleteKeyWord(KeyWordID);
		}

		#endregion

		#region Images

        public override ImageCollection GetImagesByCategoryID(int catID, bool activeOnly)
		{
			IDataReader reader = DbProvider.Instance().GetImagesByCategoryID(catID, activeOnly);
			try
			{
				ImageCollection ic = new ImageCollection();
				while(reader.Read())
				{
					ic.Category = DataHelper.LoadSingleLinkCategory(reader);
					break;
				}
				reader.NextResult();
				while(reader.Read())
				{
					ic.Add(DataHelper.LoadSingleImage(reader));
				}
				return ic;
			}
			finally
			{
				reader.Close();
			}
		}

		public override Image GetSingleImage(int imageID, bool activeOnly)
		{
			IDataReader reader = DbProvider.Instance().GetSingleImage(imageID, activeOnly);
			try
			{
				Image image = null;
				while(reader.Read())
				{
					image = DataHelper.LoadSingleImage(reader);
				}
				return image;
			}
			finally
			{
				reader.Close();
			}
		}

		public override int InsertImage(Subtext.Framework.Components.Image _image)
		{
			return DbProvider.Instance().InsertImage(_image);
		}

		public override bool UpdateImage(Subtext.Framework.Components.Image image)
		{
			return DbProvider.Instance().UpdateImage(image);
		}

		public override bool DeleteImage(int ImageID)
		{
			return DbProvider.Instance().DeleteImage(ImageID);
		}

		#endregion

		#region Archives

		public override ArchiveCountCollection GetPostsByMonthArchive()
		{
			IDataReader reader = DbProvider.Instance().GetPostsByMonthArchive();
			try
			{
				ArchiveCountCollection acc = DataHelper.LoadArchiveCount(reader);
				return acc;
			}
			finally
			{
				reader.Close();
			}
		}

		public override ArchiveCountCollection GetPostsByYearArchive()
		{
			IDataReader reader = DbProvider.Instance().GetPostsByYearArchive();
			try
			{
				ArchiveCountCollection acc = DataHelper.LoadArchiveCount(reader);
				return acc;
			}
			finally
			{
				reader.Close();
			}
		}

		#endregion

		/// <summary>
		/// Initializes the specified provider.
		/// </summary>
		/// <param name="name">Friendly Name of the provider.</param>
		/// <param name="configValue">Config value.</param>
		public override void Initialize(string name, NameValueCollection configValue)
		{
			_name = name;
		}

		/// <summary>
		/// Returns the friendly name of the provider when the provider is initialized.
		/// </summary>
		/// <value></value>
		public override string Name
		{
			get
			{
				return _name;
			}
		}
	}
}
