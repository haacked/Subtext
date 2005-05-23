#region Disclaimer/Info
///////////////////////////////////////////////////////////////////////////////////////////////////
// .Text WebLog
// 
// .Text is an open source weblog system started by Scott Watermasysk. 
// Blog: http://ScottWater.com/blog 
// RSS: http://scottwater.com/blog/rss.aspx
// Email: Dottext@ScottWater.com
//
// For updated news and information please visit http://scottwater.com/dottext and subscribe to 
// the Rss feed @ http://scottwater.com/dottext/rss.aspx
//
// On its release (on or about August 1, 2003) this application is licensed under the BSD. However, I reserve the 
// right to change or modify this at any time. The most recent and up to date license can always be fount at:
// http://ScottWater.com/License.txt
// 
// Please direct all code related questions to:
// GotDotNet Workspace: http://www.gotdotnet.com/Community/Workspaces/workspace.aspx?id=e99fccb3-1a8c-42b5-90ee-348f6b77c407
// Yahoo Group http://groups.yahoo.com/group/DotText/
// 
///////////////////////////////////////////////////////////////////////////////////////////////////
#endregion

using System;
using Subtext.Framework.Components;
using Subtext.Framework.Format;
using Subtext.Framework.Providers;

namespace Subtext.Framework.Configuration
{
	/// <summary>
	/// Contains configuration information about a blog.  
	/// This data is loaded from the blog_config table.
	/// </summary>
	[Serializable]
	public class BlogConfig : IBlogIdentifier
	{
		private object urlLock = new object();

		private UrlFormats _UrlFormats = null;

		/// <summary>
		/// Returns a <see cref="BlogConfigCollection"/> containing the <see cref="BlogConfig"/> 
		/// instances within the specified range.
		/// </summary>
		/// <param name="pageIndex">Page index.</param>
		/// <param name="pageSize">Size of the page.</param>
		/// <param name="sortDescending">Sort descending.</param>
		/// <returns></returns>
		public static BlogConfigCollection GetBlogs(int pageIndex, int pageSize, bool sortDescending)
		{
			return DTOProvider.Instance().GetPagedBlogs(pageIndex, pageSize, sortDescending);
		}

		/// <summary>
		/// Returns a <see cref="BlogConfigCollection"/> containing the <see cref="BlogConfig"/> 
		/// instances that have the specified hostname.
		/// </summary>
		/// <param name="host">host.</param>
		/// <returns></returns>
		public static BlogConfigCollection GetBlogsByHost(string host)
		{
			return DTOProvider.Instance().GetBlogsByHost(host);
		}

		/// <summary>
		/// Returns a <see cref="BlogConfigCollection"/> containing ACTIVE the <see cref="BlogConfig"/> 
		/// instances within the specified range.
		/// </summary>
		/// <param name="pageIndex">Page index.</param>
		/// <param name="pageSize">Size of the page.</param>
		/// <param name="sortDescending">Sort descending.</param>
		/// <param name="totalBlogs">Indicates the total number of blogs</param>
		/// <returns></returns>
		public static BlogConfigCollection GetActiveBlogs(int pageIndex, int pageSize, bool sortDescending, out int totalBlogs)
		{
			BlogConfigCollection blogs = DTOProvider.Instance().GetPagedBlogs(pageIndex, pageSize, sortDescending);
			totalBlogs = blogs.Count;
			for(int i = blogs.Count - 1; i > -1; i--)
			{
				if(!blogs[i].IsActive)
					blogs.RemoveAt(i);
			}
			return blogs;
		}

		/// <summary>
		/// Returns a <see cref="BlogConfigCollection"/> containing ACTIVE the <see cref="BlogConfig"/> 
		/// instances within the specified range.
		/// </summary>
		/// <param name="pageIndex">Page index.</param>
		/// <param name="pageSize">Size of the page.</param>
		/// <param name="sortDescending">Sort descending.</param>
		/// <returns></returns>
		public static BlogConfigCollection GetActiveBlogs(int pageIndex, int pageSize, bool sortDescending)
		{
			int totalCount;
			return GetActiveBlogs(pageIndex, pageSize, sortDescending, out totalCount);
		}

		/// <summary>
		/// Gets the blog by id.
		/// </summary>
		/// <param name="blogId">Blog id.</param>
		/// <returns></returns>
		public static BlogConfig GetBlogById(int blogId)
		{
			return DTOProvider.Instance().GetBlogById(blogId);
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
				if(_UrlFormats == null)
				{
					lock(urlLock)
					{
						if(_UrlFormats == null)
						{
							_UrlFormats = UrlFormatProvider.Instance(this.FullyQualifiedUrl);
						}
					}
				}
				return _UrlFormats;
			}
		}

		private string _imageDirectory;
		/// <summary>
		/// Gets or sets the image directory path.
		/// </summary>
		/// <value></value>
		public string ImageDirectory
		{
			get{return _imageDirectory;}
			set{_imageDirectory = value;}
		}

		private string _imagePath;
		/// <summary>
		/// Gets or sets the path to the image directory.
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

		private int _blogID = -1;
		/// <summary>
		/// Gets or sets the ID of the blog.  This is the 
		/// primary key in the blog_config table.
		/// </summary>
		/// <value></value>
		public int BlogID
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
			get{return _host;}
			set{_host = value.Replace("www.", string.Empty);}
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
		public bool EnableComments
		{
			get{return FlagPropertyCheck(ConfigurationFlag.EnableComments);}
			set{FlagSetter(ConfigurationFlag.EnableComments,value);}
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
		/// Gets or sets a value indicating whether this blog is active.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if it is active; otherwise, <c>false</c>.
		/// </value>
		public bool IsActive
		{
			get{return FlagPropertyCheck(ConfigurationFlag.IsActive);}
			set{FlagSetter(ConfigurationFlag.IsActive,value);}
		}

		private string _application;
		/// <summary>
		/// Gets or sets the application.
		/// </summary>
		/// <value></value>
		public string Application
		{
			get{return _application;}
			set
			{
				if(value != null)
					value = value.Replace("/", string.Empty); //For legacy data.
				_application = value;
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

		private string fullyQualifiedUrl;
		/// <summary>
		/// Gets or sets the fully qualified URL for 
		/// the blog.
		/// </summary>
		/// <value></value>
		public string FullyQualifiedUrl
		{
			get
			{
				return fullyQualifiedUrl;
			}
			set
			{
				if(value != null && value.StartsWith("http://"))
				{
					fullyQualifiedUrl = value;
				}
				else
				{
					fullyQualifiedUrl = "http://" + value;
				}
				if(!fullyQualifiedUrl.EndsWith("/"))
				{
					fullyQualifiedUrl += "/";
				}
			}
		}

		private ConfigurationFlag _flag = ConfigurationFlag.Empty;
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
		/// Returns the Application string without any dashes.
		/// </summary>
		/// <value></value>
		public string CleanApplication
		{
			get {return this.Application.Replace("/", string.Empty).Trim();}
			
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

			return ((BlogConfig)obj).BlogID == this.BlogID;
		}

		/// <summary>
		/// Serves as the hash function for the type <see cref="BlogConfig" />, 
		/// suitable for use in hashing functions.
		/// </summary>
		/// <returns></returns>
		public override int GetHashCode()
		{
			return this.Host.GetHashCode() ^ this.Application.GetHashCode();
		}
	}
}

