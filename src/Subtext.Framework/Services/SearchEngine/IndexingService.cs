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
using Subtext.Framework.Util;

namespace Subtext.Framework.Services.SearchEngine
{
    public class IndexingService : IIndexingService
    {
        public IndexingService(ISubtextContext subtextContext, ISearchEngineService searchEngine)
        {
            SubtextContext = subtextContext;
            SearchEngineService = searchEngine;
        }

        protected ObjectRepository Repository
        {
            get { return SubtextContext.Repository; }
        }

        public ISearchEngineService SearchEngineService { get; private set; }
        public ISubtextContext SubtextContext { get; private set; }

        public void RebuildIndexAsync()
        {
            ThreadHelper.FireAndForget(o => RebuildIndex(), "Error while rebuilding index");
        }

        public IEnumerable<IndexingError> RebuildIndex()
        {
            return SearchEngineService.AddPosts(GetBlogPosts());
        }

        private IEnumerable<SearchEngineEntry> GetBlogPosts()
        {
            const int pageSize = 100;
            var collectionBook = new CollectionBook<EntryStatsView>((pageIndex, sizeOfPage) => Repository.GetEntries(PostType.BlogPost,null, pageIndex, sizeOfPage), pageSize);
            foreach (var entry in collectionBook.AsFlattenedEnumerable())
            {
                if(entry.IsActive)
                    yield return entry.ConvertToSearchEngineEntry();
            }
        }

        public IEnumerable<IndexingError> AddPost(Entry entry)
        {
            return ExecuteAddPost(entry.ConvertToSearchEngineEntry());
        }

        public IEnumerable<IndexingError> AddPost(Entry entry, IList<string> tags)
        {
            return ExecuteAddPost(entry.ConvertToSearchEngineEntry(tags));
        }

        private IEnumerable<IndexingError> ExecuteAddPost(SearchEngineEntry entry)
        {
            if (entry.IsPublished)
                return SearchEngineService.AddPost(entry);
            SearchEngineService.RemovePost(entry.EntryId);
            return new List<IndexingError>();
        }
    }
}
