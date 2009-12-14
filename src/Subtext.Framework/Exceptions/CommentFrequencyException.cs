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

using Subtext.Framework.Properties;

namespace Subtext.Framework.Exceptions
{
    /// <summary>
    /// Exception thrown when comments are posted too frequently.
    /// </summary>
    public class CommentFrequencyException : BaseCommentException
    {
        public CommentFrequencyException(int commentDelayInMinutes)
        {
            CommentDelayInMinutes = commentDelayInMinutes;
        }

        public int CommentDelayInMinutes
        {
            get; 
            private set;
        }

        /// <summary>
        /// Gets the message.
        /// </summary>
        /// <value></value>
        public override string Message
        {
            get
            {
                string minutesText = CommentDelayInMinutes > 1 ? Resources.Minutes_Plural : Resources.Minutes_Singular;
                string message = string.Format(Resources.CommentFrequencyException_Message, CommentDelayInMinutes + " " + minutesText);
                return message;
            }
        }
    }
}