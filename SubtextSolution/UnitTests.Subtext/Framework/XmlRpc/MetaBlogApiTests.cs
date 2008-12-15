using System;
using System.Globalization;
using MbUnit.Framework;
using Subtext.Extensibility;
using Subtext.Framework;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;
using Subtext.Framework.XmlRpc;
using Enclosure=Subtext.Framework.XmlRpc.Enclosure;
using FrameworkEnclosure = Subtext.Framework.Components.Enclosure;

namespace UnitTests.Subtext.Framework.XmlRpc
{
    [TestFixture]
    public class MetaBlogApiTests
    {
		[Test]
		[RollBack]
		public void NewPostWithCategoryCreatesEntryWithCategory()
		{
			string hostname = UnitTestHelper.GenerateUniqueString();
			Config.CreateBlog("", "username", "password", hostname, "");
			UnitTestHelper.SetHttpContextWithBlogRequest(hostname, "");
			Config.CurrentBlog.AllowServiceAccess = true;

			LinkCategory category = new LinkCategory();
			category.IsActive = true;
			category.Description = "Test category";
			category.Title = "CategoryA";
			Links.CreateLinkCategory(category);
			
			MetaWeblog api = new MetaWeblog();
			Post post = new Post();
			post.categories = new string[] {"CategoryA"};
			post.description = "A unit test";
			post.title = "A unit testing title";
			post.dateCreated = DateTime.Now;

			string result = api.newPost(Config.CurrentBlog.Id.ToString(CultureInfo.InvariantCulture), "username", "password", post, true);
			int entryId = int.Parse(result);

			Entry entry = Entries.GetEntry(entryId, PostConfig.None, true);
			Assert.IsNotNull(entry, "Guess the entry did not get created properly.");
			Assert.AreEqual(1, entry.Categories.Count, "We expected one category. We didn't get what we expected.");
			Assert.AreEqual("CategoryA", entry.Categories[0], "The wrong category was created.");
		}
    	
    	[Test]
    	[RollBack]
    	public void NewPostAcceptsNullCategories()
    	{
			string hostname = UnitTestHelper.GenerateUniqueString();
			Config.CreateBlog("", "username", "password", hostname, "");
			UnitTestHelper.SetHttpContextWithBlogRequest(hostname, "");
			Config.CurrentBlog.AllowServiceAccess = true;

			MetaWeblog api = new MetaWeblog();
			Post post = new Post();
    		post.categories = null;
			post.description = "A unit test";
			post.title = "A unit testing title";
    		post.dateCreated = DateTime.Now;
    		
    		string result = api.newPost(Config.CurrentBlog.Id.ToString(CultureInfo.InvariantCulture), "username", "password", post, true);
            int entryId = int.Parse(result);

            Entry entry = Entries.GetEntry(entryId, PostConfig.None, true);
            Assert.IsNotNull(entry, "Guess the entry did not get created properly.");
            Assert.AreEqual(0, entry.Categories.Count, "Should not have added categories.");
    	}

        [Test]
        [RollBack]
        public void NewPostWithFutureDateSyndicatesInTheFuture() {
            string hostname = UnitTestHelper.GenerateUniqueString();
            Config.CreateBlog("", "username", "password", hostname, "");
            UnitTestHelper.SetHttpContextWithBlogRequest(hostname, "");
            Config.CurrentBlog.AllowServiceAccess = true;

            MetaWeblog api = new MetaWeblog();
            Post post = new Post();
            post.categories = null;
            post.description = "A unit test";
            post.title = "A unit testing title";
            post.dateCreated = DateTime.Now.AddDays(1);

            string result = api.newPost(Config.CurrentBlog.Id.ToString(CultureInfo.InvariantCulture), "username", "password", post, true);
            int entryId = int.Parse(result);

            Entry entry = Entries.GetEntry(entryId, PostConfig.None, true);
            Assert.IsNotNull(entry, "Guess the entry did not get created properly.");
            Assert.IsTrue(entry.DateSyndicated > DateTime.Now.AddDays(.75));
            Assert.IsTrue(entry.DateSyndicated <= DateTime.Now.AddDays(1));
        }


