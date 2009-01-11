using System;
using System.Collections.Generic;
using Subtext.Framework;
using Subtext.Framework.Components;

#region Disclaimer/Info
///////////////////////////////////////////////////////////////////////////////////////////////////
// Subtext WebLog
// 
// Subtext is an open source weblog system that is a fork of the .TEXT
// weblog system.
//
// For updated news and information please visit http://subtextproject.com/
// Subtext is hosted at SourceForge at http://sourceforge.net/projects/subtext
// The development mailing list is at subtext-devs@lists.sourceforge.net 
//
// This project is licensed under the BSD license.  See the License.txt file for more information.
///////////////////////////////////////////////////////////////////////////////////////////////////
#endregion

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
		protected Subtext.Web.UI.Controls.CategoryList Categories;

		/// <summary>
		/// Raises the <see cref="E:System.Web.UI.Control.Load"/>
		/// event.  Then calls <see cref="GetArchiveCategories"/> to 
		/// populate the <see cref="CategoryList"/> control.
		/// </summary>
		/// <param name="e">The <see cref="T:System.EventArgs"/> object that contains the event data.</param>
		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad (e);
			Categories.LinkCategories = GetArchiveCategories();
		}

		protected ICollection<LinkCategory> GetArchiveCategories()
		{
            List<LinkCategory> lcc = new List<LinkCategory>();

			LinkCategory storyCollection = UIData.Links(CategoryType.StoryCollection, Blog.UrlFormats);
			if(storyCollection != null)
				lcc.Add(storyCollection);

			LinkCategory archiveMonth = UIData.ArchiveMonth(Url);
			if (archiveMonth != null)
				lcc.Add(archiveMonth);

            /*   
             * Uncomment this to test the Category Archive with post count
             * and comment to following block of code 
             */ 

            //LinkCategory archiveCategories = UIData.ArchiveCategory(CurrentBlog.UrlFormats);
            //if (archiveCategories != null)
            //    lcc.Add(archiveCategories);
            

			LinkCategory postCollection = UIData.Links(CategoryType.PostCollection, Blog.UrlFormats);
			if (postCollection != null)
                lcc.Add(postCollection);

			LinkCategory imageCollection = UIData.Links(CategoryType.ImageCollection, Blog.UrlFormats);
			if (imageCollection != null)
                lcc.Add(imageCollection);
			
			lcc.AddRange(Links.GetActiveCategories());
			return lcc;
		}
	}
}

