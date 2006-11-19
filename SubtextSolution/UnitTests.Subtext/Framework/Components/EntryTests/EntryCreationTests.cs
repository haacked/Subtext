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
		/// <summary>
		/// Tests that the fully qualified url is correct.
		/// </summary>
		[RowTest]
		[Row("", "", "")]
		[Row("blog", "", "/blog")]
		[Row("", "Subtext.Web", "/Subtext.Web")]
		[Row("blog", "Subtext.Web", "/Subtext.Web/blog")]
		[RollBack]
		public void CreatedEntryHasCorrectFullyQualifiedLink(string subfolder, string applicationPath, string expectedUrlPrefix)
		{
			UnitTestHelper.SetupBlog(subfolder, applicationPath);

			Entry entry = new Entry(PostType.BlogPost);
			entry.DateCreated = DateTime.ParseExact("2005/01/23", "yyyy/MM/dd", CultureInfo.InvariantCulture);
			entry.Title = "Some Title";
			entry.Body = "Some Body";
			int id = Entries.Create(entry);

			string expectedLink = string.Format("{0}/archive/2005/01/23/{1}.aspx", expectedUrlPrefix, id);
			string expectedFullyQualifiedLink = "http://" + Config.CurrentBlog.Host + expectedLink;

            Entry savedEntry = Entries.GetEntry(id, PostConfig.None, false);
			Assert.AreEqual(expectedLink, savedEntry.Url, "The link was not what we expected.");
			Assert.AreEqual(expectedFullyQualifiedLink, savedEntry.FullyQualifiedUrl.ToString(), "The link was not what we expected.");
		}
		
		[Test]
		[RollBack]
		public void EntryDateSyndicatedIsNullEquivalentUnlessPublished()
		{
			UnitTestHelper.SetupBlog();

			Entry entry = new Entry(PostType.BlogPost);
			
			entry.DateCreated = DateTime.Now;
			entry.Title = "Some Title";
			entry.Body = "Some Body";
			
			int id = Entries.Create(entry);
            Entry savedEntry = Entries.GetEntry(id, PostConfig.None, false);
			
			Assert.AreEqual(NullValue.NullDateTime, savedEntry.DateSyndicated, "DateSyndicated should be null since it was not syndicated.");
			
			savedEntry.IsActive = true;
			Thread.Sleep(1000);
			savedEntry.IncludeInMainSyndication = true;
			Entries.Update(savedEntry);

			//Convert to UTC before comparing
			DateTime utcSyndicatedDate = Config.CurrentBlog.TimeZone.ToUniversalTime(savedEntry.DateSyndicated);

			Assert.IsTrue(utcSyndicatedDate > entry.DateCreated.ToUniversalTime(), string.Format("DateSyndicated '{0}' should larger than date created '{1}'.", savedEntry.DateSyndicated, savedEntry.DateCreated));

            savedEntry = Entries.GetEntry(id, PostConfig.None, false);

			//Convert to UTC before comparing
			utcSyndicatedDate = Config.CurrentBlog.TimeZone.ToUniversalTime(savedEntry.DateSyndicated);

			Assert.IsTrue(utcSyndicatedDate > entry.DateCreated.ToUniversalTime(), string.Format("After reloading from DB, DateSyndicated '{0}' should larger than date created '{1}'.", savedEntry.DateSyndicated, savedEntry.DateCreated));
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
