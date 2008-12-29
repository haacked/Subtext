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
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using CookComputing.XmlRpc;
using Subtext.Extensibility;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;
using Subtext.Framework.Logging;
using Subtext.Framework.Security;
using Subtext.Framework.Tracking;

//Need to find a method that has access to context, so we can terminate the request if AllowServiceAccess == false.
//Users will be able to access the metablogapi page, but will not be able to make a request, but the page should not be visible

namespace Subtext.Framework.XmlRpc
{
	/// <summary>
	/// Implements the MetaBlog API.
	/// </summary>
	public class MetaWeblog : SubtextXmlRpcService, IMetaWeblog, IWordPressApi
	{
        static Log Log = new Log();

        public MetaWeblog(ISubtextContext context) : base(context) {
        }

        #region Helper Methods
        private static void ValidateUser(string username, string password, bool allowServiceAccess)
        {
            if (!Config.Settings.AllowServiceAccess || !allowServiceAccess) {
                throw new XmlRpcFaultException(0, "Web Service Access is not enabled.");
            }

            bool isValid = SecurityHelper.IsValidUser(username, password);
            if (!isValid) {
                throw new XmlRpcFaultException(0, "Username and password denied.");
            }
        }

        private string PostContent(string username, string password, ref Post post, bool publish, PostType postType)
        {
            ValidateUser(username, password, Blog.AllowServiceAccess);

            Entry entry = new Entry(postType);
            entry.Author = Blog.Author;
            entry.Email = Blog.Email;
            entry.Body = post.description;
            entry.Title = post.title;
            entry.Description = string.Empty;

            //TODO: Figure out why this is here.
            //		Probably means the poster forgot to set the date.

            DateTime dateTimeInPost = Blog.TimeZone.ToLocalTime(post.dateCreated);

            if (dateTimeInPost.Year >= 2003) {
                entry.DateCreated = dateTimeInPost;
                entry.DateModified = dateTimeInPost;
            }
            else {
                entry.DateCreated = Blog.TimeZone.Now;
                entry.DateModified = entry.DateCreated;
            }

            if (post.categories != null) {
                entry.Categories.AddRange(post.categories);
            }

            if (!string.IsNullOrEmpty(post.wp_slug))
            {
                entry.EntryName = post.wp_slug;
            }

            entry.PostType = postType;

            entry.IsActive = publish;
            if (publish) 
            { 
                entry.DateSyndicated = entry.DateCreated;
            }
            entry.AllowComments = true;
            entry.DisplayOnHomePage = true;
            entry.IncludeInMainSyndication = true;
            entry.IsAggregated = true;
            entry.SyndicateDescriptionOnly = false;

            int postID;
            try
            {
                postID = Entries.Create(entry);

                if (!string.IsNullOrEmpty(post.enclosure.url))
                {
                    Components.Enclosure enc = new Components.Enclosure();
                    enc.Url = post.enclosure.url;
                    enc.MimeType = post.enclosure.type;
                    enc.Size = post.enclosure.length;
                    enc.EntryId = postID;
                    Enclosures.Create(enc);
                }

                AddCommunityCredits(entry);
            }
            catch (Exception e) {
                throw new XmlRpcFaultException(0, e.Message + " " + e.StackTrace);
            }
            if (postID < 0) {
                throw new XmlRpcFaultException(0, "The post could not be added");
            }
            return postID.ToString(CultureInfo.InvariantCulture);
        }

        private void AddCommunityCredits(Entry entry)
        {
            string result = string.Empty;

            try
            {
                CommunityCreditNotification.AddCommunityCredits(entry, Url, Blog);
            }
            catch (CommunityCreditNotificationException ex)
            {
                Log.WarnFormat("Community Credit ws returned the following response while notifying for the url {0}: {1}", Url.EntryUrl(entry).ToFullyQualifiedUrl(Blog).ToString(), ex.Message);
            }
            catch (Exception ex)
            {
                Log.Error("Error while connecting to the Community Credit webservice", ex);
            }
        } 
        #endregion

		#region BlogApi Members
		public BlogInfo[] getUsersBlogs(string appKey, string username, string password)
		{
			Framework.Blog info = Blog;
			ValidateUser(username, password, info.AllowServiceAccess);
			
			BlogInfo[] bi = new BlogInfo[1];
			BlogInfo b = new BlogInfo();
			b.blogid = info.Id.ToString(CultureInfo.InvariantCulture);
			b.blogName = info.Title;
			b.url = info.HomeFullyQualifiedUrl.ToString();
			bi[0] = b;
			return bi;

		}

