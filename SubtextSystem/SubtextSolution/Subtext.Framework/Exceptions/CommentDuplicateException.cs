using System;

namespace Subtext.Framework.Exceptions
{
	/// <summary>
	/// Exception thrown when a duplicate comment occurs, but duplicates are not allowed.
	/// </summary>
	public class CommentDuplicateException : BaseCommentException
	{
		/// <summary>
		/// Gets the message resource key.
		/// </summary>
		/// <value></value>
		public override string MessageResourceKey
		{
			get { throw new NotImplementedException(); }
		}

		/// <summary>
		/// Gets the message.
		/// </summary>
		/// <value></value>
		public override string Message
		{
			get
			{
				return "Sorry, but this comment is a duplicate of another comment.  Duplicate comments are not allowed.";
			}
		}

	}
}
