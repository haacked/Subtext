using System;
using BlogML;

namespace Subtext.BlogMl.Interfaces
{
	public interface IBlogMlCategory
	{
		/// <summary>
		/// Id of the category.
		/// </summary>
		string Id { get;}

		/// <summary>
		/// Title of the category.
		/// </summary>
		string Title { get;}

		/// <summary>
		/// Content type of the title.
		/// </summary>
		ContentTypes TitleContentType { get;}

		/// <summary>
		/// The date the category was created.
		/// </summary>
		DateTime DateCreated { get;}

		/// <summary>
		/// The date the category was modified.
		/// </summary>
		DateTime DateModified { get;}

		/// <summary>
		/// Whether or not the category is active.
		/// </summary>
		bool Approved { get;}

		/// <summary>
		/// Description of the category.
		/// </summary>
		string Description { get;}

		/// <summary>
		/// Parent category (or other such parent) if any.
		/// </summary>
		string ParentId { get;}
	}
}
