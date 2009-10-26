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
using System.Data;
using Subtext.Framework.Components;
using Subtext.Framework.Data;
using Subtext.Framework.Routing;

namespace Subtext.Framework.Providers
{
    public class SearchEngine
    {
        readonly Blog _blog;
        readonly StoredProcedures _procedures;
        readonly UrlHelper _urlHelper;

        public SearchEngine(Blog blog, UrlHelper urlHelper, string connectionString)
        {
            _blog = blog;
            _procedures = new StoredProcedures(connectionString);
            _urlHelper = urlHelper;
        }

        /// <summary>
        /// Searches the specified blog for items that match the search term.
        /// </summary>
        /// <param name="blogId"></param>
        /// <param name="searchTerm"></param>
        /// <returns></returns>
        public virtual ICollection<SearchResult> Search(int blogId, string searchTerm)
        {
            ICollection<SearchResult> results = new List<SearchResult>();

            using(IDataReader reader = _procedures.SearchEntries(blogId, searchTerm, _blog.TimeZone.Now))
            {
                while(reader.Read())
                {
                    Entry foundEntry = reader.ReadEntry(true);
                    results.Add(new SearchResult(foundEntry.Title, _urlHelper.EntryUrl(foundEntry).ToUri()));
                }
            }
            return results;
        }
    }
}