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
    ///		Summary description for CategoryDisplayByColumn.
    /// </summary>
    public class ArchiveLinks : CachedColumnControl
    {
        protected CategoryList Categories;

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            Categories.LinkCategories = GetArchiveCategories(SubtextContext.Blog);
        }

        protected ICollection<LinkCategory> GetArchiveCategories(Blog blog)
        {
            var linkCategories = new List<LinkCategory>();

            // we want to make sure that the LinkCategory is NOT null before we add it to the collection.
            LinkCategory category = Repository.Links(CategoryType.PostCollection, blog, Url);
            if (category != null)
            {
                linkCategories.Add(category);
            }
            category = Repository.Links(CategoryType.StoryCollection, blog, Url);
            if (category != null)
            {
                linkCategories.Add(category);
            }
            category = Repository.ArchiveMonth(Url, blog);
            if (category != null)
            {
                linkCategories.Add(category);
            }
            category = Repository.Links(CategoryType.ImageCollection, blog, Url);
            if (category != null)
            {
                linkCategories.Add(category);
            }
            return linkCategories;
        }
    }
}