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
    /// <summary>
	///		Summary description for CategoryDisplayByColumn.
	/// </summary>
	public class ArchiveLinks : CachedColumnControl
	{
		protected CategoryList Categories;

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad (e);
			Categories.LinkCategories = GetArchiveCategories();
		}

		protected ICollection<LinkCategory> GetArchiveCategories()
		{
            List<LinkCategory> lcc = new List<LinkCategory>();
		    LinkCategory lc;
		    
            // we want to make sure that the LinkCategory is NOT null before we add it to the collection.
            lc = UIData.Links(CategoryType.PostCollection, Blog.UrlFormats);
		    if (lc != null)
		    {
		        lcc.Add(lc);
		    }
            lc = UIData.Links(CategoryType.StoryCollection, Blog.UrlFormats);
		    if (lc != null)
		    {
		        lcc.Add(lc);
		    }
			lc = UIData.ArchiveMonth(Url);
            if (lc != null)
            {
                lcc.Add(lc);
            }
			lc = UIData.Links(CategoryType.ImageCollection, Blog.UrlFormats);
            if (lc != null)
            {
                lcc.Add(lc);
            }
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

