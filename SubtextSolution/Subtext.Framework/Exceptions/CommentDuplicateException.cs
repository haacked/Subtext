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
using System.Runtime.Serialization;

namespace Subtext.Framework.Exceptions
{
	/// <summary>
	/// Exception thrown when a duplicate comment occurs, but duplicates are not allowed.
	/// </summary>
	[Serializable]
	public class CommentDuplicateException : BaseCommentException
	{
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

		public CommentDuplicateException() : base() {}
		public CommentDuplicateException(string message) : base(message) {}
		public CommentDuplicateException(string message, Exception innerException) : base(message, innerException) {}
		protected CommentDuplicateException(SerializationInfo info, StreamingContext context) : base(info, context) { }

	}
}
