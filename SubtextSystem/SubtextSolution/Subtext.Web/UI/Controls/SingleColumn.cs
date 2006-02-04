using System;
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
	///		Summary description for CategoryDisplayByColumn.
	/// </summary>
	public  class SingleColumn : CachedColumnControl
	{
		protected Subtext.Web.UI.Controls.CategoryList Categories;

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad (e);
			
			Categories.LinkCategories = GetArchiveCategories();
		}

		protected LinkCategoryCollection GetArchiveCategories()
		{
			LinkCategoryCollection lcc = new LinkCategoryCollection();

			lcc.Add(UIData.Links(CategoryType.StoryCollection,CurrentBlog.UrlFormats));			
			lcc.Add(UIData.ArchiveMonth(CurrentBlog.UrlFormats));
			lcc.Add(UIData.Links(CategoryType.PostCollection,CurrentBlog.UrlFormats));
			lcc.Add(UIData.Links(CategoryType.ImageCollection,CurrentBlog.UrlFormats));
			lcc.AddRange(Links.GetActiveCategories());
			return lcc;
		}
	}
}

