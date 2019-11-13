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

using Subtext.Framework.Components;

namespace Subtext.Framework.Services
{
    /// <summary>
    /// Base interface for comment spam services such as Akismet.
    /// </summary>
    public interface ICommentSpamService
    {
        /// <summary>
        /// Examines the item and determines whether or not it is spam.
        /// </summary>
        /// <param name="feedback"></param>
        /// <returns></returns>
        bool IsSpam(FeedbackItem feedback);

        /// <summary>
        /// Submits the item to the service as a false positive. 
        /// Something that should not have been marked as spam.
        /// </summary>
        /// <param name="feedback"></param>
        void SubmitGoodFeedback(FeedbackItem feedback);

        /// <summary>
        /// Submits the item to the service as a piece of SPAM that got through 
        /// the filter. Something that should've been marked as SPAM.
        /// </summary>
        /// <param name="feedback"></param>
        void SubmitSpam(FeedbackItem feedback);
    }
}