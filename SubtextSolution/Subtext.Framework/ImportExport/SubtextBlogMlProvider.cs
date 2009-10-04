#region Disclaimer/Info

///////////////////////////////////////////////////////////////////////////////////////////////////
// Subtext WebLog
// 
// Subtext is an open source weblog system that is a fork of the .TEXT
// weblog system.
//
// For updated news and information please visit http://subtextproject.com/
// Subtext is hosted at Google Code at http://code.google.com/p/subtext/
// The development mailing list is at subtext-devs@lists.sourceforge.net 
//
// This project is licensed under the BSD license.  See the License.txt file for more information.
///////////////////////////////////////////////////////////////////////////////////////////////////

#endregion

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using BlogML;
using BlogML.Xml;
using Subtext.BlogML;
using Subtext.Extensibility;
using Subtext.Framework;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;
using Subtext.Framework.Format;
using Subtext.Framework.Providers;
using Subtext.Framework.Routing;
using Subtext.Framework.Services;
using Subtext.Framework.Text;

namespace Subtext.ImportExport
{
    public class SubtextBlogMLProvider : BlogMLProvider
    {
        bool _duplicateCommentsEnabled;

        public SubtextBlogMLProvider(ISubtextContext context, ICommentService commentService,
                                     IEntryPublisher entryPublisher)
        {
            SubtextContext = context;
            CommentService = commentService;
            EntryPublisher = entryPublisher;
            PageSize = 100;
        }

        public ISubtextContext SubtextContext { get; private set; }

        public IEntryPublisher EntryPublisher { get; private set; }

        public ICommentService CommentService { get; private set; }

        protected Blog Blog
        {
            get { return SubtextContext.Blog; }
        }

        protected UrlHelper Url
        {
            get { return SubtextContext.UrlHelper; }
        }

        protected ObjectProvider Repository
        {
            get { return SubtextContext.Repository; }
        }

        /// <summary>
        /// Returns the context under which blogml import or export is running under.
        /// </summary>
        /// <returns></returns>
        public override BlogMLContext GetBlogMLContext()
        {
            bool embedValue = false;
            if(HttpContext.Current != null && HttpContext.Current.Request != null)
            {
                embedValue = String.Equals(HttpContext.Current.Request.QueryString["embed"], "true",
                                           StringComparison.OrdinalIgnoreCase);
            }

            return new BlogMLContext(Blog.Id.ToString(CultureInfo.InvariantCulture), embedValue);
        }

        /// <summary>
        /// Method called before an import begins. Allows the provider to 
        /// initialize any state in the current blog.
        /// </summary>
        public override void PreImport()
        {
            _duplicateCommentsEnabled = Blog.DuplicateCommentsEnabled;
            if(!_duplicateCommentsEnabled)
            {
                // Allow duplicate comments temporarily.
                Blog.DuplicateCommentsEnabled = true;
                Config.UpdateConfigData(Blog);
            }
        }

        /// <summary>
        /// Method called when an import is complete.
        public override void ImportComplete()
        {
            if(Blog.DuplicateCommentsEnabled != _duplicateCommentsEnabled)
            {
                Blog.DuplicateCommentsEnabled = _duplicateCommentsEnabled;
                Config.UpdateConfigData(Blog);
            }
        }

        /// <summary>
        /// Creates categories from the blog ml.
        /// </summary>
        /// <remarks>
        /// At this time, we only support PostCollection link categories.
        /// </remarks>
        /// <param name="blog"></param>
        public override IDictionary<string, string> CreateCategories(BlogMLBlog blog)
        {
            IDictionary<string, string> idMap = new Dictionary<string, string>();
            foreach(BlogMLCategory bmlCategory in blog.Categories)
            {
                var category = new LinkCategory
                {
                    BlogId = Blog.Id,
                    Title = bmlCategory.Title,
                    Description = bmlCategory.Description,
                    IsActive = bmlCategory.Approved,
                    CategoryType = CategoryType.PostCollection
                };
                Links.CreateLinkCategory(category);
                idMap.Add(bmlCategory.ID, category.Title);
            }
            return idMap;
        }

