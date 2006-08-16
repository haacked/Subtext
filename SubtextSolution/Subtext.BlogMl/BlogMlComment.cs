using System;
using BlogML;
using Subtext.BlogMl.Interfaces;

namespace Subtext.BlogMl
{
	public class BlogMlComment : BlogMlEntry, IBlogMlComment
	{
		private string content;
		private ContentTypes commentContentType;
		private string email;
		private string userName;

		public BlogMlComment(string id, string title, ContentTypes titleContentType, string url, string content, ContentTypes commentContentType, string email, string userName, bool approved, DateTime dateCreated, DateTime dateModified) : base(id, title, titleContentType, url, approved, dateCreated, dateModified)
		{
			this.content = content;
			this.commentContentType = commentContentType;
			this.email = email;
			this.userName = userName;
		}

		/// <summary>
		/// The user entering the comment.
		/// </summary>
		public string UserName
		{
			get { return this.userName; }
		}

		/// <summary>
		/// The email address of the user leaving the comment.
		/// </summary>
		public string Email
		{
			get { return this.email; }
		}

		/// <summary>
		/// The content type of the comment.
		/// </summary>
		public ContentTypes CommentContentType
		{
			get { return this.commentContentType; }
		}

		/// <summary>
		/// Description of the category.
		/// </summary>
		public string Content
		{
			get { return this.content; }
		}
	}
}
