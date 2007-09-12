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
using System.Web;
using System.Web.Security;
using Subtext.Extensibility;
using Subtext.Extensibility.Interfaces;
using Subtext.Framework.Configuration;
using Subtext.Framework.Properties;
using System.Collections.Generic;

namespace Subtext.Framework.Components
{
	/// <summary>
	/// Summary description for Entry.
	/// </summary>
	[Serializable]
	public class Entry : IIdentifiable
	{
		/// <summary>
		/// Creates a new <see cref="Entry"/> instance.
		/// </summary>
		/// <param name="ptype">Ptype.</param>
		public Entry(PostType ptype)
		{
			_postType = ptype;
		}

		private int _blogID;
		/// <summary>
		/// Gets or sets the blog ID.
		/// </summary>
		/// <value>The blog ID.</value>
		public int BlogId
		{
			get { return _blogID; }
			set { _blogID = value; }
		}

		private int _entryid = NullValue.NullInt32;
		/// <summary>
		/// Gets or sets the entry ID.
		/// </summary>
		/// <value>The entry ID.</value>
		public int Id
		{
			get { return _entryid; }
			set { _entryid = value; }
		}

		/// <summary>
		/// Gets a value indicating whether this instance is updated.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if this instance is updated; otherwise, <c>false</c>.
		/// </value>
		public bool IsUpdated
		{
			get
			{
				return DateCreated != DateModified;
			}
		}

		/// <summary>
		/// Gets a value indicating whether this instance has description.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if this instance has description; otherwise, <c>false</c>.
		/// </value>
		public bool HasDescription
		{
			get
			{
				return ((Description != null) && (Description.Trim().Length > 0));
			}
		}

		/// <summary>
		/// Gets or sets the type of the post.
		/// </summary>
		/// <value>The type of the post.</value>
		public virtual PostType PostType
		{
			get
			{
				return _postType;
			}
			set
			{
				_postType = value;
			}
		}
		private PostType _postType = PostType.None;

		/// <summary>
		/// Gets or sets the description or excerpt for this blog post. 
		/// Some blogs like to sydicate description only.
		/// </summary>
		/// <value>The description.</value>
		public string Description
		{
			get { return _description; }
			set { _description = value; }
		}
		private string _description;

		/// <summary>
		/// Gets or sets the name of the entry.  This is used 
		/// to create a friendly URL for this entry.
		/// </summary>
		/// <value>The name of the entry.</value>
		public string EntryName
		{
			get { return _entryName; }
			set
			{
				//TODO: Validate the value and throw an exception if it 
				//		doesn't validate.  Here is the regex to use.
				//		^[a-z]*([a-z-_]+\.)*[a-z-_]+$
				_entryName = value;
			}
		}
		private string _entryName;

		/// <summary>
		/// Gets a value indicating whether this instance has entry name.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if this instance has entry name; otherwise, <c>false</c>.
		/// </value>
		public bool HasEntryName
		{
			get
			{
				return EntryName != null && EntryName.Trim().Length > 0;
			}
		}

		/// <summary>
		/// Gets or sets the title of this post.
		/// </summary>
		/// <value>The title.</value>
		public string Title
		{
			get { return _title; }
			set { _title = value; }
		}
		private string _title;

		/// <summary>
		/// Gets or sets the body of the Entry.  This is the 
		/// main content of the entry.
		/// </summary>
		/// <value></value>
		public string Body
		{
			get
			{
				return _body;
			}
			set
			{
				_body = value;
			}
		}
		private string _body;

		/// <summary>
		/// Gets or sets the author name of the entry.  
		/// For comments, this is the name given by the commenter. 
		/// </summary>
		/// <value>The author.</value>
		public MembershipUser Author
		{
			get
			{
				if (author == null)
				{
					if (authorId != Guid.Empty)
					{
						author = Membership.GetUser(authorId);
					}
				}
				return author;
			}
			set { author = value; }
		}
		private MembershipUser author;

