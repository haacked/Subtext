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
    ///	<para>
    ///	This control displays links by categories.  Categories 
    ///	include "Archives", "Image Galleries", And custom link Categories.
    ///	</para>
    ///	<para>
    ///	Makes use of the <see cref="CategoryList"/> control.
    ///	</para>
    /// </summary>
    public class SingleColumn : CachedColumnControl
    {
        protected CategoryList Categories;

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Load"/>
        /// event.  Then calls <see cref="GetArchiveCategories"/> to 
        /// populate the <see cref="CategoryList"/> control.
        /// </summary>
        /// <param name="e">The <see cref="T:System.EventArgs"/> object that contains the event data.</param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            Categories.LinkCategories = GetArchiveCategories(SubtextContext.Blog);
        }

        protected ICollection<LinkCategory> GetArchiveCategories(Blog blog)
        {
            var lcc = new List<LinkCategory>();

            LinkCategory storyCollection = Repository.Links(CategoryType.StoryCollection, blog, Url);
            if (storyCollection != null)
            {
                lcc.Add(storyCollection);
            }

            LinkCategory archiveMonth = Repository.ArchiveMonth(Url, blog);
            if (archiveMonth != null)
            {
                lcc.Add(archiveMonth);
            }

            /*   
             * Uncomment this to test the Category Archive with post count
             * and comment to following block of code 
             */

            //LinkCategory archiveCategories = UIHelpers.ArchiveCategory(CurrentBlog.UrlFormats);
            //if (archiveCategories != null)
            //    lcc.Add(archiveCategories);


            LinkCategory postCollection = Repository.Links(CategoryType.PostCollection, blog, Url);
            if (postCollection != null)
            {
                lcc.Add(postCollection);
            }

            LinkCategory imageCollection = Repository.Links(CategoryType.ImageCollection, blog, Url);
            if (imageCollection != null)
            {
                lcc.Add(imageCollection);
            }

            lcc.AddRange(Repository.GetActiveCategories());
            return lcc;
        }
    }
}