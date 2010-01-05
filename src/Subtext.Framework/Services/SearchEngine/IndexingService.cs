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

using System.Collections.Generic;
using System.Threading;
using Subtext.Extensibility;
using Subtext.Extensibility.Collections;
using Subtext.Framework.Components;
using Subtext.Framework.Providers;

namespace Subtext.Framework.Services.SearchEngine
{
    public class IndexingService : IIndexingService
    {
        public IndexingService(ISubtextContext subtextContext, ISearchEngineService searchEngine)
        {
            SubtextContext = subtextContext;
            SearchEngineService = searchEngine;
        }

        protected ObjectProvider Repository
        {
            get { return SubtextContext.Repository; }
        }

        public ISearchEngineService SearchEngineService { get; private set; }
        public ISubtextContext SubtextContext { get; private set; }

        public void RebuildIndexAsync()
        {
            ThreadPool.QueueUserWorkItem(delegate { RebuildIndex(); });
        }

        public IEnumerable<IndexingError> RebuildIndex()
        {
            return SearchEngineService.AddPosts(GetBlogPosts());
        }

        private IEnumerable<SearchEngineEntry> GetBlogPosts()
        {
            const int pageSize = 100;
            var collectionBook = new CollectionBook<EntryStatsView>((pageIndex, sizeOfPage) => Repository.GetEntries(PostType.BlogPost,null, pageIndex, sizeOfPage), pageSize);
            foreach (Entry entry in collectionBook.AsFlattenedEnumerable())
            {
                yield return entry.ConvertToSearchEngineEntry();
            }
        }
    }
}
