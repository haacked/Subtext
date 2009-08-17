using System;
using System.Globalization;
using System.Linq;
using MbUnit.Framework;
using Moq;
using Subtext.Extensibility;
using Subtext.Framework;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;
using Subtext.Framework.Providers;
using Subtext.Framework.Routing;
using Subtext.Framework.Services;
using Subtext.Framework.Web.HttpModules;
using Subtext.Framework.XmlRpc;
using Enclosure = Subtext.Framework.XmlRpc.Enclosure;
using FrameworkEnclosure = Subtext.Framework.Components.Enclosure;

namespace UnitTests.Subtext.Framework.XmlRpc
{
    [TestFixture]
    public class MetaBlogApiTests
    {
        [Test]
        public void getCategories_ReturnsCategoriesInRepository()
        {
            //arrange
            Blog blog = new Blog { AllowServiceAccess = true, Host = "localhost", UserName = "username", Password = "password" };
            LinkCategory category = new LinkCategory();
            category.BlogId = blog.Id;
            category.IsActive = true;
            category.Description = "Test category";
            category.Title = "CategoryA";
            category.CategoryType = CategoryType.PostCollection;
            category.Id = 42;

            var subtextContext = new Mock<ISubtextContext>();
            subtextContext.Setup(c => c.Blog).Returns(blog);
            subtextContext.Setup(c => c.UrlHelper.CategoryUrl(It.IsAny<LinkCategory>())).Returns("/Category/42.aspx");
            subtextContext.Setup(c => c.UrlHelper.CategoryRssUrl(It.IsAny<LinkCategory>())).Returns("/rss.aspx?catId=42");
            subtextContext.Setup(c => c.Repository.GetCategories(CategoryType.PostCollection, false)).Returns(new[] { category });
            MetaWeblog api = new MetaWeblog(subtextContext.Object);

            //act
            CategoryInfo[] categories = api.getCategories(blog.Id.ToString(), "username", "password");

            //assert
            Assert.AreEqual(1, categories.Length);
            Assert.AreEqual("http://localhost/Category/42.aspx", categories[0].htmlUrl);
            Assert.AreEqual("http://localhost/rss.aspx?catId=42", categories[0].rssUrl);
        }

        [Test]
        public void NewPostWithCategoryCreatesEntryWithCategory()
        {
            //arrange
            Blog blog = new Blog { Id = 42, UserName = "username", Password = "password", AllowServiceAccess = true, Host = "localhost" };

            LinkCategory category = new LinkCategory();
            category.IsActive = true;
            category.Description = "Test category";
            category.Title = "CategoryA";

            var entryPublisher = new Mock<IEntryPublisher>();
            Entry publishedEntry = null;
            entryPublisher.Setup(publisher => publisher.Publish(It.IsAny<Entry>())).Callback<Entry>(e => publishedEntry = e);
            var subtextContext = new Mock<ISubtextContext>();
            subtextContext.Setup(c => c.Blog).Returns(blog);
            subtextContext.Setup(c => c.Repository).Returns(ObjectProvider.Instance());

            MetaWeblog api = new MetaWeblog(subtextContext.Object, entryPublisher.Object);
            Post post = new Post();
            post.categories = new string[] { "CategoryA" };
            post.description = "A unit test";
            post.title = "A unit testing title";
            post.dateCreated = DateTime.UtcNow;

            //act
            string result = api.newPost("42", "username", "password", post, true);

            //assert
            int entryId = int.Parse(result);
            Assert.IsNotNull(publishedEntry);
            Assert.AreEqual(1, publishedEntry.Categories.Count);
            Assert.AreEqual("CategoryA", publishedEntry.Categories.First());
        }

