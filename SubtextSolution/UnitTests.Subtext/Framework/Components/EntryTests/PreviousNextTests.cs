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
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using MbUnit.Framework;
using Subtext.Extensibility;
using Subtext.Framework;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;
using Subtext.Framework.Data;
using Subtext.Framework.Web.HttpModules;

namespace UnitTests.Subtext.Framework.Components.EntryTestsi
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
			string hostname = UnitTestHelper.GenerateUniqueString();
			Config.CreateBlog("", "username", "password", hostname, string.Empty);
			UnitTestHelper.SetHttpContextWithBlogRequest(hostname, string.Empty);
            BlogRequest.Current.Blog = Config.GetBlog(hostname, string.Empty);

			Entry previousEntry = UnitTestHelper.CreateEntryInstanceForSyndication("test", "test", "body", UnitTestHelper.GenerateUniqueString(), DateTime.Now.AddDays(-1));
			Entry currentEntry = UnitTestHelper.CreateEntryInstanceForSyndication("test", "test", "body", UnitTestHelper.GenerateUniqueString(), DateTime.Now);

			int previousId = UnitTestHelper.Create(previousEntry);
			int currentId = UnitTestHelper.Create(currentEntry);

			ICollection<Entry> entries = DatabaseObjectProvider.Instance().GetPreviousAndNextEntries(currentId, PostType.BlogPost);
			Assert.AreEqual(1, entries.Count, "Since there is no next entry, should return only 1");
			Assert.AreEqual(previousId, entries.First().Id, "The previous entry does not match expectations.");
		}

		/// <summary>
		/// Test the case where we have a next, but no previous entry.
		/// </summary>
		[Test]
		[RollBack]
		public void GetPreviousAndNextEntriesReturnsNextWhenNoPreviousExists()
		{
			string hostname = UnitTestHelper.GenerateUniqueString();
			Config.CreateBlog("", "username", "password", hostname, string.Empty);
			UnitTestHelper.SetHttpContextWithBlogRequest(hostname, string.Empty);
            BlogRequest.Current.Blog = Config.GetBlog(hostname, string.Empty);

			Entry currentEntry = UnitTestHelper.CreateEntryInstanceForSyndication("test", "test", "body", UnitTestHelper.GenerateUniqueString(), DateTime.Now.AddDays(-1));
			Entry nextEntry = UnitTestHelper.CreateEntryInstanceForSyndication("test", "test", "body", UnitTestHelper.GenerateUniqueString(), DateTime.Now);

			int currentId = UnitTestHelper.Create(currentEntry);
			int nextId = UnitTestHelper.Create(nextEntry);

			ICollection<Entry> entries = DatabaseObjectProvider.Instance().GetPreviousAndNextEntries(currentId, PostType.BlogPost);
			Assert.AreEqual(1, entries.Count, "Since there is no previous entry, should return only next");
			Assert.AreEqual(nextId, entries.First().Id, "The next entry does not match expectations.");
		}

		/// <summary>
		/// Test the case where we have both a previous and next.
		/// </summary>
		[Test]
		[RollBack]
		public void GetPreviousAndNextEntriesReturnsBoth()
		{
			string hostname = UnitTestHelper.GenerateUniqueString();
			Config.CreateBlog("", "username", "password", hostname, string.Empty);
			UnitTestHelper.SetHttpContextWithBlogRequest(hostname, string.Empty);
            BlogRequest.Current.Blog = Config.GetBlog(hostname, string.Empty);

			Entry previousEntry = UnitTestHelper.CreateEntryInstanceForSyndication("test", "test", "body", UnitTestHelper.GenerateUniqueString(), DateTime.Now.AddDays(-2));
			Entry currentEntry = UnitTestHelper.CreateEntryInstanceForSyndication("test", "test", "body", UnitTestHelper.GenerateUniqueString(), DateTime.Now.AddDays(-1));
			Entry nextEntry = UnitTestHelper.CreateEntryInstanceForSyndication("test", "test", "body", UnitTestHelper.GenerateUniqueString(), DateTime.Now);

			int previousId = UnitTestHelper.Create(previousEntry);
			Thread.Sleep(100);
			int currentId = UnitTestHelper.Create(currentEntry);
			Thread.Sleep(100);
			int nextId = UnitTestHelper.Create(nextEntry);

			ICollection<Entry> entries = DatabaseObjectProvider.Instance().GetPreviousAndNextEntries(currentId, PostType.BlogPost);
			Assert.AreEqual(2, entries.Count, "Expected both previous and next.");
			
			//The more recent one is next because of desceding sort.
			Assert.AreEqual(nextId, entries.First().Id, "The next entry does not match expectations.");
			Assert.AreEqual(previousId, entries.ElementAt(1).Id, "The previous entry does not match expectations.");
		}

		/// <summary>
		/// Test the case where we have more than three entries.
		/// </summary>
		[Test]
		[RollBack]
		public void GetPreviousAndNextEntriesReturnsCorrectEntries()
		{
			string hostname = UnitTestHelper.GenerateUniqueString();
			Config.CreateBlog("", "username", "password", hostname, string.Empty);
			UnitTestHelper.SetHttpContextWithBlogRequest(hostname, string.Empty);
            BlogRequest.Current.Blog = Config.GetBlog(hostname, string.Empty);

			Entry firstEntry = UnitTestHelper.CreateEntryInstanceForSyndication("test", "test", "body", UnitTestHelper.GenerateUniqueString(), DateTime.Now.AddDays(-3));
			Entry previousEntry = UnitTestHelper.CreateEntryInstanceForSyndication("test", "test", "body", UnitTestHelper.GenerateUniqueString(), DateTime.Now.AddDays(-2));
			Entry currentEntry = UnitTestHelper.CreateEntryInstanceForSyndication("test", "test", "body", UnitTestHelper.GenerateUniqueString(), DateTime.Now.AddDays(-1));
			Entry nextEntry = UnitTestHelper.CreateEntryInstanceForSyndication("test", "test", "body", UnitTestHelper.GenerateUniqueString(), DateTime.Now);
			Entry lastEntry = UnitTestHelper.CreateEntryInstanceForSyndication("test", "test", "body", UnitTestHelper.GenerateUniqueString(), DateTime.Now.AddDays(1));

			Thread.Sleep(100);
			int previousId = UnitTestHelper.Create(previousEntry);
			Thread.Sleep(100);
			int currentId = UnitTestHelper.Create(currentEntry);
			Thread.Sleep(100);
			int nextId = UnitTestHelper.Create(nextEntry);
			Thread.Sleep(100);

			ICollection<Entry> entries = DatabaseObjectProvider.Instance().GetPreviousAndNextEntries(currentId, PostType.BlogPost);
			Assert.AreEqual(2, entries.Count, "Expected both previous and next.");

			//The more recent one is next because of desceding sort.
			Assert.AreEqual(nextId, entries.First().Id, "The next entry does not match expectations.");
			Assert.AreEqual(previousId, entries.ElementAt(1).Id, "The previous entry does not match expectations.");
		}

		/// <summary>
		/// Make sure that previous and next are based on syndication date and not entry id.
		/// </summary>
		[Test]
		[RollBack]
		public void GetPreviousAndNextBasedOnSyndicationDateNotEntryId()
		{
			string hostname = UnitTestHelper.GenerateUniqueString();
			Config.CreateBlog("", "username", "password", hostname, string.Empty);
			UnitTestHelper.SetHttpContextWithBlogRequest(hostname, string.Empty);
            BlogRequest.Current.Blog = Config.GetBlog(hostname, string.Empty);

			Entry previousEntry = UnitTestHelper.CreateEntryInstanceForSyndication("test", "test", "body");
			Entry currentEntry = UnitTestHelper.CreateEntryInstanceForSyndication("test", "test", "body");
			Entry nextEntry = UnitTestHelper.CreateEntryInstanceForSyndication("test", "test", "body");

			previousEntry.IsActive = false;
            currentEntry.IsActive = false;
            nextEntry.IsActive = false;

			//Create out of order.
			int currentId = UnitTestHelper.Create(currentEntry);
			int nextId = UnitTestHelper.Create(nextEntry);
			int previousId = UnitTestHelper.Create(previousEntry);
			
			//Now syndicate.
            previousEntry.IsActive = true;
			Entries.Update(previousEntry);
			Thread.Sleep(100);
            currentEntry.IsActive = true;
			Entries.Update(currentEntry);
			Thread.Sleep(100);
            nextEntry.IsActive = true;
			Entries.Update(nextEntry);
			
			Assert.IsTrue(previousId > currentId, "Ids are out of order.");

			ICollection<Entry> entries = DatabaseObjectProvider.Instance().GetPreviousAndNextEntries(currentId, PostType.BlogPost);
			Assert.AreEqual(2, entries.Count, "Expected both previous and next.");
			//The first should be next because of descending sort.
			Assert.AreEqual(nextId, entries.First().Id, "The next entry does not match expectations.");
			Assert.AreEqual(previousId, entries.ElementAt(1).Id, "The previous entry does not match expectations.");
		}

	}
}
