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
using System.Globalization;
using System.Threading;
using MbUnit.Framework;
using Subtext.Extensibility;
using Subtext.Framework;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;

namespace UnitTests.Subtext.Framework.Components.EntryTests
{
	/// <summary>
	/// Tests of the Entry creation filter. Applies to Trackbacks, PingBacks, 
	/// and Comments.
	/// </summary>
	[TestFixture]
	public class EntryCreationTests
	{
		string _hostName = string.Empty;

		[Test]
		[RollBack2]
		[ExpectedException(typeof(IllegalPostCharactersException))]
		public void EntryDoesNotAllowScriptTags()
		{
			string hostname = UnitTestHelper.GenerateUniqueString();
			Config.CreateBlog("", "username", "password", hostname, "");
			UnitTestHelper.SetHttpContextWithBlogRequest(hostname, "", "");

			Entry entry = new Entry(PostType.BlogPost);
			entry.DateCreated = DateTime.ParseExact("2005/01/23", "yyyy/MM/dd", CultureInfo.InvariantCulture);
			entry.Title = "Some Really Random Title";
			entry.Body = "Some <script></script> Body";
			Config.Settings.AllowScriptsInPosts = false;
			Entries.Create(entry);
		}

		[Test]
		[RollBack2]
		public void EntryAllowsScriptTagsIfAllowScriptsInPostsIsTrue()
		{
			string hostname = UnitTestHelper.GenerateUniqueString();
			Config.CreateBlog("", "username", "password", hostname, "");
			UnitTestHelper.SetHttpContextWithBlogRequest(hostname, "", "");

			Entry entry = new Entry(PostType.BlogPost);
			entry.DateCreated = DateTime.ParseExact("2005/01/23", "yyyy/MM/dd", CultureInfo.InvariantCulture);
			entry.Title = "Some Really Random Title";
			entry.Body = "Some <script></script> Body";
			Config.Settings.AllowScriptsInPosts = true;
		    entry.DateSyndicated = entry.DateCreated.AddMonths(1);
		    entry.IsActive = true;
			Entries.Create(entry);
		}

		[Test]
		[RollBack2]
		public void EntryDateSyndicatedIsNullEquivalentUnlessPublished()
		{
			Config.CreateBlog("", "username", "password", _hostName, string.Empty);

			Entry entry = new Entry(PostType.BlogPost);
			
			entry.DateCreated = DateTime.Now;
			entry.Title = "Some Title";
			entry.Body = "Some Body";
			
			int id = Entries.Create(entry);
            Entry savedEntry = Entries.GetEntry(id, PostConfig.None, false);
			
			Assert.AreEqual(NullValue.NullDateTime, savedEntry.DateSyndicated, "DateSyndicated should be null since it was not syndicated.");

            Thread.Sleep(1000);
			savedEntry.IsActive = true;
            Entries.Update(savedEntry);

            Assert.IsTrue(savedEntry.DateSyndicated > savedEntry.DateCreated, string.Format("DateSyndicated '{0}' should larger than date created '{1}'.", savedEntry.DateSyndicated, savedEntry.DateCreated));
		    
            savedEntry = Entries.GetEntry(id, PostConfig.None, false);
            Assert.IsTrue(savedEntry.DateSyndicated > savedEntry.DateCreated, string.Format("After reloading from DB, DateSyndicated '{0}' should larger than date created '{1}'.", savedEntry.DateSyndicated, savedEntry.DateCreated));
            DateTime dateAfterPublishing = savedEntry.DateSyndicated;

			Thread.Sleep(1000);
			savedEntry.IncludeInMainSyndication = true;
			Entries.Update(savedEntry);

            Assert.IsTrue(dateAfterPublishing.Equals(savedEntry.DateSyndicated), string.Format("DateSyndicated '{0}' should be the same as the date of publication '{1}'.", savedEntry.DateSyndicated, dateAfterPublishing));

            savedEntry = Entries.GetEntry(id, PostConfig.None, false);
            Assert.AreEqual(dateAfterPublishing, savedEntry.DateSyndicated, string.Format("After reloading from DB, DateSyndicated '{0}' should be the same as the date of publication '{1}'.", savedEntry.DateSyndicated, dateAfterPublishing));
		}

        [RowTest]
        [Row(true)]
        [Row(false)]
        [RollBack2]
        public void CreateEntryCorrectsNumericEntryName(bool isAutoGenerate)
        {
            Config.CreateBlog("", "username", "password", _hostName, string.Empty);
            Blog info = Config.CurrentBlog;
            info.AutoFriendlyUrlEnabled = isAutoGenerate;
            Config.UpdateConfigData(info);

            Entry entry = new Entry(PostType.BlogPost);
            entry.DateCreated = DateTime.Now;
            entry.Title = "My Title";
            entry.Body = "My Post Body";
            entry.EntryName = "9876";

            Entries.Create(entry);
            Entry savedEntry = Entries.GetEntry(entry.Id, PostConfig.None, false);

            Assert.AreEqual("n_9876", savedEntry.EntryName, "Expected entryName = 'n_9876'");
        }

		/// <summary>
		/// Sets the up test fixture.  This is called once for 
		/// this test fixture before all the tests run.
		/// </summary>
		[TestFixtureSetUp]
		public void SetUpTestFixture()
		{		
			//Confirm app settings
            UnitTestHelper.AssertAppSettings();
		}

		[SetUp]
		public void SetUp()
		{
			_hostName = UnitTestHelper.GenerateUniqueString();
			UnitTestHelper.SetHttpContextWithBlogRequest(_hostName, string.Empty);
		}

		[TearDown]
		public void TearDown()
		{
			Config.ConfigurationProvider = null;
		}
	}
}
