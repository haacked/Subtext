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
using System.Collections.Generic;
using System.Configuration;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Security;
using log4net;
using Subtext.Extensibility.Interfaces;
using Subtext.Framework.Configuration;
using Subtext.Framework.Format;
using Subtext.Framework.Logging;
using Subtext.Framework.Providers;
using Subtext.Framework.Services;
using Subtext.Framework.Text;
using Subtext.Framework.Web.HttpModules;
using Subtext.Framework.Components;
using Subtext.Framework.Util.TimeZoneUtil;

namespace Subtext.Framework
{
	/// <summary>
	/// Represents an instance of a blog.  This was formerly known as the BlogConfig class. 
	/// We are attempting to distinguish this from settings stored in web.config. This class 
	/// is persisted via a <see cref="ObjectProvider"/>.
	/// </summary>
	[Serializable]
	public class BlogInfo : IBlogInfo
	{
		private readonly static ILog Log = new Log();
		const int DefaultRecentCommentsLength = 50;
		private UrlFormats _urlFormats;

		/// <summary>
		/// Strips the port number from the host name.
		/// </summary>
		/// <param name="host">Host.</param>
		/// <returns></returns>
		public static string StripPortFromHost(string host)
		{
			if (String.IsNullOrEmpty(host))
				throw new ArgumentException("Cannot strip the port from a null host", "host");

			return Regex.Replace(host, @":.*$", string.Empty);
		}

		/// <summary>
		/// Strips www prefix from host name.
		/// </summary>
		/// <param name="host">Host.</param>
		/// <returns></returns>
		public static string StripWwwPrefixFromHost(string host)
		{
			if (String.IsNullOrEmpty(host))
				throw new ArgumentException("Cannot strip the www prefix from a null host", "host");

			return Regex.Replace(host, @"^www.", string.Empty, RegexOptions.IgnoreCase);
		}

		/// <summary>
		/// If the host starts with www., gets the host without the www. If it 
		/// doesn't start with www., returns the host with wwww..
		/// </summary>
		/// <param name="host">Host.</param>
		/// <returns></returns>
		public static string GetAlternateHostAlias(string host)
		{
			if (String.IsNullOrEmpty(host))
				throw new ArgumentException("Cannot get an alternative alias to a null host", "host");

			if (host.StartsWith("www.", StringComparison.CurrentCultureIgnoreCase))
				return StripWwwPrefixFromHost(host);
			else
				return "www." + host;
		}
		
		/// <summary>
		/// Gets the active blog count by host.
		/// </summary>
		/// <param name="host">The host.</param>
		/// <param name="pageIndex">Index of the page.</param>
		/// <param name="pageSize">Size of the page.</param>
		/// <param name="flags">The flags.</param>
		/// <returns></returns>
        /// <param name="pageIndex">Zero based index of the page to retrieve.</param>
        /// <param name="pageSize">Number of records to display on the page.</param>
        /// <param name="flags">Configuration flags to filter blogs retrieved.</param>
        public static IPagedCollection<BlogInfo> GetBlogsByHost(string host, int pageIndex, int pageSize, ConfigurationFlags flags)
		{
			if (String.IsNullOrEmpty(host))
				throw new ArgumentNullException("host", "Host must not be null or empty.");

			return ObjectProvider.Instance().GetPagedBlogs(host, pageIndex, pageSize, flags);
		}

		public IPagedCollection<BlogAlias> GetBlogAliases(int pageIndex, int pageSize)
		{
			return ObjectProvider.Instance().GetPagedBlogDomainAlias(this, pageIndex, pageSize);
		}

		/// <summary>
		/// Returns a <see cref="IList{T}"/> containing ACTIVE the <see cref="BlogInfo"/> 
		/// instances within the specified range.
		/// </summary>
		/// <param name="pageIndex">Page index.</param>
		/// <param name="pageSize">Size of the page.</param>
		/// <param name="flag"></param>
		/// <returns></returns>
		public static IPagedCollection<BlogInfo> GetBlogs(int pageIndex, int pageSize, ConfigurationFlags flag)
		{
			return ObjectProvider.Instance().GetPagedBlogs(null, pageIndex, pageSize, flag);
		}

