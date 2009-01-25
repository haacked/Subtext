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
using Subtext.Extensibility.Interfaces;
using Subtext.Framework.Components;
using Subtext.Framework.Providers;

namespace Subtext.Framework
{ 
	/// <summary>
	/// Summary description for Links.
	/// </summary>
	public static class Links
	{
		/// <summary>
		/// Returns a pageable collection of Link instances for the specified category.
		/// </summary>
		/// <param name="categoryId"></param>
		/// <param name="pageIndex"></param>
		/// <param name="pageSize"></param>
		/// <param name="sortDescending"></param>
		/// <returns></returns>
        public static IPagedCollection<Link> GetPagedLinks(int categoryId, int pageIndex, int pageSize, bool sortDescending)
		{
			return ObjectProvider.Instance().GetPagedLinks(categoryId, pageIndex, pageSize, sortDescending);
		}

        public static ICollection<Link> GetLinkCollectionByPostID(int PostID)
		{
			return ObjectProvider.Instance().GetLinkCollectionByPostID(PostID);
		}

		public static Link GetSingleLink(int linkID)
		{
			return ObjectProvider.Instance().GetLink(linkID);
		}

        #region ICollection<LinkCategory>

        public static ICollection<LinkCategory> GetCategories(CategoryType catType, ActiveFilter status)
		{
            return ObjectProvider.Instance().GetCategories(catType, status == ActiveFilter.ActiveOnly);
		}

        public static ICollection<LinkCategory> GetActiveCategories()
		{
			return ObjectProvider.Instance().GetActiveCategories();
		}

        public static ICollection<LinkCategory> GetLinkCategoriesByPostID(int postId)
        {
            List<Link> links = new List<Link>(GetLinkCollectionByPostID(postId));
            ICollection<LinkCategory> postCategories = GetCategories(CategoryType.PostCollection, ActiveFilter.None);
            LinkCategory[] categories = new LinkCategory[postCategories.Count];
            postCategories.CopyTo(categories, 0);

            foreach (LinkCategory category in categories)
            {
                if (!links.Exists(
                    delegate(Link link)
                    {
                        return (category.Id == link.CategoryID);
                    }
                    ))
                {
                    postCategories.Remove(category);
                }
            }
            return postCategories;
        }

		#endregion

		#region LinkCategory

		public static LinkCategory GetLinkCategory(int categoryId, bool isActive)
		{
			//TODO: We need to check this for null and throw a custom 
			//		exception in the case that the category does not exist.
			return ObjectProvider.Instance().GetLinkCategory(categoryId, isActive);
		}

		public static LinkCategory GetLinkCategory(string categoryName, bool isActive)
		{
			//TODO: We need to check this for null and throw a custom 
			//		exception in the case that the category does not exist.
			return ObjectProvider.Instance().GetLinkCategory(categoryName, isActive);
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
			lc.Id = ObjectProvider.Instance().CreateLinkCategory(lc);
            return lc.Id;
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



