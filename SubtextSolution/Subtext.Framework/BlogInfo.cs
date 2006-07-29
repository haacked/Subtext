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
using System.Web;
using System.Xml.Serialization;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;
using Subtext.Framework.Format;
using Subtext.Framework.Providers;
using Subtext.Framework.Text;
using Subtext.Framework.Web.HttpModules;

namespace Subtext.Framework
{
	/// <summary>
	/// Represents an instance of a blog.  This was formerly known as the BlogConfig class. 
	/// We are attempting to distinguish this from settings stored in web.config. This class 
	/// is persisted via a <see cref="ObjectProvider"/>.
	/// </summary>
	[Serializable]
	public class BlogInfo
	{
		const int DefaultRecentCommentsLength = 50;
		private UrlFormats _urlFormats = null;

		/// <summary>
		/// Removes the preceding "www." on a host name.
		/// </summary>
		/// <param name="host">Host.</param>
		/// <returns></returns>
		public static string NormalizeHostName(string host)
		{
			return StringHelper.LeftBefore(
			    StringHelper.RightAfter(host, "www.", ComparisonType.CaseInsensitive), ":");
		}

		/// <summary>
		/// Returns a <see cref="IPagedCollection{T}"/> containing the <see cref="BlogInfo"/> 
		/// instances within the specified range.
		/// </summary>
		/// <param name="pageIndex">Page index.</param>
		/// <param name="pageSize">Size of the page.</param>
		/// <param name="sortDescending">Sort descending.</param>
		/// <returns></returns>
        public static IPagedCollection<BlogInfo> GetBlogs(int pageIndex, int pageSize, bool sortDescending)
		{
			return ObjectProvider.Instance().GetPagedBlogs(pageIndex, pageSize, sortDescending);
		}

		/// <summary>
		/// Returns a <see cref="IPagedCollection"/> containing the <see cref="BlogInfo"/> 
		/// instances that have the specified hostname.
		/// </summary>
		/// <param name="host">host.</param>
		/// <returns></returns>
        public static IPagedCollection<BlogInfo> GetBlogsByHost(string host)
		{
			return ObjectProvider.Instance().GetBlogsByHost(host);
		}

		/// <summary>
		/// Gets the active blog count by host.
		/// </summary>
		/// <param name="host">The host.</param>
		/// <returns></returns>
        public static IPagedCollection<BlogInfo> GetActiveBlogsByHost(string host)
		{
            IPagedCollection<BlogInfo> blogsWithHost = BlogInfo.GetBlogsByHost(host);
			for(int i = blogsWithHost.Count - 1; i >= 0; i--)
			{
				if(!blogsWithHost[i].IsActive)
					blogsWithHost.RemoveAt(i);
			}
			return blogsWithHost;
		}

		/// <summary>
		/// Returns a <see cref="IList{T}"/> containing ACTIVE the <see cref="BlogInfo"/> 
		/// instances within the specified range.
		/// </summary>
		/// <param name="pageIndex">Page index.</param>
		/// <param name="pageSize">Size of the page.</param>
		/// <param name="sortDescending">Sort descending.</param>
		/// <param name="totalBlogs">Indicates the total number of blogs</param>
		/// <returns></returns>
        public static IPagedCollection<BlogInfo> GetActiveBlogs(int pageIndex, int pageSize, bool sortDescending, out int totalBlogs)
		{
            IPagedCollection<BlogInfo> blogs = ObjectProvider.Instance().GetPagedBlogs(pageIndex, pageSize, sortDescending);
			// the ObjectProvider puts the returned TotalRecords value in the MaxItems property.
			totalBlogs = blogs.MaxItems;
			for (int i = blogs.Count - 1; i > -1; i--)
			{
				if (!blogs[i].IsActive)
				{
					blogs.RemoveAt(i);
				}
			}
			return blogs;
		}