		public bool deletePost(string appKey,string postid,string username,string password,[XmlRpcParameter(Description="Where applicable, this specifies whether the blog should be republished after the post has been deleted.")] bool publish)
		{
			ValidateUser(username,password,Blog.AllowServiceAccess);
			
			try
			{
				Entries.Delete(Int32.Parse(postid));
				return true;
			}
			catch
			{
				throw new XmlRpcFaultException(1, "Could not delete post: " + postid);
			}			
		}

		#endregion

        #region MetaWeblog Members
        public bool editPost(string postid, string username, string password, Post post, bool publish)
        {
            ValidateUser(username, password, Blog.AllowServiceAccess);

            Entry entry = Entries.GetEntry(Int32.Parse(postid), PostConfig.None, true);
            if (entry != null)
            {
                entry.Author = Blog.Author;
                entry.Email = Blog.Email;
                entry.Body = post.description;
                entry.Title = post.title;
                entry.Description = string.Empty;
                entry.IncludeInMainSyndication = true;

                entry.Categories.Clear();
                if (post.categories != null)
                    entry.Categories.AddRange(post.categories);

                entry.PostType = PostType.BlogPost;
                //User trying to change future dating.

                DateTime dateTimeInPost = Blog.TimeZone.ToLocalTime(post.dateCreated);

                if (dateTimeInPost > Blog.TimeZone.Now && publish) 
                {
                    entry.DateSyndicated = dateTimeInPost;
                }
                entry.IsActive = publish;
                

                entry.DateModified = Blog.TimeZone.Now;
                int[] categoryIds = { };
                if (entry.Categories.Count > 0)
                {
                    categoryIds = Entries.GetCategoryIdsFromCategoryTitles(entry);
                }

                if (!string.IsNullOrEmpty(post.wp_slug))
                {
                    entry.EntryName = post.wp_slug;
                }

                Entries.Update(entry);
                Entries.SetEntryCategoryList(entry.Id, categoryIds);

                if (entry.Enclosure == null)
                {
                    if (!string.IsNullOrEmpty(post.enclosure.url))
                    {
                        Components.Enclosure enc = new Components.Enclosure();
                        enc.Url = post.enclosure.url;
                        enc.MimeType = post.enclosure.type;
                        enc.Size = post.enclosure.length;
                        enc.EntryId = entry.Id;
                        Enclosures.Create(enc);
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(post.enclosure.url))
                    {
                        Components.Enclosure enc = entry.Enclosure;
                        enc.Url = post.enclosure.url;
                        enc.MimeType = post.enclosure.type;
                        enc.Size = post.enclosure.length;
                        Enclosures.Update(enc);
                    }
                    else
                    {
                        Enclosures.Delete(entry.Enclosure.Id);
                    }
                }
            }
            return false;
        }

        public Post getPost(string postid, string username, string password)
        {
            ValidateUser(username, password, Blog.AllowServiceAccess);

            Entry entry = Entries.GetEntry(Int32.Parse(postid), PostConfig.None, true);
            if (entry == null) {
                throw new XmlRpcFaultException(0, "The entry could not be found");
            }
            Post post = new Post();
            post.link = Url.EntryUrl(entry).ToFullyQualifiedUrl(Blog).ToString();
            post.description = entry.Body;
            post.dateCreated = entry.DateCreated;
            post.postid = entry.Id;
            post.title = entry.Title;
            post.permalink = Url.EntryUrl(entry).ToFullyQualifiedUrl(Blog).ToString();
            post.categories = new string[entry.Categories.Count];

            if (entry.Enclosure != null) {
                post.enclosure = new Enclosure {
                    length = (int)entry.Enclosure.Size,
                    type = entry.Enclosure.MimeType,
                    url = entry.Enclosure.Url
                };
            }
            
            if (entry.HasEntryName) {
                post.wp_slug = entry.EntryName;
            }

            entry.Categories.CopyTo(post.categories, 0);

            return post;
        }

        public Post[] getRecentPosts(string blogid, string username, string password, int numberOfPosts)
        {
            ValidateUser(username, password, Blog.AllowServiceAccess);

            ICollection<Entry> entries = Entries.GetRecentPosts(numberOfPosts, PostType.BlogPost, PostConfig.IsActive, true);
            
            var posts = from entry in entries
                        select new Post {
                             dateCreated = entry.DateCreated,
                             description = entry.Body,
                             link = Url.EntryUrl(entry),
                             permalink = Url.EntryUrl(entry).ToFullyQualifiedUrl(Blog).ToString(),
                             title = entry.Title,
                             postid = entry.Id.ToString(CultureInfo.InvariantCulture),
                             userid = entry.Body.GetHashCode().ToString(CultureInfo.InvariantCulture),
                             wp_slug = (entry.HasEntryName ? entry.EntryName : null),
                             categories = (entry.Categories ?? new string[0]).ToArray(),
                             enclosure = (entry.Enclosure == null ? new Enclosure() : new Enclosure {
                                 length = (int)entry.Enclosure.Size,
                                 url = entry.Enclosure.Url,
                                 type = entry.Enclosure.MimeType
                             })
                        };

            return posts.ToArray();
        }

