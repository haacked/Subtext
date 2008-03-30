using System;
using System.Collections.Generic;
using MbUnit.Framework;
using Subtext.Framework;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;
using Subtext.Framework.Data;

namespace UnitTests.Subtext.Framework.Components.EntryTests
{
	[TestFixture]
	public class EntryTagTests
	{
		[Test]
		[RollBack]
		public void TagDoesNotRetrieveDraftEntry()
		{
			string hostname = UnitTestHelper.GenerateRandomString();
			Assert.IsTrue(Config.CreateBlog("", "username", "password", hostname, string.Empty));
			UnitTestHelper.SetHttpContextWithBlogRequest(hostname, string.Empty);
			Entry entry = UnitTestHelper.CreateEntryInstanceForSyndication("me", "title-zero", "body-zero");
			entry.IsActive = false;
			Entries.Create(entry);
			List<string> tags = new List<string>(new string[] { "Tag1", "Tag2" });
			new DatabaseObjectProvider().SetEntryTagList(entry.Id, tags);
			IList<Entry> entries = Entries.GetEntriesByTag(1, "Tag1");
			Assert.AreEqual(0, entries.Count, "Should not retrieve draft entry.");
		}

		[Test]
		[RollBack]
		public void CanTagEntry()
		{
			string hostname = UnitTestHelper.GenerateRandomString();
			Assert.IsTrue(Config.CreateBlog("", "username", "password", hostname, string.Empty));
			UnitTestHelper.SetHttpContextWithBlogRequest(hostname, string.Empty);
			Entry entry = UnitTestHelper.CreateEntryInstanceForSyndication("me", "title-zero", "body-zero");
			Entries.Create(entry);

			List<string> tags = new List<string>(new string[] {"Tag1", "Tag2"});
			new DatabaseObjectProvider().SetEntryTagList(entry.Id, tags);

			IList<Entry> entries = Entries.GetEntriesByTag(1, "Tag1");
			Assert.AreEqual(1, entries.Count);
			Assert.AreEqual(entry.Id, entries[0].Id);
		}

		[Test]
		[RollBack]
		public void CanParseMultilineTag()
		{
			string hostname = UnitTestHelper.GenerateRandomString();
			Assert.IsTrue(Config.CreateBlog("", "username", "password", hostname, string.Empty));
			UnitTestHelper.SetHttpContextWithBlogRequest(hostname, string.Empty);
			Entry entry = UnitTestHelper.CreateEntryInstanceForSyndication("me", "title-zero", "body-zero");
			Entries.Create(entry);
			entry.Body = "<a title=\"title-bar!\" " + Environment.NewLine + " href=\"http://blah/yourtag\" " + Environment.NewLine + "rel=\"tag\">nothing</a>";
			Entries.Update(entry);

			IList<Entry> entries = Entries.GetEntriesByTag(1, "yourtag");
			Assert.AreEqual(1, entries.Count);
			Assert.AreEqual(entry.Id, entries[0].Id);
		}

		[RowTest]
		[Row("http://blah.com/blah/", "blah")]
		[Row("http://blah.com/foo-bar", "foo-bar")]
        [Row("http://blah.com/query?someparm=somevalue", "query")]
		[Row("http://blah.com/query/?someparm=somevalue", "query")]
        [Row("http://blah.com/decode+test", "decode test")]
        [Row("http://blah.com/decode%20test2", "decode test2")]
        [Row("http://blah.com/another+decode%20test", "another decode test")]
		[RollBack]
		public void CanParseEntryTags(string url, string expectedTag)
		{
			string hostname = UnitTestHelper.GenerateRandomString();
			Assert.IsTrue(Config.CreateBlog("", "username", "password", hostname, string.Empty));
			UnitTestHelper.SetHttpContextWithBlogRequest(hostname, string.Empty);
			Entry entry = UnitTestHelper.CreateEntryInstanceForSyndication("me", "title-zero", "body-zero");
			Entries.Create(entry);
			entry.Body = "<a href=\"" + url + "\" rel=\"tag\">nothing</a>";
			Entries.Update(entry);

			IList<Entry> entries = Entries.GetEntriesByTag(1, expectedTag);
			Assert.AreEqual(1, entries.Count);
			Assert.AreEqual(entry.Id, entries[0].Id);
		}

		[Test]
		[RollBack]
		public void CanParseMultiRelTag()
		{
			string hostname = UnitTestHelper.GenerateRandomString();
			Assert.IsTrue(Config.CreateBlog("", "username", "password", hostname, string.Empty));
			UnitTestHelper.SetHttpContextWithBlogRequest(hostname, string.Empty);
			Entry entry = UnitTestHelper.CreateEntryInstanceForSyndication("me", "title-zero", "body-zero");
			Entries.Create(entry);
			entry.Body = "<a href=\"http://blah/yourtag\" rel=\"tag friend\">nothing</a>";
			Entries.Update(entry);

			IList<Entry> entries = Entries.GetEntriesByTag(1, "yourtag");
			Assert.AreEqual(1, entries.Count);
			Assert.AreEqual(entry.Id, entries[0].Id);
		}

		[Test]
		[RollBack]
		public void CanParseAnchorWithWhiteSpace()
		{
			string hostname = UnitTestHelper.GenerateRandomString();
			Assert.IsTrue(Config.CreateBlog("", "username", "password", hostname, string.Empty));
			UnitTestHelper.SetHttpContextWithBlogRequest(hostname, string.Empty);
			Entry entry = UnitTestHelper.CreateEntryInstanceForSyndication("me", "title-zero", "body-zero");
			Entries.Create(entry);
			entry.Body = "<a href	  =  \"http://blah/sometag\" rel	=  \"tag friend\">nothing</a>";
			Entries.Update(entry);

			IList<Entry> entries = Entries.GetEntriesByTag(1, "sometag");
			Assert.AreEqual(1, entries.Count);
			Assert.AreEqual(entry.Id, entries[0].Id);
		}

		[Test]
		[RollBack]
		public void DuplicateTagsDoNotThrowException()
		{
			string hostname = UnitTestHelper.GenerateRandomString();
			Assert.IsTrue(Config.CreateBlog("", "username", "password", hostname, string.Empty));
			UnitTestHelper.SetHttpContextWithBlogRequest(hostname, string.Empty);
			Entry entry = UnitTestHelper.CreateEntryInstanceForSyndication("me", "title-zero", "body-zero");
			Entries.Create(entry);
			entry.Body = "<a href= \"http://blah/sometag\" rel= \"tag friend\">nothing</a><a href= \"http://blah/sometag\" rel= \"tag friend\">something</a>";
			Entries.Update(entry);

			IList<Entry> entries = Entries.GetEntriesByTag(1, "sometag");
			Assert.AreEqual(1, entries.Count);
			Assert.AreEqual(entry.Id, entries[0].Id);
		}
	}
}