        [Test]
        [RollBack]
        public void NewPostWithEnclosureCreatesEntryWithEnclosure()
        {
            string hostname = UnitTestHelper.GenerateUniqueString();
            Config.CreateBlog("", "username", "password", hostname, "");
            UnitTestHelper.SetHttpContextWithBlogRequest(hostname, "");
            Config.CurrentBlog.AllowServiceAccess = true;

            MetaWeblog api = new MetaWeblog();
            Post post = new Post();
            post.categories = null;
            post.description = "A unit test";
            post.title = "A unit testing title";
            post.dateCreated = DateTime.Now;

            Enclosure postEnclosure = new Enclosure();
            postEnclosure.url = "http://codeclimber.net.nz/podcast/mypodcast.mp3";
            postEnclosure.type = "audio/mp3";
            postEnclosure.length = 123456789;
            post.enclosure = postEnclosure;

            string result = api.newPost(Config.CurrentBlog.Id.ToString(CultureInfo.InvariantCulture), "username", "password", post, true);
            int entryId = int.Parse(result);

            Entry entry = Entries.GetEntry(entryId, PostConfig.None, true);
            Assert.IsNotNull(entry, "Guess the entry did not get created properly.");
            Assert.IsNotNull(entry.Enclosure,"Should have created the enclosure as well.");
            Assert.AreEqual("http://codeclimber.net.nz/podcast/mypodcast.mp3", entry.Enclosure.Url,"Not the expected enclosure url.");
            Assert.AreEqual("audio/mp3", entry.Enclosure.MimeType, "Not the expected enclosure mimetype.");
            Assert.AreEqual(123456789, entry.Enclosure.Size,"Not the expected enclosure size.");
        }

        [Test]
        [RollBack]
        public void NewPostAcceptsNullEnclosure()
        {
            string hostname = UnitTestHelper.GenerateUniqueString();
            Config.CreateBlog("", "username", "password", hostname, "");
            UnitTestHelper.SetHttpContextWithBlogRequest(hostname, "");
            Config.CurrentBlog.AllowServiceAccess = true;

            MetaWeblog api = new MetaWeblog();
            Post post = new Post();
            post.categories = null;
            post.description = "A unit test";
            post.title = "A unit testing title";
            post.dateCreated = DateTime.Now;

            string result = api.newPost(Config.CurrentBlog.Id.ToString(CultureInfo.InvariantCulture), "username", "password", post, true);
            int entryId = int.Parse(result);

            Entry entry = Entries.GetEntry(entryId, PostConfig.None, true);
            Assert.IsNotNull(entry, "Guess the entry did not get created properly.");
            Assert.IsNull(entry.Enclosure, "Should have not created the enclosure.");
        }

        [Test]
        [RollBack]
        public void CanUpdatePostWithEnclosure()
        {
            string hostname = UnitTestHelper.GenerateUniqueString();
            Config.CreateBlog("", "username", "password", hostname, "");
            UnitTestHelper.SetHttpContextWithBlogRequest(hostname, "");
            Config.CurrentBlog.AllowServiceAccess = true;

            Entry entry = new Entry(PostType.BlogPost);
            entry.Title = "Title 1";
            entry.Body = "Blah";
            entry.IsActive = true;
            entry.DateCreated = entry.DateSyndicated = entry.DateModified = DateTime.ParseExact("1975/01/23", "yyyy/MM/dd", CultureInfo.InvariantCulture);
            int entryId = Entries.Create(entry);

            string enclosureUrl = "http://perseus.franklins.net/hanselminutes_0107.mp3";
            string enclosureMimeType = "audio/mp3";
            long enclosureSize = 26707573;

            FrameworkEnclosure enc = UnitTestHelper.BuildEnclosure("<Digital Photography Explained (for Geeks) with Aaron Hockley/>", enclosureUrl, enclosureMimeType, entryId, enclosureSize, true, true);
            Enclosures.Create(enc);

            entry = Entries.GetEntry(entryId, PostConfig.None, true);
            Assert.IsNotNull(entry.Enclosure, "There should be a enclosure here.");


            MetaWeblog api = new MetaWeblog();
            Post post = new Post();
            post.title = "Title 2";
            post.description = "Blah";
            post.dateCreated = DateTime.Now;

            Enclosure postEnclosure = new Enclosure();
            postEnclosure.url = "http://codeclimber.net.nz/podcast/mypodcastUpdated.mp3";
            postEnclosure.type = "audio/mp3";
            postEnclosure.length = 123456789;
            post.enclosure = postEnclosure;

            bool result = api.editPost(entryId.ToString(CultureInfo.InvariantCulture), "username", "password", post, true);

            entry = Entries.GetEntry(entryId, PostConfig.None, true);

            Assert.IsNotNull(entry.Enclosure, "Should have kept the enclosure.");
            Assert.AreEqual("http://codeclimber.net.nz/podcast/mypodcastUpdated.mp3", entry.Enclosure.Url, "Not the updated enclosure url.");
        }

