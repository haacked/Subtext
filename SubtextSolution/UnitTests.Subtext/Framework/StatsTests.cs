using MbUnit.Framework;
using Moq;
using Subtext.Framework;
using Subtext.Framework.Components;
using Subtext.Framework.Providers;

namespace UnitTests.Subtext.Framework
{
	[TestFixture]
	public class StatsTests {
        [Test]
        public void TrackReferral_WithValidEntryView_RecordsEntry() {
            //arrange
            EntryView referral = new EntryView();
            referral.EntryId = 123;
            referral.BlogId = 2;
            referral.PageViewType = PageViewType.WebView;
            referral.ReferralUrl = "http://haacked.com/";
            
            var trackingSettings = new global::Subtext.Framework.Configuration.Tracking();
            var repository = new Mock<ObjectProvider>();
            EntryView trackedEntryView = null;
            repository.Setup(r => r.TrackEntry(It.IsAny<EntryView>())).Callback<EntryView>(ev => trackedEntryView = ev);
            StatsRepository stats = new StatsRepository(repository.Object, trackingSettings);

            //act
            stats.TrackEntry(referral);

            //assert
            Assert.AreEqual(referral, trackedEntryView);
        }

        [Test]
        public void GetPagedReferrers_WithoutEntryIdArgument_SetsNullEntryId() {
            //arrange
            var trackingSettings = new global::Subtext.Framework.Configuration.Tracking();
            var repository = new Mock<ObjectProvider>();
            repository.Setup(r => r.GetPagedReferrers(It.IsAny<int>(), It.IsAny<int>(), NullValue.NullInt32));
            StatsRepository stats = new StatsRepository(repository.Object, trackingSettings);

            //act
            stats.GetPagedReferrers(0, 0);

            //assert
            repository.Verify(r => r.GetPagedReferrers(It.IsAny<int>(), It.IsAny<int>(), NullValue.NullInt32));
        }

		[Test]
		public void EntryViewInitializesIdsToNullValue() {
			EntryView view = new EntryView();
			Assert.AreEqual(NullValue.NullInt32, view.EntryId);
			Assert.AreEqual(NullValue.NullInt32, view.BlogId);
		}
	}
}
