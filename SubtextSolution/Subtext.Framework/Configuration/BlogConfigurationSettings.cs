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
		/// Gets a value indicating whether or not to use XHTML.  This is 
		/// dependent on the DocTypeDeclaration chosen.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if using XHTML; otherwise, <c>false</c>.
		/// </value>
		[XmlIgnore]
		public bool UseXHTML
		{
			get
			{
				return this.DocTypeDeclaration != null 
					&& (this.DocTypeDeclaration.IndexOf("http://www.w3.org/TR/xhtml1/DTD/xhtml1-") > 0);
			}
		}

		private int feedItemCount = 15;
		/// <summary>
		/// Gets or sets the default number of items to display 
		/// for syndication feeds.
		/// </summary>
		/// <value></value>
		public int ItemCount
		{
			get{return feedItemCount;}
			set{feedItemCount = value;}
		}

		private int serverTimeZone = -5;
		/// <summary>
		/// Gets or sets the server time zone.
		/// </summary>
		/// <value></value>
		public int ServerTimeZone
		{
			get{return serverTimeZone;}
			set{serverTimeZone = value;}
		}

		/// <summary>
		/// Gets or sets the doc type declaration to use 
		/// at the top of each page.
		/// </summary>
		/// <value></value>
		public string DocTypeDeclaration
		{
			get { return _docTypeDeclaration; }
			set { _docTypeDeclaration = value; }
		}

		string _docTypeDeclaration;

		/// <summary>
		/// Gets the connection string for the application.
		/// </summary>
		/// <value></value>
		[XmlIgnore]
		public ConnectionString ConnectionString
		{
			get
			{
                if (_connectionString == null && System.Configuration.ConfigurationManager.AppSettings["ConnectionString"] != null)
                    _connectionString = System.Configuration.ConfigurationManager.AppSettings["ConnectionString"];

				return _connectionString;
			}
		}

		ConnectionString _connectionString = null;

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