        [Test]
        [RollBack]
        public void UpdatingWithEnclosureAddNewEnclosure()
        {
            string hostname = UnitTestHelper.GenerateUniqueString();
            Config.CreateBlog("", "username", "password", hostname, "");
            UnitTestHelper.SetHttpContextWithBlogRequest(hostname, "");
            Config.CurrentBlog.AllowServiceAccess = true;

            Entry entry = new Entry(PostType.BlogPost);
            entry.Title = "Title 1";
            entry.Body = "Blah";
            entry.IsActive = true;
            entry.DateCreated = entry.DateSyndicated = entry.DateModified = DateTime.ParseExact("1975/01/23", "yyyy/MM/dd", CultureInfo.InvariantCulture);
            int entryId = Entries.Create(entry);

            Assert.IsNull(entry.Enclosure, "There should not be any enclosure here.");

            MetaWeblog api = new MetaWeblog();
            Post post = new Post();
            post.title = "Title 2";
            post.description = "Blah";
            post.dateCreated = DateTime.Now;

            Enclosure postEnclosure = new Enclosure();
            postEnclosure.url = "http://codeclimber.net.nz/podcast/mypodcast.mp3";
            postEnclosure.type = "audio/mp3";
            postEnclosure.length = 123456789;
            post.enclosure = postEnclosure;

            bool result = api.editPost(entryId.ToString(CultureInfo.InvariantCulture), "username", "password", post, true);

            entry = Entries.GetEntry(entryId, PostConfig.None, true);

            Assert.IsNotNull(entry.Enclosure, "Should have created the enclosure as well.");
            Assert.AreEqual("http://codeclimber.net.nz/podcast/mypodcast.mp3", entry.Enclosure.Url, "Not the expected enclosure url.");
            Assert.AreEqual("audio/mp3", entry.Enclosure.MimeType, "Not the expected enclosure mimetype.");
            Assert.AreEqual(123456789, entry.Enclosure.Size, "Not the expected enclosure size.");
        }


        [Test]
        [RollBack]
        public void PostWithoutEnclosureRemovesEnclosureFromEntry()
        {
            string hostname = UnitTestHelper.GenerateUniqueString();
            Config.CreateBlog("", "username", "password", hostname, "");
            UnitTestHelper.SetHttpContextWithBlogRequest(hostname, "");
            Config.CurrentBlog.AllowServiceAccess = true;

            Entry entry = new Entry(PostType.BlogPost);
            entry.Title = "Title 1";
            entry.Body = "Blah";
            entry.IsActive = true;
            entry.DateCreated = entry.DateSyndicated = entry.DateModified = DateTime.ParseExact("1975/01/23", "yyyy/MM/dd", CultureInfo.InvariantCulture);
            int entryId = Entries.Create(entry);

            string enclosureUrl = "http://perseus.franklins.net/hanselminutes_0107.mp3";
            string enclosureMimeType = "audio/mp3";
            long enclosureSize = 26707573;

            FrameworkEnclosure enc = UnitTestHelper.BuildEnclosure("<Digital Photography Explained (for Geeks) with Aaron Hockley/>", enclosureUrl, enclosureMimeType, entryId, enclosureSize, true, true);
            Enclosures.Create(enc);

            entry = Entries.GetEntry(entryId, PostConfig.None, true);
            Assert.IsNotNull(entry.Enclosure, "There should be a enclosure here.");

            MetaWeblog api = new MetaWeblog();
            Post post = new Post();
            post.title = "Title 2";
            post.description = "Blah";
            post.dateCreated = DateTime.Now;

            bool result = api.editPost(entryId.ToString(CultureInfo.InvariantCulture), "username", "password", post, true);

            entry = Entries.GetEntry(entryId, PostConfig.None, true);

            Assert.IsNull(entry.Enclosure, "Enclosure should have been removed.");
        }

