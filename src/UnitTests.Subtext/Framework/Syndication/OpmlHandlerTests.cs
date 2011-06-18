using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
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
        private static HostInfo CreateHostInfo()
        {
            return new HostInfo(new NameValueCollection());
        }

        [Test]
        public void OpmlHandler_WithRequest_SetsContentTypeToXml()
        {
            //arrange
            var repository = new Mock<ObjectRepository>();
            repository.Setup(r => r.GetBlogsByGroup("http://subtextproject.com/", 1)).Returns(new Blog[] { new Blog { } });
            var queryString = new NameValueCollection();
            queryString.Add("GroupID", "1");
            var context = new Mock<ISubtextContext>();
            context.Setup(c => c.Repository).Returns(repository.Object);
            context.Setup(c => c.HttpContext.Request.Url).Returns(new Uri("http://subtextproject.com/"));
            context.Setup(c => c.HttpContext.Request.QueryString).Returns(queryString);
            context.Stub(c => c.HttpContext.Response.ContentType);
            context.Setup(c => c.HttpContext.Response.Output).Returns(new StringWriter());
            context.SetupUrlHelper(new Mock<BlogUrlHelper>());
            var writer = new Mock<OpmlWriter>();
            writer.Setup(w => w.Write(It.IsAny<IEnumerable<Blog>>(), It.IsAny<TextWriter>(), It.IsAny<BlogUrlHelper>()));
            var handler = new OpmlHandler(context.Object, writer.Object, new Lazy<HostInfo>(CreateHostInfo));

            //act
            handler.ProcessRequest();

            //assert
            Assert.AreEqual("text/xml", context.Object.HttpContext.Response.ContentType);
        }

        [Test]
        public void OpmlHandler_WithRequestForAggregateBlog_GetsGroupIdFromQueryString()
        {
            //arrange
            var queryString = new NameValueCollection { { "GroupID", "310" } };

            var context = new Mock<ISubtextContext>();
            context.Stub(c => c.HttpContext.Response.ContentType);
            context.Setup(c => c.HttpContext.Response.Output).Returns(new StringWriter());
            context.Setup(c => c.HttpContext.Request.QueryString).Returns(queryString);
            context.Setup(c => c.HttpContext.Request.Url).Returns(new Uri("http://example.com/"));
            context.SetupUrlHelper(new Mock<BlogUrlHelper>());
            var repository = new Mock<ObjectRepository>();
            int? parsedGroupId = null;
            repository.Setup(r => r.GetBlogsByGroup("example.com", It.IsAny<int?>())).Callback<string, int?>(
                (host, groupId) => parsedGroupId = groupId);
            context.SetupRepository(repository);
            var writer = new Mock<OpmlWriter>();
            writer.Setup(w => w.Write(It.IsAny<IEnumerable<Blog>>(), It.IsAny<TextWriter>(), It.IsAny<BlogUrlHelper>()));

            var appSettings = new NameValueCollection();
            appSettings.Add("AggregateEnabled", "true");
            var hostInfo = new HostInfo(appSettings);
            var handler = new OpmlHandler(context.Object, writer.Object, new Lazy<HostInfo>(() => hostInfo));

            //act
            handler.ProcessRequest();

            //assert
            Assert.AreEqual(310, parsedGroupId.Value);
        }
    }
}