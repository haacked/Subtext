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
using System.Data;
using System.Globalization;
using BlogML;
using BlogML.Xml;
using Subtext.Extensibility;
using Subtext.Framework.Components;
using Subtext.Framework.Data;

namespace Subtext.ImportExport
{
	/// <summary>
	/// Class used to hydrade blogml objects from a data reader.
	/// </summary>
	public static class ObjectHydrator
	{
		/// <summary>
		/// Loads the post from data reader.
		/// </summary>
		/// <param name="reader">The reader.</param>
		/// <returns></returns>
		public static BlogMLPost LoadPostFromDataReader(IDataReader reader)
		{
			Entry entry = DataHelper.LoadEntry(reader);
			BlogMLPost bmlPost = new BlogMLPost();
			bmlPost.ID = entry.Id.ToString(CultureInfo.InvariantCulture);
			bmlPost.Title = entry.Title;
			bmlPost.PostUrl = entry.FullyQualifiedUrl.ToString();
			bmlPost.Approved = entry.IsActive;
			bmlPost.Content.Text = entry.Body;
			bmlPost.DateCreated = entry.DateCreated;
			bmlPost.DateModified = entry.DateModified;
            bmlPost.PostType = (entry.PostType == PostType.Story) ? BlogPostTypes.Article : BlogPostTypes.Normal;
            bmlPost.Views = 0; // I think we have this statistic in the db... right?

			if (entry.HasEntryName)
			{
				bmlPost.PostName = entry.EntryName;
			}

			bmlPost.HasExcerpt = entry.HasDescription;
			if (entry.HasDescription)
			{
				bmlPost.Excerpt.Text = entry.Description;
			}

			return bmlPost;
		}

		/// <summary>
		/// Loads the comment from data reader.
		/// </summary>
		/// <param name="reader">The reader.</param>
		/// <returns></returns>
		public static BlogMLComment LoadCommentFromDataReader(IDataReader reader)
		{
			FeedbackItem feedbackItem = DataHelper.LoadFeedbackItem(reader);
			BlogMLComment comment = new BlogMLComment();
			comment.ID = feedbackItem.Id.ToString(CultureInfo.InvariantCulture);
			comment.Title = feedbackItem.Title;
			comment.Approved = feedbackItem.Approved;
			comment.Content.Text = feedbackItem.Body;
			comment.DateCreated = feedbackItem.DateCreated;
			comment.DateModified = feedbackItem.DateModified;
			comment.UserEMail = feedbackItem.Email;
			comment.UserName = feedbackItem.Author;
            if (feedbackItem.SourceUrl != null)
            {
                comment.UserUrl = feedbackItem.SourceUrl.ToString();
            }

			return comment;
		}

		/// <summary>
		/// Loads the trackback from data reader.
		/// </summary>
		/// <param name="reader">The reader.</param>
		/// <returns></returns>
		public static BlogMLTrackback LoadTrackbackFromDataReader(IDataReader reader)
		{
			FeedbackItem trackback = DataHelper.LoadFeedbackItem(reader);
			BlogMLTrackback blogMLTrackback = new BlogMLTrackback();
			blogMLTrackback.ID = trackback.Id.ToString(CultureInfo.InvariantCulture);
			blogMLTrackback.Title = trackback.Title;
			blogMLTrackback.Approved = trackback.Approved;
			blogMLTrackback.DateCreated = trackback.DateCreated;
			blogMLTrackback.DateModified = trackback.DateModified;
            if (trackback.SourceUrl != null)
            {
                blogMLTrackback.Url = trackback.SourceUrl.ToString();
            }

			return blogMLTrackback;
		}

		public static BlogMLCategory CreateCategoryInstance(string id, string title, string description, bool approved, string parentId, DateTime dateCreated, DateTime dateModified)
		{
			BlogMLCategory category = new BlogMLCategory();
			category.ID = id;
			category.Title = title;
			category.Description = description;
			category.Approved = approved;
			category.ParentRef = parentId;
			category.DateCreated = dateCreated;
			category.DateModified = dateModified;
			return category;
		}

		public static BlogMLBlog CreateBlogInstance(string title, string subtitle, string rootUrl, string author, string email, DateTime dateCreated)
		{
			BlogMLBlog blog = new BlogMLBlog();
			BlogMLAuthor blogAuthor = new BlogMLAuthor();
			blogAuthor.Title = author;
			blogAuthor.Email = email;
			blog.Authors.Add(blogAuthor);
			blog.Title = title;
			blog.SubTitle = subtitle;
			blog.RootUrl = rootUrl;
			blog.DateCreated = dateCreated;
			return blog;
		}

		public static BlogMLPost CreatePostInstance(string id, string title, string url, bool approved, string content, DateTime dateCreated, DateTime dateModified)
		{
			BlogMLPost post = new BlogMLPost();
			post.ID = id;
			post.Title = title;
			post.PostUrl = url;
			post.Approved = approved;
			post.Content = new BlogMLContent();
			post.Content.Text = content;
			post.DateCreated = dateCreated;
			post.DateModified = dateModified;
			return post;
		}

		public static BlogMLComment CreateCommentInstance(string id, string title, string url, string content, string email, string userName, bool approved, DateTime dateCreated, DateTime dateModified)
		{
			BlogMLComment comment = new BlogMLComment();
			comment.ID = id;
			comment.Title = title;
			comment.UserUrl = url;
			comment.UserEMail = email;
			comment.UserName = userName;
			comment.Approved = approved;
			comment.Content = new BlogMLContent();
			comment.Content.Text = content;
			comment.DateCreated = dateCreated;
			comment.DateModified = dateModified;
			return comment;
		}

		public static BlogMLTrackback CreateTrackBackInstance(string id, string title, string url, bool approved, DateTime dateCreated, DateTime dateModified)
		{
			BlogMLTrackback trackback = new BlogMLTrackback();
			trackback.ID = id;
			trackback.Url = url;
			trackback.Title = title;
			trackback.Approved = approved;
			trackback.DateCreated = dateCreated;
			trackback.DateModified = dateModified;
			return trackback;
		}
	}
}
