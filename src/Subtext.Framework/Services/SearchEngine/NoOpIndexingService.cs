using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Subtext.Framework.Services.SearchEngine
{
    public class NoOpIndexingService: IIndexingService
    {
        #region IIndexingService Members

        public void RebuildIndexAsync()
        {
            
        }

        public IEnumerable<IndexingError> RebuildIndex()
        {
            var errors = new List<IndexingError>();
            errors.Add(new IndexingError(new SearchEngineEntry() { EntryId=0 }, new NotSupportedException("The Search Engine has been disabled. Please contact the webmaster if you need further assistence")));
            return errors;
        }

        public IEnumerable<IndexingError> AddPost(Subtext.Framework.Components.Entry entry)
        {
            return new List<IndexingError>();
        }

        public IEnumerable<IndexingError> AddPost(Subtext.Framework.Components.Entry entry, IList<string> tags)
        {
            return new List<IndexingError>();
        }

        #endregion
    }
}
