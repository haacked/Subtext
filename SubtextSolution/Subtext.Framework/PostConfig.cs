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
using System.Diagnostics.CodeAnalysis;

namespace Subtext.Extensibility
{
    /// <summary>
    /// Think of PostConfig items as filters. These values are often used in the 
    /// WHERE clause of stored procedures, for example.
    /// </summary>
    [SuppressMessage("Microsoft.Naming", "CA1714:FlagsEnumsShouldHavePluralNames")]
    [Flags]
    public enum PostConfig
    {
        None = 0, //no filter. Therefore if getting items, all items will be gotten.
        IsActive = 1, //filter returns only the active items
        IsXhtml = 2,
        AllowComments = 4,
        DisplayOnHomepage = 8,
        IncludeInMainSyndication = 16,
        SyndicateDescriptionOnly = 32,
        IsAggregated = 64,
        CommentsClosed = 128,
        NeedsModeratorApproval = 256,
    }
}