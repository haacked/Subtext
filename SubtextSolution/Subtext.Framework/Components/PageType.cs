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
    /// <summary>
    /// Summary description for PageType.
    /// </summary>
    public enum PageType
    {
        //0 = HomePage, 1 = RSS, 2 = Date,3 = Post, 4 = Story, 5 = Other
        HomePage = 0,
        RSS = 1,
        Date = 2,
        Post = 3,
        Story = 4,
        Other = 5,
        ImagePage = 6,
        NotSpecified = NullValue.NullInt32
    } ;
}