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
using System.IO;
using Subtext.Framework.Components;

namespace Subtext.Framework.Syndication
{
    /// <summary>
    /// Summary description for CategoryWriter.
    /// </summary>
    public class CategoryWriter : RssWriter
    {
        /// <summary>
        /// Creates a new <see cref="CategoryWriter"/> instance.
        /// </summary>
        public CategoryWriter(TextWriter writer, ICollection<Entry> ec, LinkCategory lc, Uri url,
                              ISubtextContext context) : base(writer, ec, NullValue.NullDateTime, false, context)
        {
            Category = lc;
            Url = url;
        }

        public LinkCategory Category { get; set; }

        public Uri Url { get; set; }

        //TODO: implement dateLastViewedFeedItemPublished
        //TODO: Implement useDeltaEncoding

        protected override void WriteChannel()
        {
            if(Category == null)
            {
                base.WriteChannel();
            }
            else
            {
                BuildChannel(Category.Title, Url, Blog.Email,
                             Category.HasDescription ? Category.Description : Category.Title, Blog.Language, Blog.Author,
                             Blog.LicenseUrl);
            }
        }
    }
}