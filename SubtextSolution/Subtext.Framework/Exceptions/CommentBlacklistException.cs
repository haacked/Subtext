#region Disclaimer/Info
///////////////////////////////////////////////////////////////////////////////////////////////////
// Subtext WebLog
// 
// Subtext is an open source weblog system that is a fork of the .TEXT
// weblog system.
//
// For updated news and information please visit http://subtextproject.com/
// Subtext is hosted at SourceForge at http://sourceforge.net/projects/subtext
// The development mailing list is at subtext-devs@lists.sourceforge.net 
//
// This project is licensed under the BSD license.  See the License.txt file for more information.
///////////////////////////////////////////////////////////////////////////////////////////////////
#endregion

using System;

namespace Subtext.Framework.Exceptions
{
	/// <summary>
	/// Exception thrown when a commenter has been blacklisted
	/// </summary>
	/// <remarks>
	/// This exception does not have any custom properties, 
	/// thus it does not implement ISerializable.
	/// </remarks>
	[Serializable]
	public class CommentBlackListException : BaseCommentException
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="CommentBlackListException"/> class.
		/// </summary>
		public CommentBlackListException() : base()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="CommentBlackListException"/> class.
		/// </summary>
		/// <param name="message">The message.</param>
		public CommentBlackListException(string message) : base(message)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="CommentBlackListException"/> class.
		/// </summary>
		/// <param name="message">The message.</param>
		/// <param name="innerException">The inner exception.</param>
		public CommentBlackListException(string message, Exception innerException) : base(message, innerException)
		{
		}
	}
}
