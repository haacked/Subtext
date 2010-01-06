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
using Subtext.Framework.Configuration;

namespace Subtext.Framework.Services.SearchEngine
{
    public interface ISearchEngineService: IDisposable
    {
        /// <summary>
        /// Adds an entry to the full text index
        /// </summary>
        /// <param name="post">The Entry</param>
        IEnumerable<IndexingError> AddPost(SearchEngineEntry post);
        /// <summary>
        /// Adds many entries to the full text index.
        /// This optimizes the index after adding the posts.
        /// </summary>
        /// <param name="posts">List of entries</param>
        IEnumerable<IndexingError> AddPosts(IEnumerable<SearchEngineEntry> posts);
        /// <summary>
        /// Adds many entries to the full text index
        /// </summary>
        /// <param name="posts">List of entries</param>
        /// <param name="optimize">False to not optimize the index after adding the posts</param>
        IEnumerable<IndexingError> AddPosts(IEnumerable<SearchEngineEntry> posts, bool optimize);
        /// <summary>
        /// Search the full text index by query string
        /// </summary>
        /// <param name="queryString">the query string</param>
        /// <param name="max">Max number of results to retrieve</param>
        /// <param name="blogId">The id of the blog being searched</param>
        /// <returns></returns>
        IEnumerable<SearchEngineResult> Search(string queryString, int max, int blogId);
        /// <summary>
        /// Search the full text index by query string
        /// </summary>
        /// <param name="queryString">the query string</param>
        /// <param name="max">Max number of results to retrieve</param>
        /// <param name="blogId">The id of the blog being searched</param>
        /// <param name="entryId">The id of the entry that must be filtered out of the results (-1 if none)</param>
        /// <returns></returns>
        IEnumerable<SearchEngineResult> Search(string queryString, int max, int blogId, int entryId);
        /// <summary>
        /// Removes an entry from the index.
        /// </summary>
        /// <param name="postId">Id of the entry</param>
        void RemovePost(int postId);
        /// <summary>
        /// Gets the number of entries indexed for a blog.
        /// </summary>
        /// <param name="blogId">Id of the blog</param>
        /// <returns></returns>
        int GetIndexedEntryCount(int blogId);
        /// <summary>
        /// Gets the number of entries available in the whole index
        /// </summary>
        /// <returns></returns>
        int GetTotalIndexedEntryCount();
        /// <summary>
        /// Returns all entries "similar" to the entry specified by the id.
        /// </summary>
        /// <param name="entryId">The id of the Entry</param>
        /// <param name="max">The maximum number of results to return</param>
        /// <param name="blogId">The id of the blog being searched</param>
        /// <returns></returns>
        IEnumerable<SearchEngineResult> RelatedContents(int entryId, int max, int blogId);

    }
}