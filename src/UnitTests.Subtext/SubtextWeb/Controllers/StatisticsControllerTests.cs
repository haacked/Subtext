using System;
using System.Collections.Specialized;
using System.Globalization;
using System.Web;
using MbUnit.Framework;
using Moq;
using Subtext.Framework;
using Subtext.Framework.Components;
using Subtext.Framework.Services;
using Subtext.Infrastructure.ActionResults;
using Subtext.Web.Controllers;

namespace UnitTests.Subtext.SubtextWeb.Controllers
{
    [TestFixture]
    public class StatisticsControllerTests
    {
        [Test]
        public void CtorSetsStatisticsService()
        {
            // arrange
            IStatisticsService service = new Mock<IStatisticsService>().Object;
            ISubtextContext subtextContext = new Mock<ISubtextContext>().Object;

            // act
            var controller = new StatisticsController(subtextContext, service);

            // assert
            Assert.AreSame(service, controller.StatisticsService);
        }

        [Test]
        public void TwoRequestsWithinTimeoutGetsNotModifiedResult()
        {
            // arrange
            IStatisticsService service = new Mock<IStatisticsService>().Object;
            var subtextContext = new Mock<ISubtextContext>();
            var headers = new NameValueCollection();
            headers.Add("If-Modified-Since", DateTime.UtcNow.ToString(CultureInfo.InvariantCulture));
            subtextContext.Setup(c => c.HttpContext.Request.Headers).Returns(headers);
            var controller = new StatisticsController(subtextContext.Object, service);

            // act
            var result = controller.RecordAggregatorView(123) as NotModifiedResult;

            // assert
            Assert.IsNotNull(result);
        }

        [Test]
        public void RequestForEntryRecordedAsAnAggregatorVisit()
        {
            // arrange
            EntryView entryView = null;
            var service = new Mock<IStatisticsService>();
            service.Setup(s => s.RecordAggregatorView(It.IsAny<EntryView>())).Callback<EntryView>(
                view => entryView = view);
            var headers = new NameValueCollection();
            var subtextContext = new Mock<ISubtextContext>();
            subtextContext.Setup(c => c.Blog).Returns(new Blog {Id = 99});
            subtextContext.Setup(c => c.HttpContext.Request.Headers).Returns(headers);
            var controller = new StatisticsController(subtextContext.Object, service.Object);

            // act
            controller.RecordAggregatorView(123);

            // assert
            Assert.IsNotNull(entryView);
            Assert.AreEqual(PageViewType.AggView, entryView.PageViewType);
            Assert.AreEqual(99, entryView.BlogId);
            Assert.AreEqual(123, entryView.EntryId);
        }

        [Test]
        public void RequestForInvalidEntryIdNotRecordedAsAnAggregatorVisit()
        {
            // arrange
            var service = new Mock<IStatisticsService>();
            service.Setup(s => s.RecordAggregatorView(It.IsAny<EntryView>())).Throws(
                new InvalidOperationException("RecordAggregatorView should not be called"));
            var headers = new NameValueCollection();
            var subtextContext = new Mock<ISubtextContext>();
            subtextContext.Setup(c => c.Blog).Returns(new Blog {Id = 99});
            subtextContext.Setup(c => c.HttpContext.Request.Headers).Returns(headers);
            var controller = new StatisticsController(subtextContext.Object, service.Object);

            // act, no assert
            controller.RecordAggregatorView(-1);
        }

        [Test]
        public void RequestReturnsFileResultContainingSinglePixelImage()
        {
            // arrange
            var service = new Mock<IStatisticsService>();
            var headers = new NameValueCollection();
            var subtextContext = new Mock<ISubtextContext>();
            subtextContext.Setup(c => c.Blog).Returns(new Blog {Id = 99});
            subtextContext.Setup(c => c.HttpContext.Request.Headers).Returns(headers);
            var controller = new StatisticsController(subtextContext.Object, service.Object);

            // act
            var result = controller.RecordAggregatorView(-1) as CacheableFileContentResult;

            // assert
            Assert.IsNotNull(result);
            Assert.AreEqual(HttpCacheability.Public, result.Cacheability);
            Assert.AreEqual("image/gif", result.ContentType);
        }
    }
}