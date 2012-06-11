using System.Collections.Generic;
using System.Linq;
using BlogML.Xml;
using MbUnit.Framework;
using Moq;
using Subtext.Extensibility.Interfaces;
using Subtext.Framework;
using Subtext.Framework.Components;
using Subtext.ImportExport;

namespace UnitTests.Subtext.BlogMl
{
    [TestFixture]
    public class BlogMLSourceTests
    {
        [Test]
        public void GetBlog_WithBlogInSubtextContext_ConvertsBlogToBlogML()
        {
            // arrange
            var blog = new Blog {Title = "Test Blog Title"};
            var context = new Mock<ISubtextContext>();
            context.Setup(c => c.Repository.GetCategories(CategoryType.PostCollection, false)).Returns(new List<LinkCategory>());
            context.Setup(c => c.Blog).Returns(blog);
            var converter = new Mock<IBlogMLExportMapper>();
            converter.Setup(c => c.ConvertBlog(blog)).Returns(new BlogMLBlog {Title = "Converted"});
            var source = new BlogMLSource(context.Object, converter.Object);

            // act
            var blogMLBlog = source.GetBlog();

            // assert
            Assert.AreEqual("Converted", blogMLBlog.Title);
        }

        [Test]
        public void GetBlog_WithBlogHavingCategories_GetsCategoriesFromSource()
        {
            // arrange
            var categories = new List<LinkCategory> {new LinkCategory(1, "Any Title")};
            var context = new Mock<ISubtextContext>();
            context.Setup(c => c.Blog).Returns(new Blog());
            context.Setup(c => c.Repository.GetCategories(CategoryType.PostCollection, false /*activeOnly*/)).Returns(categories);
            var blogMLCategories = new List<BlogMLCategory> {new BlogMLCategory {Title = "The First Category"}};
            var converter = new Mock<IBlogMLExportMapper>();
            converter.Setup(c => c.ConvertBlog(It.IsAny<Blog>())).Returns(new BlogMLBlog {Title = "Whatever"});
            converter.Setup(c => c.ConvertCategories(categories)).Returns(blogMLCategories);
            var source = new BlogMLSource(context.Object, converter.Object);

            // act
            var blogMLBlog = source.GetBlog();

            // assert
            Assert.AreEqual("The First Category", blogMLBlog.Categories[0].Title);
        }

        [Test]
        public void GetBlogPosts_WithBlogHavingPosts_ReturnsAllPosts()
        {
            // arrange
            var blog = new Blog
            {Title = "Irrelevant Title", SubTitle = "Test Blog Subtitle", Author = "Charles Dickens", Host = "example.com", ModerationEnabled = true};
            var posts = new PagedCollection<EntryStatsView> {new EntryStatsView { Title = "Test Post Title"}};
            var context = new Mock<ISubtextContext>();
            context.Setup(c => c.Blog).Returns(blog);
            context.Setup(c => c.UrlHelper.BlogUrl()).Returns("/");
            context.Setup(c => c.Repository.GetEntriesForExport(It.IsAny<int>() /*pageIndex*/, 100 /*pageSize*/)).Returns(posts);
            var converter = new Mock<IBlogMLExportMapper>();
            converter.Setup(c => c.ConvertEntry(It.IsAny<EntryStatsView>(), false /*embedAttachments*/)).Returns(new BlogMLPost { Title = "Test Post Title" });
            var source = new BlogMLSource(context.Object, converter.Object);

            // act
            var blogMLPosts = source.GetBlogPosts(false /*embedAttachments*/);

            // assert
            Assert.AreEqual("Test Post Title", blogMLPosts.ToList().First().Title);
        }

        [Test]
        public void GetBlogPosts_WithBlogPostHavingCategories_ReturnsPostsWithCategories()
        {
            // arrange
            var categories = new List<LinkCategory> { new LinkCategory(1, "Category Title"), new LinkCategory(2, "Some Other Category Title") };
            var blog = new Blog { Title = "Irrelevant Title", SubTitle = "Test Blog Subtitle", Author = "Charles Dickens", Host = "example.com", ModerationEnabled = true };
            var entry = new EntryStatsView {Title = "Test Post Title"};
            entry.Categories.Add("Some Other Category Title");
            var posts = new PagedCollection<EntryStatsView> { entry };
            var context = new Mock<ISubtextContext>();
            context.Setup(c => c.Blog).Returns(blog);
            context.Setup(c => c.UrlHelper.BlogUrl()).Returns("/");
            context.Setup(c => c.UrlHelper.EntryUrl(It.IsAny<IEntryIdentity>())).Returns("/irrelevant");
            context.Setup(c => c.Repository.GetCategories(CategoryType.PostCollection, false /*activeOnly*/)).Returns(categories);
            context.Setup(c => c.Repository.GetEntriesForExport(It.IsAny<int>() /*pageIndex*/, 100 /*pageSize*/)).Returns(posts);
            var converter = new BlogMLExportMapper(context.Object);
            var source = new BlogMLSource(context.Object, converter);

            // act
            var blogMLPosts = source.GetBlogPosts(false /*embedAttachments*/);

            // assert
            Assert.AreEqual("2", blogMLPosts.First().Categories[0].Ref);
        }
    }
}