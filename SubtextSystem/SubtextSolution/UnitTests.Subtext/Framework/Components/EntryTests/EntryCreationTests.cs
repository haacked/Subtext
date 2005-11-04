using System;
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
		/// Makes sure that the content checksum hash is being created correctly.
		/// </summary>
		[Test]
		[RollBack]
		public void EntryCreateHasContentHash()
		{
			Assert.IsTrue(Config.CreateBlog("", "username", "password", _hostName, string.Empty));

			Entry entry = new Entry(PostType.PingTrack);
			entry.DateCreated = DateTime.Now;
			entry.SourceUrl = "http://localhost/ThisUrl/";
			entry.Title = "Some Title";
			entry.Body = "Some Body";
			int id = Entries.Create(entry);

			Entry savedEntry = Entries.GetEntry(id, EntryGetOption.All);
			Assert.IsTrue(savedEntry.ContentChecksumHash.Length > 0, "The Content Checksum should be larger than 0.");

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
		}

		[TearDown]
		public void TearDown()
		{
			Config.ConfigurationProvider = null;
		}
	}
}
