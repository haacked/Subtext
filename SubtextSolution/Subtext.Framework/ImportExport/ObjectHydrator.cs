using System;
using System.Data;
using System.Globalization;
using BlogML.Xml;
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
			BlogMLPost post = new BlogMLPost();
			post.ID = entry.Id.ToString(CultureInfo.InvariantCulture);
			post.Title = entry.Title;
			post.PostUrl = entry.FullyQualifiedUrl.ToString();
			post.Approved = entry.IsActive;
			post.Content.Text = entry.Body;
			post.DateCreated = entry.DateCreated;
			post.DateModified = entry.DateModified;
			return post;
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
			comment.UserUrl = feedbackItem.SourceUrl.ToString();
			comment.UserEMail = feedbackItem.Email;
			comment.UserName = feedbackItem.Author;
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
			blogMLTrackback.Url = trackback.SourceUrl.ToString();
			return blogMLTrackback;
		}
		
		public static BlogMLCategory CreateCategoryInstance(string id, string title, string description, bool approved, string parentId, DateTime dateCreated, DateTime dateModified)
		{
			BlogMLCategory category = new BlogMLCategory();
			category.ID = id;
			category.Title = title;
			//category.TitleContentType = titleContentType;
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
			blog.Title = title;
			blog.SubTitle = subtitle;
			blog.RootUrl = rootUrl;
			blog.Author = new BlogMLAuthor();
			blog.Author.Name = author;
			blog.Author.Email = email;
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