using System;
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
        string hostName;
	    
        [SetUp]
        public void SetUp()
        {
            hostName = UnitTestHelper.GenerateRandomString();
            UnitTestHelper.SetHttpContextWithBlogRequest(hostName, "blog");
        }
	    
		[Test]
		[RollBack]
		public void CanClearBlogContent()
		{
            Config.CreateBlog("test title", "test", "testaoeu!123", hostName, "");
            UnitTestHelper.SetHttpContextWithBlogRequest(hostName, "");
			
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
        [RollBack]
        public void CanClearLog()
        {
            Assert.IsTrue(Config.CreateBlog("My Blog", "username", "password", hostName, "blog"));

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
		[RollBack]
		public void CanInsertAndDeleteImage()
		{
			ObjectProvider provider = new DatabaseObjectProvider();
			
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
			image.File = "/images/cute_bunny.jpg";
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
		[RollBack]
		public void CanInsertAndDeleteKeyword()
		{
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
			int keywordID = provider.InsertKeyWord(word);

			// Retrieve the keyword from the database
			word = provider.GetKeyWord(keywordID);
			Assert.AreEqual(keywordID, word.Id);

			// Delete the keyword and make sure it's gone
			provider.DeleteKeyWord(keywordID);
			word = provider.GetKeyWord(keywordID);
			Assert.IsNull(word);
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
			provider.InsertKeyWord(null);
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