		/// <summary>
		/// Returns a <see cref="IPagedCollection"/> containing ACTIVE the <see cref="BlogInfo"/> 
		/// instances within the specified range.
		/// </summary>
		/// <param name="pageIndex">Page index.</param>
		/// <param name="pageSize">Size of the page.</param>
		/// <param name="sortDescending">Sort descending.</param>
		/// <returns></returns>
        public static IPagedCollection<BlogInfo> GetActiveBlogs(int pageIndex, int pageSize, bool sortDescending)
		{
			int totalCount;
			return GetActiveBlogs(pageIndex, pageSize, sortDescending, out totalCount);
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
				if(_urlFormats == null)
				{
					_urlFormats = new UrlFormats(this.RootUrl);
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
			get{return _imageDirectory;}
			set{_imageDirectory = value;}
		}

		private string _imagePath;
		/// <summary>
		/// Gets or sets the path (url) to the image directory.
		/// </summary>
		/// <value></value>
		public string ImagePath
		{
			get{return _imagePath;}
			set{_imagePath = value;}
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
			get{return _blogID;}
			set{_blogID = value;}
		}

		private int _timeZone = 0;
		/// <summary>
		/// Gets or sets the time zone.  0 = GMT. -8 = PST.
		/// </summary>
		/// <value></value>
		public int TimeZone
		{
			get{return _timeZone;}
			set{_timeZone = value;}
		}

		private int _itemCount = 15;
		/// <summary>
		/// Gets or sets the count of posts displayed on the front page 
		/// of the blog.
		/// </summary>
		/// <value></value>
		public int ItemCount
		{
			get{return _itemCount;}
			set{_itemCount = value;}
		}

		private string _language = "en-US";
		/// <summary>
		/// Gets or sets the language the blog is in..
		/// </summary>
		/// <value></value>
		public string Language
		{
			get{return _language;}
			set{_language = value;}
		}

		/// <summary>
		/// Gets the two (or three) letter language without the culture code.
		/// </summary>
		/// <value>The language sans culture.</value>
		public string LanguageCode
		{
			get
			{
				if(_languageCode == null || _languageCode.Length == 0)
				{
					//Just being paranoid in making this check.
					if(_language == null)
						_language = "en-US";
					_languageCode = StringHelper.LeftBefore(_language, "-");
				}
				return _languageCode;
			}
		}

		string _languageCode;

		private string _email;
		/// <summary>
		/// Gets or sets the email of the blog owner.
		/// </summary>
		/// <value></value>
		public string Email
		{
			get{return _email;}
			set{_email = value;}
		}

		private string _host;
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
				_host = NormalizeHostName(value);
			}
		}