        [Test]
        public void NewPostAcceptsNullCategories()
        {
            //arrange
            Blog blog = new Blog { Id = 42, UserName = "username", Password = "password", AllowServiceAccess = true, Host = "localhost" };

            var subtextContext = new Mock<ISubtextContext>();
            subtextContext.Setup(c => c.Blog).Returns(blog);
            Entry publishedEntry = null;
            var entryPublisher = new Mock<IEntryPublisher>();
            entryPublisher.Setup(publisher => publisher.Publish(It.IsAny<Entry>())).Returns(42).Callback<Entry>(entry => publishedEntry = entry);

            MetaWeblog api = new MetaWeblog(subtextContext.Object, entryPublisher.Object);
            Post post = new Post();
            post.categories = null;
            post.description = "A unit test";
            post.title = "A unit testing title";
            post.dateCreated = DateTime.UtcNow;

            // act
            string result = api.newPost(blog.Id.ToString(CultureInfo.InvariantCulture), "username", "password", post, true);
            
            // assert
            int entryId = int.Parse(result);
            Assert.AreEqual(42, entryId);
            Assert.AreEqual(0, publishedEntry.Categories.Count, "Should not have added categories.");
        }

        [Test]
        public void NewPostWithFutureDateSyndicatesInTheFuture()
        {
            //arrange
            Blog blog = new Blog { Id = 42, UserName = "username", Password = "password", AllowServiceAccess = true, Host = "localhost" };

            var subtextContext = new Mock<ISubtextContext>();
            subtextContext.Setup(c => c.Blog).Returns(blog);
            Entry publishedEntry = null;
            var entryPublisher = new Mock<IEntryPublisher>();
            entryPublisher.Setup(publisher => publisher.Publish(It.IsAny<Entry>())).Returns(42).Callback<Entry>(entry => publishedEntry = entry);
            var now = DateTime.Now;
            var utcNow = now.ToUniversalTime();

            MetaWeblog api = new MetaWeblog(subtextContext.Object, entryPublisher.Object);
            Post post = new Post();
            post.categories = null;
            post.description = "A unit test";
            post.title = "A unit testing title";
            post.dateCreated = utcNow.AddDays(1);

            // act
            string result = api.newPost(blog.Id.ToString(CultureInfo.InvariantCulture), "username", "password", post, true);

            // assert
            Assert.IsNotNull(publishedEntry);
            Assert.Greater(publishedEntry.DateSyndicated, now.AddDays(.75));
            Assert.LowerEqualThan(publishedEntry.DateSyndicated, now.AddDays(1));
        }

        [Test]
        public void NewPostWithEnclosureCreatesEntryWithEnclosure()
        {
            //arrange
            Blog blog = new Blog { Id = 42, UserName = "username", Password = "password", AllowServiceAccess = true, Host = "localhost" };

            var subtextContext = new Mock<ISubtextContext>();
            subtextContext.Setup(c => c.Blog).Returns(blog);
            FrameworkEnclosure publishedEnclosure = null;
            subtextContext.Setup(c => c.Repository.Create(It.IsAny<FrameworkEnclosure>())).Callback<FrameworkEnclosure>(enclosure => publishedEnclosure = enclosure);
            var entryPublisher = new Mock<IEntryPublisher>();
            entryPublisher.Setup(publisher => publisher.Publish(It.IsAny<Entry>())).Returns(42);
            var now = DateTime.Now;
            var utcNow = now.ToUniversalTime();

            MetaWeblog api = new MetaWeblog(subtextContext.Object, entryPublisher.Object);
            Post post = new Post();
            post.categories = null;
            post.description = "A unit test";
            post.title = "A unit testing title";
            post.dateCreated = utcNow.AddDays(1);

            Enclosure postEnclosure = new Enclosure();
            postEnclosure.url = "http://codeclimber.net.nz/podcast/mypodcast.mp3";
            postEnclosure.type = "audio/mp3";
            postEnclosure.length = 123456789;
            post.enclosure = postEnclosure;

            // act
            string result = api.newPost(blog.Id.ToString(CultureInfo.InvariantCulture), "username", "password", post, true);
            
            // assert
            Assert.IsNotNull(publishedEnclosure);
            Assert.AreEqual("http://codeclimber.net.nz/podcast/mypodcast.mp3", publishedEnclosure.Url);
            Assert.AreEqual("audio/mp3", publishedEnclosure.MimeType);
            Assert.AreEqual(123456789, publishedEnclosure.Size);
        }

