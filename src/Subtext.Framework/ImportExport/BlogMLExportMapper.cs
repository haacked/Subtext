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
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using BlogML;
using BlogML.Xml;
using log4net;
using Subtext.Extensibility;
using Subtext.Framework;
using Subtext.Framework.Components;
using Subtext.Framework.Logging;
using Subtext.Framework.Properties;
using Subtext.Framework.Routing;
using Subtext.Framework.Text;
using Subtext.Framework.Web;

namespace Subtext.ImportExport
{
    public class BlogMLExportMapper : IBlogMLExportMapper
    {
        private readonly static ILog Log = new Log();

        public BlogMLExportMapper(ISubtextContext subtextContext)
        {
            SubtextContext = subtextContext;
            Blog = subtextContext.Blog;
            Url = subtextContext.UrlHelper;
        }

        protected ISubtextContext SubtextContext { get; private set; }
        protected Blog Blog { get; private set; }
        protected BlogUrlHelper Url { get; private set; }

        public BlogMLBlog ConvertBlog(Blog blog)
        {
            var bmlBlog = new BlogMLBlog
            {
                Title = blog.Title,
                SubTitle = blog.SubTitle,
                RootUrl = Url.BlogUrl().ToFullyQualifiedUrl(blog).ToString(),
                DateCreated = blog.DateCreatedUtc
            };

            PopulateAuthors(blog, bmlBlog);
            PopulateExtendedProperties(blog, bmlBlog);

            return bmlBlog;
        }

        public IEnumerable<BlogMLCategory> ConvertCategories(IEnumerable<LinkCategory> categories)
        {
            var blogCategories = from category in categories
                                 select new BlogMLCategory
                                 {
                                     Title = category.Title,
                                     Approved = category.IsActive,
                                     Description = category.Description,
                                     ID = category.Id.ToString(CultureInfo.InvariantCulture),
                                 };
            return blogCategories;
        }

        public BlogMLPost ConvertEntry(EntryStatsView entry, bool embedAttachments)
        {
            string postUrl = null;
            var entryVirtualPath = Url.EntryUrl(entry);
            if (entryVirtualPath != null)
            {
                postUrl = entryVirtualPath.ToFullyQualifiedUrl(Blog).ToString();
            }
            var post = new BlogMLPost
            {
                Title = entry.Title,
                PostUrl = postUrl,
                PostType = (entry.PostType == PostType.Story) ? BlogPostTypes.Article : BlogPostTypes.Normal,
                Approved = entry.IsActive,
                Content = BlogMLContent.Create(entry.Body ?? string.Empty, ContentTypes.Base64),
                HasExcerpt = entry.HasDescription,
                Excerpt = BlogMLContent.Create(entry.Description ?? string.Empty, ContentTypes.Base64),
                DateCreated = entry.DateCreatedUtc,
                DateModified = entry.IsActive ? entry.DatePublishedUtc : entry.DateModifiedUtc,
                Views = (uint)entry.WebCount
            };

            if (entry.HasEntryName)
            {
                post.PostName = entry.EntryName;
            }

            // When we support multiple authors, this will have to change
            post.Authors.Add(Blog.Id.ToString(CultureInfo.InvariantCulture));
            post.Attachments.AddRange(GetPostAttachments(entry.Body, embedAttachments).ToArray());
            var comments = (from c in entry.Comments where c.FeedbackType == FeedbackType.Comment select ConvertComment(c)).ToList();
            if (comments.Count > 0)
            {
                post.Comments.AddRange(comments);
            }
            var trackbacks = (from c in entry.Comments where c.FeedbackType == FeedbackType.PingTrack select ConvertTrackback(c)).ToList();
            if (trackbacks.Count > 0)
            {
                post.Trackbacks.AddRange(trackbacks);
            }
            return post;
        }

        public IEnumerable<BlogMLAttachment> GetPostAttachments(string body, bool embedAttachments)
        {
            IEnumerable<string> attachmentUrls = body.GetAttributeValues("img", "src");

            foreach (string attachmentUrl in attachmentUrls)
            {
                string blogHostUrl = ("http://" + Blog.Host + "/").ToLowerInvariant();
                string attachmentUrlLowerCase = attachmentUrl.ToLowerInvariant();
                // If the URL for the attachment is local then we'll want to build a new BlogMLAttachment 
                // add add it to the list of attachments for this post.
                if (!attachmentUrlLowerCase.StartsWith("http") || attachmentUrlLowerCase.StartsWith(blogHostUrl))
                {
                    yield return GetAttachment(blogHostUrl, attachmentUrl, attachmentUrlLowerCase, embedAttachments);
                }
            }
        }