		/// <summary>
		/// Gets the blog by id.
		/// </summary>
		/// <param name="blogId">Blog id.</param>
		/// <returns></returns>
		public static BlogInfo GetBlogById(int blogId)
		{
			return ObjectProvider.Instance().GetBlogById(blogId);
		}

		/// <summary>
		/// Class used to encapsulate URL formats for 
		/// various sections of the blog.
		/// </summary>
		/// <value></value>
		public UrlFormats UrlFormats
		{
			get
			{
				if (_urlFormats == null)
				{
					_urlFormats = new UrlFormats(RootUrl);
				}
				return _urlFormats;
			}
		}

		private string _imageDirectory;
		/// <summary>
		/// Gets or sets the physical path to the image directory.
		/// </summary>
		/// <value></value>
		public string ImageDirectory
		{
			get { return _imageDirectory; }
			set { _imageDirectory = value; }
		}

        MembershipUser _owner;
        internal Guid _ownerId;
        /// <summary>
		/// Gets or sets the _owner of the blog.
		/// </summary>
		/// <value>The _owner.</value>
		public MembershipUser Owner
		{
			get
			{
				if (_owner == null && _ownerId != Guid.Empty)
				{
					_owner = Membership.GetUser(_ownerId);
				}
				return _owner;
			}
			set { _owner = value; }
		}

        private string _imagePath;
		/// <summary>
		/// Gets or sets the path (url) to the image directory.
		/// </summary>
		/// <value></value>
		public string ImagePath
		{
			get { return _imagePath; }
			set { _imagePath = value; }
		}

		private DateTime _lastupdated;
		/// <summary>
		/// Gets or sets the date that the blog's configuration 
		/// was last updated.
		/// </summary>
		/// <value></value>
		public DateTime LastUpdated
		{
			get
			{
				return _lastupdated;
			}
			set
			{
				_lastupdated = value;
			}
		}

		private int _blogID = NullValue.NullInt32;
		/// <summary>
		/// Gets or sets the ID of the blog.  This is the 
		/// primary key in the blog_config table.
		/// </summary>
		/// <value></value>
		public int Id
		{
			get { return _blogID; }
			set { _blogID = value; }
		}

		/// <summary>
		/// Gets the time zone.
		/// </summary>
		/// <value>The time zone.</value>
		public WindowsTimeZone TimeZone
		{
			get
			{
				WindowsTimeZone timezone = WindowsTimeZone.GetById(_timeZoneId);
				if (timezone == null)
					return WindowsTimeZone.GetById(System.TimeZone.CurrentTimeZone.StandardName.GetHashCode());
				return timezone;
			}
		}

		/// <summary>
		/// Gets or sets the time zone for the blogger.  
		/// 0 = GMT. -8 = PST.
		/// </summary>
		/// <value></value>
		public int TimeZoneId
		{
			get { return _timeZoneId; }
			set { _timeZoneId = value; }
		}
		private int _timeZoneId;

		private int _itemCount = 15;
		/// <summary>
		/// Gets or sets the count of posts displayed on the front page 
		/// of the blog.
		/// </summary>
		/// <value></value>
		public int ItemCount
		{
			get { return _itemCount; }
			set { _itemCount = value; }
		}

		private int _categoryListPostCount = 10;
		/// <summary>
		/// Gets or sets the count of posts displayed on the category pages. 
		/// </summary>
		/// <value></value>
		public int CategoryListPostCount
		{
			get { return _categoryListPostCount; }
			set
			{
				if (value < 0)
				{
					value = 0;//needed when upgrading from versions that did not have this column ("CategoryListPostCount") in the subtext_Config table.
				}
				_categoryListPostCount = value;
			}
		}

		private int _storyCount;
		/// <summary>
		/// Gets or sets the story count.
		/// </summary>
		/// <value></value>
		public int StoryCount
		{
			get { return _storyCount; }
			set { _storyCount = value; }
		}

		private string _language = "en-US";
		/// <summary>
		/// Gets or sets the language the blog is in..
		/// </summary>
		/// <value></value>
		public string Language
		{
			get { return String.IsNullOrEmpty(_language) ? "en-US" : _language; }
			set
			{
				_language = value;
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
				if (String.IsNullOrEmpty(_languageCode))
				{
					_languageCode = StringHelper.LeftBefore(Language, "-");
				}
				return _languageCode;
			}
		}

