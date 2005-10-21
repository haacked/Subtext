using System;

namespace Subtext.Framework.Exceptions
{
	/// <summary>
	/// Base exception class for comment errors.
	/// </summary>
	[Serializable]
	public abstract class BaseCommentException : BaseSubtextException
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="CommentSpamException"/> class.
		/// </summary>
		public BaseCommentException() : base()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="CommentSpamException"/> class.
		/// </summary>
		/// <param name="message">The message.</param>
		public BaseCommentException(string message) : base(message)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="BaseCommentException"/> class.
		/// </summary>
		/// <param name="innerException">The inner exception.</param>
		public BaseCommentException(Exception innerException) : base(innerException)
		{}

		/// <summary>
		/// Initializes a new instance of the <see cref="CommentSpamException"/> class.
		/// </summary>
		/// <param name="message">The message.</param>
		/// <param name="innerException">The inner exception.</param>
		public BaseCommentException(string message, Exception innerException) : base(message, innerException)
		{
		}
	}
}
