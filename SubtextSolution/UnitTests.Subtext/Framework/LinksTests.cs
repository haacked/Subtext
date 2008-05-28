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
using System.Web;
using MbUnit.Framework;
using Subtext.Extensibility.Interfaces;
using Subtext.Framework;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;

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

			int category1Id = Links.CreateLinkCategory(CreateCategory("Post Category 1", "Cody roolz!", CategoryType.PostCollection, true));
			int category2Id = Links.CreateLinkCategory(CreateCategory("Post Category 2", "Cody roolz again!", CategoryType.PostCollection, true));
			Links.CreateLinkCategory(CreateCategory("Post Category 3", "Cody roolz and again!", CategoryType.PostCollection, true));

			Entry entry = UnitTestHelper.CreateEntryInstanceForSyndication("phil", "title", "body");
			int entryId = Entries.Create(entry);
			Entries.SetEntryCategoryList(entryId, category1Id, category2Id);

            IList<LinkCategory> categories = Links.GetLinkCategoriesByPostID(entryId);
			Assert.AreEqual(2, categories.Count, "Expected two of the three categories");

			Assert.AreEqual(category1Id, categories[0].Id);
			Assert.AreEqual(category2Id, categories[1].Id);

			Assert.AreEqual(Config.CurrentBlog.Id, categories[0].BlogId);
		}

		[Test]
		[RollBack2]
		public void CanGetActiveCategories()
		{
			UnitTestHelper.SetupBlog();

			int[] categoryIds = CreateSomeLinkCategories();
			CreateLink("Link one", categoryIds[0], null);
			CreateLink("Link two", categoryIds[0], null);
			CreateLink("Link one-two", categoryIds[1], null);

			IList<LinkCategory> linkCollections = Links.GetActiveCategories();
			
			//Test ordering by title
			Assert.AreEqual("Google Blogs", linkCollections[0].Title);
			Assert.AreEqual("My Favorite Feeds", linkCollections[1].Title);

			//Check link counts
			Assert.AreEqual(1, linkCollections[0].Links.Count);
			Assert.AreEqual(2, linkCollections[1].Links.Count);
		}

		[Test]
		[RollBack2]
		public void CanUpdateLink()
		{
			UnitTestHelper.SetupBlog();
			// Create the categories
			CreateSomeLinkCategories();

			int categoryId = Links.CreateLinkCategory(CreateCategory("My Favorite Feeds", "Some of my favorite RSS feeds", CategoryType.LinkCollection, true));
			Link link = CreateLink("Test", categoryId, null);
			int linkId = link.Id;
			
			Link loaded = Links.GetSingleLink(linkId);
			Assert.AreEqual("Test", loaded.Title);

			Entry entry = UnitTestHelper.CreateEntryInstanceForSyndication("test", "test", "test");
			
			//Make changes then update.
			link.PostID = entry.Id;
			link.Title = "Another title";
			link.NewWindow = true;
			Links.UpdateLink(link);
			loaded = Links.GetSingleLink(linkId);
			Assert.AreEqual("Another title", loaded.Title);
			Assert.IsTrue(loaded.NewWindow);
			Assert.AreEqual(entry.Id, loaded.PostID);
		}

		[Test]
		[RollBack2]
		public void CanCreateAndDeleteLink()
		{
			UnitTestHelper.SetupBlog();
			
			int categoryId = Links.CreateLinkCategory(CreateCategory("My Favorite Feeds", "Some of my favorite RSS feeds", CategoryType.LinkCollection, true));

			Link link = CreateLink("Title", categoryId, null);
			int linkId = link.Id;

			Link loaded = Links.GetSingleLink(linkId);
			Assert.AreEqual("Title", loaded.Title);
			Assert.AreEqual(NullValue.NullInt32, loaded.PostID);
            Assert.AreEqual(Config.CurrentBlog.Id, loaded.BlogId);
			
			Links.DeleteLink(linkId);

			Assert.IsNull(Links.GetSingleLink(linkId));
		}
		
		[Test]
		[RollBack2]
		public void CanCreateAndDeleteLinkCategory()
		{
			UnitTestHelper.SetupBlog();

			// Create some categories
			int categoryId = Links.CreateLinkCategory(CreateCategory("My Favorite Feeds", "Some of my favorite RSS feeds", CategoryType.LinkCollection, true));

			LinkCategory category = Links.GetLinkCategory(categoryId, true);
			Assert.AreEqual(Config.CurrentBlog.Id, category.BlogId);
			Assert.AreEqual("My Favorite Feeds", category.Title);
			Assert.AreEqual("Some of my favorite RSS feeds", category.Description);
			Assert.IsTrue(category.HasDescription);
			Assert.IsFalse(category.HasLinks);
			Assert.IsFalse(category.HasImages);
			Assert.IsTrue(category.IsActive);
			Assert.AreEqual(CategoryType.LinkCollection, category.CategoryType);
			Assert.IsNotNull(category);

			Links.DeleteLinkCategory(categoryId);
			Assert.IsNull(Links.GetLinkCategory(categoryId, true));
		}

		/// <summary>
		/// Ensures CreateLinkCategory assigns unique CatIDs
		/// </summary>
		[Test]
		[RollBack2]
		public void CreateLinkCategoryAssignsUniqueCatIDs()
		{
			UnitTestHelper.SetupBlog();

			// Create some categories
			CreateSomeLinkCategories();
            IList<LinkCategory> linkCategoryCollection = Links.GetCategories(CategoryType.LinkCollection, ActiveFilter.None);

            LinkCategory first = null;
            LinkCategory second = null;
            LinkCategory third = null;
		    foreach(LinkCategory linkCategory in linkCategoryCollection)
		    {
                if (first == null)
                {
                    first = linkCategory;
                    continue;
                }

		        if(second == null)
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
			UnitTestHelper.AssertAreNotEqual(first.Id, second.Id);
            UnitTestHelper.AssertAreNotEqual(first.Id, third.Id);
            UnitTestHelper.AssertAreNotEqual(second.Id, third.Id);
		}

		/// <summary>
		/// Ensure UpdateLInkCategory updates the correct link category
		/// </summary>
		[Test]
		[RollBack2]
		public void UpdateLinkCategoryIsFine()
		{
			UnitTestHelper.SetupBlog();

			// Create the categories
			CreateSomeLinkCategories();

			// Retrieve the categories, grab the first one and update it
            IList<LinkCategory> originalCategories = Links.GetCategories(CategoryType.LinkCollection, ActiveFilter.None);
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
			bool updated = Links.UpdateLinkCategory(originalCategory);

			// Retrieve the categories and find the one we updated
            IList<LinkCategory> updatedCategories = Links.GetCategories(CategoryType.LinkCollection, ActiveFilter.None);
			LinkCategory updatedCategory = null;
			foreach(LinkCategory lc in updatedCategories)
				if (lc.Id == originalCategory.Id)
					updatedCategory = lc;

			// Ensure the update was successful
			Assert.IsTrue(updated);
			Assert.IsNotNull(updatedCategory);
			Assert.AreEqual("New Description", updatedCategory.Description);
			Assert.AreEqual(false, updatedCategory.IsActive);
		}

        static int[] CreateSomeLinkCategories()
        {
            int[] categoryIds = new int[3];
            categoryIds[0] = Links.CreateLinkCategory(CreateCategory("My Favorite Feeds", "Some of my favorite RSS feeds", CategoryType.LinkCollection, true));
            categoryIds[1] = Links.CreateLinkCategory(CreateCategory("Google Blogs", "My favorite Google blogs", CategoryType.LinkCollection, true));
            categoryIds[2] = Links.CreateLinkCategory(CreateCategory("Microsoft Blogs", "My favorite Microsoft blogs", CategoryType.LinkCollection, false));
            return categoryIds;
        }

		static LinkCategory CreateCategory(string title, string description, CategoryType categoryType, bool isActive)
		{
			LinkCategory linkCategory = new LinkCategory();
			linkCategory.BlogId = Config.CurrentBlog.Id;
			linkCategory.Title = title;
			linkCategory.Description = description;
			linkCategory.CategoryType = categoryType;
			linkCategory.IsActive = isActive;
			return linkCategory;
		}

        static Link CreateLink(string title, int? categoryId, int? postId)
        {
            Link link = new Link();
            link.IsActive = true;
            link.BlogId = Config.CurrentBlog.Id;
            if (categoryId != null)
                link.CategoryID = (int)categoryId;
            link.Title = title;
            if (postId != null)
                link.PostID = (int)postId;
            int linkId = Links.CreateLink(link);
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
