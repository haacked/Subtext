using System;
using MbUnit.Framework;
using Moq;
using Subtext.Framework;
using Subtext.Framework.Components;
using Subtext.Framework.Services;

namespace UnitTests.Subtext.Framework.Services
{
    [TestFixture]
    public class StatisticsServiceTests
    {
        [Test]
        public void CtorSetsSubtextContextAndSettings()
        {
            //arrange
            var settings = new global::Subtext.Framework.Configuration.Tracking {EnableWebStats = true};
            ISubtextContext subtextContext = new Mock<ISubtextContext>().Object;

            //act
            var statisticsService = new StatisticsService(subtextContext, settings);

            //assert
            Assert.AreEqual(subtextContext, statisticsService.SubtextContext);
            Assert.AreEqual(settings, statisticsService.Settings);
        }

        [Test]
        public void RecordAggregatorViewRecordsEntry()
        {
            //arrange
            var subtextContext = new Mock<ISubtextContext>();
            EntryView entryView = null;
            subtextContext.Setup(c => c.Repository.TrackEntry(It.IsAny<EntryView>())).Callback<EntryView>(
                e => entryView = e);
            subtextContext.Setup(c => c.HttpContext.Request.HttpMethod).Returns("GET");
            var settings = new global::Subtext.Framework.Configuration.Tracking {EnableAggBugs = true};
            var statisticsService = new StatisticsService(subtextContext.Object, settings);

            //act
            statisticsService.RecordAggregatorView(new EntryView {EntryId = 66});

            //assert
            Assert.AreEqual(66, entryView.EntryId);
        }

        [Test]
        public void RecordAggViewWithStatsDisabledDoesNotTrackEntry()
        {
            //arrange
            var subtextContext = new Mock<ISubtextContext>();
            bool wasCalled = false;
            subtextContext.Setup(c => c.Repository.TrackEntry(It.IsAny<EntryView>())).Callback(() => wasCalled = true);
            subtextContext.Setup(c => c.HttpContext.Request.HttpMethod).Returns("GET");
            var settings = new global::Subtext.Framework.Configuration.Tracking {EnableAggBugs = false};
            var statisticsService = new StatisticsService(subtextContext.Object, settings);

            //act
            statisticsService.RecordAggregatorView(new EntryView {EntryId = 66});

            //assert
            Assert.IsFalse(wasCalled);
        }

        [Test]
        public void RecordAggViewDoesNotRecordHttpPost()
        {
            //arrange
            var subtextContext = new Mock<ISubtextContext>();
            bool wasCalled = false;
            subtextContext.Setup(c => c.Repository.TrackEntry(It.IsAny<EntryView>())).Callback(() => wasCalled = true);
            subtextContext.Setup(c => c.HttpContext.Request.HttpMethod).Returns("POST");
            var settings = new global::Subtext.Framework.Configuration.Tracking {EnableAggBugs = true};
            var statisticsService = new StatisticsService(subtextContext.Object, settings);

            //act
            statisticsService.RecordAggregatorView(new EntryView {EntryId = 66});

            //assert
            Assert.IsFalse(wasCalled);
        }

        [Test]
        public void RecordWebViewRecordsEntry()
        {
            //arrange
            var subtextContext = new Mock<ISubtextContext>();
            EntryView entryView = null;
            subtextContext.Setup(c => c.Repository.TrackEntry(It.IsAny<EntryView>())).Callback<EntryView>(
                e => entryView = e);
            subtextContext.Setup(c => c.HttpContext.Request.HttpMethod).Returns("GET");
            var settings = new global::Subtext.Framework.Configuration.Tracking {EnableWebStats = true};
            var statisticsService = new StatisticsService(subtextContext.Object, settings);

            //act
            statisticsService.RecordWebView(new EntryView {EntryId = 66});

            //assert
            Assert.AreEqual(66, entryView.EntryId);
        }

        [Test]
        public void RecordWebViewWithStatsDisabledDoesNotTrackEntry()
        {
            //arrange
            var subtextContext = new Mock<ISubtextContext>();
            bool wasCalled = false;
            subtextContext.Setup(c => c.Repository.TrackEntry(It.IsAny<EntryView>())).Callback(() => wasCalled = true);
            subtextContext.Setup(c => c.HttpContext.Request.HttpMethod).Returns("GET");
            var settings = new global::Subtext.Framework.Configuration.Tracking {EnableWebStats = false};
            var statisticsService = new StatisticsService(subtextContext.Object, settings);

            //act
            statisticsService.RecordWebView(new EntryView {EntryId = 66});

            //assert
            Assert.IsFalse(wasCalled);
        }

