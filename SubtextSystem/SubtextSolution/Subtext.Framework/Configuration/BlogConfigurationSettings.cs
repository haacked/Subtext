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
using System.Configuration;
using System.Web;
using Subtext.Framework.Providers;

namespace Subtext.Framework.Configuration
{
	/// <summary>
	/// Returns settings straight from Web.config.
	/// </summary>
	[Serializable]
	public class BlogConfigurationSettings 
	{
		#region Static
		/// <summary>
		/// Returns a singleton instance of the settings.
		/// </summary>
		/// <returns></returns>
		public static BlogConfigurationSettings Instance()
		{
			return ((BlogConfigurationSettings)ConfigurationSettings.GetConfig("BlogConfigurationSettings"));

		}

		/// <summary>
		/// Returns a singleton instance of the settings.
		/// </summary>
		/// <param name="context">Context.</param>
		/// <returns></returns>
		public static BlogConfigurationSettings Instance(HttpContext context)
		{
			return Instance();
		}

		#endregion

		#region Properties 

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
		public bool AllowImages
		{
			get{return _allowImages;}
			set{_allowImages = value;}
		}

		private bool _useXHMTL = false;
		public bool UseXHTML
		{
			get{return _useXHMTL;}
			set{_useXHMTL = value;}
		}



		private int feedItemCount = 15;
		public int ItemCount
		{
			get{return feedItemCount;}
			set{feedItemCount = value;}
		}

		private int serverTimeZone = -5;
		public int ServerTimeZone
		{
			get{return serverTimeZone;}
			set{serverTimeZone = value;}
		}

		/// <summary>
		/// Gets or sets the days till comments close on a post.  
		/// The count starts when a post is created.
		/// </summary>
		/// <value></value>
		public int DaysTillCommentsClose
		{
			//TODO: Move this to BlogConfig and the blog_config table.
			get { return _daysTillCommentsClose; }
			set { _daysTillCommentsClose = value; }
		}

		int _daysTillCommentsClose = 30;

		#endregion

		private BlogProviders _blogProviders;
		public BlogProviders BlogProviders
		{
			get {return this._blogProviders;}
			set {this._blogProviders = value;}
		}

	}
}

