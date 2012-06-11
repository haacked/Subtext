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

namespace Subtext.Framework.Syndication
{
    public abstract class BaseSyndicationWriter<T> : BaseSyndicationWriter
    {
        /// <summary>
        /// Creates a new <see cref="BaseSyndicationWriter"/> instance.
        /// </summary>
        protected BaseSyndicationWriter(TextWriter writer, DateTime dateLastViewedFeedItemPublished,
                                        bool useDeltaEncoding, ISubtextContext context)
            : base(writer, dateLastViewedFeedItemPublished, useDeltaEncoding, context)
        {
        }

        /// <summary>
        /// Gets or sets the entries to be rendered in the feed.
        /// </summary>
        /// <value>The entries.</value>
        public ICollection<T> Items { get; set; }
    }
}