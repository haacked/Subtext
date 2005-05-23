using System;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;
using Subtext.Framework.Data;
using Subtext.Framework.Text;

namespace UnitTests.Subtext
{
	/// <summary>
	/// Implements the <see cref="IDTOProvider" /> interface specifically for the purpose 
	/// of unit testing.
	/// </summary>
	public class UnitTestDTOProvider : IDTOProvider
	{
		static int _nextBlogId = 1;
		BlogConfig _blogById = null;
		BlogConfigCollection _pagedBlogs = new BlogConfigCollection();

		/// <summary>
		/// Sets the blog to be returned by a call to GetBlogById().
		/// </summary>
		/// <param name="blog">Blog.</param>
		public void SetBlogById(BlogConfig blog)
		{
			_blogById = blog;
		}

		/// <summary>
		/// Sets the <see cref="BlogConfigCollection"/> instance that will 
		/// be returned by a call to GetPagedBlogs().
		/// </summary>
		/// <param name="blogs">Blogs.</param>
		public void SetPagedBlogs(BlogConfigCollection blogs)
		{
			_pagedBlogs = blogs;
		}

		/// <summary>
		/// Clears the blogs.
		/// </summary>
		public void ClearBlogs()
		{
			_pagedBlogs.Clear();	
		}

		#region IDTOProvider Implementation
		public BlogConfigCollection GetPagedBlogs(int pageIndex, int pageSize, bool sortDescending)
		{
			return _pagedBlogs;
		}

		public BlogConfig GetBlogById(int blogId)
		{
			return _blogById;
		}

		public BlogConfigCollection GetBlogsByHost(string host)
		{
			BlogConfigCollection blogsWithHost = new BlogConfigCollection();
			foreach(BlogConfig config in _pagedBlogs)
			{
				if(StringHelper.AreEqualIgnoringCase(config.Host, host))
				{
					blogsWithHost.Add(config);
				}
			}
			return blogsWithHost;
		}

		public PagedEntryCollection GetPagedEntries(PostType postType, int categoryID, int pageIndex, int pageSize, bool sortDescending)
		{
			throw new NotImplementedException();
		}

		public PagedEntryCollection GetPagedFeedback(int pageIndex, int pageSize, bool sortDescending)
		{
			throw new NotImplementedException();
		}

		public EntryDay GetSingleDay(DateTime dt)
		{
			throw new NotImplementedException();
		}

		public EntryDayCollection GetRecentDayPosts(int ItemCount, bool ActiveOnly)
		{
			throw new NotImplementedException();
		}

		public EntryDayCollection GetPostsByMonth(int month, int year)
		{
			throw new NotImplementedException();
		}

		public EntryDayCollection GetPostsByCategoryID(int ItemCount, int catID)
		{
			throw new NotImplementedException();
		}

		public EntryDayCollection GetConditionalEntries(int ItemCount, PostConfig pc)
		{
			throw new NotImplementedException();
		}

		public EntryCollection GetConditionalEntries(int ItemCount, PostType pt, PostConfig pc)
		{
			throw new NotImplementedException();
		}

		public EntryCollection GetConditionalEntries(int ItemCount, PostType pt, PostConfig pc, DateTime DateUpdated)
		{
			throw new NotImplementedException();
		}

		public EntryCollection GetFeedBack(int ParrentID)
		{
			throw new NotImplementedException();
		}

		public EntryCollection GetFeedBack(Entry ParentEntry)
		{
			throw new NotImplementedException();
		}

		public EntryCollection GetRecentPostsWithCategories(int ItemCount, bool ActiveOnly)
		{
			throw new NotImplementedException();
		}

		public EntryCollection GetRecentPosts(int ItemCount, PostType postType, bool ActiveOnly)
		{
			throw new NotImplementedException();
		}

		public EntryCollection GetRecentPosts(int ItemCount, PostType postType, bool ActiveOnly, DateTime DateUpdated)
		{
			throw new NotImplementedException();
		}

		public EntryCollection GetPostCollectionByMonth(int month, int year)
		{
			throw new NotImplementedException();
		}

		public EntryCollection GetPostsByDayRange(DateTime start, DateTime stop, PostType postType, bool ActiveOnly)
		{
			throw new NotImplementedException();
		}

		public EntryCollection GetEntriesByCategory(int ItemCount, int catID, bool ActiveOnly)
		{
			throw new NotImplementedException();
		}

		public EntryCollection GetEntriesByCategory(int ItemCount, int catID, DateTime DateUpdated, bool ActiveOnly)
		{
			throw new NotImplementedException();
		}

		public EntryCollection GetEntriesByCategory(int ItemCount, string categoryName, bool ActiveOnly)
		{
			throw new NotImplementedException();
		}

		public EntryCollection GetEntriesByCategory(int ItemCount, string categoryName, DateTime DateUpdated, bool ActiveOnly)
		{
			throw new NotImplementedException();
		}

		public Entry GetEntry(int postID, bool ActiveOnly)
		{
			throw new NotImplementedException();
		}

		public Entry GetEntry(string EntryName, bool ActiveOnly)
		{
			throw new NotImplementedException();
		}

		public CategoryEntry GetCategoryEntry(int postid, bool ActiveOnly)
		{
			throw new NotImplementedException();
		}

		public CategoryEntry GetCategoryEntry(string EntryName, bool ActiveOnly)
		{
			throw new NotImplementedException();
		}

		public bool Delete(int PostID)
		{
			throw new NotImplementedException();
		}

		public int Create(Entry entry)
		{
			throw new NotImplementedException();
		}

		public int Create(Entry entry, int[] CategoryIDs)
		{
			throw new NotImplementedException();
		}

		public bool Update(Entry entry)
		{
			throw new NotImplementedException();
		}