        [Test]
        public void NewPostAcceptsNullEnclosure()
        {
            //arrange
            Blog blog = new Blog { Id = 42, UserName = "username", Password = "password", AllowServiceAccess = true, Host = "localhost" };

            var subtextContext = new Mock<ISubtextContext>();
            subtextContext.Setup(c => c.Blog).Returns(blog);
            Entry publishedEntry = null;
            var entryPublisher = new Mock<IEntryPublisher>();
            entryPublisher.Setup(publisher => publisher.Publish(It.IsAny<Entry>())).Returns(42).Callback<Entry>(entry => publishedEntry = entry);
            var now = DateTime.Now;
            var utcNow = now.ToUniversalTime();

            MetaWeblog api = new MetaWeblog(subtextContext.Object, entryPublisher.Object);
            Post post = new Post();
            post.categories = null;
            post.description = "A unit test";
            post.title = "A unit testing title";
            post.dateCreated = utcNow.AddDays(1);
            post.enclosure = null;

            // act
            string result = api.newPost(blog.Id.ToString(CultureInfo.InvariantCulture), "username", "password", post, true);

            // assert
            Assert.IsNull(publishedEntry.Enclosure);
        }

        [Test]
        [RollBack]
        public void CanUpdatePostWithEnclosure()
        {
            string hostname = UnitTestHelper.GenerateUniqueString();
            Config.CreateBlog("", "username", "password", hostname, "");
            UnitTestHelper.SetHttpContextWithBlogRequest(hostname, "");
            BlogRequest.Current.Blog = Config.GetBlog(hostname, "");
            Config.CurrentBlog.AllowServiceAccess = true;

            Entry entry = new Entry(PostType.BlogPost);
            entry.Title = "Title 1";
            entry.Body = "Blah";
            entry.IsActive = true;
            entry.DateCreated = entry.DateSyndicated = entry.DateModified = DateTime.ParseExact("1975/01/23", "yyyy/MM/dd", CultureInfo.InvariantCulture);
            int entryId = UnitTestHelper.Create(entry);

            string enclosureUrl = "http://perseus.franklins.net/hanselminutes_0107.mp3";
            string enclosureMimeType = "audio/mp3";
            long enclosureSize = 26707573;

            FrameworkEnclosure enc = UnitTestHelper.BuildEnclosure("<Digital Photography Explained (for Geeks) with Aaron Hockley/>", enclosureUrl, enclosureMimeType, entryId, enclosureSize, true, true);
            Enclosures.Create(enc);

            entry = Entries.GetEntry(entryId, PostConfig.None, true);
            Assert.IsNotNull(entry.Enclosure, "There should be a enclosure here.");

            var subtextContext = new Mock<ISubtextContext>();
            subtextContext.Setup(c => c.Blog).Returns(Config.CurrentBlog);
            //TODO: FIX!!!
            subtextContext.Setup(c => c.Repository).Returns(ObjectProvider.Instance());


            MetaWeblog api = new MetaWeblog(subtextContext.Object);
            Post post = new Post();
            post.title = "Title 2";
            post.description = "Blah";
            post.dateCreated = DateTime.UtcNow;

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
            BlogRequest.Current.Blog = Config.GetBlog(hostname, "");
            Config.CurrentBlog.AllowServiceAccess = true;

            Entry entry = new Entry(PostType.BlogPost);
            entry.Title = "Title 1";
            entry.Body = "Blah";
            entry.IsActive = true;
            entry.DateCreated = entry.DateSyndicated = entry.DateModified = DateTime.ParseExact("1975/01/23", "yyyy/MM/dd", CultureInfo.InvariantCulture);
            int entryId = UnitTestHelper.Create(entry);

            Assert.IsNull(entry.Enclosure, "There should not be any enclosure here.");

            var subtextContext = new Mock<ISubtextContext>();
            subtextContext.Setup(c => c.Blog).Returns(Config.CurrentBlog);
            //TODO: FIX!!!
            subtextContext.Setup(c => c.Repository).Returns(ObjectProvider.Instance());


            MetaWeblog api = new MetaWeblog(subtextContext.Object);
            Post post = new Post();
            post.title = "Title 2";
            post.description = "Blah";
            post.dateCreated = DateTime.UtcNow;

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
            BlogRequest.Current.Blog = Config.GetBlog(hostname, "");
            Config.CurrentBlog.AllowServiceAccess = true;

            Entry entry = new Entry(PostType.BlogPost);
            entry.Title = "Title 1";
            entry.Body = "Blah";
            entry.IsActive = true;
            entry.DateCreated = entry.DateSyndicated = entry.DateModified = DateTime.ParseExact("1975/01/23", "yyyy/MM/dd", CultureInfo.InvariantCulture);
            int entryId = UnitTestHelper.Create(entry);

            string enclosureUrl = "http://perseus.franklins.net/hanselminutes_0107.mp3";
            string enclosureMimeType = "audio/mp3";
            long enclosureSize = 26707573;

            FrameworkEnclosure enc = UnitTestHelper.BuildEnclosure("<Digital Photography Explained (for Geeks) with Aaron Hockley/>", enclosureUrl, enclosureMimeType, entryId, enclosureSize, true, true);
            Enclosures.Create(enc);

            entry = Entries.GetEntry(entryId, PostConfig.None, true);
            Assert.IsNotNull(entry.Enclosure, "There should be a enclosure here.");

            var subtextContext = new Mock<ISubtextContext>();
            subtextContext.Setup(c => c.Blog).Returns(Config.CurrentBlog);
            //TODO: FIX!!!
            subtextContext.Setup(c => c.Repository).Returns(ObjectProvider.Instance());


            MetaWeblog api = new MetaWeblog(subtextContext.Object);
            Post post = new Post();
            post.title = "Title 2";
            post.description = "Blah";
            post.dateCreated = DateTime.UtcNow;

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
            BlogRequest.Current.Blog = Config.GetBlog(hostname, "");
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
            int entryId = UnitTestHelper.Create(entry);

            var subtextContext = new Mock<ISubtextContext>();
            subtextContext.Setup(c => c.Blog).Returns(Config.CurrentBlog);
            //TODO: FIX!!!
            subtextContext.Setup(c => c.Repository).Returns(ObjectProvider.Instance());


            MetaWeblog api = new MetaWeblog(subtextContext.Object);
            Post post = new Post();
            post.title = "Title 2";
            post.description = "Blah";
            post.categories = new string[] { category2Name };
            post.dateCreated = DateTime.UtcNow;

            bool result = api.editPost(entryId.ToString(CultureInfo.InvariantCulture), "username", "password", post, true);

            entry = Entries.GetEntry(entryId, PostConfig.None, true);
            Assert.AreEqual(1, entry.Categories.Count, "We expected one category. We didn't get what we expected.");
            Assert.AreEqual(category2Name, entry.Categories.First(), "Category has not been updated correctly.");
        }

