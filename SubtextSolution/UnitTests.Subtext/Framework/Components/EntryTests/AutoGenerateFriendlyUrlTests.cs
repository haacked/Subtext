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
using Subtext.Framework.Exceptions;

namespace UnitTests.Subtext.Framework.Components.EntryTests
{
	/// <summary>
	/// Tests the feature to auto generate the EntryName property 
	/// of an entry. This serves as a friendly url.
	/// </summary>
	[TestFixture]
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
		/// Makes sure we are generating nice friendly URLs Using Underscores.
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
		[Row("Title", '_', "Title")]
		[Row("Title.", '_', "Title")]
		[Row("A Very Good Book Yo", '_', "A_Very_Good_Book_Yo")]
		[Row("a very good book yo", '_', "a_very_good_book_yo")]
		[Row("A Very ::Good Book", '_', "A_Very_Good_Book")]
		[Row("A Very ;;Good Book", '_', "A_Very_Good_Book")]
		[Row("A Very Good Book yo.", '_', "A_Very_Good_Book_yo")]
		[Row("A Very Good Book yo..", '_', "A_Very_Good_Book_yo")]
		[Row("Å Vêry Good Book yo..", '_', "A_Very_Good_Book_yo")]
		[Row("Trouble With VS.NET Yo", '_', "Trouble_With_VS.NET_Yo")]
		[Row("Barça is a nice town", '_', "Barca_is_a_nice_town")]
        [Row("Diseñadora de interfaces", '_', "Disenadora_de_interfaces")]
		[Row("Perchè Più felicità può ed é?", '_', "Perche_Piu_felicita_puo_ed_e")]
		[Row(@"[!""'`;:~@#$%^&*(){\[}\]?+/=\\|<> Y", '_', "Y")]
		[Row(@"[!""'`;:~@#$%^&*(){\[}\]?+/=\\|<>YY", '_', "YY")]
		[Row("Title", char.MinValue, "Title")]
		[Row("Title.", char.MinValue, "Title")]
		[Row("A Very Good Book", char.MinValue, "AVeryGoodBook")]
		[Row("a very good book", char.MinValue, "AVeryGoodBook")]
		[Row("A Very :Good Book", char.MinValue, "AVeryGoodBook")]
		[Row("A Very ;Good Book", char.MinValue, "AVeryGoodBook")]
		[Row("A Very Good Book.", char.MinValue, "AVeryGoodBook")]
		[Row("A Very Good Book..", char.MinValue, "AVeryGoodBook")]
		[Row("A Very Good..Book", char.MinValue, "AVeryGood.Book")]
		[Row("A Very Good...Book", char.MinValue, "AVeryGood.Book")]
		[Row("Å Vêry G®®d B®®k..", char.MinValue, "AVeryGdBk")]
		[Row("\u0130\u0069Turkish Character Test", char.MinValue, "IiTurkishCharacterTest")]
		[Row("Trouble With VS.NET", char.MinValue, "TroubleWithVS.NET")]
		[Row("Barça is a nice town", char.MinValue, "BarcaIsANiceTown")]
        [Row("Diseñadora de interfaces", char.MinValue, "DisenadoraDeInterfaces")]
		[Row("Perchè Più felicità può ed é?", char.MinValue, "PerchePiuFelicitaPuoEdE")]
		[Row(@"[!""'`;:~@#$%^&*(){\[}\]?+/=\\|<> X", char.MinValue, "X")]
		[Row(@"This  is cool", '_', "This_is_cool")]
		[Row(@"This - is cool", '-', "This-is-cool")]
		[RollBack2]
		public void FriendlyUrlGeneratesNiceUrl(string title, char separator, string expected)
		{
			UnitTestHelper.SetupBlog();
			Assert.AreEqual(expected, Entries.AutoGenerateFriendlyUrl(title, separator), "The auto generated entry name is not what we expected.");
		}

