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
using Subtext.Extensibility;
using Subtext.Extensibility.Interfaces;
using Subtext.Framework.Configuration;

namespace Subtext.Framework.Components
{
    /// <summary>
    /// Summary description for Entry.
    /// </summary>
    [Serializable]
    public class Entry : IEntryIdentity
    {
        DateTime _dateSyndicated = NullValue.NullDateTime;

        public Entry(PostType postType, Blog blog)
        {
            Categories = new List<string>();
            PostConfig = PostConfig.None;
            DateModified = NullValue.NullDateTime;
            DateCreated = NullValue.NullDateTime;
            PostType = postType;
            Blog = blog;
            Id = NullValue.NullInt32;
        }

        public Entry(PostType postType)
            : this(postType, Config.CurrentBlog)
        {
        }

        /// <summary>
        /// Gets or sets the blog ID.
        /// </summary>
        /// <value>The blog ID.</value>
        public int BlogId { get; set; }

        public Blog Blog { get; set; }

        /// <summary>
        /// Gets a value indicating whether this instance has description.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance has description; otherwise, <c>false</c>.
        /// </value>
        public bool HasDescription
        {
            get { return !String.IsNullOrEmpty(Description); }
        }

        /// <summary>
        /// Gets or sets the description or excerpt for this blog post. 
        /// Some blogs like to sydicate description only.
        /// </summary>
        /// <value>The description.</value>
        public string Description
        { //todo: let's rename this property to excerpt.
            get;
            set;
        }

        /// <summary>
        /// Gets a value indicating whether this instance has entry name.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance has entry name; otherwise, <c>false</c>.
        /// </value>
        public bool HasEntryName
        {
            get { return EntryName != null && EntryName.Trim().Length > 0; }
        }

        /// <summary>
        /// Gets or sets the title of this post.
        /// </summary>
        /// <value>The title.</value>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the body of the Entry.  This is the 
        /// main content of the entry.
        /// </summary>
        /// <value></value>
        public string Body { get; set; }

        /// <summary>
        /// Gets or sets the author name of the entry.  
        /// For comments, this is the name given by the commenter. 
        /// </summary>
        /// <value>The author.</value>
        public string Author { get; set; }

        /// <summary>
        /// Gets or sets the email of the author.
        /// </summary>
        /// <value>The email.</value>
        public string Email { get; set; }


        /// <summary>
        /// Gets or sets the date this entry was last updated.
        /// </summary>
        /// <value></value>
        public DateTime DateModified { get; set; }

        /// <summary>
        /// Gets or sets the date the item was published.
        /// </summary>
        /// <value></value>
        public DateTime DateSyndicated
        {
            get { return _dateSyndicated; }
            set
            {
                if(NullValue.IsNull(value))
                {
                    IncludeInMainSyndication = false;
                }
                _dateSyndicated = value;
            }
        }

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
            get { return EntryPropertyCheck(PostConfig.DisplayOnHomepage); }
            set { PostConfigSetter(PostConfig.DisplayOnHomepage, value); }
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
                if(!CommentingClosedByAge)
                {
                    PostConfigSetter(PostConfig.CommentsClosed, value);
                }
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
                if(Blog.DaysTillCommentsClose == int.MaxValue)
                {
                    return false;
                }

                return Blog.TimeZone.Now > DateSyndicated.AddDays(Blog.DaysTillCommentsClose);
            }
        }

        public int FeedBackCount { get; set; }

        public PostConfig PostConfig { get; set; }

        /// <summary>
        /// Returns the categories for this entry.
        /// </summary>
        public ICollection<string> Categories { get; private set; }

        /// <summary>
        /// Gets and sets the enclosure for the entry.
        /// </summary>
        public Enclosure Enclosure { get; set; }

        /// <summary>
        /// Gets or sets the entry ID.
        /// </summary>
        /// <value>The entry ID.</value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the type of the post.
        /// </summary>
        /// <value>The type of the post.</value>
        public PostType PostType { get; set; }

        /// <summary>
        /// Gets or sets the name of the entry.  This is used 
        /// to create a friendly URL for this entry.
        /// </summary>
        /// <value>The name of the entry.</value>
        public string EntryName { get; set; }

        /// <summary>
        /// Gets or sets the date this item was created.
        /// </summary>
        /// <value></value>
        public DateTime DateCreated { get; set; }

        protected bool EntryPropertyCheck(PostConfig ep)
        {
            return (PostConfig & ep) == ep;
        }

        protected void PostConfigSetter(PostConfig ep, bool select)
        {
            if(select)
            {
                PostConfig = PostConfig | ep;
            }
            else
            {
                PostConfig = PostConfig & ~ep;
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
            if(text == null)
            {
                throw new ArgumentNullException("text");
            }
            int checksum = 0;
            foreach(char c in text)
            {
                checksum += c;
            }
            return checksum;
        }

        public ICollection<FeedbackItem> Comments
        {
            get
            {
                if(_comments == null)
                {
                    _comments = new List<FeedbackItem>();
                }
                return _comments;
            }
        }

        List<FeedbackItem> _comments;
    }
}