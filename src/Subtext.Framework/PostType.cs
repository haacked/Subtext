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

namespace Subtext.Extensibility
{
    /// <summary>
    /// Enumerates the various types of entries within the 
    /// subtext_Content table.  A record in that table 
    /// might be any one of the following enumerations.
    /// </summary>
    public enum PostType
    {
        None = 0,
        BlogPost = 1,
        Story = 2,
    }

    /// <summary>
    /// Enumates the various types of comments within the subtext content table.
    /// </summary>
    public enum FeedbackType
    {
        None = 0,
        Comment = 1,
        PingTrack = 2,
        ContactPage = 3, //Only applies if "ContactToFeedback" is set to true.
    }
}