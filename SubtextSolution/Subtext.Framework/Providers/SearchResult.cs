using System;

namespace Subtext.Framework.Providers
{
    public class SearchResult
    {
        public SearchResult(string title, Uri url)
        {
            Title = title;
            Url = url;
        }

        public string Title
        {
            get;
            private set;
        }

        public Uri Url
        {
            get;
            private set;
        }
    }
}
