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
		/// <summary>
		/// Ensures CreateLinkCategory assigns unique CatIDs
		/// </summary>
		[Test]
		[RollBack2]
		public void CreateLinkCategoryAssignsUniqueCatIDs()
		{
			UnitTestHelper.CreateBlogAndSetupContext();

			// Create some categories
			CreateSomeLinkCategories();
            ICollection<LinkCategory> linkCategoryCollection = Links.GetCategories(CategoryType.LinkCollection, ActiveFilter.None);

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
			UnitTestHelper.CreateBlogAndSetupContext();

			// Create the categories
			CreateSomeLinkCategories();

			// Retrieve the categories, grab the first one and update it
            ICollection<LinkCategory> originalCategories = Links.GetCategories(CategoryType.LinkCollection, ActiveFilter.None);
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
            ICollection<LinkCategory> updatedCategories = Links.GetCategories(CategoryType.LinkCollection, ActiveFilter.None);
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

		static void CreateSomeLinkCategories()
		{
			try
			{
				Links.CreateLinkCategory(
					CreateCategory("My Favorite Feeds", "Some of my favorite RSS feeds", CategoryType.LinkCollection, true));
				Links.CreateLinkCategory(
					CreateCategory("Google Blogs", "My favorite Google blogs", CategoryType.LinkCollection, true));
				Links.CreateLinkCategory(
					CreateCategory("Microsoft Blogs", "My favorite Microsoft blogs", CategoryType.LinkCollection, false));
			}
			catch(Exception)
			{
				Console.WriteLine("Current Blog ID: {0}", Config.CurrentBlog.Id);

				IPagedCollection<BlogInfo> blogs = BlogInfo.GetBlogs(0, 10, ConfigurationFlag.None);
				Console.WriteLine("All Blogs");
				Console.WriteLine("------------------------------------------");
				foreach(BlogInfo blog in blogs)
				{
					Console.WriteLine("ID: {0}, Host: {1}", blog.Id, blog.Host);
				}

				ICollection<LinkCategory> linkCategories = Links.GetCategories(CategoryType.LinkCollection, ActiveFilter.None);
				Console.WriteLine("All Link Categories");
				Console.WriteLine("------------------------------------------");
				foreach (LinkCategory linkCategory in linkCategories)
				{
					Console.WriteLine("ID: {0}, Description: {1}", linkCategory.Id, linkCategory.Description);
				}
				throw;
			}
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