		internal Guid authorId = Guid.Empty;

		/// <summary>
		/// Gets or sets the date this item was created.
		/// </summary>
		/// <value></value>
		public DateTime DateCreated
		{
			get
			{
				return _datecreated;
			}
			set { _datecreated = value; }
		}
		private DateTime _datecreated = NullValue.NullDateTime;

		/// <summary>
		/// Gets or sets the date this entry was last updated.
		/// </summary>
		/// <value></value>
		public DateTime DateModified
		{
			get
			{
				return _dateupated;
			}
			set { _dateupated = value; }
		}
		private DateTime _dateupated = NullValue.NullDateTime;

		/// <summary>
		/// Gets or sets the date the item was published.
		/// </summary>
		/// <value></value>
		public DateTime DateSyndicated
		{
			get { return _dateSyndicated; }
			set
			{
				if (NullValue.IsNull(value))
				{
					this.IncludeInMainSyndication = false;
				}
				_dateSyndicated = value;
			}
		}

		DateTime _dateSyndicated = NullValue.NullDateTime;

		/// <summary>
		/// Gets or sets a value indicating whether this entry is active.
		/// </summary>
		/// <value><c>true</c> if this instance is active; otherwise, <c>false</c>.</value>
		public bool IsActive
		{
			get { return EntryPropertyCheck(PostConfig.IsActive); }
			set { PostConfigSetter(PostConfig.IsActive, value); }
		}

		/// <summary>
		/// Gets or sets a value indicating whether this entry allows comments.
		/// </summary>
		/// <value><c>true</c> if [allows comments]; otherwise, <c>false</c>.</value>
		public bool AllowComments
		{
			get { return EntryPropertyCheck(PostConfig.AllowComments); }
			set { PostConfigSetter(PostConfig.AllowComments, value); }
		}

		/// <summary>
		/// Gets or sets a value indicating whether this entry is displayed on the home page.
		/// </summary>
		/// <value><c>true</c> if [display on home page]; otherwise, <c>false</c>.</value>
		public bool DisplayOnHomePage
		{
			get { return EntryPropertyCheck(PostConfig.DisplayOnHomePage); }
			set { PostConfigSetter(PostConfig.DisplayOnHomePage, value); }
		}

		/// <summary>
		/// Gets or sets a value indicating whether the description only should be syndicated.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if [syndicate description only]; otherwise, <c>false</c>.
		/// </value>
		public bool SyndicateDescriptionOnly
		{
			get { return EntryPropertyCheck(PostConfig.SyndicateDescriptionOnly); }
			set { PostConfigSetter(PostConfig.SyndicateDescriptionOnly, value); }
		}

		/// <summary>
		/// Gets or sets a value indicating whether [include in main syndication].
		/// </summary>
		/// <value>
		/// 	<c>true</c> if [include in main syndication]; otherwise, <c>false</c>.
		/// </value>
		public bool IncludeInMainSyndication
		{
			get
			{
				return EntryPropertyCheck(PostConfig.IncludeInMainSyndication);
			}
			set
			{
				if (value && NullValue.IsNull(DateSyndicated) && this.IsActive)
				{
					DateSyndicated = Config.CurrentBlog.TimeZone.Now;
				}
				PostConfigSetter(PostConfig.IncludeInMainSyndication, value);
			}
		}

		/// <summary>
		/// Whether or not this entry is aggregated.
		/// </summary>
		public bool IsAggregated
		{
			get { return EntryPropertyCheck(PostConfig.IsAggregated); }
			set { PostConfigSetter(PostConfig.IsAggregated, value); }
		}

