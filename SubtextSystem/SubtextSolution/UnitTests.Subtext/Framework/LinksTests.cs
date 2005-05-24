using System;
using NUnit.Framework;
using Subtext.Framework;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;
using Subtext.Framework.Providers;

namespace UnitTests.Subtext.Framework
{
	/// <summary>
	/// Unit tests of Subtext.Framework.Links class methods
	/// </summary>
	[TestFixture]
	public class LinksTests
	{
		public LinksTests() {}

		/// <summary>
		/// Ensures CreateLinkCategory assigns unique CatIDs
		/// </summary>
		[Test]
		public void CreateLinkCategoryAssignsUniqueCatIDs()
		{
			// Create some categories
			CreateSomeLinkCategories();
			LinkCategoryCollection linkCategoryCollection = Links.GetCategories(CategoryType.LinkCollection, false);

			// Ensure the CategoryIDs are unique
			Assert.AreNotEqual(linkCategoryCollection[0].CategoryID, linkCategoryCollection[1].CategoryID);
			Assert.AreNotEqual(linkCategoryCollection[0].CategoryID, linkCategoryCollection[2].CategoryID);
			Assert.AreNotEqual(linkCategoryCollection[1].CategoryID, linkCategoryCollection[2].CategoryID);
		}

		/// <summary>
		/// Ensure UpdateLInkCategory updates the correct link category
		/// </summary>
		[Test]
		public void UpdateLinkCategoryIsFine()
		{
			// Create the categories
			CreateSomeLinkCategories();

			// Retrieve the categories, grab the first one and update it
			LinkCategoryCollection originalCategories = Links.GetCategories(CategoryType.LinkCollection, false);
			LinkCategory originalCategory = originalCategories[0];
			originalCategory.Description = "New Description";
			originalCategory.IsActive = false;
			bool updated = Links.UpdateLinkCategory(originalCategory);

			// Retrieve the categories and find the one we updated
			LinkCategoryCollection updatedCategories = Links.GetCategories(CategoryType.LinkCollection, false);
			LinkCategory updatedCategory = null;
			foreach(LinkCategory lc in updatedCategories)
				if (lc.CategoryID == originalCategory.CategoryID)
					updatedCategory = lc;

			// Ensure the update was successful
			Assert.IsTrue(updated);
			Assert.IsNotNull(updatedCategory);
			Assert.AreEqual("New Description", updatedCategory.Description);
			Assert.AreEqual(false, updatedCategory.IsActive);
		}

		void CreateSomeLinkCategories()
		{
			Links.CreateLinkCategory(CreateCategory("My Favorite Feeds", "Some of my favorite RSS feeds", CategoryType.LinkCollection, true));
			Links.CreateLinkCategory(CreateCategory("Google Blogs", "My favorite Google blogs", CategoryType.LinkCollection, true));
			Links.CreateLinkCategory(CreateCategory("Microsoft Blogs", "My favorite Microsoft blogs", CategoryType.LinkCollection, false));
		}

		LinkCategory CreateCategory(string title, string description, CategoryType categoryType, bool isActive)
		{
			LinkCategory linkCategory = new LinkCategory();
			linkCategory.BlogID = Config.GetConfig("www.subtext.com", "myBlog").BlogID;
			linkCategory.Title = title;
			linkCategory.Description = description;
			linkCategory.CategoryType = categoryType;
			linkCategory.IsActive = isActive;
			return linkCategory;
		}
		
		[SetUp]
		public void SetUp()
		{
			//This file needs to be there already.
			UnitTestHelper.UnpackEmbeddedResource("App.config", "UnitTests.Subtext.dll.config");
			
			//Confirm app settings
			Assert.AreEqual("~/Admin/Resources/PageTemplate.ascx", System.Configuration.ConfigurationSettings.AppSettings["Admin.DefaultTemplate"]) ;

			//Create a test Blog
			UnitTestDTOProvider dtoProvider = (UnitTestDTOProvider)DTOProvider.Instance();
			dtoProvider.ClearBlogs();
			dtoProvider.AddBlogConfiguration("title", "smarcuccio", "mypassword", "www.subtext.com", "myBlog");
		}

		[TearDown]
		public void TearDown()
		{
		}
	}
}
