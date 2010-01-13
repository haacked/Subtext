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
using Subtext.Framework.Properties;
using Subtext.Framework.Routing;
using Subtext.Framework.Security;
using Subtext.Framework.Services;
using Subtext.Framework.Tracking;
using Subtext.Framework.Util;

//Need to find a method that has access to context, so we can terminate the request if AllowServiceAccess == false.
//Users will be able to access the metablogapi page, but will not be able to make a request, but the page should not be visible

namespace Subtext.Framework.XmlRpc
{
    /// <summary>
    /// Implements the MetaBlog API.
    /// </summary>
    public class MetaWeblog : SubtextXmlRpcService, IMetaWeblog, IWordPressApi
    {
        static readonly Log Log = new Log();

        public MetaWeblog(ISubtextContext context) : this(context, context.ServiceLocator.GetService<IEntryPublisher>())
        {
        }

        public MetaWeblog(ISubtextContext context, IEntryPublisher entryPublisher)
            : base(context)
        {
            EntryPublisher = entryPublisher;
        }

        protected IEntryPublisher EntryPublisher
        {
            get; 
            private set;
        }

        private Entry GetBlogPost(string pageId)
        {
            Entry entry = Repository.GetEntry(Int32.Parse(pageId, CultureInfo.InvariantCulture), false /*activeOnly*/, true /*includeCategories*/);
            if(entry == null)
            {
                return null;
            }
            entry.Blog = Blog;
            return entry;
        }

        public BlogInfo[] getUsersBlogs(string appKey, string username, string password)
        {
            Blog info = Blog;
            ValidateUser(username, password, info.AllowServiceAccess);

            var bi = new[] {
                new BlogInfo
                {
                    blogid = info.Id.ToString(CultureInfo.InvariantCulture),
                    blogName = info.Title,
                    url = Url.BlogUrl().ToFullyQualifiedUrl(info).ToString()
                }
            };
            return bi;
        }

        public bool deletePost(string appKey, string postid, string username, string password,
                               [XmlRpcParameter(
                                   Description =
                                   "Where applicable, this specifies whether the blog should be republished after the post has been deleted."
                                   )] bool publish)
        {
            ValidateUser(username, password, Blog.AllowServiceAccess);

            try
            {
                Repository.DeleteEntry(Int32.Parse(postid, CultureInfo.InvariantCulture));
                return true;
            }
            catch
            {
                throw new XmlRpcFaultException(1,
                                               String.Format(CultureInfo.InvariantCulture,
                                                             Resources.XmlRpcFault_CannotDeletePost, postid));
            }
        }

        public bool editPost(string postid, string username, string password, Post post, bool publish)
        {
            ValidateUser(username, password, Blog.AllowServiceAccess);

            Entry entry = GetBlogPost(postid);
            if(entry != null)
            {
                entry.Author = Blog.Author;
                entry.Email = Blog.Email;
                entry.Categories.Clear();
                post.CopyValuesTo(entry);
                entry.IncludeInMainSyndication = true;
                entry.PostType = PostType.BlogPost;

                //User trying to change future dating.
                if(publish && post.dateCreated != null &&
                   Blog.TimeZone.IsInFuture(post.dateCreated.Value, TimeZoneInfo.Utc))
                {
                    entry.DateSyndicated = post.dateCreated.Value;
                }
                entry.IsActive = publish;

                entry.DateModified = Blog.TimeZone.Now;

                EntryPublisher.Publish(entry);
                
                if(entry.Enclosure == null)
                {
                    if(post.enclosure != null)
                    {
                        Components.Enclosure enclosure = post.enclosure.Value.CopyValuesToEnclosure();
                        enclosure.EntryId = entry.Id;
                        Repository.Create(enclosure);
                    }
                }
                else // if(entry.Enclosure != null)
                {
                    if(post.enclosure != null)
                    {
                        Components.Enclosure enclosure = entry.Enclosure;
                        post.enclosure.Value.CopyValuesTo(enclosure);
                        Repository.Update(enclosure);
                    }
                    else
                    {
                        Repository.DeleteEnclosure(entry.Enclosure.Id);
                    }
                }
            }
            return true;
        }

