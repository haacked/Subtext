using System;

namespace Subtext.Framework.Services.SearchEngine
{
    public class IndexingError
    {
        public IndexingError(SearchEngineEntry entry, Exception exception)
        {
            Entry = entry;
            Exception = exception;
        }

        public SearchEngineEntry Entry { get; set; }
        public Exception Exception { get; set; }
    }
}