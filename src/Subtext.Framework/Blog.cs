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
using System.Collections.Generic;
using Ninject;
using Subtext.Extensibility.Interfaces;
using Subtext.Framework.Configuration;
using Subtext.Framework.Properties;
using Subtext.Framework.Providers;
using Subtext.Framework.Text;
using Subtext.Framework.Util;
using Subtext.Framework.Web;
using Subtext.Infrastructure;

namespace Subtext.Framework
{
    /// <summary>
    /// Represents an instance of a blog.  This was formerly known as the BlogConfig class. 
    /// We are attempting to distinguish this from settings stored in web.config. This class 
    /// is persisted via a <see cref="ObjectProvider"/>.
    /// </summary>
    [Serializable]
    public class Blog
    {
        const string DefaultLanguage = "en-US";
        const int DefaultRecentCommentsLength = 50;
        private int _categoryListPostCount = 10;
        int _commentDelayInMinutes;
        string _feedbackSpamServiceKey;
        private string _host;
        private string _language = DefaultLanguage;
        string _languageCode;
        int _numberOfRecentComments;
        private string _password;
        int _recentCommentsLength;
        string _rssProxyUrl;
        private string _subfolder;
        ITimeZone _timeZone;
        string _timeZoneId;
        private string _username;

        [Inject]
        public Blog()
        {
            Id = NullValue.NullInt32;
            ItemCount = 25;
            Author = "Subtext Weblog";
            Flag = ConfigurationFlags.None;
            DaysTillCommentsClose = Int32.MaxValue;
        }

        public Blog(bool isAggregateBlog) : this()
        {
            IsAggregateBlog = isAggregateBlog;
        }

        public bool IsAggregateBlog { get; private set; }

        /// <summary>
        /// Gets or sets the date that the blog's configuration 
        /// was last updated.
        /// </summary>
        /// <value></value>
        public DateTime LastUpdated { get; set; }

        /// <summary>
        /// Gets or sets the ID of the blog.  This is the 
        /// primary key in the blog_config table.
        /// </summary>
        /// <value></value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the option to show the blog owners email address in rss feeds.
        /// </summary>
        public bool ShowEmailAddressInRss
        {
            get { return FlagPropertyCheck(ConfigurationFlags.ShowAuthorEmailAddressinRss); }
            set { FlagSetter(ConfigurationFlags.ShowAuthorEmailAddressinRss, value); }
        }

        /// <summary>
        /// Gets the time zone.
        /// </summary>
        /// <value>The time zone.</value>
        public virtual ITimeZone TimeZone
        {
            get
            {
                if(_timeZone == null)
                {
                    TimeZoneInfo timeZone = TimeZones.GetTimeZones().GetById(TimeZoneId) ?? TimeZoneInfo.Local;
                    _timeZone = new TimeZoneWrapper(timeZone);
                }
                return _timeZone;
            }
        }

