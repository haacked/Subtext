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
using System.Data;
using Subtext.Extensibility.Interfaces;
using Subtext.Framework.Components;

namespace Subtext.Framework.Data
{
    public partial class DatabaseObjectProvider
    {
        public override IPagedCollection<Link> GetPagedLinks(int? categoryId, int pageIndex, int pageSize, bool sortDescending)
        {
            //TODO: Update proc to allow for sort parameter.
            using (IDataReader reader = _procedures.GetPageableLinks(BlogId, categoryId, pageIndex, pageSize))
            {
                return reader.ReadPagedCollection(r => reader.ReadObject<Link>());
            }
        }

        public override ICollection<Link> GetLinkCollectionByPostId(int postId)
        {
            using (IDataReader reader = _procedures.GetLinkCollectionByPostID(postId, BlogId))
            {
                return reader.ReadCollection<Link>();
            }
        }

        public override Link GetLink(int linkId)
        {
            using (IDataReader reader = _procedures.GetSingleLink(linkId, BlogId))
            {
                Link link = null;
                while (reader.Read())
                {
                    link = reader.ReadObject<Link>();
                    break;
                }
                return link;
            }
        }

        /// <summary>
        /// Gets the categories for the specified category type.
        /// </summary>
        /// <param name="catType">Type of the cat.</param>
        /// <param name="activeOnly">if set to <c>true</c> [active only].</param>
        /// <returns></returns>
        public override ICollection<LinkCategory> GetCategories(CategoryType catType, bool activeOnly)
        {
            using (IDataReader reader = _procedures.GetCategory(null, null, activeOnly, BlogId, (int)catType))
            {
                return reader.ReadCollection(r => r.ReadLinkCategory());
            }
        }

        /// <summary>
        /// Gets the link category for the specified category id.
        /// </summary>
        /// <param name="categoryId">The category id.</param>
        /// <param name="activeOnly">if set to <c>true</c> [active only].</param>
        /// <returns></returns>
        public override LinkCategory GetLinkCategory(int? categoryId, bool activeOnly)
        {
            using (IDataReader reader = _procedures.GetCategory(null, categoryId, activeOnly, BlogId, null))
            {
                return ReadLinkCategory(reader);
            }
        }

        /// <summary>
        /// Gets the link category for the specified category name.
        /// </summary>
        /// <param name="categoryName">The category name.</param>
        /// <param name="activeOnly">if set to <c>true</c> [active only].</param>
        /// <returns></returns>
        public override LinkCategory GetLinkCategory(string categoryName, bool activeOnly)
        {
            using (IDataReader reader = _procedures.GetCategory(categoryName, null, activeOnly, BlogId, null))
            {
                return ReadLinkCategory(reader);
            }
        }

        public override int CreateLink(Link link)
        {
            int linkId = _procedures.InsertLink(link.Title,
                link.Url,
                link.Rss ?? string.Empty,
                link.IsActive,
                link.NewWindow,
                link.CategoryId,
                link.PostId.NullIfMinValue(),
                BlogId,
                link.Relation);
            link.Id = linkId;
            return linkId;
        }

        public override bool UpdateLink(Link link)
        {
            return _procedures.UpdateLink(link.Id,
                link.Title,
                link.Url,
                link.Rss ?? string.Empty,
                link.IsActive,
                link.NewWindow,
                link.CategoryId,
                link.Relation,
                BlogId);
        }

        public override bool UpdateLinkCategory(LinkCategory category)
        {
            return _procedures.UpdateCategory(category.Id,
                category.Title,
                category.IsActive,
                (int)category.CategoryType,
                category.Description ?? string.Empty,
                BlogId);
        }

        public override int CreateLinkCategory(LinkCategory lc)
        {
            return _procedures.InsertCategory(lc.Title,
                lc.IsActive,
                BlogId,
                (int)lc.CategoryType,
                lc.Description ?? string.Empty);
        }

        public override bool DeleteLinkCategory(int categoryId)
        {
            return _procedures.DeleteCategory(categoryId, BlogId);
        }

        public override bool DeleteLink(int linkId)
        {
            return _procedures.DeleteLink(linkId, BlogId);
        }
    }
}
