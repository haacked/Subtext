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
using System.Linq;
using System.Web.UI.WebControls;
using Subtext.Framework;
using Subtext.Framework.Util;

namespace Subtext.Web.UI.Controls
{
    /// <summary>
    ///		Summary description for ArchiveMonth.
    /// </summary>
    public class ArchiveCategory : BaseControl
    {
        protected DayCollection Days;
        protected Literal Title;

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            int? categoryId = SubtextContext.RequestContext.GetIdFromRequest();
            if (categoryId != null)
            {
                Days.Days = Repository.GetBlogPostsByCategoryGroupedByDay(Blog.ItemCount, categoryId.Value).ToList();
            }
        }
    }
}