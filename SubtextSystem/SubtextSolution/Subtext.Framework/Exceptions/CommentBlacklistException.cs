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

		/// <summary>
		/// Returns a resource key for the message.  This is used to 
		/// look up the message in the correct language within a 
		/// resource file (when we get around to I8N).
		/// </summary>
		/// <value></value>
		public override string MessageResourceKey
		{
			get { throw new NotImplementedException(); }
		}
	}
}
