using System;
using Subtext.Framework.Configuration;

namespace Subtext.Framework.Exceptions
{
	/// <summary>
	/// Exception thrown when comments are posted too frequently.
	/// </summary>
	public class CommentFrequencyException : BaseCommentException
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
				return "Sorry, but there is a delay between allowing comments originating from the same source. Please wait for " + Config.CurrentBlog.CommentDelayInMinutes  + " minutes and try again.";
			}
		}

	}
}
