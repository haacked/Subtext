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
using Subtext.Framework.Configuration;
using System.Runtime.Serialization;

namespace Subtext.Framework.Exceptions
{
	/// <summary>
	/// Exception thrown when comments are posted too frequently.
	/// </summary>
	[Serializable]
	public class CommentFrequencyException : BaseCommentException
	{
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

		public CommentFrequencyException() : base() {}
		public CommentFrequencyException(string message) : base(message) {}
		public CommentFrequencyException(string message, Exception innerException) : base(message, innerException) {}
		protected CommentFrequencyException(SerializationInfo info, StreamingContext context) : base(info, context) { }

	}
}
