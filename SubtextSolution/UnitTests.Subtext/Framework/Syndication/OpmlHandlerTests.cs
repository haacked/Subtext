using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Web;
using MbUnit.Framework;
using Moq;
using Moq.Stub;
using Subtext.Framework;
using Subtext.Framework.Providers;
using Subtext.Framework.Routing;
using Subtext.Framework.Syndication;

namespace UnitTests.Subtext.Framework.Syndication
{
    [TestFixture]
    public class OpmlHandlerTests
    {
        [Test]
        public void OpmlHandler_WithRequest_SetsContentTypeToXml() { 
            //arrange
            var context = new Mock<ISubtextContext>();
            var httpContext = new Mock<HttpContextBase>();
            httpContext.Stub(h => h.Response.ContentType);
            httpContext.Setup(h => h.Response.Output).Returns(new StringWriter());
            context.SetupRequestContext(httpContext);
            context.SetupUrlHelper(new Mock<UrlHelper>());
            var writer = new Mock<OpmlWriter>();
            writer.Setup(w => w.Write(It.IsAny<IEnumerable<Blog>>(), It.IsAny<TextWriter>(), It.IsAny<UrlHelper>()));
            OpmlHandler handler = new OpmlHandler(writer.Object);
            handler.SubtextContext = context.Object;

            //act
            handler.ProcessRequest(new HostInfo());

            //assert
            Assert.AreEqual("text/xml", httpContext.Object.Response.ContentType);
        }

        [Test]
        public void OpmlHandler_WithRequestForAggregateBlog_GetsGroupIdFromQueryString()
        {
            //arrange
            var queryString = new NameValueCollection();
            queryString.Add("GroupID", "310");
            
            var context = new Mock<ISubtextContext>();
            var httpContext = new Mock<HttpContextBase>();
            httpContext.Stub(h => h.Response.ContentType);
            httpContext.Setup(h => h.Response.Output).Returns(new StringWriter());
            httpContext.Setup(h => h.Request.QueryString).Returns(queryString);
            context.SetupRequestContext(httpContext);
            context.SetupUrlHelper(new Mock<UrlHelper>());
            var repository = new Mock<ObjectProvider>();
            int? parsedGroupId = null;
            repository.Setup(r => r.GetBlogsByGroup(It.IsAny<string>(), It.IsAny<int?>())).Callback<string, int?>((host, groupId) => parsedGroupId = groupId);
            context.SetupRepository(repository);

            var writer = new Mock<OpmlWriter>();
            writer.Setup(w => w.Write(It.IsAny<IEnumerable<Blog>>(), It.IsAny<TextWriter>(), It.IsAny<UrlHelper>()));
            OpmlHandler handler = new OpmlHandler(writer.Object);
            handler.SubtextContext = context.Object;
            var hostInfo = new HostInfo();
            hostInfo.BlogAggregationEnabled = true;
            hostInfo.AggregateBlog = new Blog();

            //act
            handler.ProcessRequest(hostInfo);

            //assert
            Assert.AreEqual(310, parsedGroupId.Value);
        }

    }
}