	    /// <summary>
	    /// The port the blog is listening on.
	    /// </summary>
	    public int Port
	    {
	        get
	        {
	            if(HttpContext.Current != null)
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
			get{return FlagPropertyCheck(ConfigurationFlag.EnableServiceAccess);}
			set{FlagSetter(ConfigurationFlag.EnableServiceAccess,value);}
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
			get{return FlagPropertyCheck(ConfigurationFlag.IsPasswordHashed);}
			set{FlagSetter(ConfigurationFlag.IsPasswordHashed,value);}
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
			get{return FlagPropertyCheck(ConfigurationFlag.CompressSyndicatedFeed);}
			set{FlagSetter(ConfigurationFlag.CompressSyndicatedFeed, value);}
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
			get{return FlagPropertyCheck(ConfigurationFlag.IsAggregated);}
			set{FlagSetter(ConfigurationFlag.IsAggregated,value);}
		}

		/// <summary>
		/// Gets or sets a value indicating whether comments are enabled.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if comments are enabled, otherwise, <c>false</c>.
		/// </value>
		public bool CommentsEnabled
		{
			get{return FlagPropertyCheck(ConfigurationFlag.CommentsEnabled);}
			set{FlagSetter(ConfigurationFlag.CommentsEnabled,value);}
		}

		/// <summary>
		/// Gets or sets a value indicating whether comments are enabled.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if comments are enabled, otherwise, <c>false</c>.
		/// </value>
		public bool CoCommentsEnabled
		{
			get{return FlagPropertyCheck(ConfigurationFlag.CoCommentEnabled);}
			set{FlagSetter(ConfigurationFlag.CoCommentEnabled, value);}
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
			get{return FlagPropertyCheck(ConfigurationFlag.AutoFriendlyUrlEnabled);}
			set{FlagSetter(ConfigurationFlag.AutoFriendlyUrlEnabled, value);}
		}

		/// <summary>
		/// Gets or sets a value indicating whether trackbacks and pingbacks are enabled.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if comments are enabled, otherwise, <c>false</c>.
		/// </value>
		public bool TrackbacksEnabled
		{
			get{return FlagPropertyCheck(ConfigurationFlag.TrackbacksEnabled);}
			set{FlagSetter(ConfigurationFlag.TrackbacksEnabled, value);}
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
			get{return FlagPropertyCheck(ConfigurationFlag.DuplicateCommentsEnabled);}
			set{FlagSetter(ConfigurationFlag.DuplicateCommentsEnabled, value);}
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
			get{return FlagPropertyCheck(ConfigurationFlag.RFC3229DeltaEncodingEnabled);}
			set{FlagSetter(ConfigurationFlag.RFC3229DeltaEncodingEnabled, value);}
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
				if(_commentDelayInMinutes < 0 || _commentDelayInMinutes == int.MaxValue)
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
				if(_numberOfRecentComments < 0 || _numberOfRecentComments == int.MaxValue)
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
				if(_recentCommentsLength < 0 || _recentCommentsLength == int.MaxValue)
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
			get{return FlagPropertyCheck(ConfigurationFlag.IsActive);}
			set{FlagSetter(ConfigurationFlag.IsActive, value);}
		}

		/// <summary>
		/// Gets or sets a value indicating whether or not comments are moderated
		/// </summary>
		/// <value>
		/// 	<c>true</c> if it is active; otherwise, <c>false</c>.
		/// </value>
		public bool ModerationEnabled
		{
			get { return FlagPropertyCheck(ConfigurationFlag.CommentModerationEnabled); }
			set { FlagSetter(ConfigurationFlag.CommentModerationEnabled, value); }
		}

		private string subfolder;
		/// <summary>
		/// Gets or sets the subfolder the blog lives in.
		/// </summary>
		/// <value></value>
		public string Subfolder
		{
			get
			{
				return this.subfolder;
			}
			set
			{
				if(value != null)
					value = UrlFormats.StripSurroundingSlashes(value);
				
				this.subfolder = value;
			}
		}

		private string _password;
		/// <summary>
		/// Gets or sets the password.
		/// </summary>
		/// <value></value>
		public string Password
		{
			get
			{
				if(_password == null)
				{
					//TODO: Throw a specific exception.
					throw new Exception("Invalid Password Setting");
				}
				return _password;
			}
			set{_password = value;}
		}

		private string _username;
		/// <summary>
		/// Gets or sets the user name for the owner of the blog.
		/// </summary>
		/// <value></value>
		public string UserName
		{
			get
			{
				if(_username == null)
				{
					//TODO: Throw a specific exception.
					throw new Exception("Invalid UserName Setting");
				}
				return _username;
			}
			set{_username = value;}
		}

		private string _title;
		/// <summary>
		/// Gets or sets the title of the blog.
		/// </summary>
		/// <value></value>
		public string Title
		{
			get{return _title;}
			set{_title = value;}
		}

		private string _subtitle;
		/// <summary>
		/// Gets or sets the sub title of the blog.
		/// </summary>
		/// <value></value>
		public string SubTitle
		{
			get{return _subtitle;}
			set{_subtitle = value;}
		}

		private SkinConfig _skin;
		/// <summary>
		/// Gets or sets the <see cref="SkinConfig"/> instance 
		/// which contains information about the specified skin.
		/// </summary>
		/// <value></value>
		public SkinConfig Skin
		{
			get{return _skin;}
			set{_skin = value;}
		}

		/// <summary>
		/// Gets a value indicating whether the blog has news. 
		/// News can be entered in the Admin section.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if the blog has news; otherwise, <c>false</c>.
		/// </value>
		public bool HasNews
		{
			get{ return News != null && News.Trim().Length > 0;}
		}

		private string news;
		/// <summary>
		/// Gets or sets the news.
		/// </summary>
		/// <value></value>
		public string News
		{
			get{return news;}
			set{news = value;}
		}

		private string _author = "Subtext Weblog";
		/// <summary>
		/// Gets or sets the author of the blog.
		/// </summary>
		/// <value></value>
		public string Author
		{
			get{return _author;}
			set{_author = value;}
		}

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

		string _licenseUrl;

		/// <summary>
		/// Gets the root URL for this blog.  For example, "http://example.com/" or "http://example.com/blog/".
		/// </summary>
		/// <value></value>
		[XmlIgnore]
		public string RootUrl
		{
			get
			{
				if(_rootUrl == null)
				{
                    string host = this._host;
                    if (this.Port != BlogRequest.DefaultPort)
                    {
                        host += ":" + this.Port;
                    }
                    _rootUrl = "http://" + host + VirtualUrl;					
				}
				return _rootUrl;
			}
		}
		string _rootUrl = null;

		/// <summary>
		/// Gets the virtual URL for the site.  For example, "/" or "/Subtext.Web/" or "/Blog/".
		/// </summary>
		/// <value>The virtual URL.</value>
		[XmlIgnore]
		public string VirtualUrl
		{
			get
			{
				if(this.virtualUrl == null)
				{
					this.virtualUrl = "/";
					string appPath = UrlFormats.StripSurroundingSlashes(HttpContext.Current.Request.ApplicationPath);
					if(appPath.Length > 0)
					{
						this.virtualUrl += appPath + "/";
					}
					
					if(this.Subfolder.Length > 0)
					{
						this.virtualUrl += this.Subfolder + "/";
					}
				}
				return this.virtualUrl;
			}
		}
		string virtualUrl;

		/// <summary>
		/// Gets the virtual directory/application root for the site.  
		/// This is really just a formatted version of the 
		/// HttpContext.Current.Request.ApplicationPath property that always ends with a slash.
		/// </summary>
		/// <value>The virtual URL.</value>
		[XmlIgnore]
		public string VirtualDirectoryRoot
		{
			get
			{
				string virtualDirectory = UrlFormats.StripSurroundingSlashes(HttpContext.Current.Request.ApplicationPath);
				if(virtualDirectory.Length == 0)
				{
					return "/";
				}
				if(!virtualDirectory.EndsWith("/"))
				{
					virtualDirectory += "/";
				}

				if(!virtualDirectory.StartsWith("/"))
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
		public string HomeFullyQualifiedUrl
		{
			get
			{
				return RootUrl + "Default.aspx";
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

		private ConfigurationFlag _flag = ConfigurationFlag.None;
		/// <summary>
		/// Gets or sets the flags pertaining to this blog.  
		/// This is a bitmask of <see cref="ConfigurationFlag"/>s.
		/// </summary>
		/// <value></value>
		public ConfigurationFlag Flag
		{
			get{return _flag;}
			set{_flag = value;}
		}

		/// <summary>
		/// Returns the Subfolder name without any dashes.
		/// </summary>
		/// <value></value>
		public string CleanSubfolder
		{
			get {return this.Subfolder.Replace("/", string.Empty).Trim();}
			
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
			get {return this._postCount;}
			set {this._postCount = value;}
		}

		private int _commentCount;
		/// <summary>
		/// Gets or sets the comment count.
		/// </summary>
		/// <value></value>
		public int CommentCount
		{
			get {return this._commentCount;}
			set {this._commentCount = value;}
		}

		private int _storyCount;
		/// <summary>
		/// Gets or sets the story count.
		/// </summary>
		/// <value></value>
		public int StoryCount
		{
			get {return this._storyCount;}
			set {this._storyCount = value;}
		}

		private int _pingTrackCount;
		/// <summary>
		/// Gets or sets the ping track count.
		/// </summary>
		/// <value></value>
		public int PingTrackCount
		{
			get {return this._pingTrackCount;}
			set {this._pingTrackCount = value;}
		}

		#endregion

		/// <summary>
		/// Adds or removes a <see cref="ConfigurationFlag"/> to the 
		/// flags set for this blog via bitmask operations.
		/// </summary>
		/// <param name="cf">Cf.</param>
		/// <param name="select">Select.</param>
		protected void FlagSetter(ConfigurationFlag cf, bool select)
		{
			if(select)
			{
				this.Flag = Flag | cf;
			}
			else
			{
				this.Flag = Flag & ~cf;
			}
		}

		/// <summary>
		/// Checks to see if the specified <see cref="ConfigurationFlag"/> 
		/// matches a flag set for this blog.
		/// </summary>
		/// <param name="cf">Cf.</param>
		/// <returns></returns>
		protected bool FlagPropertyCheck(ConfigurationFlag cf)
		{
			return (this.Flag & cf) == cf;
		}

		/// <summary>
		/// Returns true if the two instances are equal
		/// </summary>
		/// <param name="obj">Obj.</param>
		/// <returns></returns>
		public override bool Equals(object obj)
		{
			if(obj == null || GetType() != obj.GetType())
				return false;

			return ((BlogInfo)obj).Id == this.Id;
		}

		/// <summary>
		/// Serves as the hash function for the type <see cref="BlogInfo" />, 
		/// suitable for use in hashing functions.
		/// </summary>
		/// <returns></returns>
		public override int GetHashCode()
		{
			return this.Host.GetHashCode() ^ this.Subfolder.GetHashCode();
		}
	}
}

