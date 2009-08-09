using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Subtext.Framework.Components;
using Subtext.Framework;
using Subtext.Framework.Routing;
using Subtext.Extensibility.Interfaces;

namespace Subtext.Web.UI.ViewModels
{
    public class EntryViewModel : IEntryIdentity
    {
        public EntryViewModel(Entry entry, ISubtextContext context) {
            Entry = entry;
            SubtextContext = context;
        }

        protected Entry Entry
        {
            get;
            private set;
        }

        protected UrlHelper UrlHelper {
            get {
                return SubtextContext.UrlHelper;
            }
        }

        protected ISubtextContext SubtextContext
        {
            get;
            private set;
        }

        public VirtualPath Url {
            get {
                if (_url == null) { 
                    _url = UrlHelper.EntryUrl(Entry);
                }
                return _url;
            }
        }
        VirtualPath _url;

        public string FullyQualifiedUrl
        {
            get {
                if (_fullyQualifiedUrl == null) {
                    _fullyQualifiedUrl = Url.ToFullyQualifiedUrl(SubtextContext.Blog).ToString();
                }
                return _fullyQualifiedUrl;
            }
        }
        string _fullyQualifiedUrl = null;

        public string EntryName
        {
            get {
                return Entry.EntryName;
            }
        }

        public DateTime DateCreated
        {
            get {
                return Entry.DateCreated;
            }
        }

        public Subtext.Extensibility.PostType PostType
        {
            get {
                return Entry.PostType;
            }
        }

        public int Id
        {
            get {
                return Entry.Id;
            }
        }

        public string Title {
            get {
                return Entry.Title;
            }
        }

        public bool AllowComments {
            get {
                return Entry.AllowComments;
            }
        }

        public bool CommentingClosed {
            get {
                return Entry.CommentingClosed;
            }
        }

        public int FeedBackCount
        {
            get {
                return Entry.FeedBackCount;
            }
        }
    }
}