        [Test]
        [RollBack]
        public void CanUpdatePostWithCategories()
        {
            string hostname = UnitTestHelper.GenerateUniqueString();
            Config.CreateBlog("", "username", "password", hostname, "");
            UnitTestHelper.SetHttpContextWithBlogRequest(hostname, "");
            Config.CurrentBlog.AllowServiceAccess = true;

            string category1Name = UnitTestHelper.GenerateUniqueString();
            string category2Name = UnitTestHelper.GenerateUniqueString();
            UnitTestHelper.CreateCategory(Config.CurrentBlog.Id, category1Name);
            UnitTestHelper.CreateCategory(Config.CurrentBlog.Id, category2Name);

            Entry entry = new Entry(PostType.BlogPost);
            entry.Title = "Title 1";
            entry.Body = "Blah";
            entry.IsActive = true;
            entry.DateCreated = entry.DateSyndicated = entry.DateModified = DateTime.ParseExact("1975/01/23", "yyyy/MM/dd", CultureInfo.InvariantCulture);
            entry.Categories.Add(category1Name);
            int entryId = Entries.Create(entry);

            MetaWeblog api = new MetaWeblog();
            Post post = new Post();
            post.title = "Title 2";
            post.description = "Blah";
            post.categories = new string[] { category2Name };
            post.dateCreated = DateTime.Now;

            bool result = api.editPost(entryId.ToString(CultureInfo.InvariantCulture), "username", "password", post, true);

            entry = Entries.GetEntry(entryId, PostConfig.None, true);
            Assert.AreEqual(1, entry.Categories.Count, "We expected one category. We didn't get what we expected.");
            Assert.AreEqual(category2Name, entry.Categories[0], "Category has not been updated correctly.");
        }

        [Test]
        [RollBack]
        public void PostWithNoCategoriesRemovesCategoriesFromEntry()
        {
            string hostname = UnitTestHelper.GenerateUniqueString();
            Config.CreateBlog("", "username", "password", hostname, "");
            UnitTestHelper.SetHttpContextWithBlogRequest(hostname, "");
            Config.CurrentBlog.AllowServiceAccess = true;

            string category1Name = UnitTestHelper.GenerateUniqueString();
            UnitTestHelper.CreateCategory(Config.CurrentBlog.Id, category1Name);

            Entry entry = new Entry(PostType.BlogPost);
            entry.Title = "Title 1";
            entry.Body = "Blah";
            entry.IsActive = true;
            entry.DateCreated = entry.DateSyndicated = entry.DateModified = DateTime.ParseExact("1975/01/23", "yyyy/MM/dd", CultureInfo.InvariantCulture);
            entry.Categories.Add(category1Name);
            int entryId = Entries.Create(entry);

            MetaWeblog api = new MetaWeblog();
            Post post = new Post();
            post.title = "Title 2";
            post.description = "Blah";
            post.categories = null;
            post.dateCreated = DateTime.Now;

            bool result = api.editPost(entryId.ToString(CultureInfo.InvariantCulture), "username", "password", post, true);

            entry = Entries.GetEntry(entryId, PostConfig.None, true);
            Assert.AreEqual(0, entry.Categories.Count, "We expected no category.");
        }
    	