		string _languageCode;

		/// <summary>
		/// Gets or sets the host for the blog.  For 
		/// example, www.haacked.com might be a host.
		/// </summary>
		/// <value></value>
		public string Host
		{
			get
			{
				return _host;
			}
			set
			{
				_host = StripPortFromHost(value);
			}
		}
		private string _host;

		/// <summary>
		/// The port the blog is listening on.
		/// </summary>
		public static int Port
		{
			get
			{
				if (HttpContext.Current != null)
				{
					return HttpContext.Current.Request.Url.Port;
				}
				return 80;
			}
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
		public int DaysTillCommentsClose
		{
			get { return _daysTillCommentsClose; }
			set { _daysTillCommentsClose = value; }
		}

		int _daysTillCommentsClose = int.MaxValue;

		/// <summary>
		/// Gets or sets the delay in minutes, between any two successive comments from 
		/// the same IP address.  This helps prevents comment bombing attacks.
		/// </summary>
		/// <value></value>
		public int CommentDelayInMinutes
		{
			get
			{
				if (_commentDelayInMinutes < 0 || _commentDelayInMinutes == int.MaxValue)
					return 0;
				else
					return _commentDelayInMinutes;
			}
			set { _commentDelayInMinutes = value; }
		}

		int _commentDelayInMinutes;

		/// <summary>
		/// Gets or sets the number of recent comments to display in 
		/// the RecentComments control.
		/// </summary>
		/// <value></value>
		public int NumberOfRecentComments
		{
			get
			{
				if (_numberOfRecentComments < 0 || _numberOfRecentComments == int.MaxValue)
					return 0;
				else
					return _numberOfRecentComments;
			}
			set { _numberOfRecentComments = value; }
		}

		int _numberOfRecentComments;

		/// <summary>
		/// Gets or sets the number of characters to use to display recent comments  
		/// in the RecentComments control.
		/// </summary>
		/// <value></value>
		public int RecentCommentsLength
		{
			get
			{
				if (_recentCommentsLength < 0 || _recentCommentsLength == int.MaxValue)
					return DefaultRecentCommentsLength;
				else
					return _recentCommentsLength;
			}
			set { _recentCommentsLength = value; }
		}

		int _recentCommentsLength;

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

		private string _subfolder;
		/// <summary>
		/// Gets or sets the _subfolder the blog lives in.
		/// </summary>
		/// <value></value>
		public string Subfolder
		{
			get
			{
				return _subfolder;
			}
			set
			{
				if (value != null)
					value = UrlFormats.StripSurroundingSlashes(value);

				_subfolder = value;
			}
		}

		/// <summary>
		/// Gets or sets the name of the Membership application this 
		/// blog is mapped to.
		/// </summary>
		/// <value>The name of the application.</value>
		public string ApplicationName
		{
			get
			{
				return applicationName ?? Host + "/" + Subfolder;
			}
			set { applicationName = value; }
		}

		string applicationName;

		private string _title;
		/// <summary>
		/// Gets or sets the title of the blog.
		/// </summary>
		/// <value></value>
		public string Title
		{
			get { return _title; }
			set { _title = value; }
		}

		private string _subtitle;
		/// <summary>
		/// Gets or sets the sub title of the blog.
		/// </summary>
		/// <value></value>
		public string SubTitle
		{
			get { return _subtitle; }
			set { _subtitle = value; }
		}

		private SkinConfig _skin;
		/// <summary>
		/// Gets or sets the <see cref="SkinConfig"/> instance 
		/// which contains information about the specified skin.
		/// </summary>
		/// <value></value>
		public SkinConfig Skin
		{
			get { return _skin; }
			set { _skin = value; }
		}

		/// <summary>
		/// Gets a value indicating whether the blog has _news. 
		/// News can be entered in the Admin section.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if the blog has _news; otherwise, <c>false</c>.
		/// </value>
		public bool HasNews
		{
			get { return News != null && News.Trim().Length > 0; }
		}

		private string _news;
		/// <summary>
		/// Gets or sets the _news.
		/// </summary>
		/// <value></value>
		public string News
		{
			get { return _news; }
			set { _news = value; }
		}

		/// <summary>
		/// Gets or sets the author of the blog.
		/// </summary>
		/// <value></value>
		public string Author
		{
			get
			{
				if(Owner != null)
					return Owner.UserName;
				return "Subtext Weblog";
			}
		}


		private string _customMetaTags;
		/// <summary>
		/// Gets or sets custom meta tags.
		/// </summary>
		/// <value></value>
		public string CustomMetaTags
		{
			get { return _customMetaTags; }
			set { _customMetaTags = value; }
		}

		private string _trackingCode;

		/// <summary>
		/// Gets or sets blog tracking code.
		/// </summary>
		/// <value></value>
		public string TrackingCode
		{
			get { return _trackingCode; }
			set { _trackingCode = value; }
		}

        private int _blogGroup;
        /// <summary>
        /// Gets or sets the Blog Group ID
        /// </summary>
        /// <value></value>
        public int BlogGroupId
        {
            get { return _blogGroup; }
            set { _blogGroup = value; }
        }

        private string _blogGroupTitle;
        /// <summary>
        /// Gets or sets the Blog Group Title
        /// </summary>
        /// <value></value>
        public string BlogGroupTitle
        {
            get { return _blogGroupTitle; }
            set { _blogGroupTitle = value; }
        }

        string _licenseUrl;
		/// <summary>
		/// Gets or sets the license URL.  This is used to 
		/// Used to specify a license within a syndicated feed. 
		/// Does not have to be a creative commons license. 
		/// <see href="http://backend.userland.com/creativeCommonsRssModule" />
		/// </summary>
		/// <value></value>
		public string LicenseUrl
		{
			get { return _licenseUrl; }
			set { _licenseUrl = value; }
		}


        string _feedbackSpamServiceKey;
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
			get
			{
				return !String.IsNullOrEmpty(_feedbackSpamServiceKey);
			}
		}

