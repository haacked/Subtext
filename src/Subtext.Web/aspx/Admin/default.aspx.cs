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
using Subtext.Framework.Components;

namespace Subtext.Web.Admin.Pages
{
    /// <summary>
    /// Summary description for _default.
    /// </summary>
    public class HomePageDefault : AdminPage
    {
        public HomePageDefault()
        {
            TabSectionId = "dashboard";
        }


        protected BlogStatistics Statistics { get; private set; }

        public int CategoryCount
        {
            get { return Repository.GetActiveCategories().Count; }
        }

        public int IndexedEntryCount
        {
            get { return SearchEngine.GetIndexedEntryCount(Blog.Id); }
        }

        protected override void OnLoad(EventArgs e)
        {
            Statistics = Repository.GetBlogStatistics(Blog.Id);
            base.OnLoad(e);
        }
    }
}