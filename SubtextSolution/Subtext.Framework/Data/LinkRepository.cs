using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Subtext.Framework.Text;
using Subtext.Framework.Components;
using Subtext.Extensibility.Interfaces;

namespace Subtext.Framework.Data
{
    public partial class DatabaseObjectProvider
    {
        public override IPagedCollection<Link> GetPagedLinks(int categoryId, int pageIndex, int pageSize, bool sortDescending)
        {
            //TODO: Update proc to allow for sort parameter.
            using (IDataReader reader = _procedures.GetPageableLinks(BlogId, categoryId, pageIndex, pageSize))
            {
                return reader.GetPagedCollection(r => DataHelper.LoadLink(r));
            }
        }

        public override ICollection<Link> GetLinkCollectionByPostID(int postId)
        {
            using (IDataReader reader = _procedures.GetLinkCollectionByPostID(postId, BlogId))
            {
                ICollection<Link> lc = new List<Link>();
                while (reader.Read())
                {
                    lc.Add(DataHelper.LoadLink(reader));
                }
                return lc;
            }
        }

        public override Link GetLink(int linkId)
        {
            using (IDataReader reader = _procedures.GetSingleLink(linkId, BlogId))
            {
                Link link = null;
                while (reader.Read())
                {
                    link = DataHelper.LoadLink(reader);
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
            ICollection<LinkCategory> lcc = new List<LinkCategory>();
            using (IDataReader reader = _procedures.GetCategory(null, null, activeOnly, BlogId, (int)catType))
            {
                while (reader.Read())
                {
                    lcc.Add(DataHelper.LoadLinkCategory(reader));
                }
                return lcc;
            }
        }

        /// <summary>
        /// Gets the link category for the specified category id.
        /// </summary>
        /// <param name="categoryId">The category id.</param>
        /// <param name="activeOnly">if set to <c>true</c> [active only].</param>
        /// <returns></returns>
        public override LinkCategory GetLinkCategory(int categoryId, bool activeOnly)
        {
            using (IDataReader reader = _procedures.GetCategory(null, categoryId.NullIfMinValue(), activeOnly, BlogId, null))
            {
                return reader.LoadLinkCategoryFromReader();
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
                return reader.LoadLinkCategoryFromReader();
            }
        }

        public override int CreateLink(Link link)
        {
            int linkId = _procedures.InsertLink(link.Title,
                link.Url,
                link.Rss ?? string.Empty,
                link.IsActive,
                link.NewWindow,
                link.CategoryID,
                link.PostID.NullIfMinValue(),
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
                link.CategoryID,
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
