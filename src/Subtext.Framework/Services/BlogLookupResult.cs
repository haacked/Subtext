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

namespace Subtext.Framework.Services
{
    public class BlogLookupResult
    {
        public BlogLookupResult(Blog blog, Uri alternateUrl)
        {
            Blog = blog;
            AlternateUrl = alternateUrl;
        }

        /// <summary>
        /// The found blog. Null if not found.
        /// </summary>
        public Blog Blog { get; private set; }

        /// <summary>
        /// No blog was found, redirect to this alternate URL instead.
        /// </summary>
        public Uri AlternateUrl { get; private set; }
    }
}