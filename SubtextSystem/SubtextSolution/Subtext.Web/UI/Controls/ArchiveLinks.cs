using System;
using Subtext.Framework.Components;

#region Disclaimer/Info
///////////////////////////////////////////////////////////////////////////////////////////////////
// .Text WebLog
// 
// .Text is an open source weblog system started by Scott Watermasysk. 
// Blog: http://ScottWater.com/blog 
// RSS: http://scottwater.com/blog/rss.aspx
// Email: Dottext@ScottWater.com
//
// For updated news and information please visit http://scottwater.com/dottext and subscribe to 
// the Rss feed @ http://scottwater.com/dottext/rss.aspx
//
// On its release (on or about August 1, 2003) this application is licensed under the BSD. However, I reserve the 
// right to change or modify this at any time. The most recent and up to date license can always be fount at:
// http://ScottWater.com/License.txt
// 
// Please direct all code related questions to:
// GotDotNet Workspace: http://www.gotdotnet.com/Community/Workspaces/workspace.aspx?id=e99fccb3-1a8c-42b5-90ee-348f6b77c407
// Yahoo Group http://groups.yahoo.com/group/DotText/
// 
///////////////////////////////////////////////////////////////////////////////////////////////////
#endregion

namespace Subtext.Web.UI.Controls
{
	using System;


	/// <summary>
	///		Summary description for CategoryDisplayByColumn.
	/// </summary>
	public  class ArchiveLinks : CachedColumnControl
	{
		protected Subtext.Web.UI.Controls.CategoryList Categories;

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad (e);
			Categories.LinkCategories = GetArchiveCategories();
		}

		protected LinkCategoryCollection GetArchiveCategories()
		{
			//string cacheKey = this.ControlCacheKey;
			//LinkCategoryCollection lcc = (LinkCategoryCollection)Cache[cacheKey];
			//if(lcc == null)
			//{
				LinkCategoryCollection lcc = new LinkCategoryCollection();

				lcc.Add(UIData.Links(CategoryType.PostCollection,CurrentBlog.UrlFormats));
				lcc.Add(UIData.Links(CategoryType.StoryCollection,CurrentBlog.UrlFormats));

				lcc.Add(UIData.ArchiveMonth(CurrentBlog.UrlFormats));

				lcc.Add(UIData.Links(CategoryType.ImageCollection,CurrentBlog.UrlFormats));
//				Cacher.CacherCache(cacheKey,Context,lcc,CacheTime.Medium);
//			}

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

