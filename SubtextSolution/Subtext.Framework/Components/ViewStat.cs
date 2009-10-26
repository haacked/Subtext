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

namespace Subtext.Framework.Components
{
    /// <summary>
    /// Summary description for ViewStat.
    /// </summary>
    [Serializable]
    public class ViewStat
    {
        public ViewStat()
        {
            PageType = PageType.NotSpecified;
        }

        public string PageTitle { get; set; }

        public int ViewCount { get; set; }

        public DateTime ViewDate { get; set; }

        public PageType PageType { get; set; }

        public int BlogId { get; set; }
    }
}