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
            get { return "Sorry, but this comment is a duplicate of another comment.  Duplicate comments are not allowed."; }
        }
    }
}