        /// <summary>
        /// Gets or sets the time zone for the blogger.  
        /// </summary>
        /// <value></value>
        public string TimeZoneId
        {
            get { return _timeZoneId; }
            set
            {
                if(_timeZoneId != value)
                {
                    _timeZone = null;
                    _timeZoneId = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the count of posts displayed on the front page 
        /// of the blog.
        /// </summary>
        /// <value></value>
        public int ItemCount { get; set; }

        /// <summary>
        /// Gets or sets the count of posts displayed on the category pages. 
        /// </summary>
        /// <value></value>
        public int CategoryListPostCount
        {
            get { return _categoryListPostCount; }
            set
            {
                if(value < 0)
                {
                    value = 0;
                    //needed when upgrading from versions that did not have this column ("CategoryListPostCount") in the subtext_Config table.
                }
                _categoryListPostCount = value;
            }
        }

        /// <summary>
        /// Gets or sets the story count.
        /// </summary>
        /// <value></value>
        public int StoryCount { get; set; }

        /// <summary>
        /// Gets or sets the language the blog is in..
        /// </summary>
        /// <value></value>
        public string Language
        {
            get { return _language; }
            set
            {
                _language = value ?? DefaultLanguage;
                _languageCode = null;
            }
        }

        /// <summary>
        /// Gets the two (or three) letter language without the culture code.
        /// </summary>
        /// <value>The language sans culture.</value>
        public string LanguageCode
        {
            get
            {
                if(string.IsNullOrEmpty(_languageCode))
                {
                    //Just being paranoid in making this check.
                    if(_language == null)
                    {
                        _language = "en-US";
                    }
                    _languageCode = StringHelper.LeftBefore(_language, "-");
                }
                return _languageCode;
            }
        }

        /// <summary>
        /// Gets or sets the email of the blog owner.
        /// </summary>
        /// <value></value>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the host for the blog.  For 
        /// example, www.haacked.com might be a host.
        /// </summary>
        /// <value></value>
        public string Host
        {
            get { return _host; }
            set { _host = StripPortFromHost(value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this site can 
        /// be accessed via MetaBlogAPI, XML Web Services, etc..
        /// </summary>
        /// <value>
        /// 	<c>true</c> if the blog allow service access; otherwise, <c>false</c>.
        /// </value>
        public bool AllowServiceAccess
        {
            get { return FlagPropertyCheck(ConfigurationFlags.EnableServiceAccess); }
            set { FlagSetter(ConfigurationFlags.EnableServiceAccess, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether passwords are 
        /// stored in the database as cleartext or hashed.  If true, 
        /// passwords are hashed before storage.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if passwords are hashed; otherwise, <c>false</c>.
        /// </value>
        public bool IsPasswordHashed
        {
            get { return FlagPropertyCheck(ConfigurationFlags.IsPasswordHashed); }
            set { FlagSetter(ConfigurationFlags.IsPasswordHashed, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether syndicated feeds (such as 
        /// RSS or ATOM) are compressed.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if using compression, otherwise, <c>false</c>.
        /// </value>
        public bool UseSyndicationCompression
        {
            get { return FlagPropertyCheck(ConfigurationFlags.CompressSyndicatedFeed); }
            set { FlagSetter(ConfigurationFlags.CompressSyndicatedFeed, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this blog 
        /// contains some sort of feed (such as RSS or ATOM).
        /// </summary>
        /// <value>
        /// 	<c>true</c> if it is aggregated; otherwise, <c>false</c>.
        /// </value>
        public bool IsAggregated
        {
            get { return FlagPropertyCheck(ConfigurationFlags.IsAggregated); }
            set { FlagSetter(ConfigurationFlags.IsAggregated, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether comments are enabled.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if comments are enabled, otherwise, <c>false</c>.
        /// </value>
        public bool CommentsEnabled
        {
            get { return FlagPropertyCheck(ConfigurationFlags.CommentsEnabled); }
            set { FlagSetter(ConfigurationFlags.CommentsEnabled, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether comments are enabled.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if comments are enabled, otherwise, <c>false</c>.
        /// </value>
        public bool CoCommentsEnabled
        {
            get { return FlagPropertyCheck(ConfigurationFlags.CoCommentEnabled); }
            set { FlagSetter(ConfigurationFlags.CoCommentEnabled, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether blog posts and articles 
        /// have a friendly URL generated automatically from the title.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if comments are enabled, otherwise, <c>false</c>.
        /// </value>
        public bool AutoFriendlyUrlEnabled
        {
            get { return FlagPropertyCheck(ConfigurationFlags.AutoFriendlyUrlEnabled); }
            set { FlagSetter(ConfigurationFlags.AutoFriendlyUrlEnabled, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether trackbacks and pingbacks are enabled.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if comments are enabled, otherwise, <c>false</c>.
        /// </value>
        public bool TrackbacksEnabled
        {
            get { return FlagPropertyCheck(ConfigurationFlags.TrackbacksEnabled); }
            set { FlagSetter(ConfigurationFlags.TrackbacksEnabled, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether duplicate comments are enabled.  
        /// If not, duplicate comments are not allowed.
        /// </summary>
        /// <remarks>
        /// This may cause a problem with "me too!" comments.  
        /// If that is an issue, we can tweak this to only check 
        /// comments that are larger than a certain size.
        /// </remarks>
        /// <value>
        /// 	<c>true</c> if comments are enabled, otherwise, <c>false</c>.
        /// </value>
        public bool DuplicateCommentsEnabled
        {
            get { return FlagPropertyCheck(ConfigurationFlags.DuplicateCommentsEnabled); }
            set { FlagSetter(ConfigurationFlags.DuplicateCommentsEnabled, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether 
        /// <see href="http://bobwyman.pubsub.com/main/2004/09/using_rfc3229_w.html">RFC3229 for feeds</see> 
        /// delta encoding is enabled.
        /// </summary>
        /// <remarks>
        /// This can reduce bandwidth usage for RSS feeds.  When clients request a 
        /// feed using this protocol, only items that have not been sent to the client 
        /// already are sent.
        /// </remarks>
        /// <value>
        /// 	<c>true</c> if RFC3229 delta encoding is enabled.; otherwise, <c>false</c>.
        /// </value>
        public bool RFC3229DeltaEncodingEnabled
        {
            get { return FlagPropertyCheck(ConfigurationFlags.RFC3229DeltaEncodingEnabled); }
            set { FlagSetter(ConfigurationFlags.RFC3229DeltaEncodingEnabled, value); }
        }

        /// <summary>
        /// Gets or sets the days till comments close on a post.  
        /// The count starts when a post is created.
        /// </summary>
        /// <value></value>
        public int DaysTillCommentsClose { get; set; }

        /// <summary>
        /// Gets or sets the delay in minutes, between any two successive comments from 
        /// the same IP address.  This helps prevents comment bombing attacks.
        /// </summary>
        /// <value></value>
        public int CommentDelayInMinutes
        {
            get
            {
                if(_commentDelayInMinutes < 0 || _commentDelayInMinutes == int.MaxValue)
                {
                    return 0;
                }
                return _commentDelayInMinutes;
            }
            set { _commentDelayInMinutes = value; }
        }

        /// <summary>
        /// Gets or sets the number of recent comments to display in 
        /// the RecentComments control.
        /// </summary>
        /// <value></value>
        public int NumberOfRecentComments
        {
            get
            {
                if(_numberOfRecentComments < 0 || _numberOfRecentComments == int.MaxValue)
                {
                    return 0;
                }
                return _numberOfRecentComments;
            }
            set { _numberOfRecentComments = value; }
        }

        /// <summary>
        /// Gets or sets the number of characters to use to display recent comments  
        /// in the RecentComments control.
        /// </summary>
        /// <value></value>
        public int RecentCommentsLength
        {
            get
            {
                if(_recentCommentsLength < 0 || _recentCommentsLength == int.MaxValue)
                {
                    return DefaultRecentCommentsLength;
                }
                return _recentCommentsLength;
            }
            set { _recentCommentsLength = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this blog is active.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if it is active; otherwise, <c>false</c>.
        /// </value>
        public bool IsActive
        {
            get { return FlagPropertyCheck(ConfigurationFlags.IsActive); }
            set { FlagSetter(ConfigurationFlags.IsActive, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether or not comments are moderated
        /// </summary>
        /// <value>
        /// 	<c>true</c> if it is active; otherwise, <c>false</c>.
        /// </value>
        public bool ModerationEnabled
        {
            get { return FlagPropertyCheck(ConfigurationFlags.CommentModerationEnabled); }
            set { FlagSetter(ConfigurationFlags.CommentModerationEnabled, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether captcha is enabled.
        /// </summary>
        /// <value><c>true</c> if captcha is enabled; otherwise, <c>false</c>.</value>
        public bool CaptchaEnabled
        {
            get { return FlagPropertyCheck(ConfigurationFlags.CaptchaEnabled); }
            set { FlagSetter(ConfigurationFlags.CaptchaEnabled, value); }
        }

        /// <summary>
        /// Gets or sets the _subfolder the blog lives in.
        /// </summary>
        /// <value></value>
        public string Subfolder
        {
            get { return _subfolder ?? string.Empty; }
            set
            {
                if(!String.IsNullOrEmpty(value))
                {
                    value = HttpHelper.StripSurroundingSlashes(value);
                }

                _subfolder = value;
            }
        }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        /// <value></value>
        public string Password
        {
            get { return _password; }
            set { _password = value ?? string.Empty; }
        }

        /// <summary>
        /// Gets or sets the OpenIdUrl.
        /// </summary>
        /// <value></value>
        public string OpenIdUrl
        {
            get
            {
                return _openIdUrl;
            } 
            set
            {
                if(value != null)
                {
                    if(!value.StartsWith("http://") && !value.StartsWith("https://"))
                    {
                        _openIdUrl = "http://" + value;
                    }
                    else
                    {
                        _openIdUrl = value;
                    }
                }
                else
                {
                    _openIdUrl = null;
                }
            }
        }

        string _openIdUrl;

        /// <summary>
        /// Gets or sets the OpenIdServer.
        /// </summary>
        public string OpenIdServer { get; set; }

        /// <summary>
        /// Gets or sets the OpenIdDelegate.
        /// </summary>
        public string OpenIdDelegate { get; set; }

        /// <summary>
        /// Gets or sets the CardSpaceHash.
        /// </summary>
        /// <value></value>
        public string CardSpaceHash { get; set; }

        /// <summary>
        /// Gets or sets the user name for the owner of the blog.
        /// </summary>
        /// <value></value>
        public string UserName
        {
            get { return _username; }
            set { _username = value ?? string.Empty; }
        }

        /// <summary>
        /// Gets or sets the title of the blog.
        /// </summary>
        /// <value></value>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the sub title of the blog.
        /// </summary>
        /// <value></value>
        public string SubTitle { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="SkinConfig"/> instance 
        /// which contains information about the specified skin.
        /// </summary>
        /// <value></value>
        public SkinConfig Skin { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="SkinConfig"/> instance 
        /// which contains information about the specified skin.
        /// </summary>
        /// <value></value>
        public SkinConfig MobileSkin { get; set; }

        /// <summary>
        /// Gets a value indicating whether the blog has news. 
        /// News can be entered in the Admin section.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if the blog has news; otherwise, <c>false</c>.
        /// </value>
        public bool HasNews
        {
            get { return News != null && News.Trim().Length > 0; }
        }

        /// <summary>
        /// Gets or sets the news.
        /// </summary>
        /// <value></value>
        public string News { get; set; }

        /// <summary>
        /// Gets or sets the author of the blog.
        /// </summary>
        /// <value></value>
        public string Author { get; set; }

        /// <summary>
        /// Gets or sets blog tracking code.
        /// </summary>
        /// <value></value>
        public string TrackingCode { get; set; }

        /// <summary>
        /// Gets or sets the Blog Group ID
        /// </summary>
        /// <value></value>
        public int BlogGroupId { get; set; }

        /// <summary>
        /// Gets or sets the Blog Group Title
        /// </summary>
        /// <value></value>
        public string BlogGroupTitle { get; set; }

        /// <summary>
        /// Gets or sets the license URL.  This is used to 
        /// Used to specify a license within a syndicated feed. 
        /// Does not have to be a creative commons license. 
        /// <see href="http://backend.userland.com/creativeCommonsRssModule" />
        /// </summary>
        /// <value></value>
        public string LicenseUrl { get; set; }

        /// <summary>
        /// Gets or sets the Comment Service API key. This is for a comment spam filtering 
        /// service such as http://akismet.com/
        /// </summary>
        /// <value>The akismet API key.</value>
        public string FeedbackSpamServiceKey
        {
            get { return _feedbackSpamServiceKey ?? String.Empty; }
            set { _feedbackSpamServiceKey = (value ?? string.Empty); }
        }

        /// <summary>
        /// Gets a value indicating whether [akismet enabled].
        /// </summary>
        /// <value><c>true</c> if [akismet enabled]; otherwise, <c>false</c>.</value>
        public bool FeedbackSpamServiceEnabled
        {
            get { return !String.IsNullOrEmpty(_feedbackSpamServiceKey); }
        }

        /// <summary>
        /// Gets a value indicating whether an RSS Proxy such as FeedBurner is enabled.
        /// </summary>
        public bool RssProxyEnabled
        {
            get { return !String.IsNullOrEmpty(_rssProxyUrl); }
        }

        /// <summary>
        /// Gets or sets the name of the feedburner account. This is the portion of the 
        /// feedburner URL after: http://feedproxy.google.com/ You can also specify a 
        /// full URL
        /// </summary>
        /// <value>The name of the feed burner.</value>
        public string RssProxyUrl
        {
            get { return _rssProxyUrl; }
            set
            {
                if(!String.IsNullOrEmpty(value))
                {
                    if(value.Contains("\\"))
                    {
                        throw new InvalidOperationException(Resources.InvalidOperation_BackslashesInRssProxyName);
                    }
                }
                _rssProxyUrl = value;
            }
        }

        /// <summary>
        /// Gets or sets the flags pertaining to this blog.  
        /// This is a bitmask of <see cref="ConfigurationFlags"/>s.
        /// </summary>
        /// <value></value>
        public ConfigurationFlags Flag { get; set; }

        /// <summary>
        /// Gets or sets the total number of posts.
        /// </summary>
        /// <value></value>
        public int PostCount { get; set; }

        /// <summary>
        /// Gets or sets the comment count.
        /// </summary>
        /// <value></value>
        public int CommentCount { get; set; }

        /// <summary>
        /// Gets or sets the ping track count.
        /// </summary>
        /// <value></value>
        public int PingTrackCount { get; set; }

        /// <summary>
        /// Strips the port number from the host name.
        /// </summary>
        /// <param name="host">Host.</param>
        /// <returns></returns>
        public static string StripPortFromHost(string host)
        {
            if(String.IsNullOrEmpty(host))
            {
                throw new ArgumentNullException("host");
            }

            return host.LeftBefore(":", StringComparison.Ordinal);
        }

        /// <summary>
        /// Strips www prefix from host name.
        /// </summary>
        /// <param name="host">Host.</param>
        /// <returns></returns>
        public static string StripWwwPrefixFromHost(string host)
        {
            if(String.IsNullOrEmpty(host))
            {
                throw new ArgumentNullException("host");
            }

            return host.RightAfter("www.", StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Gets the active blog count by host.
        /// </summary>
        /// <param name="host">The host.</param>
        /// <returns></returns>
        /// <param name="pageIndex">Zero based index of the page to retrieve.</param>
        /// <param name="pageSize">Number of records to display on the page.</param>
        /// <param name="flags">Configuration flags to filter blogs retrieved.</param>
        public static IPagedCollection<Blog> GetBlogsByHost(string host, int pageIndex, int pageSize,
                                                            ConfigurationFlags flags)
        {
            if(String.IsNullOrEmpty(host))
            {
                throw new ArgumentNullException("host");
            }

            return ObjectProvider.Instance().GetPagedBlogs(host, pageIndex, pageSize, flags);
        }

        public IPagedCollection<BlogAlias> GetBlogAliases(int pageIndex, int pageSize)
        {
            return ObjectProvider.Instance().GetPagedBlogDomainAlias(this, pageIndex, pageSize);
        }

        /// <summary>
        /// Returns a <see cref="IList{T}"/> containing ACTIVE the <see cref="Blog"/> 
        /// instances within the specified range.
        /// </summary>
        /// <param name="pageIndex">Page index.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="flags"></param>
        /// <returns></returns>
        public static IPagedCollection<Blog> GetBlogs(int pageIndex, int pageSize, ConfigurationFlags flags)
        {
            return ObjectProvider.Instance().GetPagedBlogs(null, pageIndex, pageSize, flags);
        }

        /// <summary>
        /// Adds or removes a <see cref="ConfigurationFlags"/> to the 
        /// flags set for this blog via bitmask operations.
        /// </summary>
        /// <param name="cf">Cf.</param>
        /// <param name="select">Select.</param>
        protected void FlagSetter(ConfigurationFlags cf, bool select)
        {
            if(select)
            {
                Flag = Flag | cf;
            }
            else
            {
                Flag = Flag & ~cf;
            }
        }

        /// <summary>
        /// Checks to see if the specified <see cref="ConfigurationFlags"/> 
        /// matches a flag set for this blog.
        /// </summary>
        /// <param name="cf">Cf.</param>
        /// <returns></returns>
        protected bool FlagPropertyCheck(ConfigurationFlags cf)
        {
            return (Flag & cf) == cf;
        }

        /// <summary>
        /// Returns true if the two instances are equal
        /// </summary>
        /// <param name="obj">Obj.</param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if(obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            return ((Blog)obj).Id == Id;
        }

        /// <summary>
        /// Serves as the hash function for the type <see cref="Blog" />, 
        /// suitable for use in hashing functions.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return (Host ?? string.Empty).GetHashCode() ^ (Subfolder ?? string.Empty).GetHashCode() ^ Id.GetHashCode();
        }

        public static void ClearBlogContent(int blogId)
        {
            ObjectProvider.Instance().ClearBlogContent(blogId);
        }
    }
}