        /// <summary>
        /// The physical path to the attachment directory.
        /// </summary>
        /// <remarks>
        /// The attachment is passed in to give the blog engine 
        /// the opportunity to use attachment specific directories 
        /// (ex. based on mime type) should it choose.
        /// </remarks>
        public override string GetAttachmentDirectoryPath(BlogMLAttachment attachment)
        {
            return Url.ImageDirectoryPath(Blog);
        }

        /// <summary>
        /// The url to the attachment directory
        /// </summary>
        /// <remarks>
        /// The attachment is passed in to give the blog engine 
        /// the opportunity to use attachment specific directories 
        /// (ex. based on mime type) should it choose.
        /// </remarks>
        public override string GetAttachmentDirectoryUrl(BlogMLAttachment attachment)
        {
            return Url.ImageDirectoryUrl(Blog);
        }

        /// <summary>
        /// Creates a blog post and returns the id.
        /// </summary>
        public override string CreateBlogPost(BlogMLBlog blog, BlogMLPost post,
                                              IDictionary<string, string> categoryIdMap)
        {
            Entry newEntry = CreateEntryFromBlogMLBlogPost(blog, post, categoryIdMap);
            newEntry.BlogId = Blog.Id;
            var publisher = EntryPublisher as EntryPublisher;
            if(publisher != null)
            {
                var transform = publisher.Transformation as CompositeTextTransformation;
                if(transform != null)
                {
                    transform.Remove<KeywordExpander>();
                }
            }

            return EntryPublisher.Publish(newEntry).ToString(CultureInfo.InvariantCulture);
        }

        public static Entry CreateEntryFromBlogMLBlogPost(BlogMLBlog blog, BlogMLPost post,
                                                          IDictionary<string, string> categoryIdMap)
        {
            var newEntry = new Entry((post.PostType == BlogPostTypes.Article) ? PostType.Story : PostType.BlogPost)
            {
                Title = GetTitleFromPost(post),
                DateCreated = post.DateCreated,
                DateModified = post.DateModified,
                DateSyndicated = post.DateModified,
                Body = post.Content.Text
            };
            if(post.HasExcerpt)
            {
                newEntry.Description = post.Excerpt.Text;
            }
            newEntry.IsActive = post.Approved;
            newEntry.DisplayOnHomePage = post.Approved;
            newEntry.IncludeInMainSyndication = post.Approved;
            newEntry.IsAggregated = post.Approved;
            newEntry.AllowComments = true;
            if(!string.IsNullOrEmpty(post.PostName))
            {
                newEntry.EntryName = post.PostName;
            }
            else
            {
                SetEntryNameForBlogspotImport(post, newEntry);
            }

            if(post.Authors.Count > 0)
            {
                foreach(BlogMLAuthor author in blog.Authors)
                {
                    if(author.ID == post.Authors[0].Ref)
                    {
                        newEntry.Author = author.Title;
                        newEntry.Email = author.Email;
                        break;
                    }
                }
            }

            foreach(BlogMLCategoryReference categoryRef in post.Categories)
            {
                string categoryTitle;
                if(categoryIdMap.TryGetValue(categoryRef.Ref, out categoryTitle))
                {
                    newEntry.Categories.Add(categoryTitle);
                }
            }
            return newEntry;
        }

        private static void SetEntryNameForBlogspotImport(BlogMLPost post, Entry newEntry)
        {
            if(!String.IsNullOrEmpty(post.PostUrl) &&
               post.PostUrl.Contains("blogspot.com/", StringComparison.OrdinalIgnoreCase))
            {
                Uri postUrl = post.PostUrl.ParseUri();
                string fileName = postUrl.Segments.Last();
                newEntry.EntryName = Path.GetFileNameWithoutExtension(fileName);
                if(String.IsNullOrEmpty(post.Title) && String.IsNullOrEmpty(post.PostName))
                {
                    newEntry.Title = newEntry.EntryName.Replace("-", " ").Replace("+", " ").Replace("_", " ");
                }
            }
        }

