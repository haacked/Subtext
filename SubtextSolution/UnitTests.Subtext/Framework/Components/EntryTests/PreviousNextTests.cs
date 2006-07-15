#region Disclaimer/Info
///////////////////////////////////////////////////////////////////////////////////////////////////
// Subtext WebLog
// 
// Subtext is an open source weblog system that is a fork of the .TEXT
// weblog system.
//
// For updated news and information please visit http://subtextproject.com/
// Subtext is hosted at SourceForge at http://sourceforge.net/projects/subtext
// The development mailing list is at subtext-devs@lists.sourceforge.net 
//
// This project is licensed under the BSD license.  See the License.txt file for more information.
///////////////////////////////////////////////////////////////////////////////////////////////////
#endregion

using System;
using System.Collections.Generic;
using System.Threading;
using MbUnit.Framework;
using Subtext.Extensibility;
using Subtext.Framework;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;
using Subtext.Framework.Data;

namespace UnitTests.Subtext.Framework.Components.EntryTests
{
	/// <summary>
	/// Tests the methods to obtain the previous and next entry to an entry.
	/// </summary>
	[TestFixture]
	public class PreviousNextTests
	{
		/// <summary>
		/// Test the case where we have a previous, but no next entry.
		/// </summary>
		[Test]
		[RollBack]
		public void GetPreviousAndNextEntriesReturnsPreviousWhenNoNextExists()
		{
			string hostname = UnitTestHelper.GenerateRandomString();
			Assert.IsTrue(Config.CreateBlog("", "username", "password", hostname, string.Empty));
			UnitTestHelper.SetHttpContextWithBlogRequest(hostname, string.Empty);

			Entry previousEntry = UnitTestHelper.CreateEntryInstanceForSyndication("test", "test", "body", UnitTestHelper.GenerateRandomString(), DateTime.Now.AddDays(-1));
			Entry currentEntry = UnitTestHelper.CreateEntryInstanceForSyndication("test", "test", "body", UnitTestHelper.GenerateRandomString(), DateTime.Now);

			int previousId = Entries.Create(previousEntry);
			int currentId = Entries.Create(currentEntry);

			IList<Entry> entries = DatabaseObjectProvider.Instance().GetPreviousAndNextEntries(currentId, PostType.BlogPost);
			Assert.AreEqual(1, entries.Count, "Since there is no next entry, should return only 1");
			Assert.AreEqual(previousId, entries[0].Id, "The previous entry does not match expectations.");
		}

		/// <summary>
		/// Test the case where we have a next, but no previous entry.
		/// </summary>
		[Test]
		[RollBack]
		public void GetPreviousAndNextEntriesReturnsNextWhenNoPreviousExists()
		{
			string hostname = UnitTestHelper.GenerateRandomString();
			Assert.IsTrue(Config.CreateBlog("", "username", "password", hostname, string.Empty));
			UnitTestHelper.SetHttpContextWithBlogRequest(hostname, string.Empty);

			Entry currentEntry = UnitTestHelper.CreateEntryInstanceForSyndication("test", "test", "body", UnitTestHelper.GenerateRandomString(), DateTime.Now.AddDays(-1));
			Entry nextEntry = UnitTestHelper.CreateEntryInstanceForSyndication("test", "test", "body", UnitTestHelper.GenerateRandomString(), DateTime.Now);

			int currentId = Entries.Create(currentEntry);
			int nextId = Entries.Create(nextEntry);

			IList<Entry> entries = DatabaseObjectProvider.Instance().GetPreviousAndNextEntries(currentId, PostType.BlogPost);
			Assert.AreEqual(1, entries.Count, "Since there is no previous entry, should return only next");
			Assert.AreEqual(nextId, entries[0].Id, "The next entry does not match expectations.");
		}

		/// <summary>
		/// Test the case where we have both a previous and next.
		/// </summary>
		[Test]
		[RollBack]
		public void GetPreviousAndNextEntriesReturnsBoth()
		{
			string hostname = UnitTestHelper.GenerateRandomString();
			Assert.IsTrue(Config.CreateBlog("", "username", "password", hostname, string.Empty));
			UnitTestHelper.SetHttpContextWithBlogRequest(hostname, string.Empty);

			Entry previousEntry = UnitTestHelper.CreateEntryInstanceForSyndication("test", "test", "body", UnitTestHelper.GenerateRandomString(), DateTime.Now.AddDays(-2));
			Entry currentEntry = UnitTestHelper.CreateEntryInstanceForSyndication("test", "test", "body", UnitTestHelper.GenerateRandomString(), DateTime.Now.AddDays(-1));
			Entry nextEntry = UnitTestHelper.CreateEntryInstanceForSyndication("test", "test", "body", UnitTestHelper.GenerateRandomString(), DateTime.Now);

			int previousId = Entries.Create(previousEntry);
			Thread.Sleep(100);
			int currentId = Entries.Create(currentEntry);
			Thread.Sleep(100);
			int nextId = Entries.Create(nextEntry);

			IList<Entry> entries = DatabaseObjectProvider.Instance().GetPreviousAndNextEntries(currentId, PostType.BlogPost);
			Assert.AreEqual(2, entries.Count, "Expected both previous and next.");
			
			//The more recent one is next because of desceding sort.
			Assert.AreEqual(nextId, entries[0].Id, "The next entry does not match expectations.");
			Assert.AreEqual(previousId, entries[1].Id, "The previous entry does not match expectations.");
		}

