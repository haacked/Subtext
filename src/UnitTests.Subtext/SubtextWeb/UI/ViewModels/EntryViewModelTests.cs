using MbUnit.Framework;
using Moq;
using Subtext.Extensibility;
using Subtext.Framework;
using Subtext.Framework.Components;
using Subtext.Framework.Routing;
using Subtext.Web.UI.ViewModels;

namespace UnitTests.Subtext.SubtextWeb.UI.ViewModels
{
    [TestFixture]
    public class EntryViewModelTests
    {
        [Test]
        public void Ctor_CopiesAllPropertiesOfEntry()
        {
            // arrange
            var entry = new Entry(PostType.BlogPost);
            entry.Id = 123;
            entry.FeedBackCount = 99;
            entry.Title = "The title";

            // act
            var model = new EntryViewModel(entry, null);

            // assert
            Assert.AreEqual(PostType.BlogPost, model.PostType);
            Assert.AreEqual(123, model.Id);
            Assert.AreEqual(99, model.FeedBackCount);
            Assert.AreEqual("The title", model.Title);
        }

        [Test]
        public void FullyQualifiedUrl_ReturnsCorrectUrl()
        {
            // arrange
            var urlHelper = new Mock<UrlHelper>();
            var entry = new Entry(PostType.BlogPost)
            {
                Id = 123,
                EntryName = "post-slug"
            };
            var blog = new Blog {Host = "localhost"};
            var subtextContext = new Mock<ISubtextContext>();
            subtextContext.Setup(c => c.UrlHelper.EntryUrl(It.IsAny<Entry>())).Returns("/2009/01/23/post-slug.aspx");
            subtextContext.Setup(c => c.Blog).Returns(blog);

            // act
            var model = new EntryViewModel(entry, subtextContext.Object);

            // assert
            Assert.AreEqual(model.FullyQualifiedUrl, "http://localhost/2009/01/23/post-slug.aspx");
        }
    }
}