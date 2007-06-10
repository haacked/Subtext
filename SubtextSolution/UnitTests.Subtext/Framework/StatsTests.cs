using System;
using System.Threading;
using MbUnit.Framework;
using Subtext.Data;
using Subtext.Extensibility;
using Subtext.Extensibility.Interfaces;
using Subtext.Framework;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;

namespace UnitTests.Subtext.Framework
{
	[TestFixture]
	public class StatsTests
	{
		[Test]
		public void StatsQueuingWorksProperly()
		{
			try
			{
				UnitTestHelper.SetupBlog();

				Config.Settings.Tracking.QueueStats = true;
				Config.Settings.Tracking.QueueStatsCount = 2;

				Stats.ClearQueue(false);

				Entry entry = UnitTestHelper.CreateEntryInstanceForSyndication("test", "the title for this post", "test");
				int entryId = Entries.Create(entry);

				EntryView entryView = new EntryView();
				entryView.BlogId = Config.CurrentBlog.Id;
				entryView.EntryId = entryId;
				entryView.PageViewType = PageViewType.WebView;

				Stats.AddQuedStats(entryView);
				Stats.AddQuedStats(entryView);

				//Should be nothing in the db yet..
				IPagedCollection<Entry> entries = Entries.GetPagedEntries(PostType.BlogPost, NullValue.NullInt32, 0, 10);
				EntryStatsView entryStatsView = (EntryStatsView) entries[0];
				Assert.AreEqual(0, entryStatsView.WebCount);

				Stats.AddQuedStats(entryView);
				Thread.Sleep(2000); //Wait a moment for the asynch tracking to kick in.

				entries = Entries.GetPagedEntries(PostType.BlogPost, NullValue.NullInt32, 0, 10);
				entryStatsView = (EntryStatsView) entries[0];
				Assert.AreEqual(2, entryStatsView.WebCount);	
			}
			finally
			{
				//Can't run this test in a transaction. This is the best I can do.
				StoredProcedures.ClearBlogContent(Config.CurrentBlog.Id);
			}
		}

		[Test]
		[RollBack]
		public void CanTrackAggregateView()
		{
			TestTrackingEntryViews(delegate(EntryView[] entryViews)
										{
											foreach (EntryView view in entryViews)
											{
												view.PageViewType = PageViewType.AggView;
												view.ReferralUrl = "http://haacked.com/";
												Stats.TrackEntry(view);
											}
										}, 0, 2);
		}

		[Test]
		[RollBack]
		public void CanTrackWebView()
		{
			TestTrackingEntryViews(delegate(EntryView[] entryViews)
										{
											foreach(EntryView view in entryViews)
												Stats.TrackEntry(view);

										}, 2, 0);
		}

		[Test]
		[RollBack]
		public void CanTrackWebViews()
		{
			TestTrackingEntryViews(delegate(EntryView[] entryViews)
			                         	{
											Stats.TrackEntry(entryViews);
			                         	}, 2, 0);
		}

		private delegate void TrackEntries(params EntryView[] entryViews);

		private static void TestTrackingEntryViews(TrackEntries track, int expectedWebCount, int expectedAggCount)
		{
			UnitTestHelper.SetupBlog();

			Entry entry = UnitTestHelper.CreateEntryInstanceForSyndication("test", "the title for this post", "test");
			int entryId = Entries.Create(entry);

			EntryView entryView = new EntryView();
			entryView.BlogId = Config.CurrentBlog.Id;
			entryView.EntryId = entryId;
			entryView.PageViewType = PageViewType.WebView;

			track(entryView, entryView);
			
			IPagedCollection<Entry> entries = Entries.GetPagedEntries(PostType.BlogPost, NullValue.NullInt32, 0, 10);
			EntryStatsView entryStatsView = (EntryStatsView)entries[0];
			Assert.AreEqual(expectedWebCount, entryStatsView.WebCount);

			if (expectedWebCount > 0)
			{
				Assert.LowerEqualThan(entryStatsView.WebLastUpdated, DateTime.Now.AddMinutes(1));
				Assert.GreaterEqualThan(entryStatsView.WebLastUpdated, DateTime.Now.AddDays(-1));
			}

			Assert.AreEqual(expectedAggCount, entryStatsView.AggCount);
			if (expectedAggCount > 0)
			{
				Assert.LowerEqualThan(entryStatsView.AggLastUpdated, DateTime.Now.AddMinutes(1));
				Assert.GreaterEqualThan(entryStatsView.AggLastUpdated, DateTime.Now.AddDays(-1));
			}
		}
	}
}
