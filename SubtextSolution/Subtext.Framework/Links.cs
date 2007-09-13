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
		#region Paged Links

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

		#endregion

		#region LinkCollection

        public static ICollection<Link> GetLinkCollectionByPostID(int PostID)
		{
			return ObjectProvider.Instance().GetLinkCollectionByPostID(PostID);
		}

		#endregion

		#region Single Link

		public static Link GetSingleLink(int linkID)
		{
			return ObjectProvider.Instance().GetLink(linkID);
		}

		#endregion

        #region ICollection<LinkCategory>

        public static IList<LinkCategory> GetCategories(CategoryType catType, ActiveFilter status)
		{
            return ObjectProvider.Instance().GetCategories(catType, status == ActiveFilter.ActiveOnly);
		}

		/// <summary>
		/// Gets the active link collections along with their links.
		/// </summary>
		/// <returns></returns>
        public static IList<LinkCategory> GetActiveLinkCollections()
		{
			return ObjectProvider.Instance().GetActiveLinkCollections();
		}

        public static IList<LinkCategory> GetLinkCategoriesByPostId(int postId)
        {
            List<Link> links = new List<Link>(GetLinkCollectionByPostID(postId));
            IList<LinkCategory> postCategories = GetCategories(CategoryType.PostCollection, ActiveFilter.None);
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

		/// <summary>
		/// Updates the specified link in the database.
		/// </summary>
		/// <param name="link"></param>
		public static void UpdateLink(Link link)
		{
			ObjectProvider.Instance().UpdateLink(link);
		}

		/// <summary>
		/// Creates the link.
		/// </summary>
		/// <param name="link">The link.</param>
		/// <returns></returns>
		public static int CreateLink(Link link)
		{
			int linkId = ObjectProvider.Instance().CreateLink(link);
			link.Id = linkId;
			return linkId;
		}

		/// <summary>
		/// Updates the link category.
		/// </summary>
		/// <param name="lc">The lc.</param>
		/// <returns></returns>
		public static void UpdateLinkCategory(LinkCategory lc)
		{
			ObjectProvider.Instance().UpdateLinkCategory(lc);
		}

		/// <summary>
		/// Creates the link category.
		/// </summary>
		/// <param name="lc">The lc.</param>
		/// <returns></returns>
		public static int CreateLinkCategory(LinkCategory lc)
		{
			return ObjectProvider.Instance().CreateLinkCategory(lc);
		}

		/// <summary>
		/// Deletes the link category.
		/// </summary>
		/// <param name="CategoryID">The category ID.</param>
		/// <returns></returns>
		public static void DeleteLinkCategory(int CategoryID)
		{
			ObjectProvider.Instance().DeleteLinkCategory(CategoryID);
		}

		public static void DeleteLink(int linkId)
		{
			ObjectProvider.Instance().DeleteLink(linkId);
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



