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
using Subtext.Framework.Components;

namespace Subtext.Framework.Services.SearchEngine
{
    public interface IIndexingService
    {
        /// <summary>
        /// Rebuilds the index for the current blog. This is done spinning another thread.
        /// </summary>
        void RebuildIndexAsync();
        /// <summary>
        /// Rebuilds the index for the current blog.
        /// </summary>
        IEnumerable<IndexingError> RebuildIndex();
        /// <summary>
        /// Adds a entry to the full text index
        /// </summary>
        /// <param name="entry">The Entry to be added</param>
        /// <returns>A list of possible errors</returns>
        IEnumerable<IndexingError> AddPost(Entry entry);
        /// <summary>
        /// Adds a entry to the full text index
        /// </summary>
        /// <param name="entry">The Entry to be added</param>
        /// <param name="tags">The List of tags</param>
        /// <returns>A list of possible errors</returns>
        IEnumerable<IndexingError> AddPost(Entry entry, IList<string> tags);
    }
}