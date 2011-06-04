using System;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using MbUnit.Framework;
using Moq;
using Subtext.Extensibility;
using Subtext.Framework;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;
using Subtext.Framework.Data;
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
            var blog = new Blog { AllowServiceAccess = true, Host = "localhost", UserName = "username", Password = "password" };
            var category = new LinkCategory
            {
                BlogId = blog.Id,
                IsActive = true,
                Description = "Test category",
                Title = "CategoryA",
                CategoryType = CategoryType.PostCollection,
                Id = 42
            };

            var subtextContext = new Mock<ISubtextContext>();
            subtextContext.Setup(c => c.Blog).Returns(blog);
            subtextContext.Setup(c => c.UrlHelper.CategoryUrl(It.IsAny<LinkCategory>())).Returns("/Category/42.aspx");
            subtextContext.Setup(c => c.UrlHelper.CategoryRssUrl(It.IsAny<LinkCategory>())).Returns("/rss.aspx?catId=42");
            subtextContext.Setup(c => c.Repository.GetCategories(CategoryType.PostCollection, false)).Returns(new[] { category });
            subtextContext.Setup(c => c.ServiceLocator).Returns(new Mock<IDependencyResolver>().Object);
            var api = new MetaWeblog(subtextContext.Object);

            //act
            CategoryInfo[] categories = api.getCategories(blog.Id.ToString(), "username", "password");

            //assert
            Assert.AreEqual(1, categories.Length);
            Assert.AreEqual("http://localhost/Category/42.aspx", categories[0].htmlUrl);
            Assert.AreEqual("http://localhost/rss.aspx?catId=42", categories[0].rssUrl);
        }

        [Test]
        public void newPost_WithCategory_CreatesEntryWithCategory()
        {
            //arrange
            var repository = new DatabaseObjectProvider();
            var blog = new Blog { Id = 42, UserName = "username", Password = "password", AllowServiceAccess = true, Host = "localhost" };

            var entryPublisher = new Mock<IEntryPublisher>();
            Entry publishedEntry = null;
            entryPublisher.Setup(publisher => publisher.Publish(It.IsAny<Entry>())).Callback<Entry>(e => publishedEntry = e);
            var subtextContext = new Mock<ISubtextContext>();
            subtextContext.Setup(c => c.Blog).Returns(blog);
            subtextContext.Setup(c => c.Repository).Returns(repository);
            subtextContext.Setup(c => c.ServiceLocator).Returns(new Mock<IDependencyResolver>().Object);

            var api = new MetaWeblog(subtextContext.Object, entryPublisher.Object);
            var post = new Post
            {
                categories = new[] { "CategoryA" },
                description = "A unit test",
                title = "A unit testing title",
                dateCreated = DateTime.UtcNow
            };

            //act
            api.newPost("42", "username", "password", post, true);

            //assert
            Assert.IsNotNull(publishedEntry);
            Assert.AreEqual(1, publishedEntry.Categories.Count);
            Assert.AreEqual("CategoryA", publishedEntry.Categories.First());
        }

        [Test]
        public void NewPost_WithNullCategories_DoesNotTHrowException()
        {
            //arrange
            var blog = new Blog { Id = 42, UserName = "username", Password = "password", AllowServiceAccess = true, Host = "localhost" };

            var subtextContext = new Mock<ISubtextContext>();
            subtextContext.Setup(c => c.Blog).Returns(blog);
            Entry publishedEntry = null;
            var entryPublisher = new Mock<IEntryPublisher>();
            entryPublisher.Setup(publisher => publisher.Publish(It.IsAny<Entry>())).Returns(42).Callback<Entry>(
                entry => publishedEntry = entry);

            var api = new MetaWeblog(subtextContext.Object, entryPublisher.Object);
            var post = new Post
            {
                categories = null,
                description = "A unit test",
                title = "A unit testing title",
                dateCreated = DateTime.UtcNow
            };

            // act
            string result = api.newPost(blog.Id.ToString(CultureInfo.InvariantCulture), "username", "password", post,
                                        true);

            // assert
            int entryId = int.Parse(result);
            Assert.AreEqual(42, entryId);
            Assert.AreEqual(0, publishedEntry.Categories.Count, "Should not have added categories.");
        }

        [Test]
        public void NewPost_WithFutureDate_SyndicatesInTheFuture()
        {
            //arrange
            var blog = new Blog { Id = 42, UserName = "username", Password = "password", AllowServiceAccess = true, Host = "localhost" };

            var subtextContext = new Mock<ISubtextContext>();
            subtextContext.Setup(c => c.Blog).Returns(blog);
            Entry publishedEntry = null;
            var entryPublisher = new Mock<IEntryPublisher>();
            entryPublisher.Setup(publisher => publisher.Publish(It.IsAny<Entry>())).Returns(42).Callback<Entry>(
                entry => publishedEntry = entry);
            DateTime now = DateTime.UtcNow;

            var api = new MetaWeblog(subtextContext.Object, entryPublisher.Object);
            var post = new Post();
            post.categories = null;
            post.description = "A unit test";
            post.title = "A unit testing title";
            post.dateCreated = now.AddDays(1);

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
            var blog = new Blog { Id = 42, UserName = "username", Password = "password", AllowServiceAccess = true, Host = "localhost" };

            var subtextContext = new Mock<ISubtextContext>();
            subtextContext.Setup(c => c.Blog).Returns(blog);
            FrameworkEnclosure publishedEnclosure = null;
            subtextContext.Setup(c => c.Repository.Create(It.IsAny<FrameworkEnclosure>())).Callback<FrameworkEnclosure>(
                enclosure => publishedEnclosure = enclosure);
            var entryPublisher = new Mock<IEntryPublisher>();
            entryPublisher.Setup(publisher => publisher.Publish(It.IsAny<Entry>())).Returns(42);
            DateTime now = DateTime.UtcNow;
            DateTime utcNow = now.ToUniversalTime();

            var api = new MetaWeblog(subtextContext.Object, entryPublisher.Object);
            var post = new Post();
            post.categories = null;
            post.description = "A unit test";
            post.title = "A unit testing title";
            post.dateCreated = utcNow.AddDays(1);

            var postEnclosure = new Enclosure();
            postEnclosure.url = "http://codeclimber.net.nz/podcast/mypodcast.mp3";
            postEnclosure.type = "audio/mp3";
            postEnclosure.length = 123456789;
            post.enclosure = postEnclosure;

            // act
            string result = api.newPost(blog.Id.ToString(CultureInfo.InvariantCulture), "username", "password", post,
                                        true);

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
            var blog = new Blog { Id = 42, UserName = "username", Password = "password", AllowServiceAccess = true, Host = "localhost" };

            var subtextContext = new Mock<ISubtextContext>();
            subtextContext.Setup(c => c.Blog).Returns(blog);
            Entry publishedEntry = null;
            var entryPublisher = new Mock<IEntryPublisher>();
            entryPublisher.Setup(publisher => publisher.Publish(It.IsAny<Entry>())).Returns(42).Callback<Entry>(
                entry => publishedEntry = entry);
            DateTime now = DateTime.UtcNow;
            DateTime utcNow = now.ToUniversalTime();

            var api = new MetaWeblog(subtextContext.Object, entryPublisher.Object);
            var post = new Post();
            post.categories = null;
            post.description = "A unit test";
            post.title = "A unit testing title";
            post.dateCreated = utcNow.AddDays(1);
            post.enclosure = null;

            // act
            string result = api.newPost(blog.Id.ToString(CultureInfo.InvariantCulture), "username", "password", post,
                                        true);

            // assert
            Assert.IsNull(publishedEntry.Enclosure);
        }

        [Test]
        public void editPost_WithEntryHavingEnclosure_UpdatesEntryEnclosureWithNewEnclosure()
        {
            //arrange
            var entry = new Entry(PostType.BlogPost) { Title = "Title 1", Body = "Blah", IsActive = true };
            entry.DateCreatedUtc = entry.DatePublishedUtc = entry.DateModifiedUtc = DateTime.ParseExact("1975/01/23", "yyyy/MM/dd", CultureInfo.InvariantCulture);
            entry.Categories.Add("TestCategory");
            var blog = new Blog { Id = 123, Host = "localhost", AllowServiceAccess = true, UserName = "username", Password = "password" };
            var subtextContext = new Mock<ISubtextContext>();
            subtextContext.Setup(c => c.Blog).Returns(blog);
            subtextContext.Setup(c => c.Repository.GetEntry(It.IsAny<Int32>(), false, true)).Returns(entry);
            var entryPublisher = new Mock<IEntryPublisher>();
            Entry publishedEntry = null;
            entryPublisher.Setup(p => p.Publish(It.IsAny<Entry>())).Callback<Entry>(e => publishedEntry = e);
            FrameworkEnclosure enclosure = UnitTestHelper.BuildEnclosure("<Digital Photography Explained (for Geeks) with Aaron Hockley/>",
                                              "http://perseus.franklins.net/hanselminutes_0107.mp3", "audio/mp3", 123, 26707573, true, true);
            entry.Enclosure = enclosure;
            var post = new Post { title = "Title 2", description = "Blah", dateCreated = DateTime.UtcNow };

            var postEnclosure = new Enclosure
            {
                url = "http://codeclimber.net.nz/podcast/mypodcastUpdated.mp3",
                type = "audio/mp3",
                length = 123456789
            };
            post.enclosure = postEnclosure;
            var metaWeblog = new MetaWeblog(subtextContext.Object, entryPublisher.Object);

            // act
            bool result = metaWeblog.editPost("123", "username", "password", post, true);

            // assert
            Assert.IsTrue(result);
            Assert.IsNotNull(publishedEntry.Enclosure);
            Assert.AreEqual("http://codeclimber.net.nz/podcast/mypodcastUpdated.mp3", entry.Enclosure.Url);
        }

        [Test]
        public void editPost_WithEnclosure_AddsNewEnclosure()
        {
            //arrange
            FrameworkEnclosure enclosure = UnitTestHelper.BuildEnclosure("<Digital Photography Explained (for Geeks) with Aaron Hockley/>",
                                              "http://example.com/foo.mp3", "audio/mp3", 123, 26707573, true, true);
            var entry = new Entry(PostType.BlogPost) { Title = "Title 1", Body = "Blah", IsActive = true, Enclosure = enclosure };
            entry.DateCreatedUtc = entry.DatePublishedUtc = entry.DateModifiedUtc = DateTime.ParseExact("1975/01/23", "yyyy/MM/dd", CultureInfo.InvariantCulture);
            entry.Categories.Add("TestCategory");
            var blog = new Blog { Id = 123, Host = "localhost", AllowServiceAccess = true, UserName = "username", Password = "password" };
            var subtextContext = new Mock<ISubtextContext>();
            subtextContext.Setup(c => c.Blog).Returns(blog);
            subtextContext.Setup(c => c.Repository.GetEntry(It.IsAny<Int32>(), false, true)).Returns(entry);
            var entryPublisher = new Mock<IEntryPublisher>();
            Entry publishedEntry = null;
            entryPublisher.Setup(p => p.Publish(It.IsAny<Entry>())).Callback<Entry>(e => publishedEntry = e);
            var post = new Post { title = "Title 2", description = "Blah", dateCreated = DateTime.UtcNow };
            var postEnclosure = new Enclosure
            {
                url = "http://example.com/bar.mp3",
                type = "audio/mp3",
                length = 123456789
            };
            post.enclosure = postEnclosure;
            var metaWeblog = new MetaWeblog(subtextContext.Object, entryPublisher.Object);

            // act
            bool result = metaWeblog.editPost("123", "username", "password", post, true);

            // assert
            Assert.IsNotNull(publishedEntry.Enclosure);
            Assert.AreEqual("http://example.com/bar.mp3", entry.Enclosure.Url);
            Assert.AreEqual("audio/mp3", entry.Enclosure.MimeType);
            Assert.AreEqual(123456789, entry.Enclosure.Size);
            Assert.IsTrue(result);
        }

        [Test]
        public void EditPost_WithoutEnclosure_RemovesEnclosureFromEntry()
        {
            // arrange
            FrameworkEnclosure enclosure = UnitTestHelper.BuildEnclosure("<Digital Photography Explained (for Geeks) with Aaron Hockley/>",
                                              "http://example.com/foo.mp3", "audio/mp3", 123, 2650, true, true);
            enclosure.Id = 321;
            var entry = new Entry(PostType.BlogPost) { Title = "Title 1", Body = "Blah", IsActive = true, Enclosure = enclosure };
            entry.DateCreatedUtc = entry.DatePublishedUtc = entry.DateModifiedUtc = DateTime.ParseExact("1975/01/23", "yyyy/MM/dd", CultureInfo.InvariantCulture);
            var subtextContext = new Mock<ISubtextContext>();
            var blog = new Blog { Id = 999, Host = "localhost", AllowServiceAccess = true, UserName = "username", Password = "password" };
            subtextContext.Setup(c => c.Blog).Returns(blog);
            subtextContext.Setup(c => c.Repository.GetEntry(It.IsAny<Int32>(), false, true)).Returns(entry);
            bool enclosureDeleted = false;
            subtextContext.Setup(c => c.Repository.DeleteEnclosure(321)).Callback(() => enclosureDeleted = true);
            var entryPublisher = new Mock<IEntryPublisher>();
            entryPublisher.Setup(p => p.Publish(It.IsAny<Entry>()));
            var post = new Post { title = "Title 2", description = "Blah", dateCreated = DateTime.UtcNow };
            var api = new MetaWeblog(subtextContext.Object, entryPublisher.Object);

            // act
            bool result = api.editPost("999", "username", "password", post, true);

            // assert
            Assert.IsTrue(enclosureDeleted);
            Assert.IsTrue(result);
        }

        [Test]
        [RollBack2]
        public void editPost_WithPostHavingDifferentCategoryThanEntry_UpdatesCategory()
        {
            // arrange
            var entry = new Entry(PostType.BlogPost) { Id = 12345, Title = "Title 1", Body = "Blah", IsActive = true };
            entry.Categories.Add("Category1");
            entry.DateCreatedUtc = entry.DatePublishedUtc = entry.DateModifiedUtc = DateTime.ParseExact("1975/01/23", "yyyy/MM/dd", CultureInfo.InvariantCulture);
            var subtextContext = new Mock<ISubtextContext>();
            var blog = new Blog { Id = 999, Host = "localhost", AllowServiceAccess = true, UserName = "username", Password = "password" };
            subtextContext.Setup(c => c.Blog).Returns(blog);
            subtextContext.Setup(c => c.Repository.GetEntry(It.IsAny<Int32>(), false, true)).Returns(entry);
            var entryPublisher = new Mock<IEntryPublisher>();
            Entry publishedEntry = null;
            entryPublisher.Setup(p => p.Publish(It.IsAny<Entry>())).Callback<Entry>(e => publishedEntry = e);
            var post = new Post { title = "Title 2", description = "Blah", categories = new[] { "Category2" }, dateCreated = DateTime.UtcNow };
            var api = new MetaWeblog(subtextContext.Object, entryPublisher.Object);

            // act
            bool result = api.editPost("12345", "username", "password", post, true);

            // assert
            Assert.AreEqual(1, publishedEntry.Categories.Count);
            Assert.AreEqual("Category2", publishedEntry.Categories.First());
            Assert.IsTrue(result);
        }

        [Test]
        public void editPost_WithNoCategories_RemovesCategoriesFromEntry()
        {
            //arrange
            var entry = new Entry(PostType.BlogPost) { Title = "Title 1", Body = "Blah", IsActive = true };
            entry.DateCreatedUtc = entry.DatePublishedUtc = entry.DateModifiedUtc = DateTime.ParseExact("1975/01/23", "yyyy/MM/dd", CultureInfo.InvariantCulture);
            entry.Categories.Add("TestCategory");
            var blog = new Blog { Id = 123, Host = "localhost", AllowServiceAccess = true, UserName = "username", Password = "password" };
            var subtextContext = new Mock<ISubtextContext>();
            subtextContext.Setup(c => c.Blog).Returns(blog);
            subtextContext.Setup(c => c.Repository.GetEntry(It.IsAny<Int32>(), false, true)).Returns(entry);
            var entryPublisher = new Mock<IEntryPublisher>();
            Entry publishedEntry = null;
            entryPublisher.Setup(p => p.Publish(It.IsAny<Entry>())).Callback<Entry>(e => publishedEntry = e);
            var post = new Post { title = "Title 2", description = "Blah", categories = null, dateCreated = DateTime.UtcNow };
            var metaWeblog = new MetaWeblog(subtextContext.Object, entryPublisher.Object);

            // act
            metaWeblog.editPost("123", "username", "password", post, true);

            // assert
            Assert.AreEqual(0, publishedEntry.Categories.Count, "We expected no category.");
        }

        [Test]
        [RollBack2]
        public void GetRecentPosts_ReturnsRecentPosts()
        {
            string hostname = UnitTestHelper.GenerateUniqueString();
            var repository = new DatabaseObjectProvider();
            repository.CreateBlog("", "username", "password", hostname, "");
            UnitTestHelper.SetHttpContextWithBlogRequest(hostname, "");
            Blog blog = repository.GetBlog(hostname, "");
            BlogRequest.Current.Blog = blog;
            blog.AllowServiceAccess = true;

            var urlHelper = new Mock<BlogUrlHelper>();
            urlHelper.Setup(u => u.EntryUrl(It.IsAny<Entry>())).Returns("/entry/whatever");
            var subtextContext = new Mock<ISubtextContext>();
            subtextContext.Setup(c => c.Blog).Returns(Config.CurrentBlog);
            //TODO: FIX!!!
            subtextContext.Setup(c => c.Repository).Returns(repository);
            subtextContext.SetupBlog(blog);
            subtextContext.Setup(c => c.UrlHelper).Returns(urlHelper.Object);
            subtextContext.Setup(c => c.ServiceLocator).Returns(new Mock<IDependencyResolver>().Object);

            var api = new MetaWeblog(subtextContext.Object);
            Post[] posts = api.getRecentPosts(Config.CurrentBlog.Id.ToString(), "username", "password", 10);
            Assert.AreEqual(0, posts.Length);

            string category1Name = UnitTestHelper.GenerateUniqueString();
            string category2Name = UnitTestHelper.GenerateUniqueString();
            UnitTestHelper.CreateCategory(Config.CurrentBlog.Id, category1Name);
            UnitTestHelper.CreateCategory(Config.CurrentBlog.Id, category2Name);

            var entry = new Entry(PostType.BlogPost);
            entry.Title = "Title 1";
            entry.Body = "Blah";
            entry.IsActive = true;
            entry.IncludeInMainSyndication = true;
            entry.DateCreatedUtc =
                entry.DatePublishedUtc =
                entry.DateModifiedUtc = DateTime.ParseExact("1975/01/23", "yyyy/MM/dd", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal).ToUniversalTime();
            entry.Categories.Add(category1Name);
            UnitTestHelper.Create(entry);

            entry = new Entry(PostType.BlogPost);
            entry.IncludeInMainSyndication = true;
            entry.Title = "Title 2";
            entry.Body = "Blah1";
            entry.IsActive = true;
            entry.DateCreatedUtc =
                entry.DatePublishedUtc =
                entry.DateModifiedUtc = DateTime.ParseExact("1976/05/25", "yyyy/MM/dd", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal).ToUniversalTime();
            entry.Categories.Add(category1Name);
            entry.Categories.Add(category2Name);
            UnitTestHelper.Create(entry);

            entry = new Entry(PostType.BlogPost);
            entry.Title = "Title 3";
            entry.IncludeInMainSyndication = true;
            entry.Body = "Blah2";
            entry.IsActive = true;
            entry.DateCreatedUtc =
                entry.DatePublishedUtc =
                entry.DateModifiedUtc = DateTime.ParseExact("1979/09/16", "yyyy/MM/dd", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal).ToUniversalTime();
            UnitTestHelper.Create(entry);

            entry = new Entry(PostType.BlogPost);
            entry.Title = "Title 4";
            entry.IncludeInMainSyndication = true;
            entry.Body = "Blah3";
            entry.IsActive = true;
            entry.DateCreatedUtc =
                entry.DatePublishedUtc =
                entry.DateModifiedUtc = DateTime.ParseExact("2006/01/01", "yyyy/MM/dd", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal).ToUniversalTime();
            entry.Categories.Add(category2Name);
            int entryId = UnitTestHelper.Create(entry);

            string enclosureUrl = "http://perseus.franklins.net/hanselminutes_0107.mp3";
            string enclosureMimeType = "audio/mp3";
            long enclosureSize = 26707573;

            FrameworkEnclosure enc =
                UnitTestHelper.BuildEnclosure("<Digital Photography Explained (for Geeks) with Aaron Hockley/>",
                                              enclosureUrl, enclosureMimeType, entryId, enclosureSize, true, true);
            repository.Create(enc);

            posts = api.getRecentPosts(Config.CurrentBlog.Id.ToString(), "username", "password", 10);
            Assert.AreEqual(4, posts.Length, "Expected 4 posts");
            Assert.AreEqual(1, posts[3].categories.Length, "Expected our categories to be there.");
            Assert.AreEqual(2, posts[2].categories.Length, "Expected our categories to be there.");
            Assert.IsNotNull(posts[1].categories, "Expected our categories to be there.");
            Assert.AreEqual(1, posts[0].categories.Length, "Expected our categories to be there.");
            Assert.AreEqual(category1Name, posts[3].categories[0], "The category returned by the MetaBlogApi is wrong.");
            Assert.AreEqual(category2Name, posts[0].categories[0], "The category returned by the MetaBlogApi is wrong.");

            Assert.AreEqual(enclosureUrl, posts[0].enclosure.Value.url, "Not what we expected for the enclosure url.");
            Assert.AreEqual(enclosureMimeType, posts[0].enclosure.Value.type,
                            "Not what we expected for the enclosure mimetype.");
            Assert.AreEqual(enclosureSize, posts[0].enclosure.Value.length,
                            "Not what we expected for the enclosure size.");
        }

        [Test]
        [RollBack2]
        public void GetPages_WithNumberOfPosts_ReturnsPostsInPages()
        {
            //arrange
            var repository = new DatabaseObjectProvider();
            string hostname = UnitTestHelper.GenerateUniqueString();
            repository.CreateBlog("", "username", "password", hostname, "");
            UnitTestHelper.SetHttpContextWithBlogRequest(hostname, "");
            BlogRequest.Current.Blog = repository.GetBlog(hostname, "");
            Config.CurrentBlog.AllowServiceAccess = true;

            var urlHelper = new Mock<BlogUrlHelper>();
            urlHelper.Setup(u => u.EntryUrl(It.IsAny<Entry>())).Returns("/entry/whatever");
            var subtextContext = new Mock<ISubtextContext>();
            subtextContext.Setup(c => c.Blog).Returns(Config.CurrentBlog);
            //TODO: FIX!!!
            subtextContext.Setup(c => c.Repository).Returns(repository);
            subtextContext.Setup(c => c.ServiceLocator).Returns(new Mock<IDependencyResolver>().Object);
            subtextContext.Setup(c => c.UrlHelper).Returns(urlHelper.Object);

            var api = new MetaWeblog(subtextContext.Object);
            Post[] posts = api.getRecentPosts(Config.CurrentBlog.Id.ToString(), "username", "password", 10);
            Assert.AreEqual(0, posts.Length);

            string category1Name = UnitTestHelper.GenerateUniqueString();
            string category2Name = UnitTestHelper.GenerateUniqueString();
            UnitTestHelper.CreateCategory(Config.CurrentBlog.Id, category1Name);
            UnitTestHelper.CreateCategory(Config.CurrentBlog.Id, category2Name);

            var entry = new Entry(PostType.Story);
            entry.Title = "Title 1";
            entry.Body = "Blah";
            entry.IsActive = true;
            entry.IncludeInMainSyndication = true;
            entry.DateCreatedUtc =
                entry.DatePublishedUtc =
                entry.DateModifiedUtc = DateTime.ParseExact("1975/01/23", "yyyy/MM/dd", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal).ToUniversalTime();
            UnitTestHelper.Create(entry);

            entry = new Entry(PostType.Story);
            entry.IncludeInMainSyndication = true;
            entry.Title = "Title 2";
            entry.Body = "Blah1";
            entry.IsActive = true;
            entry.DateCreatedUtc =
                entry.DatePublishedUtc =
                entry.DateModifiedUtc = DateTime.ParseExact("1976/05/25", "yyyy/MM/dd", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal).ToUniversalTime();
            UnitTestHelper.Create(entry);

            entry = new Entry(PostType.Story);
            entry.Categories.Add(category1Name);
            entry.Categories.Add(category2Name);
            entry.Title = "Title 3";
            entry.IncludeInMainSyndication = true;
            entry.Body = "Blah2";
            entry.IsActive = true;
            entry.DateCreatedUtc =
                entry.DatePublishedUtc =
                entry.DateModifiedUtc = DateTime.ParseExact("1979/09/16", "yyyy/MM/dd", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal).ToUniversalTime();
            UnitTestHelper.Create(entry);

            entry = new Entry(PostType.Story);
            entry.Title = "Title 4";
            entry.IncludeInMainSyndication = true;
            entry.Body = "Blah3";
            entry.IsActive = true;
            entry.DateCreatedUtc =
                entry.DatePublishedUtc =
                entry.DateModifiedUtc = DateTime.ParseExact("2006/01/01", "yyyy/MM/dd", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal).ToUniversalTime();
            entry.Categories.Add(category2Name);
            int entryId = UnitTestHelper.Create(entry);

            string enclosureUrl = "http://perseus.franklins.net/hanselminutes_0107.mp3";
            string enclosureMimeType = "audio/mp3";
            long enclosureSize = 26707573;

            FrameworkEnclosure enc =
                UnitTestHelper.BuildEnclosure("<Digital Photography Explained (for Geeks) with Aaron Hockley/>",
                                              enclosureUrl, enclosureMimeType, entryId, enclosureSize, true, true);
            repository.Create(enc);

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
        [RollBack2]
        public void GetPost_WithEntryId_ReturnsPostWithCorrectEntryUrl()
        {
            //arrange
            var repository = new DatabaseObjectProvider();
            string hostname = UnitTestHelper.GenerateUniqueString();
            repository.CreateBlog("", "username", "password", hostname, "");
            UnitTestHelper.SetHttpContextWithBlogRequest(hostname, "");
            BlogRequest.Current.Blog = repository.GetBlog(hostname, "");
            Config.CurrentBlog.AllowServiceAccess = true;

            var urlHelper = new Mock<BlogUrlHelper>();
            urlHelper.Setup(u => u.EntryUrl(It.IsAny<Entry>())).Returns("/entry/whatever");
            var subtextContext = new Mock<ISubtextContext>();
            subtextContext.Setup(c => c.Blog).Returns(Config.CurrentBlog);
            //TODO: FIX!!!
            subtextContext.Setup(c => c.Repository).Returns(repository);
            subtextContext.Setup(c => c.ServiceLocator).Returns(new Mock<IDependencyResolver>().Object);
            subtextContext.Setup(c => c.UrlHelper).Returns(urlHelper.Object);

            var api = new MetaWeblog(subtextContext.Object);
            string category1Name = UnitTestHelper.GenerateUniqueString();
            string category2Name = UnitTestHelper.GenerateUniqueString();
            UnitTestHelper.CreateCategory(Config.CurrentBlog.Id, category1Name);
            UnitTestHelper.CreateCategory(Config.CurrentBlog.Id, category2Name);

            var entry = new Entry(PostType.BlogPost);
            entry.Title = "Title 1";
            entry.Body = "Blah";
            entry.IsActive = true;
            entry.IncludeInMainSyndication = true;
            entry.DateCreatedUtc =
                entry.DatePublishedUtc =
                entry.DateModifiedUtc = DateTime.ParseExact("1975/01/23", "yyyy/MM/dd", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal).ToUniversalTime();
            entry.Categories.Add(category1Name);
            int entryId = UnitTestHelper.Create(entry);
            string enclosureUrl = "http://perseus.franklins.net/hanselminutes_0107.mp3";
            string enclosureMimeType = "audio/mp3";
            long enclosureSize = 26707573;

            FrameworkEnclosure enc =
                UnitTestHelper.BuildEnclosure("<Digital Photography Explained (for Geeks) with Aaron Hockley/>",
                                              enclosureUrl, enclosureMimeType, entryId, enclosureSize, true, true);
            repository.Create(enc);

            //act
            Post post = api.getPost(entryId.ToString(), "username", "password");

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
        [RollBack2]
        public void GetPage_ReturnsPostWithhCorrectEntrUrl()
        {
            //arrange
            var repository = new DatabaseObjectProvider();
            string hostname = UnitTestHelper.GenerateUniqueString();
            repository.CreateBlog("", "username", "password", hostname, "");
            UnitTestHelper.SetHttpContextWithBlogRequest(hostname, "");
            BlogRequest.Current.Blog = repository.GetBlog(hostname, "");
            Config.CurrentBlog.AllowServiceAccess = true;

            var urlHelper = new Mock<BlogUrlHelper>();
            urlHelper.Setup(u => u.EntryUrl(It.IsAny<Entry>())).Returns("/entry/whatever");
            var subtextContext = new Mock<ISubtextContext>();
            subtextContext.Setup(c => c.Blog).Returns(Config.CurrentBlog);
            //TODO: FIX!!!
            subtextContext.Setup(c => c.Repository).Returns(repository);
            subtextContext.Setup(c => c.ServiceLocator).Returns(new Mock<IDependencyResolver>().Object);
            subtextContext.Setup(c => c.UrlHelper).Returns(urlHelper.Object);

            var api = new MetaWeblog(subtextContext.Object);
            string category1Name = UnitTestHelper.GenerateUniqueString();
            string category2Name = UnitTestHelper.GenerateUniqueString();
            UnitTestHelper.CreateCategory(Config.CurrentBlog.Id, category1Name);
            UnitTestHelper.CreateCategory(Config.CurrentBlog.Id, category2Name);

            var entry = new Entry(PostType.Story);
            entry.Title = "Title 1";
            entry.Body = "Blah";
            entry.IsActive = true;
            entry.IncludeInMainSyndication = true;
            entry.DateCreatedUtc =
                entry.DatePublishedUtc =
                entry.DateModifiedUtc = DateTime.ParseExact("1975/01/23", "yyyy/MM/dd", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal).ToUniversalTime();
            entry.Categories.Add(category1Name);
            int entryId = UnitTestHelper.Create(entry);
            string enclosureUrl = "http://perseus.franklins.net/hanselminutes_0107.mp3";
            string enclosureMimeType = "audio/mp3";
            long enclosureSize = 26707573;


            FrameworkEnclosure enc =
                UnitTestHelper.BuildEnclosure("<Digital Photography Explained (for Geeks) with Aaron Hockley/>",
                                              enclosureUrl, enclosureMimeType, entryId, enclosureSize, true, true);
            repository.Create(enc);

            //act
            Post post = api.getPage(Config.CurrentBlog.Id.ToString(), entryId.ToString(), "username", "password");

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