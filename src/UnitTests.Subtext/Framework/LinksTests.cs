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
using System.Linq;
using System.Web;
using MbUnit.Framework;
using Subtext.Framework;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;
using Subtext.Framework.Data;
using Subtext.Framework.Providers;

namespace UnitTests.Subtext.Framework
{
    /// <summary>
    /// Unit tests of Subtext.Framework.Links class methods
    /// </summary>
    [TestFixture]
    public class LinksTests
    {
        [Test]
        [RollBack2]
        public void CanGetCategoriesByPostId()
        {
            UnitTestHelper.SetupBlog();
            var repository = new DatabaseObjectProvider();
            int category1Id =
                repository.CreateLinkCategory(CreateCategory("Post Category 1", "Cody roolz!", CategoryType.PostCollection,
                                                        true));
            int category2Id =
                repository.CreateLinkCategory(CreateCategory("Post Category 2", "Cody roolz again!",
                                                        CategoryType.PostCollection, true));
            repository.CreateLinkCategory(CreateCategory("Post Category 3", "Cody roolz and again!",
                                                    CategoryType.PostCollection, true));

            Entry entry = UnitTestHelper.CreateEntryInstanceForSyndication("phil", "title", "body");
            int entryId = UnitTestHelper.Create(entry);
            repository.SetEntryCategoryList(entryId, new[] { category1Id, category2Id });

            ICollection<LinkCategory> categories = repository.GetLinkCategoriesByPostId(entryId);
            Assert.AreEqual(2, categories.Count, "Expected two of the three categories");

            Assert.AreEqual(category1Id, categories.First().Id);
            Assert.AreEqual(category2Id, categories.ElementAt(1).Id);

            Assert.AreEqual(Config.CurrentBlog.Id, categories.First().BlogId);
        }

        [Test]
        [RollBack2]
        public void CanGetActiveCategories()
        {
            UnitTestHelper.SetupBlog();
            var repository = new DatabaseObjectProvider();
            int[] categoryIds = CreateSomeLinkCategories(repository);
            CreateLink(repository, "Link one", categoryIds[0], null);
            CreateLink(repository, "Link two", categoryIds[0], null);
            CreateLink(repository, "Link one-two", categoryIds[1], null);

            ICollection<LinkCategory> linkCollections = repository.GetActiveCategories();

            //Test ordering by title
            Assert.AreEqual("Google Blogs", linkCollections.First().Title);
            Assert.AreEqual("My Favorite Feeds", linkCollections.ElementAt(1).Title);

            //Check link counts
            Assert.AreEqual(1, linkCollections.First().Links.Count);
            Assert.AreEqual(2, linkCollections.ElementAt(1).Links.Count);
        }

        [Test]
        [RollBack2]
        public void CanUpdateLink()
        {
            UnitTestHelper.SetupBlog();
            var repository = new DatabaseObjectProvider();
            // Create the categories
            CreateSomeLinkCategories(repository);

            int categoryId =
                repository.CreateLinkCategory(CreateCategory("My Favorite Feeds", "Some of my favorite RSS feeds",
                                                        CategoryType.LinkCollection, true));
            Link link = CreateLink(repository, "Test", categoryId, null);
            int linkId = link.Id;

            Link loaded = repository.GetLink(linkId);
            Assert.AreEqual("Test", loaded.Title);

            Entry entry = UnitTestHelper.CreateEntryInstanceForSyndication("test", "test", "test");

            //Make changes then update.
            link.PostId = entry.Id;
            link.Title = "Another title";
            link.NewWindow = true;
            repository.UpdateLink(link);
            loaded = repository.GetLink(linkId);
            Assert.AreEqual("Another title", loaded.Title);
            Assert.IsTrue(loaded.NewWindow);
            Assert.AreEqual(entry.Id, loaded.PostId);
        }