		public bool Update(Entry entry, int[] CategoryIDs)
		{
			throw new NotImplementedException();
		}

		public bool SetEntryCategoryList(int EntryID, int[] Categories)
		{
			throw new NotImplementedException();
		}

		public PagedLinkCollection GetPagedLinks(int categoryTypeID, int pageIndex, int pageSize, bool sortDescending)
		{
			throw new NotImplementedException();
		}

		public LinkCollection GetLinkCollectionByPostID(int PostID)
		{
			throw new NotImplementedException();
		}

		public LinkCollection GetLinksByCategoryID(int catID, bool ActiveOnly)
		{
			throw new NotImplementedException();
		}

		public Link GetSingleLink(int linkID)
		{
			throw new NotImplementedException();
		}

		public LinkCategoryCollection GetCategories(CategoryType catType, bool ActiveOnly)
		{
			throw new NotImplementedException();
		}

		public LinkCategoryCollection GetActiveCategories()
		{
			throw new NotImplementedException();
		}

		public LinkCategory GetLinkCategory(int CategoryID, bool IsActive)
		{
			throw new NotImplementedException();
		}

		public LinkCategory GetLinkCategory(string categoryName, bool IsActive)
		{
			throw new NotImplementedException();
		}

		public bool UpdateLink(Link link)
		{
			throw new NotImplementedException();
		}

		public int CreateLink(Link link)
		{
			throw new NotImplementedException();
		}

		public bool UpdateLinkCategory(LinkCategory lc)
		{
			throw new NotImplementedException();
		}

		public int CreateLinkCategory(LinkCategory lc)
		{
			throw new NotImplementedException();
		}

		public bool DeleteLinkCategory(int CategoryID)
		{
			throw new NotImplementedException();
		}

		public bool DeleteLink(int LinkID)
		{
			throw new NotImplementedException();
		}

		public PagedViewStatCollection GetPagedViewStats(int pageIndex, int pageSize, DateTime beginDate, DateTime endDate)
		{
			throw new NotImplementedException();
		}

		public PagedReferrerCollection GetPagedReferrers(int pageIndex, int pageSize)
		{
			throw new NotImplementedException();
		}

		public PagedReferrerCollection GetPagedReferrers(int pageIndex, int pageSize, int EntryID)
		{
			throw new NotImplementedException();
		}

		public bool TrackEntry(EntryView ev)
		{
			throw new NotImplementedException();
		}

		public bool TrackEntry(EntryViewCollection evc)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Adds the initial blog configuration.  This is a convenience method for 
		/// allowing a user with a freshly installed blog to immediately gain access 
		/// to the admin section to edit the blog.
		/// </summary>
		/// <param name="userName">Name of the user.</param>
		/// <param name="password">Password.</param>
		/// <returns></returns>
		public bool AddBlogConfiguration(string userName, string password, string host, string application)
		{
			BlogConfig blogConfig = new BlogConfig();
			blogConfig.BlogID = _nextBlogId++;
			blogConfig.UserName = userName;
			blogConfig.Password = password;
			blogConfig.Host = host;
			blogConfig.Application = application;
			_pagedBlogs.Add(blogConfig);
			return true;
		}

		/// <summary>
		/// Updates the specified blog configuration.
		/// </summary>
		/// <param name="config">Config.</param>
		/// <returns></returns>
		public bool UpdateConfigData(BlogConfig config)
		{
			return true;
		}

		/// <summary>
		/// Gets the config.
		/// </summary>
		/// <param name="hostname">Hostname.</param>
		/// <param name="application">Application.</param>
		/// <returns></returns>
		public BlogConfig GetConfig(string hostname, string application)
		{
			hostname = hostname.Replace("www.", string.Empty);

			foreach(BlogConfig config in this._pagedBlogs)
			{
				if(StringHelper.AreEqualIgnoringCase(config.Host, hostname) 
					&& StringHelper.AreEqualIgnoringCase(config.Application, application))
					return config;
			}
			return null;
		}

		public BlogConfig GetConfig(string hostname, string application, bool strict)
		{
			return GetConfig(hostname, application);
		}

		/// <summary>
		/// Gets the config. This has been depracated
		/// </summary>
		/// <param name="BlogID">Blog ID.</param>
		/// <returns></returns>
		public BlogConfig GetConfig(int BlogID)
		{
			throw new NotImplementedException();
		}

		public KeyWord GetKeyWord(int KeyWordID)
		{
			throw new NotImplementedException();
		}

		public KeyWordCollection GetKeyWords()
		{
			throw new NotImplementedException();
		}

		public PagedKeyWordCollection GetPagedKeyWords(int pageIndex, int pageSize, bool sortDescending)
		{
			throw new NotImplementedException();
		}

		public bool UpdateKeyWord(KeyWord kw)
		{
			throw new NotImplementedException();
		}

		public int InsertKeyWord(KeyWord kw)
		{
			throw new NotImplementedException();
		}

		public bool DeleteKeyWord(int KeyWordID)
		{
			throw new NotImplementedException();
		}

		public ImageCollection GetImagesByCategoryID(int catID, bool ActiveOnly)
		{
			throw new NotImplementedException();
		}

		public Image GetSingleImage(int imageID, bool ActiveOnly)
		{
			throw new NotImplementedException();
		}

		public int InsertImage(Image _image)
		{
			throw new NotImplementedException();
		}

		public bool UpdateImage(Image _image)
		{
			throw new NotImplementedException();
		}

		public bool DeleteImage(int ImageID)
		{
			throw new NotImplementedException();
		}

		public ArchiveCountCollection GetPostsByYearArchive()
		{
			throw new NotImplementedException();
		}

		public ArchiveCountCollection GetPostsByMonthArchive()
		{
			throw new NotImplementedException();
		}
		#endregion
	}
}
