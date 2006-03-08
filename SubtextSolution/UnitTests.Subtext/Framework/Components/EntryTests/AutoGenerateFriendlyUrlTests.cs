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
	public class AutoGenerateFriendlyUrlTests
	{
		string _hostName = string.Empty;

		/// <summary>
		/// Makes sure we are generating nice friendly URLs.
		/// </summary>
		[RowTest]
		[Row("Title", "Title")]
		[Row("Title.", "Title")]
		[Row("A Very Good Book", "AVeryGoodBook")]
		[Row("A Very :Good Book", "AVeryGoodBook")]
		[Row("A Very ;Good Book", "AVeryGoodBook")]
		[Row("A Very Good Book.", "AVeryGoodBook")]
		[Row("A Very Good Book..", "AVeryGoodBook")]
		[Row("Trouble With VS.NET", "TroubleWithVS.NET")]
		[Row(@"[""'`;:~@#$%^&*(){\[}\]?+/=\\|<> X", "X")]
		[RollBack]
		public void FriendlyUrlGeneratesNiceUrl(string title, string expected)
		{
			Assert.IsTrue(Config.CreateBlog("", "username", "password", _hostName, string.Empty));
			Assert.AreEqual(expected, Entries.AutoGenerateFriendlyUrl(title), "THe auto generated entry name is not what we expected.");
		}

		[Test]
		[RollBack]
		public void FriendlyUrlReturnsNull()
		{
			string title = @"[""'`~@#$%^&*(){\[}\]?+/=\\|<>";
			string expected = null;
			Assert.IsTrue(Config.CreateBlog("", "username", "password", _hostName, string.Empty));
			Assert.AreEqual(expected, Entries.AutoGenerateFriendlyUrl(title), "THe auto generated entry name is not what we expected.");
		}

		/// <summary>
		/// Make sure that generated friendly urls are unique.
		/// </summary>
		[Test]
		[RollBack]
		public void FriendlyUrlIsUnique()
		{
			Assert.IsTrue(Config.CreateBlog("", "username", "password", _hostName, string.Empty));

			Config.CurrentBlog.AutoFriendlyUrlEnabled = true;
			Entry entry = new Entry(PostType.BlogPost);
			entry.DateCreated = DateTime.Now;
			entry.SourceUrl = "http://localhost/ThisUrl/";
			entry.Title = "Some Title";
			entry.Body = "Some Body";
			int id = Entries.Create(entry);

			Entry savedEntry = Entries.GetEntry(id, EntryGetOption.All);
			Assert.AreEqual("SomeTitle", savedEntry.EntryName, "The EntryName should have been auto-friendlied.");
			Assert.AreEqual(savedEntry.Link, savedEntry.TitleUrl, "The title url should link to the entry.");

			Entry duplicate = new Entry(PostType.BlogPost);
			duplicate.DateCreated = DateTime.Now;
			duplicate.SourceUrl = "http://localhost/ThisUrl/";
			duplicate.Title = "Some Title";
			duplicate.Body = "Some Body";
			int dupeId = Entries.Create(entry);
			Entry savedDupe = Entries.GetEntry(dupeId, EntryGetOption.All);
			
			Assert.AreEqual("SomeTitleAgain", savedDupe.EntryName, "Should have appended 'Again'");
			UnitTestHelper.AssertAreNotEqual(savedEntry.EntryName, savedDupe.EntryName, "No duplicate entry names are allowed.");

			Entry yetAnotherDuplicate = new Entry(PostType.BlogPost);
			yetAnotherDuplicate.DateCreated = DateTime.Now;
			yetAnotherDuplicate.SourceUrl = "http://localhost/ThisUrl/";
			yetAnotherDuplicate.Title = "Some Title";
			yetAnotherDuplicate.Body = "Some Body";
			dupeId = Entries.Create(entry);
			savedDupe = Entries.GetEntry(dupeId, EntryGetOption.All);
			
			Assert.AreEqual("SomeTitleYetAgain", savedDupe.EntryName, "Should have appended 'YetAgain'");

			yetAnotherDuplicate = new Entry(PostType.BlogPost);
			yetAnotherDuplicate.DateCreated = DateTime.Now;
			yetAnotherDuplicate.SourceUrl = "http://localhost/ThisUrl/";
			yetAnotherDuplicate.Title = "Some Title";
			yetAnotherDuplicate.Body = "Some Body";
			dupeId = Entries.Create(entry);
			savedDupe = Entries.GetEntry(dupeId, EntryGetOption.All);
			
			Assert.AreEqual("SomeTitleAndAgain", savedDupe.EntryName, "Should have appended 'AndAgain'");

			yetAnotherDuplicate = new Entry(PostType.BlogPost);
			yetAnotherDuplicate.DateCreated = DateTime.Now;
			yetAnotherDuplicate.SourceUrl = "http://localhost/ThisUrl/";
			yetAnotherDuplicate.Title = "Some Title";
			yetAnotherDuplicate.Body = "Some Body";
			dupeId = Entries.Create(entry);
			savedDupe = Entries.GetEntry(dupeId, EntryGetOption.All);
			
			Assert.AreEqual("SomeTitleOnceMore", savedDupe.EntryName, "Should have appended 'OnceMore'");

			yetAnotherDuplicate = new Entry(PostType.BlogPost);
			yetAnotherDuplicate.DateCreated = DateTime.Now;
			yetAnotherDuplicate.SourceUrl = "http://localhost/ThisUrl/";
			yetAnotherDuplicate.Title = "Some Title";
			yetAnotherDuplicate.Body = "Some Body";
			dupeId = Entries.Create(entry);
			savedDupe = Entries.GetEntry(dupeId, EntryGetOption.All);
			
			Assert.AreEqual("SomeTitleToBeatADeadHorse", savedDupe.EntryName, "Should have appended 'ToBeatADeadHorse'");
		}

		/// <summary>
		/// Sets the up test fixture.  This is called once for 
		/// this test fixture before all the tests run.  It 
		/// essentially copies the App.config file to the 
		/// run directory.
		/// </summary>
		[TestFixtureSetUp]
		public void SetUpTestFixture()
		{
			//Confirm app settings
			Assert.AreEqual("~/Admin/Resources/PageTemplate.ascx", System.Configuration.ConfigurationSettings.AppSettings["Admin.DefaultTemplate"]) ;
		}

		[SetUp]
		public void SetUp()
		{
			_hostName = System.Guid.NewGuid().ToString().Replace("-", "") + ".com";
			UnitTestHelper.SetHttpContextWithBlogRequest(_hostName, "");
		}

		[TearDown]
		public void TearDown()
		{
			Config.ConfigurationProvider = null;
		}
	}
}