		[RowTest]
		[Row("THIS IS A NEW TEST", TextTransform.LowerCase, "this-is-a-new-test")]
		[RollBack2]
		public void FriendlyUrlCanTransformText(string title, TextTransform transform, string expected)
		{
			UnitTestHelper.SetupBlog();
			Assert.AreEqual(expected, Entries.AutoGenerateFriendlyUrl(title, '-', transform), "The auto generated entry name is not what we expected.");
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
		[Row("Å Vêry Good Book yo..", "A_Very_Good_Book_yo")]
		[Row("Trouble With VS.NET Yo", "Trouble_With_VS.NET_Yo")]
		[Row("Barça is a nice town", "Barca_is_a_nice_town")]
		[Row("Perchè Più felicità può ed é?", "Perche_Piu_felicita_puo_ed_e")]
		[Row(@"[!""'`;:~@#$%^&*(){\[}\]?+/=\\|<> Y", "Y")]
		[Row(@"[!""'`;:~@#$%^&*(){\[}\]?+/=\\|<>YY", "YY")]
		[RollBack2]
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
		[RollBack2]
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
		[RollBack2]
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
		[RollBack2]
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
        [RollBack2]
        public void GenerateFriendlyUrlFixesNumericTitles(string title, char wordSeparator, string expected)
        {
			UnitTestHelper.SetupBlog();
            string friendlyName = Entries.AutoGenerateFriendlyUrl(title, wordSeparator);
            Assert.AreEqual(expected, friendlyName, "Need to prepend an 'n' to the end of numeric EntryNames.");
        }

		/// <summary>
		/// Make sure that generated friendly urls are unique.
		/// </summary>
		[Test]
		[RollBack2]
		public void FriendlyUrlIsUnique()
		{
			UnitTestHelper.SetupBlog();

			Config.CurrentBlog.AutoFriendlyUrlEnabled = true;
			Entry entry = new Entry(PostType.BlogPost);
			entry.DateCreated = DateTime.Now;
			entry.Title = "Some Entry Title";
			entry.Body = "Some Body";
			int id = Entries.Create(entry);

            Entry savedEntry = Entries.GetEntry(id, PostConfig.None, false);
			Assert.AreEqual("Some_Entry_Title", savedEntry.EntryName, "The EntryName should have been auto-friendlied.");

			Entry duplicate = new Entry(PostType.BlogPost);
			duplicate.DateCreated = DateTime.Now;
			duplicate.Title = "Some Entry Title";
			duplicate.Body = "Some Body";
			int dupeId = Entries.Create(duplicate);
            Entry savedDupe = Entries.GetEntry(dupeId, PostConfig.None, false);
			
			Assert.AreEqual("Some_Entry_Title_Again", savedDupe.EntryName, "Should have appended 'Again'");
			UnitTestHelper.AssertAreNotEqual(savedEntry.EntryName, savedDupe.EntryName, "No duplicate entry names are allowed.");

			Entry yetAnotherDuplicate = new Entry(PostType.BlogPost);
			yetAnotherDuplicate.DateCreated = DateTime.Now;
			yetAnotherDuplicate.Title = "Some Entry Title";
			yetAnotherDuplicate.Body = "Some Body";
			dupeId = Entries.Create(yetAnotherDuplicate);
            savedDupe = Entries.GetEntry(dupeId, PostConfig.None, false);
			
			Assert.AreEqual("Some_Entry_Title_Yet_Again", savedDupe.EntryName, "Should have appended 'Yet_Again'");

			yetAnotherDuplicate = new Entry(PostType.BlogPost);
			yetAnotherDuplicate.DateCreated = DateTime.Now;
			yetAnotherDuplicate.Title = "Some Entry Title";
			yetAnotherDuplicate.Body = "Some Body";
			dupeId = Entries.Create(yetAnotherDuplicate);
            savedDupe = Entries.GetEntry(dupeId, PostConfig.None, false);
			
			Assert.AreEqual("Some_Entry_Title_And_Again", savedDupe.EntryName, "Should have appended 'And_Again'");

			yetAnotherDuplicate = new Entry(PostType.BlogPost);
			yetAnotherDuplicate.DateCreated = DateTime.Now;
			yetAnotherDuplicate.Title = "Some Entry Title";
			yetAnotherDuplicate.Body = "Some Body";
			dupeId = Entries.Create(yetAnotherDuplicate);
            savedDupe = Entries.GetEntry(dupeId, PostConfig.None, false);
			
			Assert.AreEqual("Some_Entry_Title_Once_More", savedDupe.EntryName, "Should have appended 'Once_More'");

			yetAnotherDuplicate = new Entry(PostType.BlogPost);
			yetAnotherDuplicate.DateCreated = DateTime.Now;
			yetAnotherDuplicate.Title = "Some Entry Title";
			yetAnotherDuplicate.Body = "Some Body";
			dupeId = Entries.Create(yetAnotherDuplicate);
            savedDupe = Entries.GetEntry(dupeId, PostConfig.None, false);
			
			Assert.AreEqual("Some_Entry_Title_To_Beat_A_Dead_Horse", savedDupe.EntryName, "Should have appended 'To_Beat_A_Dead_Horse'");

			yetAnotherDuplicate = new Entry(PostType.BlogPost);
			yetAnotherDuplicate.DateCreated = DateTime.Now;
			yetAnotherDuplicate.Title = "Some Entry Title";
			yetAnotherDuplicate.Body = "Some Body";

			try
			{
				Entries.Create(yetAnotherDuplicate);
				Assert.Fail("Expected a duplicate entry exception");
			}
			catch(DuplicateEntryException)
			{
			}
		}

		/// <summary>
		/// Make sure that generated friendly urls are not changed when updating entry.
		/// </summary>
		[Test]
		[RollBack2]
		public void FriendlyUrlIsNotChangedInUpdates() 
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

			Entries.Update(savedEntry);

			Entry updatedEntry = Entries.GetEntry(id, PostConfig.None, false);
			Assert.AreEqual("Some_Title", updatedEntry.EntryName, "The EntryName should not have been re-auto-friendlied.");
		}

