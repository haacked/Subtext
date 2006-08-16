using System;
using BlogML;

namespace Subtext.BlogMl.Interfaces
{
	/// <summary>
	/// Interface that contains fields common to 
	/// <see cref="IBlogMlCategory" />, <see cref="IBlogMlComment" />, <see cref="IBlogMlPost" />
	/// </summary>
	public interface IBlogMlEntry
	{
		/// <summary>
		/// Id of the entry.
		/// </summary>
		string Id { get;}

		/// <summary>
		/// Title of the entry.
		/// </summary>
		string Title { get;}

		/// <summary>
		/// Content type of the title.
		/// </summary>
		ContentTypes TitleContentType { get;}

		/// <summary>
		/// The date the entry was created.
		/// </summary>
		DateTime DateCreated { get;}

		/// <summary>
		/// The date the entry was modified.
		/// </summary>
		DateTime DateModified { get;}

		/// <summary>
		/// Whether or not the entry is active.
		/// </summary>
		bool Approved { get;}

		/// <summary>
		/// The Url of the entry.
		/// </summary>
		string Url { get; }
	}
}
