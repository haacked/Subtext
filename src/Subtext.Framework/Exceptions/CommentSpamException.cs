#region Disclaimer/Info

///////////////////////////////////////////////////////////////////////////////////////////////////
// Subtext WebLog
// 
// Subtext is an open source weblog system that is a fork of the .TEXT
// weblog system.
//
// For updated news and information please visit http://subtextproject.com/
// Subtext is hosted at Google Code at http://code.google.com/p/subtext/
// The development mailing list is at subtext@googlegroups.com 
//
// This project is licensed under the BSD license.  See the License.txt file for more information.
///////////////////////////////////////////////////////////////////////////////////////////////////

#endregion

using System;

namespace Subtext.Framework.Exceptions
{
    /// <summary>
    /// Exception thrown when a comment is identified as comment spam.
    /// </summary>
    /// <summary>
    /// Exception thrown when DESCRIPTION
    /// </summary>
    /// <remarks>
    /// Contains a custom property, thus it Implements ISerializable 
    /// and the special serialization constructor.
    /// </remarks>
    [Serializable]
    public sealed class CommentSpamException : BaseCommentException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CommentSpamException"/> class.
        /// </summary>
        public CommentSpamException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommentSpamException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public CommentSpamException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommentSpamException"/> class.
        /// </summary>
        /// <param name="innerException">The inner exception.</param>
        public CommentSpamException(Exception innerException) : base(null, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommentSpamException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        public CommentSpamException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}