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

namespace Subtext.Framework.Components
{
    public class BlogStatistics
    {
        public int ActivePostCount { get; set; }
        public int DraftPostCount { get; set; }
        public int ActiveArticleCount { get; set; }
        public int DraftArticleCount { get; set; }
        public int FeedbackCount { get; set; }
        public int ApprovedFeedbackCount { get; set; }
        public int ApprovedTrackbackCount { get; set; }
        public int SpamFalsePositiveFeedbackCount { get; set; }
        public int SpamFalsePositiveTrackbackCount { get; set; }
        public int AwaitingModerationFeedbackCount { get; set; }
        public int AwaitingModerationTrackbackCount { get; set; }
        public int FlaggedAsSpamFeedbackCount { get; set; }
        public int FlaggedAsSpamTrackbackCount { get; set; }
        public int DeletedFeedbackCount { get; set; }
        public int DeletedTrackbackCount { get; set; }
        public int DeletedSpamFeedbackCount { get; set; }
        public int DeletedSpamTrackbackCount { get; set; }
        public int AverageCommentsPerPost { get; set; }
        public int AveragePostsPerMonth { get; set; }
        public int AveragePostsPerWeek { get; set; }
        public int AverageCommentsPerMonth { get; set; }
    }
}