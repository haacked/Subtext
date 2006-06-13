using System;
using System.Collections.Generic;
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
	using System;


	/// <summary>
	///		Summary description for CategoryDisplayByColumn.
	/// </summary>
	public class ArchiveLinks : CachedColumnControl
	{
		protected Subtext.Web.UI.Controls.CategoryList Categories;

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad (e);
			Categories.LinkCategories = GetArchiveCategories();
		}

		protected ICollection<LinkCategory> GetArchiveCategories()
		{
            List<LinkCategory> lcc = new List<LinkCategory>();

			lcc.Add(UIData.Links(CategoryType.PostCollection,CurrentBlog.UrlFormats));
			lcc.Add(UIData.Links(CategoryType.StoryCollection,CurrentBlog.UrlFormats));
			lcc.Add(UIData.ArchiveMonth(CurrentBlog.UrlFormats));
			lcc.Add(UIData.Links(CategoryType.ImageCollection,CurrentBlog.UrlFormats));
			return lcc;
		}

		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		///		Required method for Designer support - do not modify
		///		the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			

		}
		#endregion
	}
}

