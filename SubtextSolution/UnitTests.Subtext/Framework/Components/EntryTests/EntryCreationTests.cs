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

		/// <summary>
		/// Tests that the fully qualified url is correct.
		/// </summary>
		[RowTest]
		[Row("", "", "")]
		[Row("blog", "", "/blog")]
		[Row("", "Subtext.Web", "/Subtext.Web")]
		[Row("blog", "Subtext.Web", "/Subtext.Web/blog")]
		[RollBack]
		public void CreatedEntryHasCorrectFullyQualifiedLink(string subfolder, string virtualDir, string expectedUrlPrefix)
		{
			string hostname = UnitTestHelper.GenerateRandomHostname();
			Assert.IsTrue(Config.CreateBlog("", "username", "password", hostname, subfolder));
			
			UnitTestHelper.SetHttpContextWithBlogRequest(hostname, subfolder, virtualDir);

			Entry entry = new Entry(PostType.BlogPost);
			entry.DateCreated = DateTime.ParseExact("2005/01/23", "yyyy/MM/dd", CultureInfo.InvariantCulture);
			entry.Title = "Some Title";
			entry.Body = "Some Body";
			int id = Entries.Create(entry);

			string expectedLink = string.Format("{0}/archive/2005/01/23/{1}.aspx", expectedUrlPrefix, id);
			string expectedFullyQualifiedLink = "http://" + hostname + expectedLink;
			
			Entry savedEntry = Entries.GetEntry(id, EntryGetOption.All);
			Assert.AreEqual(savedEntry.Url, expectedLink, "The link was not what we expected.");
			Assert.AreEqual(savedEntry.FullyQualifiedUrl, expectedFullyQualifiedLink, "The link was not what we expected.");
		}
		
		/// <summary>
		/// Makes sure that the content checksum hash is being created correctly.
		/// </summary>
		[Test]
		[RollBack]
		public void EntryCreateHasContentHash()
		{
			Assert.IsTrue(Config.CreateBlog("", "username", "password", _hostName, string.Empty));

			Entry entry = new Entry(PostType.PingTrack);
			entry.DateCreated = DateTime.Now;
			entry.SourceUrl = "http://" + UnitTestHelper.GenerateRandomHostname() + "/ThisUrl/";
			entry.Title = "Some Title";
			entry.Body = "Some Body";
			int id = Entries.Create(entry);

			Entry savedEntry = Entries.GetEntry(id, EntryGetOption.All);
			Assert.IsTrue(savedEntry.ContentChecksumHash.Length > 0, "The Content Checksum should be larger than 0.");
		}
		
		[Test]
		[RollBack]
		public void EntryDateSyndicatedIsNullEquivalentUnlessPublished()
		{
			Assert.IsTrue(Config.CreateBlog("", "username", "password", _hostName, string.Empty));

			Entry entry = new Entry(PostType.BlogPost);
			
			entry.DateCreated = DateTime.Now;
			entry.SourceUrl = "http://" + UnitTestHelper.GenerateRandomHostname() + "/ThisUrl/";
			entry.Title = "Some Title";
			entry.Body = "Some Body";
			
			int id = Entries.Create(entry);
			Entry savedEntry = Entries.GetEntry(id, EntryGetOption.All);
			
			Assert.AreEqual(NullValue.NullDateTime, savedEntry.DateSyndicated, "DateSyndicated should be null since it was not syndicated.");
			
			savedEntry.IsActive = true;
			Thread.Sleep(1000);
			savedEntry.IncludeInMainSyndication = true;
			Entries.Update(savedEntry);
					
			Assert.IsTrue(savedEntry.DateSyndicated > savedEntry.DateCreated, string.Format("DateSyndicated '{0}' should larger than date created '{1}'.", savedEntry.DateSyndicated, savedEntry.DateCreated));
			
			savedEntry = Entries.GetEntry(id, EntryGetOption.All);
			Assert.IsTrue(savedEntry.DateSyndicated > savedEntry.DateCreated, string.Format("After reloading from DB, DateSyndicated '{0}' should larger than date created '{1}'.", savedEntry.DateSyndicated, savedEntry.DateCreated));
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
			UnitTestHelper.SetHttpContextWithBlogRequest(_hostName, string.Empty);
			CommentFilter.ClearCommentCache();
		}

		[TearDown]
		public void TearDown()
		{
			Config.ConfigurationProvider = null;
		}
	}
}
