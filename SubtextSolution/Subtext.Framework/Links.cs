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
using System.Collections.Generic;
using Subtext.Framework.Components;
using Subtext.Framework.Providers;

namespace Subtext.Framework
{ 
	/// <summary>
	/// Summary description for Links.
	/// </summary>
	public static class Links
	{
		#region Paged Links

        public static IPagedCollection<Link> GetPagedLinks(int categoryTypeID, int pageIndex, int pageSize, bool sortDescending)
		{
			return ObjectProvider.Instance().GetPagedLinks(categoryTypeID,pageIndex,pageSize,sortDescending);
		}

		#endregion

		#region LinkCollection

        public static ICollection<Link> GetLinkCollectionByPostID(int PostID)
		{
			return ObjectProvider.Instance().GetLinkCollectionByPostID(PostID);
		}

		public static ICollection<Link> GetLinksByCategoryID(int catID, bool activeOnly)
		{
			return ObjectProvider.Instance().GetLinksByCategoryID(catID, activeOnly);
		}

		#endregion

		#region Single Link

		public static Link GetSingleLink(int linkID)
		{
			return ObjectProvider.Instance().GetSingleLink(linkID);
		}

		#endregion

        #region ICollection<LinkCategory>

        public static ICollection<LinkCategory> GetCategories(CategoryType catType, ActiveFilter status)
		{
            return ObjectProvider.Instance().GetCategories(catType, status == ActiveFilter.ActiveOnly);
		}

        public static ICollection<LinkCategory> GetActiveCategories()
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
    
    public enum ActiveFilter
    {
        None,
        ActiveOnly,
        InactiveOnly,
    }
}

