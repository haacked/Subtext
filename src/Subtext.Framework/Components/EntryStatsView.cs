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
using Subtext.Extensibility;

namespace Subtext.Framework.Components
{
    /// <summary>
    /// Summary description for EntryStatsView.
    /// </summary>
    public class EntryStatsView : Entry
    {
        /// <summary>
        /// Creates a new <see cref="EntryStatsView"/> instance.
        /// </summary>
        public EntryStatsView() : base(PostType.None)
        {
        }

        public int WebCount { get; set; }

        public int AggCount { get; set; }

        public DateTime WebLastUpdated { get; set; }

        public DateTime AggLastUpdated { get; set; }
    }
}