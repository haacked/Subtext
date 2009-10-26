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
    /// Generates RSS
    /// </summary>
    public class AtomWriter : BaseAtomWriter
    {
        /// <summary>
        /// Creates a new <see cref="AtomWriter"/> instance.
        /// </summary>
        public AtomWriter(TextWriter writer, ICollection<Entry> entries, DateTime dateLastViewedFeedItemPublished,
                          bool useDeltaEncoding, ISubtextContext context)
            : base(writer, dateLastViewedFeedItemPublished, useDeltaEncoding, context)
        {
            Items = entries;
            UseAggBugs = true;
        }
    }
}