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
using Subtext.Extensibility;
using Subtext.Extensibility.Interfaces;
using Subtext.Framework;
using Subtext.Framework.Components;
using Subtext.Framework.Routing;

namespace Subtext.Web.UI.ViewModels
{
    public class EntryViewModel : IEntryIdentity
    {
        string _fullyQualifiedUrl;
        VirtualPath _url;

        public EntryViewModel(Entry entry, ISubtextContext context)
        {
            if (entry == null)
            {
                throw new ArgumentNullException("entry");
            }
            Entry = entry;
            SubtextContext = context;
        }

        protected Entry Entry { get; private set; }

        protected BlogUrlHelper UrlHelper
        {
            get { return SubtextContext.UrlHelper; }
        }

        protected ISubtextContext SubtextContext { get; private set; }

        public VirtualPath Url
        {
            get
            {
                if (_url == null)
                {
                    _url = UrlHelper.EntryUrl(Entry);
                }
                return _url;
            }
        }

        public string FullyQualifiedUrl
        {
            get
            {
                if (_fullyQualifiedUrl == null)
                {
                    _fullyQualifiedUrl = Url.ToFullyQualifiedUrl(SubtextContext.Blog).ToString();
                }
                return _fullyQualifiedUrl;
            }
        }

        public string Title
        {
            get { return Entry.Title; }
        }

        public bool AllowComments
        {
            get { return Entry.AllowComments; }
        }

        public bool CommentingClosed
        {
            get { return Entry.CommentingClosed; }
        }

        public int FeedBackCount
        {
            get { return Entry.FeedBackCount; }
        }

        public string EntryName
        {
            get { return Entry.EntryName; }
        }

        public DateTime DateSyndicated
        {
            get { return Entry.DateSyndicated; }
        }

        public DateTime DatePublishedUtc
        {
            get { return Entry.DatePublishedUtc; }
            set { Entry.DatePublishedUtc = value; }
        }


        public PostType PostType
        {
            get { return Entry.PostType; }
        }

        public int Id
        {
            get { return Entry.Id; }
        }
    }
}