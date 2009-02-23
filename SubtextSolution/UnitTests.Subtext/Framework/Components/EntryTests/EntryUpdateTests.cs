using System;
using System.Threading;
using MbUnit.Framework;
using Subtext.Extensibility;
using Subtext.Framework;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;
using Subtext.Framework.Web.HttpModules;
using Subtext.Framework.Providers;

namespace UnitTests.Subtext.Framework.Components.EntryTests
{
	/// <summary>
	/// Some unit tests around updating entries.
	/// </summary>
	[TestFixture]
	public class EntryUpdateTests
	{
		string _hostName;
		
		[Test]
		[RollBack]
		public void CanDeleteEntry()
		{
			Config.CreateBlog("", "username", "password", _hostName, string.Empty);
            BlogRequest.Current.Blog = Config.GetBlog(_hostName, string.Empty);

			Entry entry = UnitTestHelper.CreateEntryInstanceForSyndication("Haacked", "Title Test", "Body Rocking");
			UnitTestHelper.Create(entry);

			Entry savedEntry = Entries.GetEntry(entry.Id, PostConfig.None, false);
			Assert.IsNotNull(savedEntry);

			ObjectProvider.Instance().DeleteEntry(entry.Id);

			savedEntry = Entries.GetEntry(entry.Id, PostConfig.None, false);
			Assert.IsNull(savedEntry, "Entry should now be null.");
		}
		
		/// <summary>
		/// Tests that setting the date syndicated to null removes the item from syndication.
		/// </summary>
		[Test]
		[RollBack]
		public void SettingDateSyndicatedToNullRemovesItemFromSyndication()
		{
            //arrange
			Config.CreateBlog("", "username", "password", _hostName, string.Empty);
            BlogRequest.Current.Blog = Config.GetBlog(_hostName, string.Empty);
			
			Entry entry = UnitTestHelper.CreateEntryInstanceForSyndication("Haacked", "Title Test", "Body Rocking");
			UnitTestHelper.Create(entry);

			Assert.IsTrue(entry.IncludeInMainSyndication, "Failed to setup this test properly.  This entry should be included in the main syndication.");
			Assert.IsFalse(NullValue.IsNull(entry.DateSyndicated), "Failed to setup this test properly. DateSyndicated should be null.");
			
            //act
			entry.DateSyndicated = NullValue.NullDateTime;

            //assert
			Assert.IsFalse(entry.IncludeInMainSyndication, "Setting the DateSyndicated to a null date should have reset 'IncludeInMainSyndication'.");

            //save it
			Entries.Update(entry);
            Entry savedEntry = Entries.GetEntry(entry.Id, PostConfig.None, false);
			
            //assert again
            Assert.IsFalse(savedEntry.IncludeInMainSyndication, "This item should still not be included in main syndication.");
		}


        [Test]
        [RollBack]
        public void UpdateEntryCorrectsNumericEntryName()
        {
            Config.CreateBlog("", "username", "password", _hostName, string.Empty);
            BlogRequest.Current.Blog = Config.GetBlog(_hostName, string.Empty);

            Blog info = Config.CurrentBlog;
            Config.UpdateConfigData(info);

            Entry entry = new Entry(PostType.BlogPost);
            entry.DateCreated = DateTime.Now;
            entry.Title = "My Title";
            entry.Body = "My Post Body";

            UnitTestHelper.Create(entry);
            entry = Entries.GetEntry(entry.Id, PostConfig.None, false);

            entry.EntryName = "4321";
            Entries.Update(entry);
            Entry updatedEntry = Entries.GetEntry(entry.Id, PostConfig.None, false);

            Assert.AreEqual("n_4321", updatedEntry.EntryName, "Expected entryName = 'n_4321'");
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