        [Test]
        [RollBack]
        public void PostWithNoCategoriesRemovesCategoriesFromEntry()
        {
            string hostname = UnitTestHelper.GenerateUniqueString();
            Config.CreateBlog("", "username", "password", hostname, "");
            UnitTestHelper.SetHttpContextWithBlogRequest(hostname, "");
            BlogRequest.Current.Blog = Config.GetBlog(hostname, "");
            Config.CurrentBlog.AllowServiceAccess = true;

            string category1Name = UnitTestHelper.GenerateUniqueString();
            UnitTestHelper.CreateCategory(Config.CurrentBlog.Id, category1Name);

            Entry entry = new Entry(PostType.BlogPost);
            entry.Title = "Title 1";
            entry.Body = "Blah";
            entry.IsActive = true;
            entry.DateCreated = entry.DateSyndicated = entry.DateModified = DateTime.ParseExact("1975/01/23", "yyyy/MM/dd", CultureInfo.InvariantCulture);
            entry.Categories.Add(category1Name);
            int entryId = UnitTestHelper.Create(entry);

            var subtextContext = new Mock<ISubtextContext>();
            subtextContext.Setup(c => c.Blog).Returns(Config.CurrentBlog);
            //TODO: FIX!!!
            subtextContext.Setup(c => c.Repository).Returns(ObjectProvider.Instance());

            MetaWeblog api = new MetaWeblog(subtextContext.Object);
            Post post = new Post();
            post.title = "Title 2";
            post.description = "Blah";
            post.categories = null;
            post.dateCreated = DateTime.UtcNow;

            bool result = api.editPost(entryId.ToString(CultureInfo.InvariantCulture), "username", "password", post, true);

            entry = Entries.GetEntry(entryId, PostConfig.None, true);
            Assert.AreEqual(0, entry.Categories.Count, "We expected no category.");
        }

