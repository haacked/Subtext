using System;
using BlogML;
using Subtext.BlogMl.Interfaces;

namespace Subtext.BlogMl
{
	public class BlogMlBlog : IBlogMlBlog
	{
		private DateTime dateCreated;
		private string email;
		private string author;
		private string rootUrl;
		private ContentTypes subTitleContentType;
		private string subtitle;
		private ContentTypes titleContentType;
		private string title;

		/// <summary>
		/// Initializes a new instance of the <see cref="BlogMlBlog"/> class.
		/// </summary>
		/// <param name="title">The title.</param>
		/// <param name="titleContentType">Type of the title content.</param>
		/// <param name="subtitle">The subtitle.</param>
		/// <param name="subtitleContentType">Type of the subtitle content.</param>
		/// <param name="rootUrl">The root URL.</param>
		/// <param name="author">The author.</param>
		/// <param name="email">The email.</param>
		/// <param name="dateCreated">The date created.</param>
		public BlogMlBlog(string title, ContentTypes titleContentType, string subtitle, ContentTypes subtitleContentType, string rootUrl, string author, string email, DateTime dateCreated)
		{
			this.title = title;
			this.titleContentType = titleContentType;
			this.subtitle = subtitle;
			this.subTitleContentType = subtitleContentType;
			this.rootUrl = rootUrl;
			this.author = author;
			this.email = email;
			this.dateCreated = dateCreated;
		}
		
		/// <summary>
		/// Title of the blog.
		/// </summary>
		public string Title
		{
			get { return this.title; }
		}

		/// <summary>
		/// Content type of the title.
		/// </summary>
		public ContentTypes TitleContentType
		{
			get { return this.titleContentType; }
		}

		/// <summary>
		/// Subtitle for the blog.
		/// </summary>
		public string Subtitle
		{
			get { return this.subtitle; }
		}

		/// <summary>
		/// Content type of the title.
		/// </summary>
		public ContentTypes SubTitleContentType
		{
			get { return this.subTitleContentType; }
		}

		/// <summary>
		/// Root URL for the blog.
		/// </summary>
		public string RootUrl
		{
			get { return this.rootUrl; }
		}

		/// <summary>
		/// The author of the blog.
		/// </summary>
		public string Author
		{
			get { return this.author; }
		}

		/// <summary>
		/// Email of the blog owner.
		/// </summary>
		public string Email
		{
			get { return this.email; }
		}

		/// <summary>
		/// The date the blog was created.
		/// </summary>
		public DateTime DateCreated
		{
			get { return this.dateCreated; }
		}
	}
}
