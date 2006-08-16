using System;
using BlogML;
using Subtext.BlogMl.Interfaces;

namespace Subtext.BlogMl
{
	/// <summary>
	/// Represents a trackback.
	/// </summary>
	public class BlogMlTrackback : BlogMlEntry, IBlogMlTrackback
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="BlogMlTrackback"/> class.
		/// </summary>
		/// <param name="id">The id.</param>
		/// <param name="title">The title.</param>
		/// <param name="titleContentType">Type of the title content.</param>
		/// <param name="url">The URL.</param>
		/// <param name="approved">if set to <c>true</c> [approved].</param>
		/// <param name="dateCreated">The date created.</param>
		/// <param name="dateModified">The date modified.</param>
		public BlogMlTrackback(string id, string title, ContentTypes titleContentType, string url, bool approved, DateTime dateCreated, DateTime dateModified) : base(id, title, titleContentType, url, approved, dateCreated, dateModified)
		{
		}
	}
}
