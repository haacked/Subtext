#region Disclaimer/Info

///////////////////////////////////////////////////////////////////////////////////////////////////
// Subtext WebLog
// 
// Subtext is an open source weblog system that is a fork of the .TEXT
// weblog system.
//
// For updated news and information please visit http://subtextproject.com/
// Subtext is hosted at Google Code at http://code.google.com/p/subtext/
// The development mailing list is at subtext-devs@lists.sourceforge.net 
//
// This project is licensed under the BSD license.  See the License.txt file for more information.
///////////////////////////////////////////////////////////////////////////////////////////////////

#endregion

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
        public static ICollection<LinkCategory> GetCategories(CategoryType catType, ActiveFilter status)
        {
            return ObjectProvider.Instance().GetCategories(catType, status == ActiveFilter.ActiveOnly);
        }

        public static ICollection<LinkCategory> GetLinkCategoriesByPostID(int postId)
        {
            var links = new List<Link>(ObjectProvider.Instance().GetLinkCollectionByPostID(postId));
            ICollection<LinkCategory> postCategories =
                ObjectProvider.Instance().GetCategories(CategoryType.PostCollection, true /* activeOnly */);
            var categories = new LinkCategory[postCategories.Count];
            postCategories.CopyTo(categories, 0);

            foreach(LinkCategory category in categories)
            {
                if(!links.Exists(link => category.Id == link.CategoryID))
                {
                    postCategories.Remove(category);
                }
            }
            return postCategories;
        }

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