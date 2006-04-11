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
			return ObjectProvider.Instance().GetPagedLinks(categoryTypeID,pageIndex,pageSize,sortDescending);
		}

		#endregion

		#region LinkCollection

		public static LinkCollection GetLinkCollectionByPostID(int PostID)
		{
			return ObjectProvider.Instance().GetLinkCollectionByPostID(PostID);
		}

		public static LinkCollection GetLinksByCategoryID(int catID, bool ActiveOnly)
		{
			return ObjectProvider.Instance().GetLinksByCategoryID(catID,ActiveOnly);
		}

		#endregion

		#region Single Link

		public static Link GetSingleLink(int linkID)
		{
			return ObjectProvider.Instance().GetSingleLink(linkID);
		}

		#endregion

		#region LinkCategoryCollection

		public static LinkCategoryCollection GetCategories(CategoryType catType, bool ActiveOnly)
		{
			return ObjectProvider.Instance().GetCategories(catType, ActiveOnly);
		}

		public static LinkCategoryCollection GetActiveCategories()
		{
			return ObjectProvider.Instance().GetActiveCategories();
		}

		#endregion

		#region LinkCategory

		public static LinkCategory GetLinkCategory(int CategoryID, bool IsActive)
		{
			//TODO: We need to check this for null and throw a custom 
			//		exception in the case that the category does not exist.
			return ObjectProvider.Instance().GetLinkCategory(CategoryID,IsActive);
		}

		public static LinkCategory GetLinkCategory(string CategoryName, bool IsActive)
		{
			//TODO: We need to check this for null and throw a custom 
			//		exception in the case that the category does not exist.
			return ObjectProvider.Instance().GetLinkCategory(CategoryName,IsActive);
		}

		#endregion

		#region Edit Links/Categories

		public static bool UpdateLink(Link link)
		{
			return ObjectProvider.Instance().UpdateLink(link);
		}

		public static int CreateLink(Link link)
		{
			return ObjectProvider.Instance().CreateLink(link);
		}

		public static bool UpdateLinkCategory(LinkCategory lc)
		{
			return ObjectProvider.Instance().UpdateLinkCategory(lc);
		}
		
		public static int CreateLinkCategory(LinkCategory lc)
		{
			return ObjectProvider.Instance().CreateLinkCategory(lc);
		}

		public static bool DeleteLinkCategory(int CategoryID)
		{
			return ObjectProvider.Instance().DeleteLinkCategory(CategoryID);
		}

		public static bool DeleteLink(int LinkID)
		{
			return ObjectProvider.Instance().DeleteLink(LinkID);
		}

		#endregion

		
	}
}

