using System;
using System.Collections.Specialized;
using Subtext.Extensibility;
using Subtext.Framework;
using Subtext.Framework.Components;
using Subtext.Framework.Providers;
using Subtext.Framework.Text;

namespace UnitTests.Subtext
{
	/// <summary>
	/// Implements the <see cref="ObjectProvider" /> interface specifically for the purpose 
	/// of unit testing.
	/// </summary>
	public class UnitTestObjectProvider : ObjectProvider
	{
		string _name;
		static int _nextBlogId = 1;
		BlogInfo _blogById = null;
		BlogInfoCollection _pagedBlogs = new BlogInfoCollection();

		static int _nextLinkCategoryId = 1;
		LinkCategoryCollection _linkCategories = new LinkCategoryCollection();

		/// <summary>
		/// Sets the blog to be returned by a call to GetBlogById().
		/// </summary>
		/// <param name="blog">Blog.</param>
		public void SetBlogById(BlogInfo blog)
		{
			_blogById = blog;
		}

		/// <summary>
		/// Sets the <see cref="BlogInfoCollection"/> instance that will 
		/// be returned by a call to GetPagedBlogs().
		/// </summary>
		/// <param name="blogs">Blogs.</param>
		public void SetPagedBlogs(BlogInfoCollection blogs)
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

		#region IObjectProvider Implementation
		public override BlogInfoCollection GetPagedBlogs(int pageIndex, int pageSize, bool sortDescending)
		{
			return _pagedBlogs;
		}

		public override BlogInfo GetBlogById(int blogId)
		{
			return _blogById;
		}

		public override BlogInfoCollection GetBlogsByHost(string host)
		{
			BlogInfoCollection blogsWithHost = new BlogInfoCollection();
			foreach(BlogInfo config in _pagedBlogs)
			{
				if(StringHelper.AreEqualIgnoringCase(config.Host, host))
				{
					blogsWithHost.Add(config);
				}
			}
			return blogsWithHost;
		}

		public override PagedEntryCollection GetPagedEntries(PostType postType, int categoryID, int pageIndex, int pageSize, bool sortDescending)
		{
			throw new NotImplementedException();
		}

		public override PagedEntryCollection GetPagedFeedback(int pageIndex, int pageSize, bool sortDescending)
		{
			throw new NotImplementedException();
		}

		public override EntryDay GetSingleDay(DateTime dt)
		{
			throw new NotImplementedException();
		}

		public override EntryDayCollection GetRecentDayPosts(int ItemCount, bool ActiveOnly)
		{
			throw new NotImplementedException();
		}

		public override EntryDayCollection GetPostsByMonth(int month, int year)
		{
			throw new NotImplementedException();
		}

		public override EntryDayCollection GetPostsByCategoryID(int ItemCount, int catID)
		{
			throw new NotImplementedException();
		}

		public override EntryDayCollection GetConditionalEntries(int ItemCount, PostConfig pc)
		{
			throw new NotImplementedException();
		}

		public override EntryCollection GetConditionalEntries(int ItemCount, PostType pt, PostConfig pc)
		{
			throw new NotImplementedException();
		}

		public override EntryCollection GetConditionalEntries(int ItemCount, PostType pt, PostConfig pc, DateTime DateUpdated)
		{
			throw new NotImplementedException();
		}

		public override EntryCollection GetFeedBack(int ParrentID)
		{
			throw new NotImplementedException();
		}

		public override EntryCollection GetFeedBack(Entry ParentEntry)
		{
			throw new NotImplementedException();
		}

		public override EntryCollection GetRecentPostsWithCategories(int ItemCount, bool ActiveOnly)
		{
			throw new NotImplementedException();
		}

		public override EntryCollection GetRecentPosts(int ItemCount, PostType postType, bool ActiveOnly)
		{
			throw new NotImplementedException();
		}

		public override EntryCollection GetRecentPosts(int ItemCount, PostType postType, bool ActiveOnly, DateTime DateUpdated)
		{
			throw new NotImplementedException();
		}

		public override EntryCollection GetPostCollectionByMonth(int month, int year)
		{
			throw new NotImplementedException();
		}

		public override EntryCollection GetPostsByDayRange(DateTime start, DateTime stop, PostType postType, bool ActiveOnly)
		{
			throw new NotImplementedException();
		}

