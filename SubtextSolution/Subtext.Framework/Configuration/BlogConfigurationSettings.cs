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
using System.Collections.Specialized;
using System.Configuration;
using System.Xml.Serialization;
using Subtext.Scripting;

namespace Subtext.Framework.Configuration
{
	/// <summary>
	/// Contains various configuration settings stored in the 
	/// web.config file.
	/// </summary>
	[Serializable]
	public class BlogConfigurationSettings 
	{
		private Tracking _tracking;

		public Tracking Tracking
		{
			get 
			{
				if(this._tracking == null)
				{
					this._tracking = new Tracking();
				}
				return this._tracking;
			}
			    set {this._tracking = value;}
		}

		private bool _useWWW;
		public bool UseWWW
		{
			get {return this._useWWW;}
			set {this._useWWW = value;}
		}

		private int _queuedThreads = 2;
		public int QueuedThreads
		{
			get {return this._queuedThreads;}
			set {this._queuedThreads = value;}
		}

		private bool _allowserviceaccess;
		public bool AllowServiceAccess
		{
			get{return _allowserviceaccess;}
			set{_allowserviceaccess = value;}
		}

		public bool AllowScriptsInPosts
		{
			get { return this.allowScriptsInPosts; }
			set{ this.allowScriptsInPosts = value; }
		}
		bool allowScriptsInPosts;

		private bool _useHashedPasswords;
		public bool UseHashedPasswords
		{
			get {return this._useHashedPasswords;}
			set {this._useHashedPasswords = value;}
		}

		private bool _allowImages;
		/// <summary>
		/// Gets or sets a value indicating whether or not to allow images.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if [allow images]; otherwise, <c>false</c>.
		/// </value>
		public bool AllowImages
		{
			get{return _allowImages;}
			set{_allowImages = value;}
		}

		/// <summary>
		/// Gets or sets the default number of items to display 
		/// for syndication feeds.
		/// </summary>
		/// <value></value>
		public int ItemCount
		{
			get{return this.feedItemCount;}
			set{this.feedItemCount = value;}
		}
		private int feedItemCount = 15;


		/// <summary>
		/// Gets or sets the number of posts to display 
		/// on the category list pages.
		/// </summary>
		/// <value></value>
		public int CategoryListPostCount
		{
			get{return this.categoryListPostCount;}
			set{this.categoryListPostCount = value;}
		}
		private int categoryListPostCount = 10;

		/// <summary>
		/// Gets or sets the server time zone.
		/// </summary>
		/// <value></value>
		public int ServerTimeZone
		{
			get{return serverTimeZone;}
			set{serverTimeZone = value;}
		}
		private int serverTimeZone = -2037797565; //PST

		/// <summary>
		/// Gets a value indicating whether invisible captcha enabled.  This is 
		/// configured within the "InvisibleCaptchaEnabled" app setting. It is not 
		/// set per blog, but system-wide. This gives hosters a way to opt-out of 
		/// this feature in case it ends up being problematci.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if [invisible captcha enabled]; otherwise, <c>false</c>.
		/// </value>
		public bool InvisibleCaptchaEnabled
		{
			get
			{
				if (String.IsNullOrEmpty(ConfigurationManager.AppSettings["InvisibleCaptchaEnabled"]))
					return true;
				
				bool enabled;
				if (bool.TryParse(ConfigurationManager.AppSettings["InvisibleCaptchaEnabled"], out enabled))
					return enabled;
				return true;
			}
		}

		/// <summary>
		/// Gets the connection string for the application.
		/// </summary>
		/// <value></value>
		[XmlIgnore]
		public ConnectionString ConnectionString
		{
			get
			{
                if (_connectionString == null && ConfigurationManager.ConnectionStrings["subtextData"] != null)
                    _connectionString = ConfigurationManager.ConnectionStrings["subtextData"].ConnectionString;

				return _connectionString;
			}
		}

		ConnectionString _connectionString;

		/// <summary>
		/// Returns a <see cref="NameValueCollection"/> containing the allowed 
		/// HTML tags within a user comment.  The value contains a comma 
		/// separated list of allowed attributes.
		/// </summary>
		/// <value>The allowed HTML tags.</value>
		[XmlIgnore]
		public NameValueCollection AllowedHtmlTags
		{
			get
			{
				if(this.allowedHtmlTags == null)
				{
                    this.allowedHtmlTags = ((NameValueCollection)(ConfigurationManager.GetSection("AllowableCommentHtml")));
				}
				return this.allowedHtmlTags;
			}
		}

		private NameValueCollection allowedHtmlTags = null;
	}
}

