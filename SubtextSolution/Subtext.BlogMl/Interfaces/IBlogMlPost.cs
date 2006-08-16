using System;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace Subtext.BlogMl.Interfaces
{
	/// <summary>
	/// A single blog post or article.
	/// </summary>
	public interface IBlogMlPost : IBlogMlEntry
	{
		/// <summary>
		/// Description of the category.
		/// </summary>
		string Content { get;}

		/// <summary>
		/// Returns all the category references (ids) for the specified blog 
		/// post. We don't need the actual category here.
		/// </summary>
		StringCollection CategoryIds {get;}

		/// <summary>
		/// Returns all the comments for the specified blog post.
		/// </summary>
		ICollection<IBlogMlComment> Comments {get;}

		/// <summary>
		/// Returns all the trackbacks/pingbacks for the post.
		/// </summary>
		ICollection<IBlogMlTrackback> Trackbacks { get; }
	}
}
