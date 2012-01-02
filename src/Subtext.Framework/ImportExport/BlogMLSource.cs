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
using System.Globalization;
using System.Linq;
using BlogML.Xml;
using Subtext.Extensibility.Collections;
using Subtext.Framework;
using Subtext.Framework.Components;

namespace Subtext.ImportExport
{
    public class BlogMLSource : IBlogMLSource
    {
        public BlogMLSource(ISubtextContext context, IBlogMLExportMapper blogMLConverter)
        {
            SubtextContext = context;
            BlogMLConverter = blogMLConverter;
        }

        protected ISubtextContext SubtextContext
        {
            get;
            private set;
        }

        protected IBlogMLExportMapper BlogMLConverter
        {
            get;
            private set;
        }

        public BlogMLBlog GetBlog()
        {
            BlogMLBlog blog = BlogMLConverter.ConvertBlog(SubtextContext.Blog);
            blog.Categories.AddRange(Categories);
            return blog;
        }

        protected IList<BlogMLCategory> Categories
        {
            get
            {
                if (_categories == null)
                {

                    var categories = SubtextContext.Repository.GetCategories(CategoryType.PostCollection, false /*activeOnly*/);
                    if (categories != null && categories.Count > 0)
                    {
                        _categories = BlogMLConverter.ConvertCategories(categories).ToList();
                    }
                }
                _categories = _categories ?? new List<BlogMLCategory>();
                return _categories;
            }
        }

        IList<BlogMLCategory> _categories;

        protected Dictionary<string, BlogMLCategory> CategoryByTitleLookup
        {
            get
            {
                // We need to build this lookup dictionary because an Entry only contains a collection
                // of Category titles and not the actual categories. :(
                if (_categoryByTitleLookup == null)
                {
                    _categoryByTitleLookup = new Dictionary<string, BlogMLCategory>();
                    foreach (var category in Categories)
                    {
                        _categoryByTitleLookup.Add(category.Title, category);
                    }
                }
                return _categoryByTitleLookup;
            }
        }

        Dictionary<string, BlogMLCategory> _categoryByTitleLookup;

        public IEnumerable<BlogMLPost> GetBlogPosts(bool embedAttachments)
        {
            const int pageSize = 100;
            var collectionBook = new CollectionBook<EntryStatsView>((pageIndex, sizeOfPage) => SubtextContext.Repository.GetEntriesForExport(pageIndex, sizeOfPage), pageSize);
            foreach (var entry in collectionBook.AsFlattenedEnumerable())
            {
                var post = BlogMLConverter.ConvertEntry(entry, embedAttachments);
                foreach (var categoryTitle in entry.Categories)
                {
                    post.Categories.Add(CategoryByTitleLookup[categoryTitle].ID.ToString(CultureInfo.InvariantCulture));
                }
                yield return post;
            }
        }
    }
}
