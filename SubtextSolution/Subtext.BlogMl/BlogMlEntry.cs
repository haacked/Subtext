using System;
using BlogML;
using Subtext.BlogMl.Interfaces;

namespace Subtext.BlogMl
{
	public abstract class BlogMlEntry : IBlogMlEntry
	{
		private string url;
		private bool approved;
		private DateTime dateModified;
		private DateTime dateCreated;
		private ContentTypes titleContentType;
		private string title;
		private string id;

		/// <summary>
		/// Initializes a new instance of the <see cref="BlogMlEntry"/> class.
		/// </summary>
		/// <param name="id">The id.</param>
		/// <param name="title">The title.</param>
		/// <param name="titleContentType">Type of the title content.</param>
		/// <param name="url">The URL.</param>
		/// <param name="approved">if set to <c>true</c> [approved].</param>
		/// <param name="dateCreated">The date created.</param>
		/// <param name="dateModified">The date modified.</param>
		public BlogMlEntry(string id, string title, ContentTypes titleContentType, string url, bool approved, DateTime dateCreated, DateTime dateModified)
		{
			this.id = id;
			this.title = title;
			this.titleContentType = titleContentType;
			this.approved = approved;
			this.dateCreated = dateCreated;
			this.dateModified = dateModified;
			this.url = url;
		}
		
		/// <summary>
		/// Id of the category.
		/// </summary>
		public string Id
		{
			get { return this.id; }
		}

		/// <summary>
		/// Title of the category.
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
		/// The date the category was created.
		/// </summary>
		public DateTime DateCreated
		{
			get { return this.dateCreated; }
		}

		/// <summary>
		/// The date the category was modified.
		/// </summary>
		public DateTime DateModified
		{
			get { return this.dateModified; }
		}

		/// <summary>
		/// Whether or not the category is active.
		/// </summary>
		public bool Approved
		{
			get { return this.approved; }
		}

		/// <summary>
		/// The Url of the post.
		/// </summary>
		public string Url
		{
			get { return this.url; }
		}
	}
}