		public override EntryCollection GetEntriesByCategory(int ItemCount, int catID, bool ActiveOnly)
		{
			throw new NotImplementedException();
		}

		public override EntryCollection GetEntriesByCategory(int ItemCount, int catID, DateTime DateUpdated, bool ActiveOnly)
		{
			throw new NotImplementedException();
		}

		public override EntryCollection GetEntriesByCategory(int ItemCount, string categoryName, bool ActiveOnly)
		{
			throw new NotImplementedException();
		}

		public override EntryCollection GetEntriesByCategory(int ItemCount, string categoryName, DateTime DateUpdated, bool ActiveOnly)
		{
			throw new NotImplementedException();
		}

		public override Entry GetCommentByChecksumHash(string checksumHash)
		{
			throw new NotImplementedException();
		}

		public override Entry GetEntry(int postID, bool ActiveOnly)
		{
			throw new NotImplementedException();
		}

		public override Entry GetEntry(string EntryName, bool ActiveOnly)
		{
			throw new NotImplementedException();
		}

		public override CategoryEntry GetCategoryEntry(int postid, bool ActiveOnly)
		{
			throw new NotImplementedException();
		}

		public override CategoryEntry GetCategoryEntry(string EntryName, bool ActiveOnly)
		{
			throw new NotImplementedException();
		}

		public override bool Delete(int PostID)
		{
			throw new NotImplementedException();
		}

		public override int Create(Entry entry)
		{
			throw new NotImplementedException();
		}

		public override int Create(Entry entry, int[] CategoryIDs)
		{
			throw new NotImplementedException();
		}

		public override bool Update(Entry entry)
		{
			throw new NotImplementedException();
		}

		public override bool Update(Entry entry, int[] CategoryIDs)
		{
			throw new NotImplementedException();
		}

		public override bool SetEntryCategoryList(int EntryID, int[] Categories)
		{
			throw new NotImplementedException();
		}

		public override PagedLinkCollection GetPagedLinks(int categoryTypeID, int pageIndex, int pageSize, bool sortDescending)
		{
			throw new NotImplementedException();
		}

		public override LinkCollection GetLinkCollectionByPostID(int PostID)
		{
			throw new NotImplementedException();
		}

		public override LinkCollection GetLinksByCategoryID(int catID, bool ActiveOnly)
		{
			throw new NotImplementedException();
		}

		public override Link GetSingleLink(int linkID)
		{
			throw new NotImplementedException();
		}

		public override LinkCategoryCollection GetCategories(CategoryType catType, bool ActiveOnly)
		{
			LinkCategoryCollection linkCategoryCollection = new LinkCategoryCollection();

			foreach(LinkCategory lc in _linkCategories)
			{
				if (lc.CategoryType == catType & (lc.IsActive || !ActiveOnly))
					linkCategoryCollection.Add(lc);
			}

			return linkCategoryCollection;		
		}

		public override LinkCategoryCollection GetActiveCategories()
		{
			throw new NotImplementedException();
		}

		public override LinkCategory GetLinkCategory(int CategoryID, bool IsActive)
		{
			throw new NotImplementedException();
		}

		public override LinkCategory GetLinkCategory(string categoryName, bool IsActive)
		{
			throw new NotImplementedException();
		}

		public override bool UpdateLink(Link link)
		{
			throw new NotImplementedException();
		}

		public override int CreateLink(Link link)
		{
			throw new NotImplementedException();
		}

		public override bool UpdateLinkCategory(LinkCategory lc)
		{
			foreach(LinkCategory category in _linkCategories)
				if (category.CategoryID == lc.CategoryID & 
					category.BlogID == lc.BlogID)
				{
					category.Title = lc.Title;
					category.Description = lc.Description;
					category.CategoryType = lc.CategoryType;
					category.IsActive = lc.IsActive;
					return true;
				}
			return false;
		}

		public override int CreateLinkCategory(LinkCategory lc)
		{
			LinkCategory linkCategory = new LinkCategory();
			linkCategory.BlogID = lc.BlogID;
			linkCategory.Title = lc.Title;
			linkCategory.Description = lc.Description;
			linkCategory.CategoryType = lc.CategoryType;
			linkCategory.CategoryID = _nextLinkCategoryId++;
			_linkCategories.Add(linkCategory);
			return linkCategory.CategoryID;
		}