        IFeedbackSpamService _feedbackService;
		/// <summary>
		/// Gets the comment spam service.
		/// </summary>
		/// <value>The comment spam service.</value>
		public IFeedbackSpamService FeedbackSpamService
		{
			get
			{
				if (_feedbackService == null && FeedbackSpamServiceEnabled)
				{
					_feedbackService = new AkismetSpamService(_feedbackSpamServiceKey, this);
				}
				return _feedbackService;
			}
			set
			{
				_feedbackService = value;
			}
		}

		

		/// <summary>
		/// Gets a value indicating whether [feed burner enabled].
		/// </summary>
		/// <value><c>true</c> if [feed burner enabled]; otherwise, <c>false</c>.</value>
		public bool FeedBurnerEnabled
		{
			get
			{
				return !String.IsNullOrEmpty(_feedBurnerName);
			}
		}

        string _feedBurnerName;
		/// <summary>
		/// Gets or sets the name of the feedburner account. 
		/// This is the portion of the feedburner URL after:
		/// http://feeds.feedburner.com/
		/// </summary>
		/// <value>The name of the feed burner.</value>
		public string FeedBurnerName
		{
			get { return _feedBurnerName; }
			set
			{
				if (!String.IsNullOrEmpty(value))
				{
					if (value.Contains("\\"))
						throw new InvalidOperationException("Backslashes are not allowed in the feedburner name.");
				}
				_feedBurnerName = value;
			}
		}


        Uri _rootUrl;
		/// <summary>
		/// Gets the root URL for this blog.  For example, "http://example.com/" or "http://example.com/blog/".
		/// </summary>
		/// <value></value>
		public Uri RootUrl
		{
			get
			{
				if (_rootUrl == null)
				{
					_rootUrl = HostFullyQualifiedUrl;
					if (Subfolder != null && Subfolder.Length > 0)
					{
						_rootUrl = new Uri(_rootUrl, Subfolder + "/");
					}
				}
				return _rootUrl;
			}
		}