        [Test]
        public void RecordWebViewDoesNotRecordHttpPost()
        {
            //arrange
            var subtextContext = new Mock<ISubtextContext>();
            bool wasCalled = false;
            subtextContext.Setup(c => c.Repository.TrackEntry(It.IsAny<EntryView>())).Callback(() => wasCalled = true);
            subtextContext.Setup(c => c.HttpContext.Request.HttpMethod).Returns("POST");
            var settings = new global::Subtext.Framework.Configuration.Tracking {EnableWebStats = true};
            var statisticsService = new StatisticsService(subtextContext.Object, settings);

            //act
            statisticsService.RecordWebView(new EntryView {EntryId = 66});

            //assert
            Assert.IsFalse(wasCalled);
        }

        [Test]
        public void RecordWebViewRecordsReferrer()
        {
            //arrange
            var subtextContext = new Mock<ISubtextContext>();
            EntryView recordedView = null;
            subtextContext.Setup(c => c.Repository.TrackEntry(It.IsAny<EntryView>())).Callback<EntryView>(
                view => recordedView = view);
            subtextContext.Setup(c => c.UrlHelper.BlogUrl()).Returns("/");
            subtextContext.Setup(c => c.Blog).Returns(new Blog {Host = "haacked.com"});
            subtextContext.Setup(c => c.HttpContext.Request.HttpMethod).Returns("GET");
            subtextContext.Setup(c => c.HttpContext.Request.UrlReferrer).Returns(new Uri("http://subtextproject.com/"));
            var settings = new global::Subtext.Framework.Configuration.Tracking {EnableWebStats = true};
            var statisticsService = new StatisticsService(subtextContext.Object, settings);

            //act
            statisticsService.RecordWebView(new EntryView {EntryId = 66});

            //assert
            Assert.AreEqual("http://subtextproject.com/", recordedView.ReferralUrl);
        }

        [Test]
        public void RecordWebViewFromSameReferrerDoesNotRecordsReferrer()
        {
            //arrange
            var subtextContext = new Mock<ISubtextContext>();
            EntryView recordedView = null;
            subtextContext.Setup(c => c.Repository.TrackEntry(It.IsAny<EntryView>())).Callback<EntryView>(
                view => recordedView = view);
            subtextContext.Setup(c => c.UrlHelper.BlogUrl()).Returns("/");
            subtextContext.Setup(c => c.Blog).Returns(new Blog {Host = "www.haacked.com"});
            subtextContext.Setup(c => c.HttpContext.Request.HttpMethod).Returns("GET");
            subtextContext.Setup(c => c.HttpContext.Request.UrlReferrer).Returns(new Uri("http://haacked.com/"));
            var settings = new global::Subtext.Framework.Configuration.Tracking {EnableWebStats = true};
            var statisticsService = new StatisticsService(subtextContext.Object, settings);

            //act
            statisticsService.RecordWebView(new EntryView {EntryId = 66});

            //assert
            Assert.IsNull(recordedView.ReferralUrl);
        }

        [Test]
        public void RecordWebViewFromSameReferrerDomainDoesNotRecordsReferrer()
        {
            //arrange
            var subtextContext = new Mock<ISubtextContext>();
            EntryView recordedView = null;
            subtextContext.Setup(c => c.Repository.TrackEntry(It.IsAny<EntryView>())).Callback<EntryView>(
                view => recordedView = view);
            subtextContext.Setup(c => c.UrlHelper.BlogUrl()).Returns("/");
            subtextContext.Setup(c => c.Blog).Returns(new Blog {Host = "haacked.com"});
            subtextContext.Setup(c => c.HttpContext.Request.HttpMethod).Returns("GET");
            subtextContext.Setup(c => c.HttpContext.Request.UrlReferrer).Returns(new Uri("http://www.haacked.com/"));
            var settings = new global::Subtext.Framework.Configuration.Tracking {EnableWebStats = true};
            var statisticsService = new StatisticsService(subtextContext.Object, settings);

            //act
            statisticsService.RecordWebView(new EntryView {EntryId = 66});

            //assert
            Assert.IsNull(recordedView.ReferralUrl);
        }

        [Test]
        public void RecordWebViewWithBadReferrerIgnoresReferer()
        {
            //arrange
            var subtextContext = new Mock<ISubtextContext>();
            EntryView recordedView = null;
            subtextContext.Setup(c => c.Repository.TrackEntry(It.IsAny<EntryView>())).Callback<EntryView>(
                view => recordedView = view);
            subtextContext.Setup(c => c.HttpContext.Request.HttpMethod).Returns("GET");
            subtextContext.Setup(c => c.HttpContext.Request.UrlReferrer).Throws(new UriFormatException());
            var settings = new global::Subtext.Framework.Configuration.Tracking {EnableWebStats = true};
            var statisticsService = new StatisticsService(subtextContext.Object, settings);

            //act
            statisticsService.RecordWebView(new EntryView {EntryId = 66});

            //assert
            Assert.IsNull(recordedView.ReferralUrl);
        }
    }
}