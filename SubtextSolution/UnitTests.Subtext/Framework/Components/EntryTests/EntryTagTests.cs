using System;
using System.Collections.Generic;
using MbUnit.Framework;
using Subtext.Data;
using Subtext.Extensibility;
using Subtext.Framework;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;
using Subtext.Framework.Data;

namespace UnitTests.Subtext.Framework.Components.EntryTests
{
	[TestFixture]
	public class EntryTagTests
	{
		/// <summary>
		/// Tests that we can rebuild all the tags. This is for posts that were written before 
		/// we had tagging, but which used the tag microformat. Hence this test has to do some 
		/// ugly stuff to simulate that.
		/// </summary>
		[Test]
		[RollBack2]
		public void CanRebuildAllTags()
		{
			string tag = Guid.NewGuid().ToString();

			UnitTestHelper.SetupBlog();
			Entry entry = UnitTestHelper.CreateEntryInstanceForSyndication("me", "title-zero", "body-zero");
			int entryId = Entries.Create(entry);

			//Update the post directly in the db, so no tag is created.
			StoredProcedures.UpdateEntry(entryId
			                             , "title-zero"
			                             , string.Format(@"blah blah <a href=""http://blah/{0}"" rel=""tag"">{0}</a> blah", tag)
			                             , (int)PostType.BlogPost
			                             , (Guid)Config.CurrentBlog.Owner.ProviderUserKey
										 , ""
			                             , DateTime.Now
			                             , (int)PostConfig.IsActive
			                             , "title-zero"
			                             , DateTime.Now
			                             , Config.CurrentBlog.Id).Execute();
			Assert.AreEqual(0, Entries.GetEntriesByTag(1, tag).Count, "Should not have found a post with this tag.");

			Entries.RebuildAllTags();
			Assert.AreEqual(1, Entries.GetEntriesByTag(1, tag).Count, "After rebuilding tags, expected to find one post with this tag.");
		}

		[Test]
		[RollBack2]
		public void TagDoesNotRetrieveDraftEntry()
		{
			UnitTestHelper.SetupBlog();
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
			UnitTestHelper.SetupBlog();
			Entry entry = UnitTestHelper.CreateEntryInstanceForSyndication("me", "title-zero", "body-zero");
			Entries.Create(entry);

			List<string> tags = new List<string>(new string[] {"Tag1", "Tag2"});
			DatabaseObjectProvider.Instance().SetEntryTagList(entry.Id, tags);

			IList<Entry> entries = Entries.GetEntriesByTag(1, "Tag1");
			Assert.AreEqual(1, entries.Count);
			Assert.AreEqual(entry.Id, entries[0].Id);
		}

		[Test]
		[RollBack2]
		public void CanParseMultilineTag()
		{
			UnitTestHelper.SetupBlog();
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
		[RollBack2]
		public void CanParseEntryTags(string url, string expectedTag)
		{
			UnitTestHelper.SetupBlog();
			Entry entry = UnitTestHelper.CreateEntryInstanceForSyndication("me", "title-zero", "body-zero");
			Entries.Create(entry);
			entry.Body = "<a href=\"" + url + "\" rel=\"tag\">nothing</a>";
			Entries.Update(entry);

			IList<Entry> entries = Entries.GetEntriesByTag(1, expectedTag);
			Assert.AreEqual(1, entries.Count);
			Assert.AreEqual(entry.Id, entries[0].Id);
		}

		[Test]
		[RollBack2]
		public void CanParseMultiRelTag()
		{
			UnitTestHelper.SetupBlog();
			Entry entry = UnitTestHelper.CreateEntryInstanceForSyndication("me", "title-zero", "body-zero");
			Entries.Create(entry);
			entry.Body = "<a href=\"http://blah/yourtag\" rel=\"tag friend\">nothing</a>";
			Entries.Update(entry);

			IList<Entry> entries = Entries.GetEntriesByTag(1, "yourtag");
			Assert.AreEqual(1, entries.Count);
			Assert.AreEqual(entry.Id, entries[0].Id);
		}

		[Test]
		[RollBack2]
		public void CanParseAnchorWithWhiteSpace()
		{
			UnitTestHelper.SetupBlog();
			Entry entry = UnitTestHelper.CreateEntryInstanceForSyndication("me", "title-zero", "body-zero");
			Entries.Create(entry);
			entry.Body = "<a href	  =  \"http://blah/sometag\" rel	=  \"tag friend\">nothing</a>";
			Entries.Update(entry);

			IList<Entry> entries = Entries.GetEntriesByTag(1, "sometag");
			Assert.AreEqual(1, entries.Count);
			Assert.AreEqual(entry.Id, entries[0].Id);
		}

		[Test]
		[RollBack2]
		public void DuplicateTagsDoNotThrowException()
		{
			UnitTestHelper.SetupBlog();
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