        [Test]
        [RollBack2]
        public void CanCreateAndDeleteLink()
        {
            UnitTestHelper.SetupBlog();
            var repository = new DatabaseObjectProvider();
            int categoryId =
                repository.CreateLinkCategory(CreateCategory("My Favorite Feeds", "Some of my favorite RSS feeds",
                                                        CategoryType.LinkCollection, true));

            Link link = CreateLink(repository, "Title", categoryId, null);
            int linkId = link.Id;

            Link loaded = repository.GetLink(linkId);
            Assert.AreEqual("Title", loaded.Title);
            Assert.AreEqual(NullValue.NullInt32, loaded.PostId);
            Assert.AreEqual(Config.CurrentBlog.Id, loaded.BlogId);

            repository.DeleteLink(linkId);

            Assert.IsNull(repository.GetLink(linkId));
        }

        [Test]
        [RollBack2]
        public void CanCreateAndDeleteLinkCategory()
        {
            UnitTestHelper.SetupBlog();
            var repository = new DatabaseObjectProvider();

            // Create some categories
            int categoryId =
                repository.CreateLinkCategory(CreateCategory("My Favorite Feeds", "Some of my favorite RSS feeds",
                                                        CategoryType.LinkCollection, true));

            LinkCategory category = repository.GetLinkCategory(categoryId, true);
            Assert.AreEqual(Config.CurrentBlog.Id, category.BlogId);
            Assert.AreEqual("My Favorite Feeds", category.Title);
            Assert.AreEqual("Some of my favorite RSS feeds", category.Description);
            Assert.IsTrue(category.HasDescription);
            Assert.IsFalse(category.HasLinks);
            Assert.IsFalse(category.HasImages);
            Assert.IsTrue(category.IsActive);
            Assert.AreEqual(CategoryType.LinkCollection, category.CategoryType);
            Assert.IsNotNull(category);

            repository.DeleteLinkCategory(categoryId);
            Assert.IsNull(repository.GetLinkCategory(categoryId, true));
        }

        /// <summary>
        /// Ensures CreateLinkCategory assigns unique CatIDs
        /// </summary>
        [Test]
        [RollBack2]
        public void CreateLinkCategoryAssignsUniqueCatIDs()
        {
            UnitTestHelper.SetupBlog();
            var repository = new DatabaseObjectProvider();
            // Create some categories
            CreateSomeLinkCategories(repository);
            ICollection<LinkCategory> linkCategoryCollection = repository.GetCategories(CategoryType.LinkCollection,
                                                                                   ActiveFilter.None);

            LinkCategory first = null;
            LinkCategory second = null;
            LinkCategory third = null;
            foreach (LinkCategory linkCategory in linkCategoryCollection)
            {
                if (first == null)
                {
                    first = linkCategory;
                    continue;
                }

                if (second == null)
                {
                    second = linkCategory;
                    continue;
                }

                if (third == null)
                {
                    third = linkCategory;
                    continue;
                }
            }

            // Ensure the CategoryIDs are unique
            Assert.AreNotEqual(first.Id, second.Id);
            Assert.AreNotEqual(first.Id, third.Id);
            Assert.AreNotEqual(second.Id, third.Id);
        }

        [Test]
        [RollBack2]
        public void CanGetPostCollectionCategories()
        {
            UnitTestHelper.SetupBlog();
            var repository = new DatabaseObjectProvider();
            CreateSomePostCategories(repository);

            // Retrieve the categories, grab the first one and update it
            ICollection<LinkCategory> originalCategories = repository.GetCategories(CategoryType.PostCollection,
                                                                               ActiveFilter.None);
            Assert.IsTrue(originalCategories.Count > 0);
        }

