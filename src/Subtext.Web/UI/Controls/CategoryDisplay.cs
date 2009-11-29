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

namespace Subtext.Web.UI.Controls
{
    /// <summary>
    ///		Summary description for CategoryDisplayByColumn.
    /// </summary>
    public class CategoryDisplay : CachedColumnControl
    {
        protected CategoryList Categories;

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            Categories.LinkCategories = Repository.GetActiveCategories();
        }
    }
}