        public Post getPost(string postid, string username, string password)
        {
            ValidateUser(username, password, Blog.AllowServiceAccess);

            Entry entry = GetBlogPost(postid);
            if(entry == null)
            {
                throw new XmlRpcFaultException(0, Resources.XmlRpcFault_CouldNotFindEntry);
            }
            var post = new Post
            {
                link = Url.EntryUrl(entry).ToFullyQualifiedUrl(Blog).ToString(),
                description = entry.Body,
                excerpt = entry.Description ?? string.Empty,
                dateCreated = entry.DateCreated,
                postid = entry.Id,
                title = entry.Title,
                permalink = Url.EntryUrl(entry).ToFullyQualifiedUrl(Blog).ToString(),
                categories = new string[entry.Categories.Count]
            };

            if(entry.Enclosure != null)
            {
                post.enclosure = new Enclosure
                {
                    length = (int)entry.Enclosure.Size,
                    type = entry.Enclosure.MimeType,
                    url = entry.Enclosure.Url
                };
            }

            if(entry.HasEntryName)
            {
                post.wp_slug = entry.EntryName;
            }

            entry.Categories.CopyTo(post.categories, 0);

            return post;
        }

        public Post[] getRecentPosts(string blogid, string username, string password, int numberOfPosts)
        {
            ValidateUser(username, password, Blog.AllowServiceAccess);

            ICollection<Entry> entries = Repository.GetEntries(numberOfPosts, PostType.BlogPost, PostConfig.IsActive, true);

            IEnumerable<Post> posts = from entry in entries
                                      select new Post
                                      {
                                          dateCreated = entry.DateCreated,
                                          description = entry.Body,
                                          excerpt = entry.Description,
                                          link = Url.EntryUrl(entry),
                                          permalink = Url.EntryUrl(entry).ToFullyQualifiedUrl(Blog).ToString(),
                                          title = entry.Title,
                                          postid = entry.Id.ToString(CultureInfo.InvariantCulture),
                                          userid = entry.Body.GetHashCode().ToString(CultureInfo.InvariantCulture),
                                          wp_slug = (entry.HasEntryName ? entry.EntryName : null),
                                          categories = (entry.Categories ?? new string[0]).ToArray(),
                                          enclosure = (entry.Enclosure == null
                                                           ? new Enclosure()
                                                           : new Enclosure
                                                           {
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

            ICollection<LinkCategory> categories = Repository.GetCategories(CategoryType.PostCollection, false);
            if(categories == null)
            {
                throw new XmlRpcFaultException(0, Resources.XmlRpcFault_NoCategories);
            }

            IEnumerable<CategoryInfo> categoryInfos = from category in categories
                                                      select new CategoryInfo
                                                      {
                                                          categoryid =
                                                              category.Id.ToString(CultureInfo.InvariantCulture),
                                                          title = category.Title,
                                                          htmlUrl =
                                                              Url.CategoryUrl(category).ToFullyQualifiedUrl(Blog).
                                                              ToString(),
                                                          rssUrl =
                                                              Url.CategoryRssUrl(category).ToFullyQualifiedUrl(Blog).
                                                              ToString(),
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
        public string newPost(string blogid, string username, string password, Post post, bool publish)
        {
            return PostContent(username, password, ref post, publish, PostType.BlogPost);
        }

        public mediaObjectInfo newMediaObject(object blogid, string username, string password, mediaObject mediaobject)
        {
            ValidateUser(username, password, Blog.AllowServiceAccess);
            string imageDirectory = Url.ImageDirectoryPath(Blog);
            try
            {
                // newMediaObject allows files to be overwritten
                // The media object's name can have extra folders appended so we check for this here too.
                FileHelper.EnsureDirectory(Path.Combine(imageDirectory,
                                                        mediaobject.name.Substring(0,
                                                                                   mediaobject.name.LastIndexOf("/",
                                                                                                                StringComparison
                                                                                                                    .
                                                                                                                    Ordinal) +
                                                                                   1).Replace("/", "\\")));
                string imageFilePhysicalPath = Path.Combine(imageDirectory, mediaobject.name);
                FileHelper.WriteBytesToFile(imageFilePhysicalPath, mediaobject.bits);
            }
            catch(IOException)
            {
                throw new XmlRpcFaultException(0, Resources.XmlRpcFault_ErrorSavingFile);
            }

            mediaObjectInfo media;
            media.url = Url.ImageUrl(Blog, mediaobject.name);
            return media;
        }

        // w.bloggar workarounds/nominal MT support - HACKS

        // w.bloggar is not correctly implementing metaWeblogAPI on its getRecentPost call, it wants 
        // an instance of blogger.getRecentPosts at various time. 
        // 
        // What works better with w.bloggar is to tell it to use MT settings. For w.bloggar users 
        // with metaWeblog configured, we'll throw a more explanatory exception than method not found.

        // Wordpress API

        #region IWordPressApi Members

        public int newCategory(string blogid, string username, string password, WordpressCategory category)
        {
            var newCategory = new LinkCategory
            {
                CategoryType = CategoryType.PostCollection,
                Title = category.name,
                IsActive = true,
                Description = category.name
            };

            newCategory.Id = Links.CreateLinkCategory(newCategory);

            return newCategory.Id;
        }

        public int newPage(string blog_id, string username, string password, Post content, bool publish)
        {
            return Convert.ToInt32(PostContent(username, password, ref content, publish, PostType.Story),
                                   CultureInfo.InvariantCulture);
        }

        public int editPage(string blog_id, string page_id, string username, string password, Post content, bool publish)
        {
            ValidateUser(username, password, Blog.AllowServiceAccess);

            Entry entry = GetBlogPost(page_id);
            if(entry != null)
            {
                entry.Author = Blog.Author;
                entry.Email = Blog.Email;
                entry.Body = content.description;
                entry.Title = content.title;
                entry.Description = content.excerpt ?? string.Empty;
                entry.IncludeInMainSyndication = true;

                if(content.categories != null)
                {
                    entry.Categories.AddRange(content.categories);
                }

                entry.PostType = PostType.Story;
                entry.IsActive = publish;

                if(!string.IsNullOrEmpty(content.wp_slug))
                {
                    entry.EntryName = content.wp_slug;
                }

                entry.DateModified = Blog.TimeZone.Now;
                EntryPublisher.Publish(entry);
            }
            return Convert.ToInt32(page_id, CultureInfo.InvariantCulture);
        }

        public Post[] getPages(string blog_id, string username, string password, int numberOfPosts)
        {
            ValidateUser(username, password, Blog.AllowServiceAccess);

            ICollection<Entry> entries = Repository.GetEntries(numberOfPosts, PostType.Story, PostConfig.IsActive, true);
            IEnumerable<Post> posts = from entry in entries
                                      select new Post
                                      {
                                          dateCreated = entry.DateCreated,
                                          description = entry.Body,
                                          excerpt = entry.Description ?? string.Empty,
                                          link = Url.EntryUrl(entry),
                                          permalink = Url.EntryUrl(entry).ToFullyQualifiedUrl(Blog).ToString(),
                                          title = entry.Title,
                                          postid = entry.Id.ToString(CultureInfo.InvariantCulture),
                                          userid = entry.Body.GetHashCode().ToString(CultureInfo.InvariantCulture),
                                          wp_slug = (entry.HasEntryName ? entry.EntryName : null),
                                          categories = (entry.Categories ?? new string[0]).ToArray(),
                                          enclosure = (entry.Enclosure == null
                                                           ? new Enclosure()
                                                           : new Enclosure
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
            Blog info = Blog;
            ValidateUser(username, password, info.AllowServiceAccess);

            Entry entry = GetBlogPost(page_id);
            var post = new Post
            {
                link = Url.EntryUrl(entry).ToFullyQualifiedUrl(Blog).ToString(),
                description = entry.Body,
                excerpt = entry.Description ?? string.Empty,
                dateCreated = entry.DateCreated,
                postid = entry.Id,
                title = entry.Title,
                permalink = Url.EntryUrl(entry).ToFullyQualifiedUrl(Blog).ToString(),
                categories = new string[entry.Categories.Count]
            };
            entry.Categories.CopyTo(post.categories, 0);
            if(entry.HasEntryName)
            {
                post.wp_slug = entry.EntryName;
            }
            if(entry.Enclosure != null)
            {
                post.enclosure = new Enclosure
                {
                    length = (int)entry.Enclosure.Size,
                    type = entry.Enclosure.MimeType,
                    url = entry.Enclosure.Url
                };
            }

            return post;
        }

        public bool deletePage(string blog_id, string username, string password, string page_id)
        {
            ValidateUser(username, password, Blog.AllowServiceAccess);

            try
            {
                Repository.DeleteEntry(Int32.Parse(page_id, CultureInfo.InvariantCulture));
                return true;
            }
            catch
            {
                throw new XmlRpcFaultException(1,
                                               String.Format(CultureInfo.InvariantCulture,
                                                             Resources.XmlRpcFault_CannotDeletePage, page_id));
            }
        }

        #endregion

        private void ValidateUser(string username, string password, bool allowServiceAccess)
        {
            if(!Config.Settings.AllowServiceAccess || !allowServiceAccess)
            {
                throw new XmlRpcFaultException(0, Resources.XmlRpcFault_WebServiceNotEnabled);
            }

            bool isValid = SecurityHelper.IsValidUser(Blog, username, password);
            if(!isValid)
            {
                throw new XmlRpcFaultException(0, Resources.XmlRpcFault_UsernameAndPasswordInvalid);
            }
        }

        private string PostContent(string username, string password, ref Post post, bool publish, PostType postType)
        {
            ValidateUser(username, password, Blog.AllowServiceAccess);

            var entry = new Entry(postType) {PostType = postType, IsActive = publish, Author = Blog.Author, Email = Blog.Email};
            post.CopyValuesTo(entry);
            entry.AllowComments = true;
            entry.DisplayOnHomePage = true;

            DateTime dateTimeInPost = post.dateCreated != null ? post.dateCreated.Value : DateTime.UtcNow;
            // Store in the blog's timezone
            dateTimeInPost = Blog.TimeZone.FromUtc(dateTimeInPost);

            entry.DateCreated = entry.DateModified = Blog.TimeZone.Now;
            if(publish)
            {
                entry.DateSyndicated = dateTimeInPost;
            }

            entry.IncludeInMainSyndication = true;
            entry.IsAggregated = true;
            entry.SyndicateDescriptionOnly = false;

            int postId;
            try
            {
                //TODO: Review whether keywords should be true.
                postId = EntryPublisher.Publish(entry);
                if(Blog.TrackbacksEnabled)
                {
                    NotificationServices.Run(entry, Blog, Url);
                }

                if(post.enclosure != null)
                {
                    Components.Enclosure enclosure = post.enclosure.Value.CopyValuesToEnclosure();
                    enclosure.EntryId = postId;
                    Repository.Create(enclosure);
                }

                AddCommunityCredits(entry);
            }
            catch(Exception e)
            {
                throw new XmlRpcFaultException(0, e.Message + " " + e.StackTrace);
            }
            if(postId < 0)
            {
                throw new XmlRpcFaultException(0, Resources.XmlRpcFault_AddPostFailed);
            }
            return postId.ToString(CultureInfo.InvariantCulture);
        }

        private void AddCommunityCredits(Entry entry)
        {
            try
            {
                CommunityCreditNotification.AddCommunityCredits(entry, Url, Blog);
            }
            catch(CommunityCreditNotificationException ex)
            {
                Log.WarnFormat(Resources.XmlRpcWarn_CommunityCredits,
                               Url.EntryUrl(entry).ToFullyQualifiedUrl(Blog).ToString(), ex.Message);
            }
            catch(Exception ex)
            {
                Log.Error(Resources.XmlRpcError_CommunityCredits, ex);
            }
        }

        [XmlRpcMethod("mt.getCategoryList",
            Description = "Gets a list of active categories for a given blog as an array of MT category struct.")]
        public MtCategory[] GetCategoryList(string blogid, string username, string password)
        {
            ValidateUser(username, password, Blog.AllowServiceAccess);

            ICollection<LinkCategory> lcc = Repository.GetCategories(CategoryType.PostCollection, false);
            if(lcc == null)
            {
                throw new XmlRpcFaultException(0, "No categories exist");
            }

            var categories = new MtCategory[lcc.Count];
            int i = 0;
            foreach(LinkCategory linkCategory in lcc)
            {
                var category = new MtCategory(linkCategory.Id.ToString(CultureInfo.InvariantCulture),
                                               linkCategory.Title);
                categories[i] = category;
                i++;
            }
            return categories;
        }

        [XmlRpcMethod("mt.setPostCategories",
            Description = "Sets the categories for a given post.")]
        public bool SetPostCategories(string postid, string username, string password,
                                      MtCategory[] categories)
        {
            ValidateUser(username, password, Blog.AllowServiceAccess);

            if(categories != null && categories.Length > 0)
            {
                int postId = Int32.Parse(postid, CultureInfo.InvariantCulture);

                IEnumerable<int> categoryIds = from category in categories
                                               select int.Parse(category.categoryId, CultureInfo.InvariantCulture);

                if(categoryIds.Any())
                {
                    Repository.SetEntryCategoryList(postId, categoryIds);
                }
            }

            return true;
        }

        [XmlRpcMethod("mt.getPostCategories",
            Description = "Sets the categories for a given post.")]
        public MtCategory[] GetPostCategories(string postid, string userName, string password)
        {
            ValidateUser(userName, password, Blog.AllowServiceAccess);

            int postId = Int32.Parse(postid, CultureInfo.InvariantCulture);
            ICollection<Link> postCategories = Repository.GetLinkCollectionByPostId(postId);
            var categories = new MtCategory[postCategories.Count];
            if(postCategories.Count > 0)
            {
                // REFACTOR: Might prefer seeing a dictionary come back straight from the provider.
                // for now we'll build our own catid->catTitle lookup--we need it below bc collection
                // from post is going to be null for title.
                ICollection<LinkCategory> cats = Repository.GetCategories(CategoryType.PostCollection, false);
                var catLookup = new Hashtable(cats.Count);
                foreach(LinkCategory currentCat in cats)
                {
                    catLookup.Add(currentCat.Id, currentCat.Title);
                }

                int i = 0;
                foreach(Link link in postCategories)
                {
                    var category = new MtCategory(link.CategoryId.ToString(CultureInfo.InvariantCulture),
                                                   (string)catLookup[link.CategoryId]);

                    categories[i] = category;
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
            Description = "Retrieve information about the text formatting plugins supported by the server.")]
        public MtTextFilter[] GetSupportedTextFilters()
        {
            return new[] {new MtTextFilter("test", "test"),};
        }

        #region Nested type: BloggerPost

        public struct BloggerPost
        {
            public string content;
            public DateTime dateCreated;
            public string postid;
            public string userid;
        }

        #endregion

        // we'll also add a couple structs and methods to give us nominal MT API-level support.
        // by doing this we'll allow w.bloggar to run against .Text using w.b's MT configuration.

        #region Nested type: MtCategory

        public struct MtCategory
        {
            public string categoryId;
            [XmlRpcMissingMapping(MappingAction.Ignore)] public string categoryName;
            [XmlRpcMissingMapping(MappingAction.Ignore)] public bool isPrimary;

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

        #endregion

        #region Nested type: MtTextFilter

        /// <summary>
        /// Represents a text filter returned by mt.supportedTextFilters.
        /// </summary>
        [XmlRpcMissingMapping(MappingAction.Ignore)]
        public struct MtTextFilter
        {
            public string key;
            public string label;

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
        }

        #endregion
    }
}