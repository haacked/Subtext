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
	/// Summary description for Configuration.
	/// </summary>
	[Serializable]
	public class BlogConfig : IBlogIdentifier
	{
		private object urlLock = new object();

		private UrlFormats _UrlFormats = null;
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
		public string ImageDirectory
		{
			get{return _imageDirectory;}
			set{_imageDirectory = value;}
		}

		private string _imagePath;
		public string ImagePath
		{
			get{return _imagePath;}
			set{_imagePath = value;}
		}
		

		private DateTime _lastupdated;
		public DateTime LastUpdated
		{
			get{return _lastupdated;}
			set{_lastupdated = value;}
		}

		private int _blogID = -1;
		public int BlogID
		{
			get{return _blogID;}
			set{_blogID = value;}
		}

		private int _timeZone = 0;
		public int TimeZone
		{
			get{return _timeZone;}
			set{_timeZone = value;}
		}

		private int _itemCount = 15;
		public int ItemCount
		{
			get{return _itemCount;}
			set{_itemCount = value;}
		}

		private string _language = "en-US";
		public string Language
		{
			get{return _language;}
			set{_language = value;}
		}

		private string _email;
		public string Email
		{
			get{return _email;}
			set{_email = value;}
		}

		private string _host;
		public string Host
		{
			get{return _host;}
			set{_host = value.Replace("www.", string.Empty);}
		}

		//not sure if this should be a persisted value or not
		private bool _isVirtual;
		public bool IsVirtual
		{
			get{return _isVirtual;}
			set{_isVirtual = value;}
		}


		public bool AllowServiceAccess
		{
			get{return FlagPropertyCheck(ConfigurationFlag.EnableServiceAccess);}
			set{FlagSetter(ConfigurationFlag.EnableServiceAccess,value);}
		}

		public bool IsPasswordHashed
		{
			get{return FlagPropertyCheck(ConfigurationFlag.IsPasswordHashed);}
			set{FlagSetter(ConfigurationFlag.IsPasswordHashed,value);}
		}

		public bool IsAggregated
		{
			get{return FlagPropertyCheck(ConfigurationFlag.IsAggregated);}
			set{FlagSetter(ConfigurationFlag.IsAggregated,value);}
		}

		public bool EnableComments
		{
			get{return FlagPropertyCheck(ConfigurationFlag.EnableComments);}
			set{FlagSetter(ConfigurationFlag.EnableComments,value);}
		}

		public bool IsActive
		{
			get{return FlagPropertyCheck(ConfigurationFlag.IsActive);}
			set{FlagSetter(ConfigurationFlag.IsActive,value);}
		}

		private string _application;
		public string Application
		{
			get{return _application;}
			set
			{
				_application = value;
				if(!_application.StartsWith("/"))
				{
					_application = "/" + _application;
				}
				if(!_application.EndsWith("/"))
				{
					_application = _application + "/";
				}
			}

		}

		private string _password;
		public string Password
		{
			get
			{
				if(_password == null)
				{
					ConfigException("Invalid Password Setting");
				}
				return _password;
			}
			set{_password = value;}
		}

		private string _username;
		public string UserName
		{
			get
			{
				if(_username == null)
				{
					ConfigException("Invalid UserName Setting");
				}
				return _username;
			}
			set{_username = value;}
		}

		private string _title;
		public string Title
		{
			get{return _title;}
			set{_title = value;}
		}

		private string _subtitle;
		public string SubTitle
		{
			get{return _subtitle;}
			set{_subtitle = value;}
		}

		private SkinConfig _skin;
		public SkinConfig Skin
		{
			get{return _skin;}
			set{_skin = value;}
		}


		public bool HasNews
		{
			get{ return News != null && News.Trim().Length > 0;}
		}

		private string news;
		public string News
		{
			get{return news;}
			set{news = value;}
		}

		private string _author = "Subtext Weblog";
		public string Author
		{
			get{return _author;}
			set{_author = value;}
		}
		

	
		private string fullyQualifiedUrl;
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
		public ConfigurationFlag Flag
		{
			get{return _flag;}
			set{_flag = value;}
		}

		public string CleanApplication
		{
			get {return this.Application.Replace("/",string.Empty).Trim();}
			
		}

		#region Counts 

		//These might need to go somewhere else.

		private int _postCount;
		public int PostCount
		{
			get {return this._postCount;}
			set {this._postCount = value;}
		}

		private int _commentCount;
		public int CommentCount
		{
			get {return this._commentCount;}
			set {this._commentCount = value;}
		}

		private int _storyCount;
		public int StoryCount
		{
			get {return this._storyCount;}
			set {this._storyCount = value;}
		}

		private int _pingTrackCount;
		public int PingTrackCount
		{
			get {return this._pingTrackCount;}
			set {this._pingTrackCount = value;}
		}

		#endregion

		#region Helper

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


		protected bool FlagPropertyCheck(ConfigurationFlag cf)
		{
			return (this.Flag & cf) == cf;
		}

		private void ConfigException(string message)
		{
			throw new Exception(message);
		}
		#endregion
	}
}