        public CategoryInfo[] getCategories(string blogid, string username, string password)
        {
            ValidateUser(username, password, Blog.AllowServiceAccess);

            //TODO Replace with repository access
            ICollection<LinkCategory> categories = Links.GetCategories(CategoryType.PostCollection, ActiveFilter.None);
            if (categories == null) {
                throw new XmlRpcFaultException(0, "No categories exist");
            }

            var categoryInfos = from category in categories
                                select new CategoryInfo { 
                                    categoryid = category.Id.ToString(CultureInfo.InvariantCulture),
                                    title = category.Title,
                                    htmlUrl = Url.CategoryUrl(category).ToFullyQualifiedUrl(Blog).ToString(),
                                    rssUrl = Url.CategoryRssUrl(category).ToFullyQualifiedUrl(Blog).ToString(),
                                    description = category.Title
                                };

            return categoryInfos.ToArray();
        }

        /// <summary>
        /// Creates a new post.  The publish boolean is used to determine whether the item 
        /// should be published or not.
        /// </summary>
        /// <param name="blogid">The blogid.</param>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <param name="post">The post.</param>
        /// <param name="publish">if set to <c>true</c> [publish].</param>
        /// <returns></returns>
        public string newPost(string blogid, string username, string password, Post post, bool publish) {
            return PostContent(username, password, ref post, publish, PostType.BlogPost);
        }

        public mediaObjectInfo newMediaObject(object blogid, string username, string password, mediaObject mediaobject)
        {
            ValidateUser(username, password, Blog.AllowServiceAccess);

            try
            {
                //We don't validate the file because newMediaObject allows file to be overwritten
                //But we do check the directory and create if necessary
                //The media object's name can have extra folders appended so we check for this here too.
                Images.EnsureDirectory(Path.Combine(Blog.ImageDirectory,mediaobject.name.Substring(0, mediaobject.name.LastIndexOf("/") + 1).Replace("/", "\\")));
                FileStream fStream = new FileStream(Path.Combine(Blog.ImageDirectory,mediaobject.name), FileMode.Create);
                BinaryWriter bw = new BinaryWriter(fStream);
                bw.Write(mediaobject.bits);
            }
            //Any IO exceptions, we throw a new XmlRpcFault Exception
            catch (IOException)
            {
                throw new XmlRpcFaultException(0, "Error saving file.");
            }

            //If all works, we return a mediaobjectinfo struct back holding the URL.
            mediaObjectInfo media;
            media.url = Blog.ImagePath + mediaobject.name;
            return media;
        }
        #endregion

	    #region w.bloggar workarounds/nominal MT support - HACKS
		
		// w.bloggar is not correctly implementing metaWeblogAPI on its getRecentPost call, it wants 
		// an instance of blogger.getRecentPosts at various time. 
		// 
		// What works better with w.bloggar is to tell it to use MT settings. For w.bloggar users 
		// with metaWeblog configured, we'll throw a more explanatory exception than method not found.

		public struct BloggerPost
		{
			public string content;
			public DateTime dateCreated;
			public string postid;
			public string userid;
		} 

		[XmlRpcMethod("blogger.getRecentPosts",
			 Description="Workaround for w.bloggar errors. Exists just to throw an exception explaining issue.")]
		public BloggerPost[] GetRecentPosts(string appKey, string blogid, string username, 
			string password, int numberOfPosts)
		{
			throw new XmlRpcFaultException(0, "You are most likely getting this message because you are using w.bloggar or trying to access Blogger API support in .Text--only metaWeblog API is currently supported. If your issue is w.bloggar, read on.\n\nw.bloggar does not correctly implement the metaWeblog API.\n\nIt is trying to call blogger.getRecentPosts, which does not exist in the metaWeblog API. Contact w.bloggar and encourage them to fix this bug.\n\nIn the meantime, to workaround this, go to the Account Properties dialog and hit 'Reload Blogs List'. This should clear the issue temporarily on w.bloggars side.");
		}		

