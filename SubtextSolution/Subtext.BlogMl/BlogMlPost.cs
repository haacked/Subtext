using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using BlogML;
using Subtext.BlogMl.Interfaces;

namespace Subtext.BlogMl
{
	public class BlogMlPost : BlogMlEntry, IBlogMlPost
	{
		private ICollection<IBlogMlTrackback> trackbacks = new Collection<IBlogMlTrackback>();
		private ICollection<IBlogMlComment> comments = new Collection<IBlogMlComment>();
		private StringCollection categoryIds = new StringCollection();
		private string content;

		public BlogMlPost(string id, string title, ContentTypes titleContentType, string url, bool approved, string content, DateTime dateCreated, DateTime dateModified) : base(id, title, titleContentType, url, approved, dateCreated, dateModified)
		{
			this.content = content;
		}

		/// <summary>
		/// Description of the category.
		/// </summary>
		public string Content
		{
			get { return this.content; }
		}

		/// <summary>
		/// Returns all the categories for the specified blog post.
		/// </summary>
		public StringCollection CategoryIds
		{
			get { return this.categoryIds; }
		}

		/// <summary>
		/// Returns all the comments for the specified blog post.
		/// </summary>
		public ICollection<IBlogMlComment> Comments
		{
			get { return this.comments; }
		}

		/// <summary>
		/// Returns all the trackbacks/pingbacks for the post.
		/// </summary>
		public ICollection<IBlogMlTrackback> Trackbacks
		{
			get { return this.trackbacks; }
		}
	}
}
