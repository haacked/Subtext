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

namespace Subtext.Framework.Components
{
    public enum CategoryType : byte
    {
        None = 0,
        PostCollection = 1,
        StoryCollection = 2,
        ImageCollection = 3,
        ArchiveMonthCollection = 4,
        LinkCollection = 5,
    }
}