        [Test]
        [RollBack]
        public void GetRecentPostsReturnsRecentPosts()
        {
            string hostname = UnitTestHelper.GenerateUniqueString();
			Config.CreateBlog("", "username", "password", hostname, "");
            UnitTestHelper.SetHttpContextWithBlogRequest(hostname, "");
            Config.CurrentBlog.AllowServiceAccess = true;

            MetaWeblog api = new MetaWeblog();
            Post[] posts = api.getRecentPosts(Config.CurrentBlog.Id.ToString(), "username", "password", 10);
            Assert.AreEqual(0, posts.Length);

            string category1Name = UnitTestHelper.GenerateUniqueString();
            string category2Name = UnitTestHelper.GenerateUniqueString();
            UnitTestHelper.CreateCategory(Config.CurrentBlog.Id, category1Name);
            UnitTestHelper.CreateCategory(Config.CurrentBlog.Id, category2Name);
            
            Entry entry = new Entry(PostType.BlogPost);
            entry.Title = "Title 1";
            entry.Body = "Blah";
            entry.IsActive = true;
            entry.IncludeInMainSyndication = true;
            entry.DateCreated = entry.DateSyndicated = entry.DateModified = DateTime.ParseExact("1975/01/23", "yyyy/MM/dd", CultureInfo.InvariantCulture);
			entry.Categories.Add(category1Name);
        	Entries.Create(entry);

            entry = new Entry(PostType.BlogPost);
            entry.IncludeInMainSyndication = true;
            entry.Title = "Title 2";
            entry.Body = "Blah1";
            entry.IsActive = true;
            entry.DateCreated = entry.DateSyndicated = entry.DateModified = DateTime.ParseExact("1976/05/25", "yyyy/MM/dd", CultureInfo.InvariantCulture);
			entry.Categories.Add(category1Name);
			entry.Categories.Add(category2Name);
            Entries.Create(entry);

            entry = new Entry(PostType.BlogPost);
            entry.Title = "Title 3";
            entry.IncludeInMainSyndication = true;
            entry.Body = "Blah2";
            entry.IsActive = true;
            entry.DateCreated = entry.DateSyndicated = entry.DateModified = DateTime.ParseExact("1979/09/16", "yyyy/MM/dd", CultureInfo.InvariantCulture);
            Entries.Create(entry);

            entry = new Entry(PostType.BlogPost);
            entry.Title = "Title 4";
            entry.IncludeInMainSyndication = true;
            entry.Body = "Blah3";
            entry.IsActive = true;
            entry.DateCreated = entry.DateSyndicated = entry.DateModified = DateTime.ParseExact("2006/01/01", "yyyy/MM/dd", CultureInfo.InvariantCulture);
			entry.Categories.Add(category2Name);
        	int entryId = Entries.Create(entry);

            string enclosureUrl = "http://perseus.franklins.net/hanselminutes_0107.mp3";
            string enclosureMimeType = "audio/mp3";
            long enclosureSize = 26707573;

            FrameworkEnclosure enc = UnitTestHelper.BuildEnclosure("<Digital Photography Explained (for Geeks) with Aaron Hockley/>", enclosureUrl, enclosureMimeType, entryId, enclosureSize, true, true);
            Enclosures.Create(enc);

            posts = api.getRecentPosts(Config.CurrentBlog.Id.ToString(), "username", "password", 10);
            Assert.AreEqual(4, posts.Length, "Expected 4 posts");
            Assert.AreEqual(1, posts[3].categories.Length, "Expected our categories to be there.");
            Assert.AreEqual(2, posts[2].categories.Length, "Expected our categories to be there.");
            Assert.IsNull(posts[1].categories, "Expected our categories to be there.");
            Assert.AreEqual(1, posts[0].categories.Length, "Expected our categories to be there.");
            Assert.AreEqual(category1Name, posts[3].categories[0], "The category returned by the MetaBlogApi is wrong.");
            Assert.AreEqual(category2Name, posts[0].categories[0], "The category returned by the MetaBlogApi is wrong.");

            Assert.AreEqual(enclosureUrl, posts[0].enclosure.url, "Not what we expected for the enclosure url.");
            Assert.AreEqual(enclosureMimeType, posts[0].enclosure.type, "Not what we expected for the enclosure mimetype.");
            Assert.AreEqual(enclosureSize, posts[0].enclosure.length, "Not what we expected for the enclosure size.");

        }
    }
}