        /// <summary>
        /// Ensure UpdateLInkCategory updates the correct link category
        /// </summary>
        [Test]
        [RollBack2]
        public void UpdateLinkCategoryIsFine()
        {
            UnitTestHelper.SetupBlog();
            var repository = new DatabaseObjectProvider();
            // Create the categories
            CreateSomeLinkCategories(repository);

            // Retrieve the categories, grab the first one and update it
            ICollection<LinkCategory> originalCategories = repository.GetCategories(CategoryType.LinkCollection,
                                                                               ActiveFilter.None);
            Assert.Greater(originalCategories.Count, 0, "Expected some categories in there.");
            LinkCategory linkCat = null;
            foreach (LinkCategory linkCategory in originalCategories)
            {
                linkCat = linkCategory;
                break;
            }
            LinkCategory originalCategory = linkCat;
            originalCategory.Description = "New Description";
            originalCategory.IsActive = false;
            bool updated = repository.UpdateLinkCategory(originalCategory);

            // Retrieve the categories and find the one we updated
            ICollection<LinkCategory> updatedCategories = repository.GetCategories(CategoryType.LinkCollection,
                                                                              ActiveFilter.None);
            LinkCategory updatedCategory = null;
            foreach (LinkCategory lc in updatedCategories)
            {
                if (lc.Id == originalCategory.Id)
                {
                    updatedCategory = lc;
                }
            }

            // Ensure the update was successful
            Assert.IsTrue(updated);
            Assert.IsNotNull(updatedCategory);
            Assert.AreEqual("New Description", updatedCategory.Description);
            Assert.AreEqual(false, updatedCategory.IsActive);
        }

        static int[] CreateSomeLinkCategories(ObjectRepository repository)
        {
            var categoryIds = new int[3];
            categoryIds[0] =
                repository.CreateLinkCategory(CreateCategory("My Favorite Feeds", "Some of my favorite RSS feeds",
                                                        CategoryType.LinkCollection, true));
            categoryIds[1] =
                repository.CreateLinkCategory(CreateCategory("Google Blogs", "My favorite Google blogs",
                                                        CategoryType.LinkCollection, true));
            categoryIds[2] =
                repository.CreateLinkCategory(CreateCategory("Microsoft Blogs", "My favorite Microsoft blogs",
                                                        CategoryType.LinkCollection, false));
            return categoryIds;
        }

        static int[] CreateSomePostCategories(ObjectRepository repository)
        {
            var categoryIds = new int[3];
            categoryIds[0] =
                repository.CreateLinkCategory(CreateCategory("My Favorite Feeds", "Some of my favorite RSS feeds",
                                                        CategoryType.PostCollection, true));
            categoryIds[1] =
                repository.CreateLinkCategory(CreateCategory("Google Blogs", "My favorite Google blogs",
                                                        CategoryType.PostCollection, true));
            categoryIds[2] =
                repository.CreateLinkCategory(CreateCategory("Microsoft Blogs", "My favorite Microsoft blogs",
                                                        CategoryType.PostCollection, false));
            return categoryIds;
        }

        static LinkCategory CreateCategory(string title, string description, CategoryType categoryType, bool isActive)
        {
            var linkCategory = new LinkCategory();
            linkCategory.BlogId = Config.CurrentBlog.Id;
            linkCategory.Title = title;
            linkCategory.Description = description;
            linkCategory.CategoryType = categoryType;
            linkCategory.IsActive = isActive;
            return linkCategory;
        }

        static Link CreateLink(ObjectRepository repository, string title, int? categoryId, int? postId)
        {
            var link = new Link();
            link.IsActive = true;
            link.BlogId = Config.CurrentBlog.Id;
            if (categoryId != null)
            {
                link.CategoryId = (int)categoryId;
            }
            link.Title = title;
            if (postId != null)
            {
                link.PostId = (int)postId;
            }
            int linkId = repository.CreateLink(link);
            Assert.AreEqual(linkId, link.Id);
            return link;
        }

        /// <summary>
        /// Sets the up test fixture.  This is called once for 
        /// this test fixture before all the tests run.
        /// </summary>
        [TestFixtureSetUp]
        public void SetUpTestFixture()
        {
            //Confirm app settings
            UnitTestHelper.AssertAppSettings();
        }

        [TearDown]
        public void TearDown()
        {
            HttpContext.Current = null;
        }
    }
}