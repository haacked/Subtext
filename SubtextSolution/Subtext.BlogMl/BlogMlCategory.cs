using System;
using BlogML;
using Subtext.BlogMl.Interfaces;

namespace Subtext.BlogMl
{
	public class BlogMlCategory : IBlogMlCategory
	{
		private string parentId;
		private string description;
		private bool approved;
		private DateTime dateModified;
		private DateTime dateCreated;
		private ContentTypes titleContentType;
		private string title;
		private string id;

		public BlogMlCategory(string id, string title, ContentTypes titleContentType, string description, bool approved, string parentId, DateTime dateCreated, DateTime dateModified)
		{
			this.id = id;
			this.title = title;
			this.titleContentType = titleContentType;
			this.description = description;
			this.approved = approved;
			this.parentId = parentId;
			this.dateCreated = dateCreated;
			this.dateModified = dateModified;
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
		/// Description of the category.
		/// </summary>
		public string Description
		{
			get { return this.description; }
		}

		/// <summary>
		/// Parent category (or other such parent) if any.
		/// </summary>
		public string ParentId
		{
			get { return this.parentId; }
		}
	}
}