		/// <summary>
		/// Make sure that generated friendly urls are not changed when updating entry.
		/// </summary>
		[Test]
		[RollBack2]
		public void FriendlyUrlIsUniqueInUpdates()
		{
		    UnitTestHelper.SetupBlog();
			Config.CurrentBlog.AutoFriendlyUrlEnabled = true;

			Entry entry1 = new Entry(PostType.BlogPost);
			entry1.DateCreated = DateTime.Now;
			entry1.Title = "Some Random Title";
			entry1.Body = "Some Body";
			int id1 = Entries.Create(entry1);

			Entry savedEntry1 = Entries.GetEntry(id1, PostConfig.None, false);
			Assert.AreEqual("Some_Random_Title", savedEntry1.EntryName, "The EntryName should have been auto-friendlied.");

			Entry entry2 = new Entry(PostType.BlogPost);
			entry2.DateCreated = DateTime.Now;
			entry2.Title = "Some Other Random Title";
			entry2.Body = "Some Body";
			int id2 = Entries.Create(entry2);

			Entry savedEntry2 = Entries.GetEntry(id2, PostConfig.None, false);
			Assert.AreEqual("Some_Other_Random_Title", savedEntry2.EntryName, "The EntryName should have been auto-friendlied.");

			savedEntry2.EntryName = "Some_New_Changed_Random_Title";
			Entries.Update(savedEntry2);

			Entry updatedEntry = Entries.GetEntry(id2, PostConfig.None, false);
			Assert.AreEqual("Some_New_Changed_Random_Title", updatedEntry.EntryName, "Able to change the entry and retrieve it.");
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
