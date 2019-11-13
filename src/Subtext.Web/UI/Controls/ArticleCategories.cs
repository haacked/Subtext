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
using Subtext.Framework;
using Subtext.Framework.Components;

namespace Subtext.Web.UI.Controls
{
    /// <summary>
    ///		Summary description for ArticleCategories.
    /// </summary>
    public class ArticleCategories : BaseControl
    {
        protected CategoryList Categories;

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            Categories.LinkCategories = GetArchiveCategories(SubtextContext.Blog);
        }

        protected ICollection<LinkCategory> GetArchiveCategories(Blog blog)
        {
            return new List<LinkCategory> { Repository.Links(CategoryType.StoryCollection, blog, Url) };
        }
    }
}