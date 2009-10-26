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

namespace Subtext.Framework.Configuration
{
    /// <summary>
    /// <p>Bitmask enumeration used to specify the several properties in one 
    /// value within the database.</p>
    /// </list>
    /// </summary>
    [Flags]
    public enum ConfigurationFlags
    {
        /// <summary>Nothing is set</summary>
        None = 0,
        /// <summary>The Blog is Active</summary>
        IsActive = 1,
        /// <summary>The Blog has a syndicated feed (RSS or ATOM)</summary>
        IsAggregated = 2,
        /// <summary>The Blog can be accessed via XML over HTTP APIs</summary>
        EnableServiceAccess = 4,
        /// <summary>Whether or not the password is hashed.</summary>
        IsPasswordHashed = 8,
        /// <summary>Whether or not Comments are enabled.</summary>
        CommentsEnabled = 16,
        /// <summary>Whether or not trackbacks and pingbacks are enabled.</summary>
        TrackbacksEnabled = 32,
        /// <summary>The Blog compresses its syndicated feeds.</summary>
        CompressSyndicatedFeed = 64,
        /// <summary>Whether or not duplicate comments are allowed.</summary>
        DuplicateCommentsEnabled = 128,
        /// <summary>
        /// Whether or not <see href="http://bobwyman.pubsub.com/main/2004/09/using_rfc3229_w.html">RFC3229 for feeds</see>
        /// is enabled. Enabling this can save on bandwidth by providing just updated posts in the 
        /// RSS feed.
        /// </summary>
        RFC3229DeltaEncodingEnabled = 256,
        /// <summary>
        /// Whether or not titles of blog posts and articles automatically have a friendly url generated.
        /// </summary>
        AutoFriendlyUrlEnabled = 512,
        /// <summary>Whether or not coComment is enabled</summary>
        CoCommentEnabled = 1024,
        /// <summary>The blog allows for comment moderation.</summary>
        CommentModerationEnabled = 2048,
        /// <summary>CAPTCHA is enabled on comment forms.</summary>
        CaptchaEnabled = 4096,
        /// <summary>Comment notification mails are enabled.</summary>
        CommentNotificationEnabled = 8192,
        /// <summary>Trackback notification mails are enabled.</summary>
        TrackbackNotificationEnabled = 16384,
        /// <summary>Show blog author email address in rss feed</summary>
        ShowAuthorEmailAddressinRss = 32768,
    } ;
}