		public override bool DeleteLinkCategory(int CategoryID)
		{
			throw new NotImplementedException();
		}

		public override bool DeleteLink(int LinkID)
		{
			throw new NotImplementedException();
		}

		public override PagedViewStatCollection GetPagedViewStats(int pageIndex, int pageSize, DateTime beginDate, DateTime endDate)
		{
			throw new NotImplementedException();
		}

		public override PagedReferrerCollection GetPagedReferrers(int pageIndex, int pageSize)
		{
			throw new NotImplementedException();
		}

		public override PagedReferrerCollection GetPagedReferrers(int pageIndex, int pageSize, int EntryID)
		{
			throw new NotImplementedException();
		}

		public override bool TrackEntry(EntryView ev)
		{
			throw new NotImplementedException();
		}

		public override bool TrackEntry(EntryViewCollection evc)
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
		public override bool CreateBlog(string title, string userName, string password, string host, string application)
		{
			BlogInfo blogInfo = new BlogInfo();
			blogInfo.Title = title;
			blogInfo.BlogID = _nextBlogId++;
			blogInfo.UserName = userName;
			blogInfo.Password = Security.HashPassword(password);
			blogInfo.Host = host;
			blogInfo.Application = application;
			_pagedBlogs.Add(blogInfo);
			return true;
		}

		/// <summary>
		/// Updates the specified blog configuration.
		/// </summary>
		/// <param name="info">Config.</param>
		/// <returns></returns>
		public override bool UpdateBlog(BlogInfo info)
		{
			return true;
		}

		/// <summary>
		/// Gets the config.
		/// </summary>
		/// <param name="hostname">Hostname.</param>
		/// <param name="application">Application.</param>
		/// <returns></returns>
		public override BlogInfo GetBlogInfo(string hostname, string application)
		{
			hostname = hostname.Replace("www.", string.Empty);

			foreach(BlogInfo config in this._pagedBlogs)
			{
				if(StringHelper.AreEqualIgnoringCase(config.Host, hostname) 
					&& StringHelper.AreEqualIgnoringCase(config.Application, application))
					return config;
			}
			return null;
		}

		public override BlogInfo GetBlogInfo(string hostname, string application, bool strict)
		{
			return GetBlogInfo(hostname, application);
		}

		/// <summary>
		/// Gets the config. This has been depracated
		/// </summary>
		/// <param name="BlogID">Blog ID.</param>
		/// <returns></returns>
		public override BlogInfo GetBlogInfo(int BlogID)
		{
			throw new NotImplementedException();
		}

		public override KeyWord GetKeyWord(int KeyWordID)
		{
			throw new NotImplementedException();
		}

		public override KeyWordCollection GetKeyWords()
		{
			throw new NotImplementedException();
		}

		public override PagedKeyWordCollection GetPagedKeyWords(int pageIndex, int pageSize, bool sortDescending)
		{
			throw new NotImplementedException();
		}

		public override bool UpdateKeyWord(KeyWord kw)
		{
			throw new NotImplementedException();
		}

		public override int InsertKeyWord(KeyWord kw)
		{
			throw new NotImplementedException();
		}

		public override bool DeleteKeyWord(int KeyWordID)
		{
			throw new NotImplementedException();
		}

		public override ImageCollection GetImagesByCategoryID(int catID, bool ActiveOnly)
		{
			throw new NotImplementedException();
		}

		public override Image GetSingleImage(int imageID, bool ActiveOnly)
		{
			throw new NotImplementedException();
		}

		public override int InsertImage(Image _image)
		{
			throw new NotImplementedException();
		}

		public override bool UpdateImage(Image _image)
		{
			throw new NotImplementedException();
		}

		public override bool DeleteImage(int ImageID)
		{
			throw new NotImplementedException();
		}

		public override ArchiveCountCollection GetPostsByYearArchive()
		{
			throw new NotImplementedException();
		}

		public override ArchiveCountCollection GetPostsByMonthArchive()
		{
			throw new NotImplementedException();
		}
		#endregion

		/// <summary>
		/// Initializes the specified provider.
		/// </summary>
		/// <param name="name">Firendly Name of the provider.</param>
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
			get { return _name ; }
		}
	}
}