        private BlogMLAttachment GetAttachment(string blogHostUrl, string attachmentUrl, string attachmentUrlLowerCase, bool embed)
        {
            string attachVirtualPath = attachmentUrlLowerCase.Replace(blogHostUrl, "/");

            var attachment = new BlogMLAttachment
            {
                Embedded = embed,
                MimeType = attachmentUrl.GetMimeType(),
                Path = attachVirtualPath,
                Url = attachmentUrl
            };

            if (embed)
            {
                try
                {
                    SetAttachmentData(attachVirtualPath, attachment);
                }
                catch (FileNotFoundException e)
                {
                    Log.Error("The attachment we wish to embed was not found", e);
                    attachment.Embedded = false;
                }
            }
            return attachment;
        }

        private void SetAttachmentData(string attachVirtualPath, BlogMLAttachment attachment)
        {
            string attachPhysicalPath = HttpUtility.UrlDecode(SubtextContext.HttpContext.Server.MapPath(attachVirtualPath));

            using (FileStream attachStream = File.OpenRead(attachPhysicalPath))
            {
                using (var reader = new BinaryReader(attachStream))
                {
                    reader.BaseStream.Position = 0;
                    byte[] data = reader.ReadBytes((int)attachStream.Length);
                    attachment.Data = data;
                }
            }
        }

        private static void PopulateExtendedProperties(Blog blog, BlogMLBlog bmlBlog)
        {
            var bmlExtProp = new Pair<string, string>
            {
                Key = BlogMLBlogExtendedProperties.CommentModeration,
                Value = blog.ModerationEnabled
                            ? CommentModerationTypes.Enabled.ToString()
                            : CommentModerationTypes.Disabled.ToString()
            };
            bmlBlog.ExtendedProperties.Add(bmlExtProp);

            bmlExtProp.Key = BlogMLBlogExtendedProperties.EnableSendingTrackbacks;
            bmlExtProp.Value = blog.TrackbacksEnabled
                                   ? SendTrackbackTypes.Yes.ToString()
                                   : SendTrackbackTypes.No.ToString();
        }

        private void PopulateAuthors(Blog blog, BlogMLBlog bmlBlog)
        {
            var bmlAuthor = new BlogMLAuthor
            {
                ID = blog.Id.ToString(CultureInfo.InvariantCulture),
                Title = blog.Author,
                Approved = true,
                Email = blog.Email,
                DateCreated = blog.DateModifiedUtc,
                DateModified = blog.DateModifiedUtc
            };
            bmlBlog.Authors.Add(bmlAuthor);
        }


        public BlogMLComment ConvertComment(FeedbackItem feedbackItem)
        {
            if (feedbackItem == null)
            {
                throw new ArgumentNullException("feedbackItem");
            }
            if (feedbackItem.FeedbackType != FeedbackType.Comment)
            {
                throw new ArgumentException(String.Format(Resources.ArgumentException_CommentTypeMismatch, feedbackItem.FeedbackType, FeedbackType.Comment), "feedbackItem");
            }

            return new BlogMLComment
            {
                ID = feedbackItem.Id.ToString(CultureInfo.InvariantCulture),
                Title = feedbackItem.Title,
                UserUrl = feedbackItem.SourceUrl != null ? feedbackItem.SourceUrl.ToString() : null,
                UserEMail = feedbackItem.Email,
                UserName = feedbackItem.Author,
                Approved = feedbackItem.Approved,
                Content = BlogMLContent.Create(feedbackItem.Body ?? string.Empty, ContentTypes.Base64),
                DateCreated = feedbackItem.DateCreatedUtc,
                DateModified = feedbackItem.DateModifiedUtc
            };
        }

        public BlogMLTrackback ConvertTrackback(FeedbackItem trackback)
        {
            if (trackback == null)
            {
                throw new ArgumentNullException("trackback");
            }
            if (trackback.FeedbackType != FeedbackType.PingTrack)
            {
                throw new ArgumentException(String.Format(Resources.ArgumentException_CommentTypeMismatch, trackback.FeedbackType, FeedbackType.PingTrack), "trackback");
            }

            return new BlogMLTrackback
            {
                ID = trackback.Id.ToString(CultureInfo.InvariantCulture),
                Url = trackback.SourceUrl != null ? trackback.SourceUrl.ToString() : null,
                Title = trackback.Title,
                Approved = trackback.Approved,
                DateCreated = trackback.DateCreatedUtc,
                DateModified = trackback.DateModifiedUtc
            };
        }
    }
}