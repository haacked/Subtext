using MbUnit.Framework;
using Moq;
using Subtext.Framework;
using Subtext.Framework.Components;
using Subtext.Framework.Data;
using Subtext.Framework.Routing;

namespace UnitTests.Subtext.Framework.Data
{
    [TestFixture]
    public class TransformerTests
    {
        [Test]
        public void MergeLinkCategoriesIntoSingleLinkCategory_WithMultipleCategories_ReturnsSingleCategoryWithLinkCollection()
        {
            // arrange
            var blog = new Blog {Host = "example.com"};

            var urlHelper = new Mock<BlogUrlHelper>();
            urlHelper.Setup(u => u.CategoryUrl(It.IsAny<Category>())).Returns("/");

            var links = new[]
            {
                new LinkCategory(1, "category 1"),
                new LinkCategory(2, "category 2"),
            };

            // act
            var mergedLinkCategory = Transformer.MergeLinkCategoriesIntoSingleLinkCategory("Title", CategoryType.StoryCollection, links, urlHelper.Object, blog);

            // assert
            Assert.AreEqual(2, mergedLinkCategory.Links.Count);
        }

        [Test]
        public void MergeLinkCategoriesIntoSingleLinkCategory_WithNoCategories_ReturnsNull()
        {
            // arrange
            var blog = new Blog { Host = "example.com" };

            var urlHelper = new Mock<BlogUrlHelper>();
            urlHelper.Setup(u => u.CategoryUrl(It.IsAny<Category>())).Returns("/");

            var links = new LinkCategory[0];

            // act
            var mergedLinkCategory = Transformer.MergeLinkCategoriesIntoSingleLinkCategory("Title", CategoryType.StoryCollection, links, urlHelper.Object, blog);

            // assert
            Assert.IsNull(mergedLinkCategory);
        }
    }
}
