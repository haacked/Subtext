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
using System.Globalization;
using BlogML;
using BlogML.Xml;
using Subtext.Framework;
using Subtext.Framework.Components;
using Subtext.Framework.Providers;
using Subtext.Framework.Routing;
using Subtext.Framework.Services;

namespace Subtext.ImportExport
{
    public class BlogImportRepository : IBlogImportRepository
    {
        public BlogImportRepository(ISubtextContext context, ICommentService commentService, IEntryPublisher entryPublisher, IBlogMLImportMapper mapper)
        {
            SubtextContext = context;
            CommentService = commentService;
            EntryPublisher = entryPublisher;
            Mapper = mapper;
        }

        protected ObjectRepository Repository
        {
            get { return SubtextContext.Repository; }
        }

        public ISubtextContext SubtextContext { get; private set; }

        public IEntryPublisher EntryPublisher { get; private set; }

        public ICommentService CommentService { get; private set; }

        public IBlogMLImportMapper Mapper { get; private set; }

        public Blog Blog
        {
            get { return SubtextContext.Blog; }
        }

        protected BlogUrlHelper Url
        {
            get { return SubtextContext.UrlHelper; }
        }

        public void CreateCategories(BlogMLBlog blog)
        {
            foreach (BlogMLCategory bmlCategory in blog.Categories)
            {
                LinkCategory category = Mapper.ConvertCategory(bmlCategory);
                Repository.CreateLinkCategory(category);
            }
        }

        public string CreateBlogPost(BlogMLBlog blog, BlogMLPost post)
        {
            Entry newEntry = Mapper.ConvertBlogPost(post, blog, Blog);
            newEntry.BlogId = Blog.Id;
            newEntry.Blog = Blog;
            var publisher = EntryPublisher as EntryPublisher;
            if (publisher != null)
            {
                var transform = publisher.Transformation as CompositeTextTransformation;
                if (transform != null)
                {
                    transform.Clear();
                }
            }

            return EntryPublisher.Publish(newEntry).ToString(CultureInfo.InvariantCulture);

        }

        public void CreateComment(BlogMLComment comment, string newPostId)
        {
            var newComment = Mapper.ConvertComment(comment, newPostId);
            CommentService.Create(newComment, false /*runfilters*/);
        }

        public void CreateTrackback(BlogMLTrackback trackback, string newPostId)
        {
            var pingTrack = Mapper.ConvertTrackback(trackback, newPostId);
            CommentService.Create(pingTrack, false /*runfilters*/);
        }

        public void SetExtendedProperties(BlogMLBlog.ExtendedPropertiesCollection extendedProperties)
        {
            if (extendedProperties != null && extendedProperties.Count > 0)
            {
                foreach (var extProp in extendedProperties)
                {
                    if (BlogMLBlogExtendedProperties.CommentModeration.Equals(extProp.Key))
                    {
                        bool modEnabled;

                        if (bool.TryParse(extProp.Value, out modEnabled))
                        {
                            Blog.ModerationEnabled = modEnabled;
                        }
                    }
                    else if (BlogMLBlogExtendedProperties.EnableSendingTrackbacks.Equals(extProp.Key))
                    {
                        bool tracksEnabled;

                        if (bool.TryParse(extProp.Value, out tracksEnabled))
                        {
                            Blog.TrackbacksEnabled = tracksEnabled;
                        }
                    }
                }

                Repository.UpdateBlog(Blog);
            }
        }

        public IDisposable SetupBlogForImport()
        {
            return new BlogImportSetup(Blog, Repository);
        }

        /// <summary>
        /// The physical path to the attachment directory.
        /// </summary>
        /// <remarks>
        /// The attachment is passed in to give the blog engine 
        /// the opportunity to use attachment specific directories 
        /// (ex. based on mime type) should it choose.
        /// </remarks>
        public string GetAttachmentDirectoryPath()
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
        public string GetAttachmentDirectoryUrl()
        {
            return Url.ImageDirectoryUrl(Blog);
        }
    }
}