		/// <summary>
		/// Test the case where we have more than three entries.
		/// </summary>
		[Test]
		[RollBack]
		public void GetPreviousAndNextEntriesReturnsCorrectEntries()
		{
			string hostname = UnitTestHelper.GenerateRandomString();
			Assert.IsTrue(Config.CreateBlog("", "username", "password", hostname, string.Empty));
			UnitTestHelper.SetHttpContextWithBlogRequest(hostname, string.Empty);

			Entry firstEntry = UnitTestHelper.CreateEntryInstanceForSyndication("test", "test", "body", UnitTestHelper.GenerateRandomString(), DateTime.Now.AddDays(-3));
			Entry previousEntry = UnitTestHelper.CreateEntryInstanceForSyndication("test", "test", "body", UnitTestHelper.GenerateRandomString(), DateTime.Now.AddDays(-2));
			Entry currentEntry = UnitTestHelper.CreateEntryInstanceForSyndication("test", "test", "body", UnitTestHelper.GenerateRandomString(), DateTime.Now.AddDays(-1));
			Entry nextEntry = UnitTestHelper.CreateEntryInstanceForSyndication("test", "test", "body", UnitTestHelper.GenerateRandomString(), DateTime.Now);
			Entry lastEntry = UnitTestHelper.CreateEntryInstanceForSyndication("test", "test", "body", UnitTestHelper.GenerateRandomString(), DateTime.Now.AddDays(1));

			Console.WriteLine("{0} Syndicate: {1:hh:mm:ss:fff}", Entries.Create(firstEntry), firstEntry.DateSyndicated);
			Thread.Sleep(100);
			int previousId = Entries.Create(previousEntry);
			Console.WriteLine("{0} Syndicate: {1:hh:mm:ss:fff}", previousId, previousEntry.DateSyndicated);
			Thread.Sleep(100);
			int currentId = Entries.Create(currentEntry);
			Console.WriteLine("{0} Syndicate: {1:hh:mm:ss:fff} ** Current", currentId, currentEntry.DateSyndicated);
			Thread.Sleep(100);
			int nextId = Entries.Create(nextEntry);
			Thread.Sleep(100);
			Console.WriteLine("{0} Syndicate: {1:hh:mm:ss:fff}", Entries.Create(lastEntry), lastEntry.DateSyndicated);

			IList<Entry> entries = DatabaseObjectProvider.Instance().GetPreviousAndNextEntries(currentId, PostType.BlogPost);
			Assert.AreEqual(2, entries.Count, "Expected both previous and next.");

			Console.WriteLine("Prev: {0} Syndicate: {1:hh:mm:ss:fff}", entries[0].Id, entries[0].DateSyndicated);
			Console.WriteLine("Next: {0} Syndicate: {1:hh:mm:ss:fff}", entries[1].Id, entries[1].DateSyndicated);

			//The more recent one is next because of desceding sort.
			Assert.AreEqual(nextId, entries[0].Id, "The next entry does not match expectations.");
			Assert.AreEqual(previousId, entries[1].Id, "The previous entry does not match expectations.");
		}

		/// <summary>
		/// Make sure that previous and next are based on syndication date and not entry id.
		/// </summary>
		[Test]
		[RollBack]
		public void GetPreviousAndNextBasedOnSyndicationDateNotEntryId()
		{
			string hostname = UnitTestHelper.GenerateRandomString();
			Assert.IsTrue(Config.CreateBlog("", "username", "password", hostname, string.Empty));
			UnitTestHelper.SetHttpContextWithBlogRequest(hostname, string.Empty);

			Entry previousEntry = UnitTestHelper.CreateEntryInstanceForSyndication("test", "test", "body");
			Entry currentEntry = UnitTestHelper.CreateEntryInstanceForSyndication("test", "test", "body");
			Entry nextEntry = UnitTestHelper.CreateEntryInstanceForSyndication("test", "test", "body");

			previousEntry.IncludeInMainSyndication = false;
			currentEntry.IncludeInMainSyndication = false;
			nextEntry.IncludeInMainSyndication = false;

			//Create out of order.
			int currentId = Entries.Create(currentEntry);
			int nextId = Entries.Create(nextEntry);
			int previousId = Entries.Create(previousEntry);
			
			//Now syndicate.
			previousEntry.IncludeInMainSyndication = true;
			Entries.Update(previousEntry);
			Thread.Sleep(100);
			currentEntry.IncludeInMainSyndication = true;
			Entries.Update(currentEntry);
			Thread.Sleep(100);
			nextEntry.IncludeInMainSyndication = true;
			Entries.Update(nextEntry);
			
			Assert.IsTrue(previousId > currentId, "Ids are out of order.");

			IList<Entry> entries = DatabaseObjectProvider.Instance().GetPreviousAndNextEntries(currentId, PostType.BlogPost);
			Assert.AreEqual(2, entries.Count, "Expected both previous and next.");
			//The first should be next because of descending sort.
			Assert.AreEqual(nextId, entries[0].Id, "The next entry does not match expectations.");
			Assert.AreEqual(previousId, entries[1].Id, "The previous entry does not match expectations.");
		}

	}
}
