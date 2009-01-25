using System.Collections.ObjectModel;
using System.IO;
using System.Web;
using System.Xml;
using MbUnit.Framework;
using Moq;
using Subtext.Framework;
using Subtext.Framework.Components;
using Subtext.Framework.ImportExport;
using Subtext.ImportExport;

namespace UnitTests.Subtext.BlogML
{
    [TestFixture]
    public class BlogMlHandlerTests
    {
        [Test]
        public void GetBlogMlProvider_ReturnsSubtextBLogMLProviderWithDefaultPageSize() {
            //arrange
            var subtextContext = new Mock<ISubtextContext>();
            var handler = new SubtextBlogMlHttpHandler();
            handler.SubtextContext = subtextContext.Object;
            
            //act
            var provider = handler.GetBlogMlProvider() as SubtextBlogMLProvider;
            
            //assert
            Assert.IsNotNull(provider);
            Assert.AreEqual(100, provider.PageSize);
        }

        [Test]
        public void ProcessRequest_RendersBlogMl() {
            //arrange
            Blog blog = new Blog { Id = 1975,
                Title = "The Title Of This Blog",
                Author = "MasterChief",
                Host = "example.com"
            };

            var subtextContext = new Mock<ISubtextContext>();
            subtextContext.Setup(c => c.Blog).Returns(blog);
            subtextContext.Setup(c => c.Repository.GetBlogById(1975)).Returns(blog);
            subtextContext.Setup(c => c.Repository.GetCategories(CategoryType.PostCollection, false)).Returns(new Collection<LinkCategory>());
            subtextContext.Setup(c => c.UrlHelper.BlogUrl()).Returns("/");
            var handler = new SubtextBlogMlHttpHandler();
            handler.SubtextContext = subtextContext.Object;
            var httpHandler = handler as IHttpHandler;
            var writer = new StringWriter();

            //act
            HttpContext.Current = new HttpContext(new HttpRequest("t", "http://example.com", ""), new HttpResponse(writer));
            httpHandler.ProcessRequest(HttpContext.Current);
            
            //assert
            string result = writer.ToString();
            //For some reason we get 2 weird chars at the beginning during the unit test.
            //Need to figure out if this is happening in a real export.

            if (result.IndexOf("<") > 0) {
                result = result.Substring(result.IndexOf("<"));
            }

            XmlDocument xml = new XmlDocument();
            xml.LoadXml(result);

            XmlNamespaceManager nsmgr = new XmlNamespaceManager(xml.NameTable);
            nsmgr.AddNamespace("bml", "http://www.blogml.com/2006/09/BlogML");

            Assert.AreEqual("The Title Of This Blog", xml.SelectSingleNode("/bml:blog/bml:title", nsmgr).InnerText);
            Assert.AreEqual("MasterChief", xml.SelectSingleNode("/bml:blog/bml:authors/bml:author/bml:title", nsmgr).InnerText);

        }
    }
}