        [Test]
        [RollBack]
        public void GetRecentPosts_ReturnsRecentPosts()
        {
            string hostname = UnitTestHelper.GenerateUniqueString();
            Config.CreateBlog("", "username", "password", hostname, "");
            UnitTestHelper.SetHttpContextWithBlogRequest(hostname, "");
            Blog blog = Config.GetBlog(hostname, "");
            BlogRequest.Current.Blog = blog;
            blog.AllowServiceAccess = true;

            var urlHelper = new Mock<UrlHelper>();
            urlHelper.Setup(u => u.EntryUrl(It.IsAny<Entry>())).Returns("/entry/whatever");
            var subtextContext = new Mock<ISubtextContext>();
            subtextContext.Setup(c => c.Blog).Returns(Config.CurrentBlog);
            //TODO: FIX!!!
            subtextContext.Setup(c => c.Repository).Returns(ObjectProvider.Instance());
            subtextContext.SetupBlog(blog);
            subtextContext.Setup(c => c.UrlHelper).Returns(urlHelper.Object);

            MetaWeblog api = new MetaWeblog(subtextContext.Object);
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
            UnitTestHelper.Create(entry);

            entry = new Entry(PostType.BlogPost);
            entry.IncludeInMainSyndication = true;
            entry.Title = "Title 2";
            entry.Body = "Blah1";
            entry.IsActive = true;
            entry.DateCreated = entry.DateSyndicated = entry.DateModified = DateTime.ParseExact("1976/05/25", "yyyy/MM/dd", CultureInfo.InvariantCulture);
            entry.Categories.Add(category1Name);
            entry.Categories.Add(category2Name);
            UnitTestHelper.Create(entry);

            entry = new Entry(PostType.BlogPost);
            entry.Title = "Title 3";
            entry.IncludeInMainSyndication = true;
            entry.Body = "Blah2";
            entry.IsActive = true;
            entry.DateCreated = entry.DateSyndicated = entry.DateModified = DateTime.ParseExact("1979/09/16", "yyyy/MM/dd", CultureInfo.InvariantCulture);
            UnitTestHelper.Create(entry);

            entry = new Entry(PostType.BlogPost);
            entry.Title = "Title 4";
            entry.IncludeInMainSyndication = true;
            entry.Body = "Blah3";
            entry.IsActive = true;
            entry.DateCreated = entry.DateSyndicated = entry.DateModified = DateTime.ParseExact("2006/01/01", "yyyy/MM/dd", CultureInfo.InvariantCulture);
            entry.Categories.Add(category2Name);
            int entryId = UnitTestHelper.Create(entry);

            string enclosureUrl = "http://perseus.franklins.net/hanselminutes_0107.mp3";
            string enclosureMimeType = "audio/mp3";
            long enclosureSize = 26707573;

            FrameworkEnclosure enc = UnitTestHelper.BuildEnclosure("<Digital Photography Explained (for Geeks) with Aaron Hockley/>", enclosureUrl, enclosureMimeType, entryId, enclosureSize, true, true);
            Enclosures.Create(enc);

            posts = api.getRecentPosts(Config.CurrentBlog.Id.ToString(), "username", "password", 10);
            Assert.AreEqual(4, posts.Length, "Expected 4 posts");
            Assert.AreEqual(1, posts[3].categories.Length, "Expected our categories to be there.");
            Assert.AreEqual(2, posts[2].categories.Length, "Expected our categories to be there.");
            Assert.IsNotNull(posts[1].categories, "Expected our categories to be there.");
            Assert.AreEqual(1, posts[0].categories.Length, "Expected our categories to be there.");
            Assert.AreEqual(category1Name, posts[3].categories[0], "The category returned by the MetaBlogApi is wrong.");
            Assert.AreEqual(category2Name, posts[0].categories[0], "The category returned by the MetaBlogApi is wrong.");

            Assert.AreEqual(enclosureUrl, posts[0].enclosure.Value.url, "Not what we expected for the enclosure url.");
            Assert.AreEqual(enclosureMimeType, posts[0].enclosure.Value.type, "Not what we expected for the enclosure mimetype.");
            Assert.AreEqual(enclosureSize, posts[0].enclosure.Value.length, "Not what we expected for the enclosure size.");
        }

