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
using System.Threading;
using MbUnit.Framework;
using Subtext.Extensibility;
using Subtext.Framework;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;
using Subtext.Framework.Exceptions;

namespace UnitTests.Subtext.Framework.Components.EntryTests
{
	/// <summary>
	/// Tests of the Entry creation filter. Applies to Trackbacks, PingBacks, 
	/// and Comments.
	/// </summary>
	[TestFixture]
	public class EntryFilterTests
	{
		string _hostName = string.Empty;

		/// <summary>
		/// If comment throttling is on, make sure we can't 
		/// create too many within the interval.
		/// </summary>
		[Test]
		[RollBack]
		[ExpectedException(typeof(CommentFrequencyException))]
		public void CannotCreateMoreThanOneCommentWithinDelay()
		{
			Assert.IsTrue(Config.CreateBlog("", "username", "password", _hostName, string.Empty));
			BlogInfo blog = Config.CurrentBlog;
			blog.CommentDelayInMinutes = 1;

			Entry entry = new Entry(PostType.PingTrack);
			entry.DateCreated = DateTime.Now;
			entry.SourceUrl = "http://localhost/ThisUrl/";
			entry.Title = "Some Title";
			entry.Body = "Some Body Some Body";
			Entries.Create(entry);
			
			Thread.Sleep(100);
			
			entry = new Entry(PostType.Comment);
			entry.DateCreated = DateTime.Now;
			entry.SourceUrl = "http://localhost/ThisUrl/";
			entry.Title = "Some Title";
			entry.Body = "Some Body Else";
			Entries.Create(entry);
		}

		/// <summary>
		/// Make sure that duplicate comments are blocked.
		/// </summary>
		[Test]
		[RollBack]
		[ExpectedException(typeof(CommentDuplicateException))]
		public void CannotCreateDuplicateComments()
		{
			Assert.IsTrue(Config.CreateBlog("", "username", "password", _hostName, string.Empty));
			BlogInfo blog = Config.CurrentBlog;
			blog.CommentDelayInMinutes = 0;

			Entry entry = new Entry(PostType.PingTrack);
			entry.DateCreated = DateTime.Now;
			entry.SourceUrl = "http://localhost/ThisUrl/";
			entry.Title = "Some Title";
			entry.Body = "Some Body";
			Entries.Create(entry);
			Entries.Create(entry);
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
