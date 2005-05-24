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

using System;
using Subtext.Framework.Components;
using Subtext.Framework.Providers;

namespace Subtext.Framework
{
	/// <summary>
	/// Summary description for Links.
	/// </summary>
	public sealed class Links
	{
		private Links(){}

		#region Paged Links

		public static PagedLinkCollection GetPagedLinks(int categoryTypeID, int pageIndex, int pageSize, bool sortDescending)
		{
			return DTOProvider.Instance().GetPagedLinks(categoryTypeID,pageIndex,pageSize,sortDescending);
		}

		#endregion

		#region LinkCollection

		public static LinkCollection GetLinkCollectionByPostID(int PostID)
		{
			return DTOProvider.Instance().GetLinkCollectionByPostID(PostID);
		}

		public static LinkCollection GetLinksByCategoryID(int catID, bool ActiveOnly)
		{
			return DTOProvider.Instance().GetLinksByCategoryID(catID,ActiveOnly);
		}

		#endregion

		#region Single Link

		public static Link GetSingleLink(int linkID)
		{
			return DTOProvider.Instance().GetSingleLink(linkID);
		}

		#endregion

		#region LinkCategoryCollection

		public static LinkCategoryCollection GetCategories(CategoryType catType, bool ActiveOnly)
		{
			return DTOProvider.Instance().GetCategories(catType, ActiveOnly);
		}

		public static LinkCategoryCollection GetActiveCategories()
		{
			return DTOProvider.Instance().GetActiveCategories();
		}

		#endregion

		#region LinkCategory

		public static LinkCategory GetLinkCategory(int CategoryID, bool IsActive)
		{
			return DTOProvider.Instance().GetLinkCategory(CategoryID,IsActive);
		}

		public static LinkCategory GetLinkCategory(string CategoryName, bool IsActive)
		{
			return DTOProvider.Instance().GetLinkCategory(CategoryName,IsActive);
		}

		#endregion

		#region Edit Links/Categories

		public static bool UpdateLink(Link link)
		{
			return DTOProvider.Instance().UpdateLink(link);
		}

		public static int CreateLink(Link link)
		{
			return DTOProvider.Instance().CreateLink(link);
		}

		public static bool UpdateLinkCategory(LinkCategory lc)
		{
			return DTOProvider.Instance().UpdateLinkCategory(lc);
		}
		
		public static int CreateLinkCategory(LinkCategory lc)
		{
			return DTOProvider.Instance().CreateLinkCategory(lc);
		}

		public static bool DeleteLinkCategory(int CategoryID)
		{
			return DTOProvider.Instance().DeleteLinkCategory(CategoryID);
		}

		public static bool DeleteLink(int LinkID)
		{
			return DTOProvider.Instance().DeleteLink(LinkID);
		}

		#endregion

		
	}
}

