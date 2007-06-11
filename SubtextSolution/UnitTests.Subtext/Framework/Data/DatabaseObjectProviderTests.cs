using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using MbUnit.Framework;
using Subtext.Extensibility;
using Subtext.Extensibility.Interfaces;
using Subtext.Framework;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;
using Subtext.Framework.Data;
using Subtext.Framework.Logging;
using Subtext.Framework.Providers;

namespace UnitTests.Subtext.Framework.Data
{
	[TestFixture]
	public class DatabaseObjectProviderTests
	{
		[Test]
		[RollBack2]
		public void CanClearBlogContent()
		{
			UnitTestHelper.SetupBlog();
			
			Entry entry = UnitTestHelper.CreateEntryInstanceForSyndication("author", "Ttitle", "Some body");
			int entryId = Entries.Create(entry);
			entry.BlogId = Config.CurrentBlog.Id;
			Entry loaded = Entries.GetEntry(entryId, PostConfig.None, false);
			Assert.IsNotNull(loaded);
			Assert.AreEqual(Config.CurrentBlog.Id, loaded.BlogId);

			DatabaseObjectProvider provider = new DatabaseObjectProvider();
			NameValueCollection config = new NameValueCollection();
			config.Add("connectionStringName", "subtextData");
			provider.Initialize("data", config);
			provider.ClearBlogContent();
			loaded = Entries.GetEntry(entryId, PostConfig.None, false);
			Assert.IsNull(loaded);
		}

        [Test]
        [RollBack2]
        public void CanClearLog()
        {
			UnitTestHelper.SetupBlog();

            LoggingProvider provider = DatabaseLoggingProvider.Instance();

            // Start from scratch and clear the log
            provider.ClearLog();

            // Add a dummy log entry
            IPagedCollectionTester tester = new LogEntryCollectionTester();
            tester.Create(1);

            // Retrieve log entries and make sure there is just the one
            IPagedCollection items = tester.GetPagedItems(1, 5);
            Assert.AreEqual(1, tester.GetCount(items));

            // Clear the log again
            provider.ClearLog();

            // Make sure there are not any left
            items = tester.GetPagedItems(1, 5);
            Assert.AreEqual(0, tester.GetCount(items));
        }

		[Test]
		[ExpectedArgumentNullException]
		public void CreateFeedbackThrowsArgumentNullException()
		{
            DatabaseObjectProvider provider = new DatabaseObjectProvider();
			provider.CreateFeedback(null);
		}

		[Test]
		[ExpectedArgumentNullException]
		public void InsertCategoryThrowsArgumentNullException()
		{
            DatabaseObjectProvider provider = new DatabaseObjectProvider();
			provider.CreateLinkCategory(null);
		}

		[Test]
		[ExpectedArgumentNullException]
		public void InsertEntryThrowsArgumentNullException()
		{
            DatabaseObjectProvider provider = new DatabaseObjectProvider();
			provider.InsertEntry(null);
		}

		[Test]
		[RollBack2]
		public void CanInsertAndDeleteImage()
		{
			UnitTestHelper.SetupBlog();
			ObjectProvider provider = DatabaseObjectProvider.Instance();
			
			// Create the required category
			LinkCategory category = new LinkCategory();
			category.CategoryType = CategoryType.LinkCollection;
			category.Title = "Cuteness";
			category.Description = "Seriously Cute Bunnies";
			int categoryID = provider.CreateLinkCategory(category);
			
			// Insert a new image
			Image image = new Image();
			image.Title = "Picture of the Cutest Bunny Rabbit Ever";
			image.CategoryID = categoryID;
			image.Height = 100;
			image.Width = 100;
			image.FileName = "/images/cute_bunny.jpg";
			image.IsActive = true;
			int imageID = provider.InsertImage(image);

			// Retrieve the image from the database
			image = provider.GetImage(imageID, true);
			Assert.AreEqual(imageID, image.ImageID);

			// Delete the image and make sure it's gone
			provider.DeleteImage(imageID);
			image = provider.GetImage(imageID, true);
			Assert.IsNull(image);
		}

