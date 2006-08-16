using System;
using BlogML;

namespace Subtext.BlogMl.Interfaces
{
	public interface IBlogMlBlog
	{
		/// <summary>
		/// Title of the blog.
		/// </summary>
		string Title { get;}

		/// <summary>
		/// Content type of the title.
		/// </summary>
		ContentTypes TitleContentType { get;}
		
		/// <summary>
		/// Subtitle for the blog.
		/// </summary>
		string Subtitle { get;}

		/// <summary>
		/// Content type of the title.
		/// </summary>
		ContentTypes SubTitleContentType { get;}

		/// <summary>
		/// Root URL for the blog.
		/// </summary>
		string RootUrl { get;}

		/// <summary>
		/// The author of the blog.
		/// </summary>
		string Author { get;}

		/// <summary>
		/// Email of the blog owner.
		/// </summary>
		string Email { get;}

		/// <summary>
		/// The date the blog was created.
		/// </summary>
		DateTime DateCreated { get;}
	}
}
