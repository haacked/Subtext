using System.Web;
using System.Web.Routing;
using MbUnit.Framework;
using Moq;
using Subtext.Framework;
using Subtext.Framework.Providers;
using Subtext.Framework.Routing;
using Subtext.Framework.Web.Handlers;

namespace UnitTests.Subtext.Framework.Routing
{
    [TestFixture]
    public class SubtextPageTests
    {
        [Test]
        public void SettingSubtextContextPopulatesOtherProperties()
        {
            //arrange
            var subtextContext = new Mock<ISubtextContext>();
            subtextContext.Setup(c => c.Repository).Returns(new Mock<ObjectRepository>().Object);
            subtextContext.Setup(c => c.UrlHelper).Returns(
                new BlogUrlHelper(new RequestContext(new Mock<HttpContextBase>().Object, new RouteData()), null));
            subtextContext.Setup(c => c.Blog).Returns(new Blog());
            var subtextPage = new SubtextPage();

            //act
            subtextPage.SubtextContext = subtextContext.Object;

            //assert
            Assert.IsNotNull(subtextPage.Repository);
            Assert.AreSame(subtextPage.SubtextContext.Repository, subtextPage.Repository);
            Assert.IsNotNull(subtextPage.Url);
            Assert.AreSame(subtextPage.SubtextContext.UrlHelper, subtextPage.Url);
            Assert.AreSame(subtextPage.SubtextContext.UrlHelper, subtextPage.AdminUrl.Url);
            Assert.IsNotNull(subtextPage.Blog);
            Assert.AreSame(subtextPage.SubtextContext.Blog, subtextPage.Blog);
        }
    }
}