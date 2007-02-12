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
using MbUnit.Framework;
using Subtext.Extensibility;
using Subtext.Framework;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;

namespace UnitTests.Subtext.Framework.Components.EntryTests
{
	/// <summary>
	/// Tests the feature to auto generate the EntryName property 
	/// of an entry. This serves as a friendly url.
	/// </summary>
	[TestFixture]
	[Author("Robb Allen", "robb.allen@gmail.com", "http://blog.robballen.com")]
	public class AutoGenerateFriendlyUrlTests
	{
		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void FriendlyUrlThrowsArgumentException()
		{
			Entries.AutoGenerateFriendlyUrl(null);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void FriendlyUrlWithSeparatorAndNullTitleThrowsArgumentException()
		{
            Entries.AutoGenerateFriendlyUrl(null, '_');
		}
	
		/// <summary>
		/// Make sure words are separated and limited correctly
		/// using 5 word limit and underscore
		/// </summary>
		
		[RowTest]
		[Row("Single", '_', "Single")]
		[Row("Single ", '.', "Single")]
		[Row("Two words", '_', "Two_words")]
		[Row("Two words", '-', "Two-words")]
		[Row("Two words", '.', "Two.words")]
		[Row("Holymolythisisalongwordthatnormallywouldn'tbeused.", '_', "Holymolythisisalongwordthatnormallywouldntbeused")]
		[Row("This is a very long.", '_', "This_is_a_very_long")]
		[Row("This is a very long.", '-', "This-is-a-very-long")]
		[Row("This is a very long.", '.', "This.is.a.very.long")]
		[RollBack]
		public void FriendlyUrlLimitedDelimited(string title, char wordSeparator, string expected)
		{
			UnitTestHelper.SetupBlog();
            Assert.AreEqual(expected, Entries.AutoGenerateFriendlyUrl(title, wordSeparator), "The auto generated entry name is not what we expected.");
		}

		/// <summary>
		/// Makes sure we are generating nice friendly URLs.
		/// </summary>
		[RowTest]
		[Row("Title", "Title")]
		[Row("Title.", "Title")]
		[Row("A Very Good Book", "AVeryGoodBook")]
		[Row("a very good book", "AVeryGoodBook")]
		[Row("A Very :Good Book", "AVeryGoodBook")]
		[Row("A Very ;Good Book", "AVeryGoodBook")]
		[Row("A Very Good Book.", "AVeryGoodBook")]
		[Row("A Very Good Book..", "AVeryGoodBook")]
		[Row("A Very Good..Book", "AVeryGood.Book")]
		[Row("A Very Good...Book", "AVeryGood.Book")]
		[Row("Å Vêry G®®d B®®k..", "%c3%85V%c3%aaryGdBk")]
		[Row("\u0130\u0069Turkish Character Test", "%c4%b0iTurkishCharacterTest")]
		[Row("Trouble With VS.NET", "TroubleWithVS.NET")]
		[Row("Barça is a nice town", "Bar%c3%a7aIsANiceTown")]
		[Row("Perchè Più felicità può ed é?", "Perch%c3%a8Pi%c3%b9Felicit%c3%a0Pu%c3%b2Ed%c3%89")]
		[Row(@"[!""'`;:~@#$%^&*(){\[}\]?+/=\\|<> X", "X")]
		[RollBack]
		public void FriendlyUrlGeneratesNiceUrl(string title, string expected)
		{
			UnitTestHelper.SetupBlog();
            Assert.AreEqual(expected, Entries.AutoGenerateFriendlyUrl(title, char.MinValue), "The auto generated entry name is not what we expected.");
		}

		/// <summary>
		/// Makes sure we are generating nice friendly URLs Using Underscores.
		/// </summary>
		[RowTest]
		[Row("Title", "Title")]
		[Row("Title.", "Title")]
		[Row("A Very Good Book Yo", "A_Very_Good_Book_Yo")]
		[Row("a very good book yo", "a_very_good_book_yo")]
		[Row("A Very ::Good Book", "A_Very_Good_Book")]
		[Row("A Very ;;Good Book", "A_Very_Good_Book")]
		[Row("A Very Good Book yo.", "A_Very_Good_Book_yo")]
		[Row("A Very Good Book yo..", "A_Very_Good_Book_yo")]
		[Row("Å Vêry Good Book yo..", "%c3%85_V%c3%aary_Good_Book_yo")]
		[Row("Trouble With VS.NET Yo", "Trouble_With_VS.NET_Yo")]
		[Row("Barça is a nice town", "Bar%c3%a7a_is_a_nice_town")]
		[Row("Perchè Più felicità può ed é?", "Perch%c3%a8_Pi%c3%b9_felicit%c3%a0_pu%c3%b2_ed_%c3%a9")]
		[Row(@"[!""'`;:~@#$%^&*(){\[}\]?+/=\\|<> Y", "Y")]
		[Row(@"[!""'`;:~@#$%^&*(){\[}\]?+/=\\|<>YY", "YY")]
		[RollBack]
		public void FriendlyUrlGeneratesNiceUrlWithUnderscores(string title, string expected)
		{
			UnitTestHelper.SetupBlog();
            Assert.AreEqual(expected, Entries.AutoGenerateFriendlyUrl(title, '_'), "THe auto generated entry name is not what we expected.");
		}
		
		/// <summary>
		/// Makes sure we are generating nice friendly URLs Using Periods.
		/// </summary>
		[RowTest]
		[Row("Title.", "Title")]
		[Row("Contains.PeriodAlready", "Contains.PeriodAlready")]
		[Row("A Very Good Book yo..", "A.Very.Good.Book.yo")]
		[RollBack]
		public void FriendlyUrlGeneratesNiceUrlWithPeriods(string title, string expected)
		{
			UnitTestHelper.SetupBlog();
            Assert.AreEqual(expected, Entries.AutoGenerateFriendlyUrl(title, '.'), "THe auto generated entry name is not what we expected.");
		}

		[RowTest]
		[Row('_', "One_Two")]
		[Row(char.MinValue, "OneTwo")]
		[Row('.', "One.Two")]
		[Row('-', "One-Two")]
		[RollBack]
		public void FriendlyUrlHandlesBadSeparators(char wordSeparator, string expected)
		{
			UnitTestHelper.SetupBlog();

			string title = "One Two";
			Assert.AreEqual(expected, Entries.AutoGenerateFriendlyUrl(title, wordSeparator), "THe auto generated entry name is not what we expected.");
		}
		
		/// <summary>
		/// Make sure that we do not override a supplied EntryName by auto-generating a url. 
		/// Entryname should take precedence.
		/// </summary>
		[Test]
		[RollBack]
		public void FriendlyUrlDoesNotOverrideEntryName()
		{
			UnitTestHelper.SetupBlog();

			Config.CurrentBlog.AutoFriendlyUrlEnabled = true;
			Entry entry = new Entry(PostType.BlogPost);
			entry.EntryName = "IWantThisUrl";
			entry.DateCreated = DateTime.Now;
			entry.Title = "Some Title";
			entry.Body = "Some Body";
			int id = Entries.Create(entry);

			Entry savedEntry = Entries.GetEntry(id, PostConfig.None, false);
			Assert.AreEqual("IWantThisUrl", savedEntry.EntryName, "The EntryName should match the EntryName, not the auto-generated.");
		}

        [RowTest]
        [Row("12345", '_', "n_12345")]
        [Row("12345f", '_', "12345f")]
        [RollBack]
        public void GenerateFriendlyUrlFixesNumericTitles(string title, char wordSeparator, string expected)
        {
            Config.CreateBlog("foo-izze", "username", "password", UnitTestHelper.GenerateRandomString(), string.Empty);
            string friendlyName = Entries.AutoGenerateFriendlyUrl(title, wordSeparator);
            Assert.AreEqual(expected, friendlyName, "Need to prepend an 'n' to the end of numeric EntryNames.");
        }

		/// <summary>
		/// Make sure that generated friendly urls are unique.
		/// </summary>
		[Test]
		[RollBack]
		public void FriendlyUrlIsUnique()
		{
			UnitTestHelper.SetupBlog();

			Config.CurrentBlog.AutoFriendlyUrlEnabled = true;
			Entry entry = new Entry(PostType.BlogPost);
			entry.DateCreated = DateTime.Now;
			entry.Title = "Some Title";
			entry.Body = "Some Body";
			int id = Entries.Create(entry);

            Entry savedEntry = Entries.GetEntry(id, PostConfig.None, false);
			Assert.AreEqual("Some_Title", savedEntry.EntryName, "The EntryName should have been auto-friendlied.");

			Entry duplicate = new Entry(PostType.BlogPost);
			duplicate.DateCreated = DateTime.Now;
			duplicate.Title = "Some Title";
			duplicate.Body = "Some Body";
			int dupeId = Entries.Create(duplicate);
            Entry savedDupe = Entries.GetEntry(dupeId, PostConfig.None, false);
			
			Assert.AreEqual("Some_TitleAgain", savedDupe.EntryName, "Should have appended 'Again'");
			UnitTestHelper.AssertAreNotEqual(savedEntry.EntryName, savedDupe.EntryName, "No duplicate entry names are allowed.");

			Entry yetAnotherDuplicate = new Entry(PostType.BlogPost);
			yetAnotherDuplicate.DateCreated = DateTime.Now;
			yetAnotherDuplicate.Title = "Some Title";
			yetAnotherDuplicate.Body = "Some Body";
			dupeId = Entries.Create(yetAnotherDuplicate);
            savedDupe = Entries.GetEntry(dupeId, PostConfig.None, false);
			
			Assert.AreEqual("Some_TitleYetAgain", savedDupe.EntryName, "Should have appended 'YetAgain'");

			yetAnotherDuplicate = new Entry(PostType.BlogPost);
			yetAnotherDuplicate.DateCreated = DateTime.Now;
			yetAnotherDuplicate.Title = "Some Title";
			yetAnotherDuplicate.Body = "Some Body";
			dupeId = Entries.Create(yetAnotherDuplicate);
            savedDupe = Entries.GetEntry(dupeId, PostConfig.None, false);
			
			Assert.AreEqual("Some_TitleAndAgain", savedDupe.EntryName, "Should have appended 'AndAgain'");

			yetAnotherDuplicate = new Entry(PostType.BlogPost);
			yetAnotherDuplicate.DateCreated = DateTime.Now;
			yetAnotherDuplicate.Title = "Some Title";
			yetAnotherDuplicate.Body = "Some Body";
			dupeId = Entries.Create(yetAnotherDuplicate);
            savedDupe = Entries.GetEntry(dupeId, PostConfig.None, false);
			
			Assert.AreEqual("Some_TitleOnceMore", savedDupe.EntryName, "Should have appended 'OnceMore'");

			yetAnotherDuplicate = new Entry(PostType.BlogPost);
			yetAnotherDuplicate.DateCreated = DateTime.Now;
			yetAnotherDuplicate.Title = "Some Title";
			yetAnotherDuplicate.Body = "Some Body";
			dupeId = Entries.Create(yetAnotherDuplicate);
            savedDupe = Entries.GetEntry(dupeId, PostConfig.None, false);
			
			Assert.AreEqual("Some_TitleToBeatADeadHorse", savedDupe.EntryName, "Should have appended 'ToBeatADeadHorse'");
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

		[TearDown]
		public void TearDown()
		{
			Config.ConfigurationProvider = null;
		}
	}
}
