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
using System.Linq;
using Subtext.Framework.Components;
using Subtext.Framework.Text;

namespace Subtext.Framework.Services.SearchEngine
{
    public static class EntryExtensionMethods
    {
        /// <summary>
        /// Converts a blog entry to the document ready for being indexed.
        /// </summary>
        /// <param name="entry">the <see cref="Entry"/> to convert</param>
        /// <param name="tags">list of tags</param>
        /// <returns>the model in the format required by the indexing service</returns>
        public static SearchEngineEntry ConvertToSearchEngineEntry(this Entry entry, IEnumerable<string> tags)
        {
            return new SearchEngineEntry()
                       {
                           BlogId = entry.BlogId,
                           BlogName = entry.Blog.Title,
                           Body = HtmlHelper.RemoveHtml(entry.Body),
                           GroupId = entry.Blog.BlogGroupId,
                           IsPublished = entry.IsActive,
                           EntryId = entry.Id,
                           PublishDate = entry.DateSyndicated,
                           Tags = String.Join(",",tags.ToArray()),
                           Title = entry.Title,
                           EntryName = entry.EntryName
                       };
        }

        /// <summary>
        /// Converts a blog entry to the document ready for being indexed.
        /// Parses the body of the entry looking for tags.
        /// </summary>
        /// <param name="entry">the <see cref="Entry"/> to convert</param>
        /// <returns>the model in the format required by the indexing service</returns>
        public static SearchEngineEntry ConvertToSearchEngineEntry(this Entry entry)
        {
            return entry.ConvertToSearchEngineEntry(entry.Body.ParseTags());
        }
    }
}
