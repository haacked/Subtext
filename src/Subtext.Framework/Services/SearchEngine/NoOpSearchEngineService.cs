using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Subtext.Framework.Services.SearchEngine
{
    public class NoOpSearchEngineService: ISearchEngineService
    {
        #region ISearchEngineService Members

        public IEnumerable<IndexingError> AddPost(SearchEngineEntry post)
        {
            return AddPosts(new[] { post }, false);
        }

        public IEnumerable<IndexingError> AddPosts(IEnumerable<SearchEngineEntry> posts)
        {
            return AddPosts(posts, true);
        }

        public IEnumerable<IndexingError> AddPosts(IEnumerable<SearchEngineEntry> posts, bool optimize)
        {
            return new List<IndexingError>();
        }

        public IEnumerable<SearchEngineResult> Search(string queryString, int max, int blogId)
        {
            return Search(queryString, max, blogId, -1);
        }

        public IEnumerable<SearchEngineResult> Search(string queryString, int max, int blogId, int entryId)
        {
            return new List<SearchEngineResult>();
        }

        public void RemovePost(int postId)
        {
            
        }

        public int GetIndexedEntryCount(int blogId)
        {
            return 0;
        }

        public int GetTotalIndexedEntryCount()
        {
            return 0;
        }

        public IEnumerable<SearchEngineResult> RelatedContents(int entryId, int max, int blogId)
        {
            return new List<SearchEngineResult>();
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            
        }

        #endregion
    }
}