        [Test]
        [RollBack]
        public void GetPages_WithNumberOfPosts_ReturnsPostsInPages()
        {
            //arrange
            string hostname = UnitTestHelper.GenerateUniqueString();
            Config.CreateBlog("", "username", "password", hostname, "");
            UnitTestHelper.SetHttpContextWithBlogRequest(hostname, "");
            BlogRequest.Current.Blog = Config.GetBlog(hostname, "");
            Config.CurrentBlog.AllowServiceAccess = true;

            var urlHelper = new Mock<UrlHelper>();
            urlHelper.Setup(u => u.EntryUrl(It.IsAny<Entry>())).Returns("/entry/whatever");
            var subtextContext = new Mock<ISubtextContext>();
            subtextContext.Setup(c => c.Blog).Returns(Config.CurrentBlog);
            //TODO: FIX!!!
            subtextContext.Setup(c => c.Repository).Returns(ObjectProvider.Instance());

            subtextContext.Setup(c => c.UrlHelper).Returns(urlHelper.Object);

            MetaWeblog api = new MetaWeblog(subtextContext.Object);
            Post[] posts = api.getRecentPosts(Config.CurrentBlog.Id.ToString(), "username", "password", 10);
            Assert.AreEqual(0, posts.Length);

            string category1Name = UnitTestHelper.GenerateUniqueString();
            string category2Name = UnitTestHelper.GenerateUniqueString();
            UnitTestHelper.CreateCategory(Config.CurrentBlog.Id, category1Name);
            UnitTestHelper.CreateCategory(Config.CurrentBlog.Id, category2Name);

            Entry entry = new Entry(PostType.Story);
            entry.Title = "Title 1";
            entry.Body = "Blah";
            entry.IsActive = true;
            entry.IncludeInMainSyndication = true;
            entry.DateCreated = entry.DateSyndicated = entry.DateModified = DateTime.ParseExact("1975/01/23", "yyyy/MM/dd", CultureInfo.InvariantCulture);
            UnitTestHelper.Create(entry);

            entry = new Entry(PostType.Story);
            entry.IncludeInMainSyndication = true;
            entry.Title = "Title 2";
            entry.Body = "Blah1";
            entry.IsActive = true;
            entry.DateCreated = entry.DateSyndicated = entry.DateModified = DateTime.ParseExact("1976/05/25", "yyyy/MM/dd", CultureInfo.InvariantCulture);
            UnitTestHelper.Create(entry);

            entry = new Entry(PostType.Story);
            entry.Categories.Add(category1Name);
            entry.Categories.Add(category2Name);
            entry.Title = "Title 3";
            entry.IncludeInMainSyndication = true;
            entry.Body = "Blah2";
            entry.IsActive = true;
            entry.DateCreated = entry.DateSyndicated = entry.DateModified = DateTime.ParseExact("1979/09/16", "yyyy/MM/dd", CultureInfo.InvariantCulture);
            UnitTestHelper.Create(entry);

            entry = new Entry(PostType.Story);
            entry.Title = "Title 4";
            entry.IncludeInMainSyndication = true;
            entry.Body = "Blah3";
            entry.IsActive = true;
            entry.DateCreated = entry.DateSyndicated = entry.DateModified = DateTime.ParseExact("2006/01/01", "yyyy/MM/dd", CultureInfo.InvariantCulture);
            entry.Categories.Add(category2Name);
            int entryId = UnitTestHelper.Create(entry);

            string enclosureUrl = "http://perseus.franklins.net/hanselminutes_0107.mp3";
            string enclosureMimeType = "audio/mp3";
            long enclosureSize = 26707573;

            FrameworkEnclosure enc = UnitTestHelper.BuildEnclosure("<Digital Photography Explained (for Geeks) with Aaron Hockley/>", enclosureUrl, enclosureMimeType, entryId, enclosureSize, true, true);
            Enclosures.Create(enc);

            //act
            posts = api.getPages(Config.CurrentBlog.Id.ToString(), "username", "password", 2);

            //assert
            Assert.AreEqual(2, posts.Length);
            Assert.AreEqual(1, posts[0].categories.Length);
            Assert.AreEqual(2, posts[1].categories.Length);
            Assert.IsNotNull(posts[1].categories, "Expected our categories to be there.");

            Assert.AreEqual(enclosureUrl, posts[0].enclosure.Value.url);
            Assert.AreEqual(enclosureMimeType, posts[0].enclosure.Value.type);
            Assert.AreEqual(enclosureSize, posts[0].enclosure.Value.length);
        }

