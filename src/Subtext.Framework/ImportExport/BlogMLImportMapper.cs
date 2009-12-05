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
using BlogML;
using BlogML.Xml;
using Subtext.Extensibility;
using Subtext.Framework.Components;
using Subtext.Framework.Text;
using Subtext.Framework;

namespace Subtext.ImportExport
{
    public class BlogMLImportMapper : IBlogMLImportMapper
    {
        private const int MaxCategoryTitleLength = 150;
        private const int AuthorTitleMaxLength = 50;
        private const int BlogPostTitleMaxLength = 255;

        public Entry ConvertBlogPost(BlogMLPost post, BlogMLBlog blogMLBlog, Blog blog)
        {
            DateTime dateModified = blog != null ? blog.TimeZone.FromUtc(post.DateModified) : post.DateModified;
            DateTime dateCreated = blog != null ? blog.TimeZone.FromUtc(post.DateCreated) : post.DateCreated;

            var newEntry = new Entry((post.PostType == BlogPostTypes.Article) ? PostType.Story : PostType.BlogPost)
            {
                Title = GetTitleFromPost(post).Left(BlogPostTitleMaxLength),
                DateCreated = dateCreated,
                DateModified = dateModified,
                DateSyndicated = post.Approved ? dateModified : DateTime.MaxValue,
                Body = post.Content.UncodedText,
                IsActive = post.Approved,
                DisplayOnHomePage = post.Approved,
                IncludeInMainSyndication = post.Approved,
                IsAggregated = post.Approved,
                AllowComments = true,
                Description = post.HasExcerpt ? post.Excerpt.UncodedText: null
            };

            if(!string.IsNullOrEmpty(post.PostName))
            {
                newEntry.EntryName = post.PostName;
            }
            else
            {
                SetEntryNameForBlogspotImport(post, newEntry);
            }

            SetEntryAuthor(post, newEntry, blogMLBlog);

            SetEntryCategories(post, newEntry, blogMLBlog);
            return newEntry;
        }

        private void SetEntryCategories(BlogMLPost post, Entry newEntry, BlogMLBlog blog)
        {
            if(post.Categories.Count > 0)
            {
                foreach(BlogMLCategoryReference categoryRef in post.Categories)
                {
                    string categoryTitle = GetCategoryTitleById(categoryRef.Ref, blog.Categories);
                    if(categoryTitle != null)
                    {
                        newEntry.Categories.Add(categoryTitle);
                    }
                }
            }
        }

        private IDictionary<string, string> _categoryIdToTitleMap;
        private string GetCategoryTitleById(string categoryId, IEnumerable<BlogMLCategory> categories)
        {
            if(_categoryIdToTitleMap == null)
            {
                _categoryIdToTitleMap = new Dictionary<string, string>();
                foreach(var category in categories)
                {
                    _categoryIdToTitleMap.Add(category.ID, category.Title);
                }
            }

            string title;
            _categoryIdToTitleMap.TryGetValue(categoryId, out title);
            return title;
        }

        private static void SetEntryAuthor(BlogMLPost post, Entry newEntry, BlogMLBlog blog)
        {
            if(post.Authors.Count > 0)
            {
                foreach(BlogMLAuthor author in blog.Authors)
                {
                    if(author.ID != post.Authors[0].Ref)
                    {
                        continue;
                    }
                    newEntry.Author = author.Title.Left(AuthorTitleMaxLength);
                    newEntry.Email = author.Email;
                    break;
                }
            }
        }

        private static string GetTitleFromPost(BlogMLPost blogPost)
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

        public LinkCategory ConvertCategory(BlogMLCategory category)
        {
            return new LinkCategory
            {
                Title = category.Title.Left(MaxCategoryTitleLength),
                Description = category.Description,
                IsActive = category.Approved,
                CategoryType = CategoryType.PostCollection
            };
        }

        public FeedbackItem ConvertComment(BlogMLComment comment, string parentPostId)
        {
            var feedback = new FeedbackItem(FeedbackType.Comment)
            {
                EntryId = int.Parse(parentPostId, CultureInfo.InvariantCulture),
                Title = comment.Title ?? string.Empty,
                DateCreated = comment.DateCreated,
                DateModified = comment.DateModified,
                Body = comment.Content.UncodedText ?? string.Empty,
                Approved = comment.Approved,
                Author = comment.UserName ?? string.Empty,
                Email = comment.UserEMail,
                SourceUrl = !String.IsNullOrEmpty(comment.UserUrl) ? ConvertUri(comment.UserUrl) : null
            };
            if(!feedback.Approved)
            {
                // Have to assume it needs moderation since that's what it most likely means in other blog systems;
                feedback.Status = FeedbackStatusFlag.NeedsModeration;
            }
            return feedback;
        }

        public FeedbackItem ConvertTrackback(BlogMLTrackback trackback, string parentPostId)
        {
            string author = null;
            Uri sourceUri = ConvertUri(trackback.Url);
            if(sourceUri != null)
            {
                author = sourceUri.Host;
            }
            return new FeedbackItem(FeedbackType.PingTrack)
            {
                EntryId = int.Parse(parentPostId, CultureInfo.InvariantCulture),
                Title = trackback.Title,
                SourceUrl = sourceUri,
                Approved = trackback.Approved,
                DateCreated = trackback.DateCreated,
                DateModified = trackback.DateModified,
                Author = author ?? string.Empty,
                Body = string.Empty
            };
        }

        private static Uri ConvertUri(string uriText)
        {
            Uri uri;
            if(!Uri.TryCreate(uriText, UriKind.Absolute, out uri))
            {
                return null;
            }
            return uri;
        }
    }
}