		[Test]
		[RollBack2]
		public void CanInsertAndDeleteKeyword()
		{
			UnitTestHelper.SetupBlog();
			ObjectProvider provider = DatabaseObjectProvider.Instance();

			// Insert a new keyword
			KeyWord word = new KeyWord();
			word.Word = "Word Up";
			word.Text = "Word Up Yo";
			word.ReplaceFirstTimeOnly = false;
			word.OpenInNewWindow = false;
			word.CaseSensitive = false;
			word.Url = "http://www.wordtoyomama.com";
			word.Title = "Howdy Y'all";
			int keywordID = provider.InsertKeyword(word);

			// Retrieve the keyword from the database
			word = provider.GetKeyword(keywordID);
			Assert.AreEqual(keywordID, word.Id);

			// Delete the keyword and make sure it's gone
			provider.DeleteKeyword(keywordID);
			word = provider.GetKeyword(keywordID);
			Assert.IsNull(word);
		}

		[Test]
		[RollBack2]
		public void CanInsertAndDeleteLink()
		{
			UnitTestHelper.SetupBlog();
			ObjectProvider provider = DatabaseObjectProvider.Instance();

			// Create the required category
			LinkCategory category = new LinkCategory();
			category.CategoryType = CategoryType.LinkCollection;
			category.Title = "Fences";
			category.Description = "All Kinds of Fences";
			int categoryID = provider.CreateLinkCategory(category);

			// Insert a new link
			Link link = new Link();
			link.CategoryID = categoryID;
			link.Title = "Chain Link";
			link.Url = "http://www.somecoolsite.com";
			int linkID = provider.CreateLink(link);

			// Retrieve the link from the database
			link = provider.GetLink(linkID);
			Assert.AreEqual(linkID, link.Id);

			// Delete the link and make sure it's gone
			provider.DeleteLink(linkID);
			link = provider.GetLink(linkID);
			Assert.IsNull(link);
		}

		[Test]
		[RollBack2]
		public void CanInsertAndDeleteLinkCategory()
		{
			UnitTestHelper.SetupBlog();
			ObjectProvider provider = DatabaseObjectProvider.Instance();

			// Create a new category
			LinkCategory category = new LinkCategory();
			category.CategoryType = CategoryType.LinkCollection;
			category.Title = "New Category";
			category.Description = "Brand New Category";
			int categoryID = provider.CreateLinkCategory(category);

			// Retrieve the category from the database
			category = provider.GetLinkCategory(categoryID, false);
			Assert.AreEqual(categoryID, category.Id);

			// Delete the link and make sure it's gone
			provider.DeleteLinkCategory(categoryID);
			category = provider.GetLinkCategory(categoryID, false);
			Assert.IsNull(category);
		}

		[Test]
		[RollBack2]
		public void CanGetActiveCategories()
		{
			UnitTestHelper.SetupBlog();
			DeleteAllActiveCategories();

			CreateLinkCategory("New Category 1", "Brand New Category 1");
			CreateLinkCategory("New Category 2", "Brand New Category 2");
			CreateLinkCategory("New Category 3", "Brand New Category 3");

			ObjectProvider provider = DatabaseObjectProvider.Instance();
			IList<LinkCategory> activeCategories = provider.GetActiveLinkCollections();
			Assert.AreEqual(3, activeCategories.Count);
		}

		private static void CreateLinkCategory(string title, string description)
		{
			ObjectProvider provider = DatabaseObjectProvider.Instance();
			
			LinkCategory category = new LinkCategory();
			category.CategoryType = CategoryType.LinkCollection;
			category.Title = title;
			category.IsActive = true;
			category.Description = description;
			provider.CreateLinkCategory(category);
		}

		private static void DeleteAllActiveCategories()
		{
			ObjectProvider provider = DatabaseObjectProvider.Instance();

			// Clear out any existing categories
			ICollection<LinkCategory> activeCategories = provider.GetActiveLinkCollections();
			
			foreach (LinkCategory category in activeCategories)
			{
				provider.DeleteLinkCategory(category.Id);
			}
			
			activeCategories = provider.GetActiveLinkCollections();
			
			Assert.AreEqual(0, activeCategories.Count);
		}

		[Test]
		[ExpectedArgumentNullException]
		public void InsertImageThrowsArgumentNullException()
		{
            DatabaseObjectProvider provider = new DatabaseObjectProvider();
			provider.InsertImage(null);
		}

		[Test]
		[ExpectedArgumentNullException]
		public void InsertKeyWordThrowsArgumentNullException()
		{
            DatabaseObjectProvider provider = new DatabaseObjectProvider();
			provider.InsertKeyword(null);
		}

		[Test]
		[ExpectedArgumentNullException]
		public void CreateLinkThrowsArgumentNullException()
		{
            DatabaseObjectProvider provider = new DatabaseObjectProvider();
			provider.CreateLink(null);
		}
	}
}