		/// <summary>
		/// True if comments have been closed. Otherwise false.  Comments are closed 
		/// either explicitly or after by global age setting which overrides explicit settings
		/// </summary>
		public bool CommentingClosed
		{
			get
			{
				return (CommentingClosedByAge || EntryPropertyCheck(PostConfig.CommentsClosed));
			}
			set
			{
				// Closing By Age overrides explicit closing
				if (CommentingClosedByAge == false)
					PostConfigSetter(PostConfig.CommentsClosed, value);
			}
		}

		/// <summary>
		/// Returns true if the comments for this entry are closed due 
		/// to the age of the entry.  This is related to the DaysTillCommentsClose setting.
		/// </summary>
		public bool CommentingClosedByAge
		{
			get
			{
				if (Config.CurrentBlog.DaysTillCommentsClose == int.MaxValue)
					return false;

				return Config.CurrentBlog.TimeZone.Now > this.DateSyndicated.AddDays(Config.CurrentBlog.DaysTillCommentsClose);
			}
		}

		private string _url;
		/// <summary>
		/// Returns the relative URL to this entry.
		/// </summary>
		/// <value>The link.</value>
		public string Url
		{
			get
			{
				return _url;
			}
			set
			{
				_url = value;
				if (HttpContext.Current != null && !String.IsNullOrEmpty(value))
				{
					if (this.PostType == PostType.BlogPost || this.PostType == PostType.Story)
						_fullyQualifiedLink = new Uri(Config.CurrentBlog.UrlFormats.EntryFullyQualifiedUrl(this));
					else
					{
						string url = _url;
						if (url.StartsWith("/"))
							url = url.Remove(0, 1);
						_fullyQualifiedLink = new Uri(Config.CurrentBlog.RootUrl, url);
					}
				}
			}
		}

		/// <summary>
		/// Gets the fully qualified url to this entry.
		/// </summary>
		/// <value>The fully qualified link.</value>
		public Uri FullyQualifiedUrl
		{
			get
			{
				return _fullyQualifiedLink;
			}
		}
		Uri _fullyQualifiedLink;

		private int _feedBackCount;
		public int FeedBackCount
		{
			get { return _feedBackCount; }
			set { _feedBackCount = value; }
		}

		private PostConfig _PostConfig = PostConfig.None;

		public PostConfig PostConfig
		{
			get { return this._PostConfig; }
			set { this._PostConfig = value; }
		}

		protected bool EntryPropertyCheck(PostConfig ep)
		{
			return (this.PostConfig & ep) == ep;
		}

		protected void PostConfigSetter(PostConfig ep, bool select)
		{
			if (select)
			{
				this.PostConfig = PostConfig | ep;
			}
			else
			{
				this.PostConfig = PostConfig & ~ep;
			}
		}

		/// <summary>
		/// Calculates a simple checksum of the specified text.  
		/// This is used for comment filtering purposes. 
		/// Once deployed, this algorithm shouldn't change.
		/// </summary>
		/// <param name="text">Text.</param>
		/// <returns></returns>
		public static int CalculateChecksum(string text)
		{
			if (text == null)
				throw new ArgumentNullException("text", Resources.ArgumentNull_String);
			int checksum = 0;
			foreach (char c in text)
			{
				checksum += c;
			}
			return checksum;
		}

		/// <summary>
		/// Returns the categories for this entry.
		/// </summary>
		public StringCollection Categories
		{
			get { return this.categories; }
            set { this.categories = value; }
		}

		private StringCollection categories = new StringCollection();



		#region Plugin Specific Properties

		private IDictionary<Guid, Plugin> _enabledPlugins;

		public IDictionary<Guid, Plugin> EnabledPlugins
		{
			get
			{
				//if the list of plugins has not been retrieved for this BlogInfo
				//I need to retrieve it from the cache (or, if I'm not lucky, from the storage)
				if (_enabledPlugins == null)
				{
					_enabledPlugins = Plugin.GetEnabledPluginsWithEntrySettingsFromCache(_entryid);
				}
				return _enabledPlugins;
			}
		}

		#endregion Plugin Specific Properties



	}
}
