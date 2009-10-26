#region Disclaimer/Info

///////////////////////////////////////////////////////////////////////////////////////////////////
// Subtext WebLog
// 
// Subtext is an open source weblog system that is a fork of the .TEXT
// weblog system.
//
// For updated news and information please visit http://subtextproject.com/
// Subtext is hosted at Google Code at http://code.google.com/p/subtext/
// The development mailing list is at subtext@googlegroups.com 
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

        public static ICollection<LinkCategory> GetLinkCategoriesByPostId(int postId)
        {
            var links = new List<Link>(ObjectProvider.Instance().GetLinkCollectionByPostId(postId));
            ICollection<LinkCategory> postCategories =
                ObjectProvider.Instance().GetCategories(CategoryType.PostCollection, true /* activeOnly */);
            var categories = new LinkCategory[postCategories.Count];
            postCategories.CopyTo(categories, 0);

            foreach(LinkCategory category in categories)
            {
                LinkCategory innerCategory = category;
                if(!links.Exists(link => innerCategory.Id == link.CategoryId))
                {
                    postCategories.Remove(category);
                }
            }
            return postCategories;
        }

        public static int CreateLink(Link link)
        {
            return ObjectProvider.Instance().CreateLink(link);
        }

        public static int CreateLinkCategory(LinkCategory lc)
        {
            lc.Id = ObjectProvider.Instance().CreateLinkCategory(lc);
            return lc.Id;
        }

        public static bool DeleteLinkCategory(int categoryId)
        {
            return ObjectProvider.Instance().DeleteLinkCategory(categoryId);
        }

        public static bool DeleteLink(int linkId)
        {
            return ObjectProvider.Instance().DeleteLink(linkId);
        }
    }

    public enum ActiveFilter
    {
        None,
        ActiveOnly,
        InactiveOnly,
    }
}