        [Test]
        [RollBack]
        public void GetPost_ReturnsPostWithhCorrectEntrUrl()
        {
            //arrange
            string hostname = UnitTestHelper.GenerateUniqueString();
            Config.CreateBlog("", "username", "password", hostname, "");
            UnitTestHelper.SetHttpContextWithBlogRequest(hostname, "");
            BlogRequest.Current.Blog = Config.GetBlog(hostname, "");
            Config.CurrentBlog.AllowServiceAccess = true;

            var urlHelper = new Mock<UrlHelper>();
            urlHelper.Setup(u => u.EntryUrl(It.IsAny<Entry>())).Returns("/entry/whatever");
            var subtextContext = new Mock<ISubtextContext>();
            subtextContext.Setup(c => c.Blog).Returns(Config.CurrentBlog);
            //TODO: FIX!!!
            subtextContext.Setup(c => c.Repository).Returns(ObjectProvider.Instance());

            subtextContext.Setup(c => c.UrlHelper).Returns(urlHelper.Object);

            MetaWeblog api = new MetaWeblog(subtextContext.Object);
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
            int entryId = UnitTestHelper.Create(entry);
            string enclosureUrl = "http://perseus.franklins.net/hanselminutes_0107.mp3";
            string enclosureMimeType = "audio/mp3";
            long enclosureSize = 26707573;

            FrameworkEnclosure enc = UnitTestHelper.BuildEnclosure("<Digital Photography Explained (for Geeks) with Aaron Hockley/>", enclosureUrl, enclosureMimeType, entryId, enclosureSize, true, true);
            Enclosures.Create(enc);

            //act
            var post = api.getPost(entryId.ToString(), "username", "password");

            //assert
            Assert.AreEqual(1, post.categories.Length);
            Assert.AreEqual("http://" + hostname + "/entry/whatever", post.link);
            Assert.AreEqual("http://" + hostname + "/entry/whatever", post.permalink);
            Assert.AreEqual(category1Name, post.categories[0]);
            Assert.AreEqual(enclosureUrl, post.enclosure.Value.url);
            Assert.AreEqual(enclosureMimeType, post.enclosure.Value.type);
            Assert.AreEqual(enclosureSize, post.enclosure.Value.length);
        }