		// we'll also add a couple structs and methods to give us nominal MT API-level support.
		// by doing this we'll allow w.bloggar to run against .Text using w.b's MT configuration.
		public struct MtCategory
		{
			public string categoryId;
			[XmlRpcMissingMapping(MappingAction.Ignore)]
			public string categoryName;
			[XmlRpcMissingMapping(MappingAction.Ignore)]
			public bool isPrimary;

			/// <summary>
			/// Initializes a new instance of the <see cref="MtCategory"/> class.
			/// </summary>
			/// <param name="category">The category.</param>
			public MtCategory(string category)
			{
				categoryId = category;
				categoryName = category;
				isPrimary = false;
			}

			/// <summary>
			/// Initializes a new instance of the <see cref="MtCategory"/> class.
			/// </summary>
			/// <param name="id">The id.</param>
			/// <param name="category">The category.</param>
			public MtCategory(string id, string category)
			{
				categoryId = id;
				categoryName = category;
				isPrimary = false;
			}
		}

		/// <summary>
		/// Represents a text filter returned by mt.supportedTextFilters.
		/// </summary>
		[XmlRpcMissingMapping(MappingAction.Ignore)]
		public struct MtTextFilter
		{
			/// <summary>
			/// Initializes a new instance of the <see cref="MtTextFilter"/> class.
			/// </summary>
			/// <param name="key">The key.</param>
			/// <param name="label">The label.</param>
			public MtTextFilter(string key, string label)
			{
				this.key = key; 
				this.label = label;
			}
			public string key;
			public string label;
		}

		[XmlRpcMethod("mt.getCategoryList", 
			 Description="Gets a list of active categories for a given blog as an array of MT category struct.")]
		public MtCategory[] GetCategoryList(string blogid, string username, string password)
		{
			ValidateUser(username,password,Blog.AllowServiceAccess);

            ICollection<LinkCategory> lcc = Links.GetCategories(CategoryType.PostCollection, ActiveFilter.None);
			if(lcc == null)
			{
				throw new XmlRpcFaultException(0, "No categories exist");
			}

			MtCategory[] categories = new MtCategory[lcc.Count];
            int i = 0;
			foreach(LinkCategory linkCategory in lcc)
			{
				MtCategory _category = new MtCategory(linkCategory.Id.ToString(CultureInfo.InvariantCulture), linkCategory.Title);
				categories[i] = _category;
                i++;
			}
			return categories;
		}

		[XmlRpcMethod("mt.setPostCategories",
			Description="Sets the categories for a given post.")]
		public bool SetPostCategories(string postid, string username, string password,
			MtCategory[] categories)
		{
			ValidateUser(username,password,Blog.AllowServiceAccess);
						
			if (categories != null && categories.Length > 0)
			{
				int postID = Int32.Parse(postid);

				ArrayList al = new ArrayList();

														
				for (int i = 0; i < categories.Length; i++)
				{
						al.Add(Int32.Parse(categories[i].categoryId));
				}

				if(al.Count > 0)
				{
					Entries.SetEntryCategoryList(postID,(int[])al.ToArray(typeof(int)));
				}
			}				
			
			return true;
		}		

		[XmlRpcMethod("mt.getPostCategories",
			 Description="Sets the categories for a given post.")]
		public MtCategory[] GetPostCategories(string postid, string username, string password)
		{
			ValidateUser(username, password, Blog.AllowServiceAccess);

			int postID = Int32.Parse(postid);
			ICollection<Link> postCategories = Links.GetLinkCollectionByPostID(postID);
			MtCategory[] categories = new MtCategory[postCategories.Count];
			if (postCategories.Count > 0)
			{
				// REFACTOR: Might prefer seeing a dictionary come back straight from the provider.
				// for now we'll build our own catid->catTitle lookup--we need it below bc collection
				// from post is going to be null for title.
                ICollection<LinkCategory> cats = Links.GetCategories(CategoryType.PostCollection, ActiveFilter.None);
				Hashtable catLookup = new Hashtable(cats.Count);
				foreach (LinkCategory currentCat in cats)
					catLookup.Add(currentCat.Id, currentCat.Title);

                int i = 0;
				foreach(Link link in postCategories)
				{
					MtCategory _category = new MtCategory(link.CategoryID.ToString(CultureInfo.InvariantCulture), (string)catLookup[link.CategoryID]);				

					categories[i] = _category;
                    i++;
				}
			}				
			
			return categories;
		}

