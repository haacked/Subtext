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
using System.Linq;
using System.Web.UI;
using Subtext.Extensibility;
using Subtext.Framework;
using Subtext.Framework.Components;

namespace Subtext.Web.UI.Controls
{
    [PartialCaching(120, null, null, "Blogger", false)]
    public class HomePage : BaseControl
    {
        protected DayCollection HomePageDays;

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            HomePageDays.Days = GetHomePageEntries(Blog.ItemCount);
        }

        private IList<EntryDay> GetHomePageEntries(int itemCount)
        {
            return Repository.GetBlogPostsForHomePage(itemCount, PostConfig.DisplayOnHomepage | PostConfig.IsActive).ToList();
        }
    }
}