        public static string GetTitleFromPost(BlogMLPost blogPost)
        {
            if(!String.IsNullOrEmpty(blogPost.Title))
            {
                return blogPost.Title;
            }
            if(!String.IsNullOrEmpty(blogPost.PostName))
            {
                return blogPost.PostName;
            }

            return "Post #" + blogPost.ID;
        }

        /// <summary>
        /// Creates a comment in the system.
        /// </summary>
        public override void CreatePostComment(BlogMLComment comment, string newPostId)
        {
            var newComment = new FeedbackItem(FeedbackType.Comment)
            {
                BlogId = Blog.Id,
                EntryId = int.Parse(newPostId, CultureInfo.InvariantCulture),
                Title = comment.Title ?? string.Empty,
                DateCreated = comment.DateCreated,
                DateModified = comment.DateModified,
                Body = comment.Content.UncodedText ?? string.Empty,
                Approved = comment.Approved,
                Author = comment.UserName ?? string.Empty,
                Email = comment.UserEMail
            };

            if(!string.IsNullOrEmpty(comment.UserUrl))
            {
                newComment.SourceUrl = new Uri(comment.UserUrl);
            }

            CommentService.Create(newComment);
        }

        /// <summary>
        /// Creates a trackback for the post.
        /// </summary>
        /// <param name="trackback"></param>
        /// <param name="newPostId"></param>
        public override void CreatePostTrackback(BlogMLTrackback trackback, string newPostId)
        {
            var newPingTrack = new FeedbackItem(FeedbackType.PingTrack)
            {
                BlogId = Blog.Id,
                EntryId = int.Parse(newPostId, CultureInfo.InvariantCulture),
                Title = trackback.Title,
                SourceUrl = new Uri(trackback.Url),
                Approved = trackback.Approved,
                DateCreated = trackback.DateCreated,
                DateModified = trackback.DateModified,
                Author = UrlFormats.GetHostFromExternalUrl(trackback.Url) ?? string.Empty,
                Body = string.Empty
            };
            // we use an actual name here, but BlogML doesn't support this, so let's try  
            // to parse the url's host out of the url.
            // so the duplicate Comment Filter doesn't break when computing the checksum

            CommentService.Create(newPingTrack);
        }

        public override void SetBlogMLExtendedProperties(BlogMLBlog.ExtendedPropertiesCollection extendedProperties)
        {
            if(extendedProperties != null && extendedProperties.Count > 0)
            {
                Blog info = Blog;

                foreach(var extProp in extendedProperties)
                {
                    if(BlogMLBlogExtendedProperties.CommentModeration.Equals(extProp.Key))
                    {
                        bool modEnabled;

                        if(bool.TryParse(extProp.Value, out modEnabled))
                        {
                            info.ModerationEnabled = modEnabled;
                        }
                    }
                    else if(BlogMLBlogExtendedProperties.EnableSendingTrackbacks.Equals(extProp.Key))
                    {
                        bool tracksEnabled;

                        if(bool.TryParse(extProp.Value, out tracksEnabled))
                        {
                            /* TODO: The blog.TrackbasksEnabled determines if Subtext will ACCEPT and SEND trackbacks.
                             * Perhaps we should separate the two out?
                             * For now, we'll assume that if a BlogML blog allows sending, it will also
                             * allow receiving track/pingbacks.
                             */
                            info.TrackbacksEnabled = tracksEnabled;
                        }
                    }
                }

                Config.UpdateConfigData(info);
            }
        }

        /// <summary>
        /// Lets the provider decide how to log errors.
        /// </summary>
        public override void LogError(string message, Exception exception)
        {
            //TODO:
        }
    }
}