        string _virtualUrl;
		/// <summary>
		/// Gets the virtual URL for the site with preceding and trailing slash.  For example, "/" or "/Subtext.Web/" or "/Blog/".
		/// </summary>
		/// <value>The virtual URL.</value>
		public string VirtualUrl
		{
			get
			{
				if (_virtualUrl == null)
				{
					_virtualUrl = "/";
					string appPath = UrlFormats.StripSurroundingSlashes(HttpContext.Current.Request.ApplicationPath);
					if (appPath.Length > 0)
					{
						_virtualUrl += appPath + "/";
					}

					if (Subfolder != null && Subfolder.Length > 0)
					{
						_virtualUrl += Subfolder + "/";
					}
				}
				return _virtualUrl;
			}
		}
		

		/// <summary>
		/// Gets the virtual directory/application root for the site.  
		/// This is really just a formatted version of the 
		/// HttpContext.Current.Request.ApplicationPath property that always ends with a slash.
		/// </summary>
		/// <value>The virtual URL.</value>
		public static string VirtualDirectoryRoot
		{
			get
			{
				string virtualDirectory = UrlFormats.StripSurroundingSlashes(HttpContext.Current.Request.ApplicationPath);
				if (virtualDirectory.Length == 0)
				{
					return "/";
				}
				if (!virtualDirectory.EndsWith("/"))
				{
					virtualDirectory += "/";
				}

				if (!virtualDirectory.StartsWith("/"))
				{
					virtualDirectory = "/" + virtualDirectory;
				}
				return virtualDirectory;
			}
		}

		/// <summary>
		/// Gets virtual URL to the admin home page.
		/// </summary>
		/// <value>The admin virtual URL.</value>
		public string AdminHomeVirtualUrl
		{
			get
			{
				return string.Format(System.Globalization.CultureInfo.InvariantCulture, "{0}Default.aspx", AdminDirectoryVirtualUrl);
			}
		}

		/// <summary>
		/// Gets virtual URL to the admin section.
		/// </summary>
		/// <value>The admin virtual URL.</value>
		public string AdminDirectoryVirtualUrl
		{
			get
			{
				return string.Format(System.Globalization.CultureInfo.InvariantCulture, "{0}Admin/", VirtualUrl);
			}
		}

		/// <summary>
		/// Gets the fully qualified blog home URL.  This is the URL to the blog's home page. 
		/// Until we integrate with IIS better, we have to append the "Default.aspx" 
		/// to the end.
		/// </summary>
		/// <value></value>
		public Uri HomeFullyQualifiedUrl
		{
			get
			{
				return new Uri(RootUrl, "Default.aspx");
			}
		}

        Uri _hostFullyQualifiedUrl;
		/// <summary>
		/// Gets the fully qualified url to the blog engine host.  This is the 
		/// blog URL without the _subfolder, but with the virtual directory 
		/// path, if any.
		/// </summary>
		/// <value></value>
		public Uri HostFullyQualifiedUrl
		{
			get
			{
				if (_hostFullyQualifiedUrl == null)
				{
					string host = HttpContext.Current.Request.Url.Scheme + "://" + _host;
					if (Port != BlogRequest.DefaultPort)
					{
						host += ":" + Port;
					}
					host += VirtualDirectoryRoot;
					_hostFullyQualifiedUrl = new Uri(host);
				}
				return _hostFullyQualifiedUrl;
			}
		}
		

		/// <summary>
		/// Gets the blog home virtual URL.  For example, "/default.aspx" or "/Blog/Default.aspx".
		/// </summary>
		/// <value>The blog home virtual URL.</value>
		public string HomeVirtualUrl
		{
			get
			{
				return VirtualUrl + "Default.aspx";
			}
		}

		private ConfigurationFlags _flag = ConfigurationFlags.None;
		/// <summary>
		/// Gets or sets the flags pertaining to this blog.  
		/// This is a bitmask of <see cref="ConfigurationFlags"/>.
		/// </summary>
		/// <value></value>
		public ConfigurationFlags Flag
		{
			get { return _flag; }
			set { _flag = value; }
		}

		#region Counts

		//TODO: These might need to go somewhere else.
		private int _postCount;
		/// <summary>
		/// Gets or sets the total number of posts.
		/// </summary>
		/// <value></value>
		public int PostCount
		{
			get { return _postCount; }
			set { _postCount = value; }
		}

