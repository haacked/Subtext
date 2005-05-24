using System;
using System.Data;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;
using Subtext.Framework.Format;
using Subtext.Framework.Providers;
using Subtext.Framework.Text;
using Subtext.Framework.Tracking;
using Subtext.Framework.Util;

namespace Subtext.Framework.Data
{
	/// <summary>
	/// Summary description for DataDTOProvider.
	/// </summary>
	public class DataDTOProvider : Subtext.Framework.Data.IDTOProvider
	{	
		#region Blogs
		/// <summary>
		/// Gets a pageable <see cref="BlogInfoCollection"/> of <see cref="BlogInfo"/> instances.
		/// </summary>
		/// <param name="pageIndex">Page index.</param>
		/// <param name="pageSize">Size of the page.</param>
		/// <param name="sortDescending">Sort descending.</param>
		/// <returns></returns>
		public BlogInfoCollection GetPagedBlogs(int pageIndex, int pageSize, bool sortDescending)
		{
			IDataReader reader = DbProvider.Instance().GetPagedBlogs(pageIndex, pageSize, sortDescending);
			try
			{
				BlogInfoCollection pec = new BlogInfoCollection();
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
		public BlogInfo GetBlogById(int blogId)
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
		/// Returns <see cref="BlogInfoCollection"/> with the blogs that 
		/// have the specified host.
		/// </summary>
		/// <param name="host">Host.</param>
		/// <returns></returns>
		public BlogInfoCollection GetBlogsByHost(string host)
		{
			IDataReader reader = DbProvider.Instance().GetBlogsByHost(host);
			try
			{
				BlogInfoCollection pec = new BlogInfoCollection();
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

		public PagedEntryCollection GetPagedEntries(PostType postType, int categoryID, int pageIndex, int pageSize, bool sortDescending)
		{
			IDataReader reader = DbProvider.Instance().GetPagedEntries(postType,categoryID,pageIndex,pageSize,sortDescending);
			try
			{
				PagedEntryCollection pec = new PagedEntryCollection();
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

		public PagedEntryCollection GetPagedFeedback(int pageIndex, int pageSize, bool sortDescending)
		{
			IDataReader reader = DbProvider.Instance().GetPagedFeedback(pageIndex,pageSize,sortDescending);
			PagedEntryCollection pec = new PagedEntryCollection();
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

		public EntryDay GetSingleDay(DateTime dt)
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

		public EntryDayCollection GetConditionalEntries(int ItemCount, PostConfig pc)
		{
			IDataReader reader = DbProvider.Instance().GetConditionalEntries(ItemCount,PostType.BlogPost,pc);
			try
			{
				EntryDayCollection edc = DataHelper.LoadEntryDayCollection(reader);
				return edc;
			}
			finally
			{
				reader.Close();
			}
		}

		public EntryDayCollection GetRecentDayPosts(int ItemCount, bool ActiveOnly)
		{
			IDataReader reader = DbProvider.Instance().GetRecentDayPosts(ItemCount,ActiveOnly);
			try
			{
				EntryDayCollection edc = DataHelper.LoadEntryDayCollection(reader);
				return edc;
			}
			finally
			{
				reader.Close();
			}
		}

		public EntryDayCollection GetPostsByMonth(int month, int year)
		{
			IDataReader reader = DbProvider.Instance().GetPostCollectionByMonth(month,year);
			try
			{
				EntryDayCollection edc = DataHelper.LoadEntryDayCollection(reader);
				return edc;
			}
			finally
			{
				reader.Close();
			}
		}

		public EntryDayCollection GetPostsByCategoryID(int ItemCount, int catID)
		{
			IDataReader reader = DbProvider.Instance().GetPostsByCategoryID(ItemCount,catID);
			try
			{
				EntryDayCollection edc = DataHelper.LoadEntryDayCollection(reader);
				return edc;
			}
			finally
			{
				reader.Close();
			}
		}

		#endregion

		#region EntryCollections

		public EntryCollection GetConditionalEntries(int ItemCount, PostType pt, PostConfig pc)
		{
			IDataReader reader = DbProvider.Instance().GetConditionalEntries(ItemCount,pt,pc);
			try
			{
				EntryCollection ec = DataHelper.LoadEntryCollection(reader);
				return ec;
			}
			finally
			{
				reader.Close();
			}
		}

		public EntryCollection GetConditionalEntries(int ItemCount, PostType pt, PostConfig pc, DateTime DateUpdated)
		{
			IDataReader reader = DbProvider.Instance().GetConditionalEntries(ItemCount,pt,pc,DateUpdated);
			try
			{
				EntryCollection ec = DataHelper.LoadEntryCollection(reader);
				return ec;
			}
			finally
			{
				reader.Close();
			}
		}

		public EntryCollection GetFeedBack(int ParrentID)
		{
			IDataReader reader = DbProvider.Instance().GetFeedBack(ParrentID);
			try
			{
				EntryCollection ec = DataHelper.LoadEntryCollection(reader);
				return ec;
			}
			finally
			{
				reader.Close();
			}
		}

		public EntryCollection GetFeedBack(Entry ParentEntry)
		{
			IDataReader reader = DbProvider.Instance().GetFeedBack(ParentEntry.EntryID);
			UrlFormats formats = Config.CurrentBlog.UrlFormats;
			try
			{
				EntryCollection ec = new EntryCollection();
				Entry entry = null;
				while(reader.Read())
				{
					entry = DataHelper.LoadSingleEntry(reader,false);
					entry.Link = formats.CommentUrl(ParentEntry,entry);
					ec.Add(entry);
				}
				return ec;
			}
			finally
			{
				reader.Close();
			}
		}

		public EntryCollection GetRecentPostsWithCategories(int ItemCount, bool ActiveOnly)
		{
			DataSet ds = DbProvider.Instance().GetRecentPostsWithCategories(ItemCount, ActiveOnly);
			EntryCollection ec = new EntryCollection();
			int count = ds.Tables[0].Rows.Count;
			for(int i =0;i<count;i++)
			{
				DataRow row = ds.Tables[0].Rows[i];
				CategoryEntry ce = DataHelper.LoadSingleCategoryEntry(row);
				ec.Add(ce);
			}
			return ec;
		}

		public EntryCollection GetRecentPosts(int ItemCount, PostType postType, bool ActiveOnly)
		{
			IDataReader reader = DbProvider.Instance().GetRecentPosts(ItemCount,postType,ActiveOnly);
			try
			{
				EntryCollection ec = DataHelper.LoadEntryCollection(reader);
				return ec;
			}
			finally
			{
				reader.Close();
			}
		}

		public EntryCollection GetRecentPosts(int ItemCount, PostType postType, bool ActiveOnly, DateTime DateUpdated)
		{
			IDataReader reader = DbProvider.Instance().GetRecentPosts(ItemCount,postType,ActiveOnly,DateUpdated);
			try
			{
				EntryCollection ec = DataHelper.LoadEntryCollection(reader);
				return ec;
			}
			finally
			{
				reader.Close();
			}
		}

		public EntryCollection GetPostCollectionByMonth(int month, int year)
		{
			IDataReader reader = DbProvider.Instance().GetPostCollectionByMonth(month,year);
			try
			{
				EntryCollection ec = DataHelper.LoadEntryCollection(reader);
				return ec;
			}
			finally
			{
				reader.Close();
			}
		}

		public EntryCollection GetPostsByDayRange(DateTime start, DateTime stop, PostType postType, bool ActiveOnly)
		{
			IDataReader reader = null;
			if(stop > start)
			{
				reader = DbProvider.Instance().GetEntriesByDateRangle(start,stop,postType,ActiveOnly);
			}
			else
			{
				reader = DbProvider.Instance().GetEntriesByDateRangle(stop,start,postType,ActiveOnly);
			}

			try
			{
				EntryCollection ec = DataHelper.LoadEntryCollection(reader);
				return ec;
			}
			finally
			{
				reader.Close();
			}
		}

		public EntryCollection GetEntriesByCategory(int ItemCount, string categoryName, bool ActiveOnly)
		{
			IDataReader reader = DbProvider.Instance().GetEntriesByCategory(ItemCount,categoryName,ActiveOnly);
			try
			{
				EntryCollection ec = DataHelper.LoadEntryCollection(reader);
				return ec;
			}
			finally
			{
				reader.Close();
			}
		}

		public EntryCollection GetEntriesByCategory(int ItemCount, string categoryName, DateTime DateUpdated, bool ActiveOnly)
		{
			IDataReader reader = DbProvider.Instance().GetEntriesByCategory(ItemCount,categoryName,DateUpdated,ActiveOnly);
			try
			{
				EntryCollection ec = DataHelper.LoadEntryCollection(reader);
				return ec;
			}
			finally
			{
				reader.Close();
			}
		}

		public EntryCollection GetEntriesByCategory(int ItemCount, int catID, bool ActiveOnly)
		{
			IDataReader reader = DbProvider.Instance().GetEntriesByCategory(ItemCount,catID,ActiveOnly);
			try
			{
				EntryCollection ec = DataHelper.LoadEntryCollection(reader);
				return ec;
			}
			finally
			{
				reader.Close();
			}
		}

		public EntryCollection GetEntriesByCategory(int ItemCount, int catID, DateTime DateUpdated, bool ActiveOnly)
		{
			IDataReader reader = DbProvider.Instance().GetEntriesByCategory(ItemCount,catID,DateUpdated,ActiveOnly);
			try
			{
				EntryCollection ec = DataHelper.LoadEntryCollection(reader);
				return ec;
			}
			finally
			{
				reader.Close();
			}
		}

		#endregion

		#region Single Entry

		public Entry GetEntry(int postID, bool ActiveOnly)
		{
			IDataReader reader = DbProvider.Instance().GetEntry(postID, ActiveOnly);
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


		public Entry GetEntry(string EntryName, bool ActiveOnly)
		{
			IDataReader reader = DbProvider.Instance().GetEntry(EntryName,ActiveOnly);
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

		public CategoryEntry GetCategoryEntry(int postid, bool ActiveOnly)
		{
			IDataReader reader = DbProvider.Instance().GetCategoryEntry(postid,ActiveOnly);
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

		public CategoryEntry GetCategoryEntry(string EntryName, bool ActiveOnly)
		{
			IDataReader reader = DbProvider.Instance().GetCategoryEntry(EntryName,ActiveOnly);
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

		public bool Delete(int PostID)
		{
			return DbProvider.Instance().DeleteEntry(PostID);
		}

		#endregion

		#region Create Entry

		public int Create(Entry entry)
		{
			return Create(entry,null);
		}

		public int Create(Entry entry, int[] CategoryIDs)
		{
			if(entry.PostType == PostType.PingTrack)
			{
				return DbProvider.Instance().InsertPingTrackEntry(entry);
			}

			if(!FormatEntry(ref entry,true))
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
				entry.Link = Subtext.Framework.Configuration.Config.CurrentBlog.UrlFormats.EntryUrl(entry);
				NotificationServices.Run(entry);
			}

			if(entry.EntryID > -1)
			{
				Config.CurrentBlog.LastUpdated = entry.DateCreated;
			}

			return entry.EntryID;
		}

		#endregion

		#region Update

		public bool Update(Entry entry)
		{
			return Update(entry,null);
		}

		public bool Update(Entry entry, int[] CategoryIDs)
		{
			if(!FormatEntry(ref entry,false))
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
					entry.Link = Config.CurrentBlog.UrlFormats.EntryUrl(entry);
				}
				else
				{
					entry.Link = Config.CurrentBlog.UrlFormats.ArticleUrl(entry);
				}

				if(entry.EntryID > -1)
				{
					Config.CurrentBlog.LastUpdated = entry.DateUpdated;
				}

				NotificationServices.Run(entry);
			}
			return true;
		}

		#endregion

		#region SetCategoriesList

		public bool SetEntryCategoryList(int EntryID, int[] Categories)
		{
			return DbProvider.Instance().SetEntryCategoryList(EntryID,Categories);
		}

		#endregion
        
		#region Format Helper
		
		private bool FormatEntry(ref Entry e, bool UseKeyWords)
		{
			//Do this before we validate the text
			if(UseKeyWords)
			{
				KeyWords.Format(ref e);
			}

			//e.Body = Transform.EmoticonTransforms(e.Body);

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
				if(!HtmlHelper.ConvertHtmlToXHtml(ref e))
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

		public PagedLinkCollection GetPagedLinks(int categoryTypeID, int pageIndex, int pageSize, bool sortDescending)
		{
			IDataReader reader = DbProvider.Instance().GetPagedLinks(categoryTypeID, pageIndex, pageSize, sortDescending);
			try
			{
				PagedLinkCollection plc = new PagedLinkCollection();
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

		public LinkCollection GetLinkCollectionByPostID(int PostID)
		{
			IDataReader reader = DbProvider.Instance().GetLinkCollectionByPostID(PostID);
			try
			{
				LinkCollection lc = new LinkCollection();
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

		public LinkCollection GetLinksByCategoryID(int catID, bool ActiveOnly)
		{
			IDataReader reader = DbProvider.Instance().GetLinksByCategoryID(catID,ActiveOnly);
			LinkCollection lc = new LinkCollection();
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

		public Link GetSingleLink(int linkID)
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

		#region LinkCategoryCollection

		public LinkCategoryCollection GetCategories(CategoryType catType, bool ActiveOnly)
		{
			IDataReader reader = DbProvider.Instance().GetCategories(catType,ActiveOnly);
			LinkCategoryCollection lcc = new LinkCategoryCollection();
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

		public LinkCategoryCollection GetActiveCategories()
		{
			DataSet ds = DbProvider.Instance().GetActiveCategories();
			LinkCategoryCollection lcc = new LinkCategoryCollection();
			foreach(DataRow dr in ds.Tables[0].Rows)
			{
				LinkCategory lc = DataHelper.LoadSingleLinkCategory(dr);
				lc.Links = new LinkCollection();
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

		public LinkCategory GetLinkCategory(int CategoryID, bool IsActive)
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

		public LinkCategory GetLinkCategory(string categoryName,bool IsActive)
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

		public bool UpdateLink(Link link)
		{
			return DbProvider.Instance().UpdateLink(link);
		}

		public int CreateLink(Link link)
		{
			return DbProvider.Instance().InsertLink(link);
		}

		public bool UpdateLinkCategory(LinkCategory lc)
		{
			return DbProvider.Instance().UpdateCategory(lc);
		}
		
		public int CreateLinkCategory(LinkCategory lc)
		{
			return DbProvider.Instance().InsertCategory(lc);
		}

		public bool DeleteLinkCategory(int CategoryID)
		{
			return DbProvider.Instance().DeleteCategory(CategoryID);
		}

		public bool DeleteLink(int LinkID)
		{
			return DbProvider.Instance().DeleteLink(LinkID);
		}

		#endregion

		#endregion

		#region Stats

		public PagedViewStatCollection GetPagedViewStats(int pageIndex, int pageSize, DateTime beginDate, DateTime endDate)
		{

			IDataReader reader = DbProvider.Instance().GetPagedViewStats(pageIndex,pageSize,beginDate,endDate);
			try
			{
				PagedViewStatCollection vs = new PagedViewStatCollection();
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

		public PagedReferrerCollection GetPagedReferrers(int pageIndex, int pageSize)
		{
			IDataReader reader = DbProvider.Instance().GetPagedReferrers(pageIndex,pageSize);
			try
			{
				PagedReferrerCollection prc = new PagedReferrerCollection();
				while(reader.Read())
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

		public PagedReferrerCollection GetPagedReferrers(int pageIndex, int pageSize, int EntryID)
		{
			IDataReader reader = DbProvider.Instance().GetPagedReferrers(pageIndex,pageSize,EntryID);
			try
			{
				PagedReferrerCollection prc = new PagedReferrerCollection();
				while(reader.Read())
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

		public bool TrackEntry(EntryView ev)
		{
			return DbProvider.Instance().TrackEntry(ev);
		}

		public bool TrackEntry(EntryViewCollection evc)
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
		/// <param name="application"></param>
		/// <param name="userName">Name of the user.</param>
		/// <param name="password">Password.</param>
		/// <returns></returns>
		public bool CreateBlog(string title, string userName, string password, string host, string application)
		{
			return DbProvider.Instance().AddBlogConfiguration(title, userName, password, host, application);
		}
		
		public bool UpdateBlog(BlogInfo info)
		{
			return DbProvider.Instance().UpdateBlog(info);
		}

		/// <summary>
		/// Returns a <see cref="BlogInfo"/> instance containing 
		/// the configuration settings for the blog specified by the 
		/// Hostname and Application.
		/// </summary>
		/// <param name="hostname">Hostname.</param>
		/// <param name="application">Application.</param>
		/// <returns></returns>
		public BlogInfo GetBlogInfo(string hostname, string application)
		{
			return GetBlogInfo(hostname, application, true);
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
		/// <param name="application">Application.</param>
		/// <param name="strict">If false, then this will return a blog record if 
		/// there is only one blog record, regardless if the application and hostname match.</param>
		/// <returns></returns>
		public BlogInfo GetBlogInfo(string hostname, string application, bool strict)
		{
			IDataReader reader = DbProvider.Instance().GetBlogInfo(hostname, application, strict);
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
		
		public BlogInfo GetBlogInfo(int BlogID)
		{
			return null;
		}


		#endregion

		#region KeyWords

		public KeyWord GetKeyWord(int KeyWordID)
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
		
		public KeyWordCollection GetKeyWords()
		{
			IDataReader reader = DbProvider.Instance().GetKeyWords();
			try
			{
				KeyWordCollection kwc = new KeyWordCollection();
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

		public PagedKeyWordCollection GetPagedKeyWords(int pageIndex, int pageSize,bool sortDescending)
		{
			IDataReader reader = DbProvider.Instance().GetPagedKeyWords(pageIndex,pageSize,sortDescending);
			try
			{
				PagedKeyWordCollection pkwc = new PagedKeyWordCollection();
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
		
		public bool UpdateKeyWord(KeyWord kw)
		{
			return DbProvider.Instance().UpdateKeyWord(kw);
		}

		public int InsertKeyWord(KeyWord kw)
		{
			return DbProvider.Instance().InsertKeyWord(kw);
		}

		public bool DeleteKeyWord(int KeyWordID)
		{
			return DbProvider.Instance().DeleteKeyWord(KeyWordID);
		}

		#endregion

		#region Images

		public ImageCollection GetImagesByCategoryID(int catID, bool ActiveOnly)
		{
			IDataReader reader = DbProvider.Instance().GetImagesByCategoryID(catID,ActiveOnly);
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

		public Image GetSingleImage(int imageID, bool ActiveOnly)
		{
			IDataReader reader = DbProvider.Instance().GetSingleImage(imageID,ActiveOnly);
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

		public int InsertImage(Subtext.Framework.Components.Image _image)
		{
			return DbProvider.Instance().InsertImage(_image);
		}

		public bool UpdateImage(Subtext.Framework.Components.Image _image)
		{
			return DbProvider.Instance().UpdateImage(_image);
		}

		public bool DeleteImage(int ImageID)
		{
			return DbProvider.Instance().DeleteImage(ImageID);
		}

		#endregion

		#region Archives

		public ArchiveCountCollection GetPostsByMonthArchive()
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

		public ArchiveCountCollection GetPostsByYearArchive()
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
		
	}
}
