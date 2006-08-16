using System;
using BlogML;

namespace Subtext.BlogMl.Interfaces
{
	public interface IBlogMlComment : IBlogMlEntry
	{
		/// <summary>
		/// The user entering the comment.
		/// </summary>
		string UserName { get;}

		/// <summary>
		/// The email address of the user leaving the comment.
		/// </summary>
		string Email { get;}
	
		/// <summary>
		/// The content type of the comment.
		/// </summary>
		ContentTypes CommentContentType { get;}

		/// <summary>
		/// Content of the comment.
		/// </summary>
		string Content { get; }
	}
}