		private int _commentCount;
		/// <summary>
		/// Gets or sets the comment count.
		/// </summary>
		/// <value></value>
		public int CommentCount
		{
			get { return _commentCount; }
			set { _commentCount = value; }
		}

		private int _pingTrackCount;
		/// <summary>
		/// Gets or sets the ping track count.
		/// </summary>
		/// <value></value>
		public int PingTrackCount
		{
			get { return _pingTrackCount; }
			set { _pingTrackCount = value; }
		}

		#endregion

		/// <summary>
		/// Adds or removes a <see cref="ConfigurationFlags"/> to the 
		/// flags set for this blog via bitmask operations.
		/// </summary>
		/// <param name="cf">Cf.</param>
		/// <param name="select">Select.</param>
		protected void FlagSetter(ConfigurationFlags cf, bool select)
		{
			if (select)
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
		bool FlagPropertyCheck(ConfigurationFlags cf)
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
			if (obj == null)
				return false;

			if (GetType() != obj.GetType())
				return false;

			return ((BlogInfo)obj).Id == Id;
		}

		/// <summary>
		/// Serves as the hash function for the type <see cref="BlogInfo" />, 
		/// suitable for use in hashing functions.
		/// </summary>
		/// <returns></returns>
		public override int GetHashCode()
		{
			return Host.GetHashCode() ^ Subfolder.GetHashCode();
		}

		private static readonly BlogInfo aggregateBlog = InitAggregateBlog();

		private static BlogInfo InitAggregateBlog()
		{
			HostInfo hostInfo = HostInfo.Instance;
			string aggregateHost = ConfigurationManager.AppSettings["AggregateUrl"];
			if (aggregateHost == null)
				return null;

			Regex regex = new Regex(@"^(https?://)?(?<host>.+?)(/.*)?$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
			Match match = regex.Match(aggregateHost);

			if (match.Success)
				aggregateHost = match.Groups["host"].Value;

			BlogInfo blog = new BlogInfo();
			blog.Title = ConfigurationManager.AppSettings["AggregateTitle"];
			blog.Skin = SkinConfig.GetDefaultSkin();
			blog.Host = aggregateHost;
			blog.Subfolder = string.Empty;

			// When running on the build server there are no Host records in the system
			// so HostInfo.Instance returns NULL, meaning a NullRefernce on the server.
			if (hostInfo == null)
				Log.Warn("There is no Host record in for this installation.");
			else
				blog.Owner = hostInfo.Owner;

			return blog;
		}

		public static BlogInfo AggregateBlog
		{
			get
			{
				return aggregateBlog;
			}
		}

		#region Notification Properties
		/// <summary>
		/// Gets or sets a value indicating whether comment notification is enabled.
		/// </summary>
		/// <value><c>true</c> if comment notification is enabled; otherwise, <c>false</c>.</value>
		public bool CommentNoficationEnabled
		{
			get { return FlagPropertyCheck(ConfigurationFlags.CommentNotificationEnabled); }
			set { FlagSetter(ConfigurationFlags.CommentNotificationEnabled, value); }
		}
		/// <summary>
		/// Gets or sets a value indicating whether trackback notification is enabled.
		/// </summary>
		/// <value><c>true</c> if comment notification is enabled; otherwise, <c>false</c>.</value>
		public bool TrackbackNoficationEnabled
		{
			get { return FlagPropertyCheck(ConfigurationFlags.TrackbackNotificationEnabled); }
			set { FlagSetter(ConfigurationFlags.TrackbackNotificationEnabled, value); }
		}
		#endregion

		#region Plugin Specific Properties

		private IDictionary<Guid, Plugin> _enabledPlugins;

		public IDictionary<Guid, Plugin> EnabledPlugins
		{
			get
			{
				//if the list of plugins has not been retrived for this BlogInfo
				//I need to retrieve it from the cache (or, if I'm not lucky, from the storage)
				if (_enabledPlugins == null)
				{
					_enabledPlugins = Plugin.GetEnabledPluginsWithBlogSettingsFromCache();
				}
				return _enabledPlugins;
			}
		}

		internal void ClearEnablePluginsCache()
		{
			_enabledPlugins = null;
		}

		#endregion Plugin Specific Properties
	}
}