		/// <summary>
		/// Retrieve information about the text formatting plugins supported by the server.
		/// </summary>
		/// <returns>
		/// an array of structs containing String key and String label. 
		/// key is the unique string identifying a text formatting plugin, 
		/// and label is the readable description to be displayed to a user. 
		/// key is the value that should be passed in the mt_convert_breaks 
		/// parameter to newPost and editPost.
		/// </returns>
		[XmlRpcMethod("mt.supportedTextFilters",
			 Description="Retrieve information about the text formatting plugins supported by the server.")]
		public MtTextFilter[] GetSupportedTextFilters()
		{
			return new MtTextFilter[] {new MtTextFilter("test", "test"), };
		}
		#endregion

        #region IWordPressApi Members

        public int newCategory(string blogid, string username, string password, WordpressCategory category)
        {
            LinkCategory newCategory = new LinkCategory();
            newCategory.CategoryType = CategoryType.PostCollection;
            newCategory.Title = category.name;
            newCategory.IsActive = true;
            newCategory.Description = category.name;

            newCategory.Id = Links.CreateLinkCategory(newCategory);

            return newCategory.Id;
        }

        public int newPage(string blog_id, string username, string password, Post content, bool publish)
        {
            return Convert.ToInt32(PostContent(username, password, ref content, publish, PostType.Story));
        }

        public int editPage(string blog_id, string page_id, string username, string password, Post content, bool publish)
        {
            ValidateUser(username, password, Blog.AllowServiceAccess);

            Entry entry = Entries.GetEntry(Int32.Parse(page_id), PostConfig.None, true);
            if (entry != null)
            {
                entry.Author = Blog.Author;
                entry.Email = Blog.Email;
                entry.Body = content.description;
                entry.Title = content.title;
                entry.Description = string.Empty;
                entry.IncludeInMainSyndication = true;

                if (content.categories != null) {
                    entry.Categories.AddRange(content.categories);
                }

                entry.PostType = PostType.Story;
                entry.IsActive = publish;

                if (!string.IsNullOrEmpty(content.wp_slug)) {
                    entry.EntryName = content.wp_slug;
                }

                entry.DateModified = Blog.TimeZone.Now;
                Entries.Update(entry);
            }
            return Convert.ToInt32(page_id);
        }

        public Post[] getPages(string blog_id, string username, string password, int numberOfPosts)
        {
            ValidateUser(username, password, Blog.AllowServiceAccess);

            ICollection<Entry> entries = Entries.GetRecentPosts(numberOfPosts, PostType.Story, PostConfig.IsActive, true);
            var posts = from entry in entries
                        select new Post
                        {
                            dateCreated = entry.DateCreated,
                            description = entry.Body,
                            link = Url.EntryUrl(entry),
                            permalink = Url.EntryUrl(entry).ToFullyQualifiedUrl(Blog).ToString(),
                            title = entry.Title,
                            postid = entry.Id.ToString(CultureInfo.InvariantCulture),
                            userid = entry.Body.GetHashCode().ToString(CultureInfo.InvariantCulture),
                            wp_slug = (entry.HasEntryName ? entry.EntryName : null),
                            categories = (entry.Categories ?? new string[0]).ToArray(),
                            enclosure = (entry.Enclosure == null ? new Enclosure() : new Enclosure
                            {
                                length = (int)entry.Enclosure.Size,
                                url = entry.Enclosure.Url,
                                type = entry.Enclosure.MimeType
                            })
                        };

            return posts.ToArray();
        }

        public Post getPage(string blog_id, string page_id, string username, string password)
        {
            Framework.Blog info = Blog;
            ValidateUser(username, password, info.AllowServiceAccess);

            Entry entry = Entries.GetEntry(Int32.Parse(page_id), PostConfig.None, true);
            Post post = new Post();
            post.link = Url.EntryUrl(entry).ToFullyQualifiedUrl(Blog).ToString();
            post.description = entry.Body;
            post.dateCreated = entry.DateCreated;
            post.postid = entry.Id;
            post.title = entry.Title;
            post.permalink = Url.EntryUrl(entry).ToFullyQualifiedUrl(Blog).ToString();
            post.categories = new string[entry.Categories.Count];
            entry.Categories.CopyTo(post.categories, 0);
            if (entry.HasEntryName) {
                post.wp_slug = entry.EntryName;
            }
            if (entry.Enclosure != null) {
                post.enclosure = new Enclosure {
                    length = (int)entry.Enclosure.Size,
                    type = entry.Enclosure.MimeType,
                    url = entry.Enclosure.Url
                };
            }

            return post;
        }

        public bool deletePage(string blog_id, string username, string password, string page_id) {
            ValidateUser(username, password, Blog.AllowServiceAccess);

            try {
                Entries.Delete(Int32.Parse(page_id));
                return true;
            }
            catch {
                throw new XmlRpcFaultException(1, "Could not delete page: " + page_id);
            }
        }

        #endregion
    }
}