        [Test]
        [RollBack]
        public void GetPage_ReturnsPostWithhCorrectEntrUrl()
        {
            //arrange
            string hostname = UnitTestHelper.GenerateUniqueString();
            Config.CreateBlog("", "username", "password", hostname, "");
            UnitTestHelper.SetHttpContextWithBlogRequest(hostname, "");
            BlogRequest.Current.Blog = Config.GetBlog(hostname, "");
            Config.CurrentBlog.AllowServiceAccess = true;

            var urlHelper = new Mock<UrlHelper>();
            urlHelper.Setup(u => u.EntryUrl(It.IsAny<Entry>())).Returns("/entry/whatever");
            var subtextContext = new Mock<ISubtextContext>();
            subtextContext.Setup(c => c.Blog).Returns(Config.CurrentBlog);
            //TODO: FIX!!!
            subtextContext.Setup(c => c.Repository).Returns(ObjectProvider.Instance());

            subtextContext.Setup(c => c.UrlHelper).Returns(urlHelper.Object);

            MetaWeblog api = new MetaWeblog(subtextContext.Object);
            string category1Name = UnitTestHelper.GenerateUniqueString();
            string category2Name = UnitTestHelper.GenerateUniqueString();
            UnitTestHelper.CreateCategory(Config.CurrentBlog.Id, category1Name);
            UnitTestHelper.CreateCategory(Config.CurrentBlog.Id, category2Name);

            Entry entry = new Entry(PostType.Story);
            entry.Title = "Title 1";
            entry.Body = "Blah";
            entry.IsActive = true;
            entry.IncludeInMainSyndication = true;
            entry.DateCreated = entry.DateSyndicated = entry.DateModified = DateTime.ParseExact("1975/01/23", "yyyy/MM/dd", CultureInfo.InvariantCulture);
            entry.Categories.Add(category1Name);
            int entryId = UnitTestHelper.Create(entry);
            string enclosureUrl = "http://perseus.franklins.net/hanselminutes_0107.mp3";
            string enclosureMimeType = "audio/mp3";
            long enclosureSize = 26707573;

            FrameworkEnclosure enc = UnitTestHelper.BuildEnclosure("<Digital Photography Explained (for Geeks) with Aaron Hockley/>", enclosureUrl, enclosureMimeType, entryId, enclosureSize, true, true);
            Enclosures.Create(enc);

            //act
            var post = api.getPage(Config.CurrentBlog.Id.ToString(), entryId.ToString(), "username", "password");

            //assert
            Assert.AreEqual(1, post.categories.Length);
            Assert.AreEqual("http://" + hostname + "/entry/whatever", post.link);
            Assert.AreEqual("http://" + hostname + "/entry/whatever", post.permalink);
            Assert.AreEqual(category1Name, post.categories[0]);
            Assert.AreEqual(enclosureUrl, post.enclosure.Value.url);
            Assert.AreEqual(enclosureMimeType, post.enclosure.Value.type);
            Assert.AreEqual(enclosureSize, post.enclosure.Value.length);
        }

    }
}
