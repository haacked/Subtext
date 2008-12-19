using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Web;
using MbUnit.Framework;
using Subtext.Framework;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;
using Subtext.Framework.Tracking;

namespace UnitTests.Subtext.Framework.Tracking
{
	/// <summary>
	/// Summary description for TrackbackHandler.
	/// </summary>
	[TestFixture]
	public class TrackbackHandlerTests
	{
		[Test]
		[RollBack]
		public void CanDisableTrackbacks()
		{
			string url = "http://haacked.com/";
			string title = "The Title of the Trackback";
			string excerpt = "Blah blah blah.";
			string blogName = "You've been haacked";

			string hostname = UnitTestHelper.GenerateUniqueString();
			UnitTestHelper.SetHttpContextWithBlogRequest(hostname, string.Empty, string.Empty);
			Config.CreateBlog("", "username", "password", hostname, string.Empty);
			Entry entry = UnitTestHelper.CreateEntryInstanceForSyndication("phil", "title", "body");
			entry.DateCreated = entry.DateSyndicated = entry.DateModified = DateTime.ParseExact("2006/05/25", "yyyy/MM/dd", CultureInfo.InvariantCulture);
			int id = Entries.Create(entry);

			StringBuilder sb = new StringBuilder();
			TextWriter output = new StringWriter(sb);
			SimulatedHttpRequest request = UnitTestHelper.SetHttpContextWithBlogRequest(hostname, string.Empty, string.Empty, "/trackback/services/" + id + ".aspx", output, "GET");
			Config.CurrentBlog.TrackbacksEnabled = false;
			Config.UpdateConfigData(Config.CurrentBlog);
			string responseText = GetTrackBackHandlerResponseText(blogName, excerpt, request, sb, title, url, false);
			Assert.AreEqual(string.Empty, responseText);
		}
		
		/// <summary>
		/// Sends an RSS Snippet for requests made using the "GET" http verb.
		/// </summary>
		[Test]
		[RollBack]
		public void TrackBackHandlerSendsRssResponseForGetRequests()
		{
			string url = "http://haacked.com/";
			string title = "The Title of the Trackback";
			string excerpt = "Blah blah blah.";
			string blogName = "You've been haacked";
			
			string hostname = UnitTestHelper.GenerateUniqueString();
			UnitTestHelper.SetHttpContextWithBlogRequest(hostname, string.Empty, string.Empty);
			Config.CreateBlog("", "username", "password", hostname, string.Empty);
			
			Entry entry = UnitTestHelper.CreateEntryInstanceForSyndication("phil", "title", "body");
			entry.DateCreated = entry.DateSyndicated = entry.DateModified = DateTime.ParseExact("2006/05/25", "yyyy/MM/dd", CultureInfo.InvariantCulture);
			int id = Entries.Create(entry);
			
			StringBuilder sb = new StringBuilder();
			TextWriter output = new StringWriter(sb);
			SimulatedHttpRequest request = UnitTestHelper.SetHttpContextWithBlogRequest(hostname, string.Empty, string.Empty, "/trackback/services/" + id + ".aspx", output, "GET");
			string responseText = GetTrackBackHandlerResponseText(blogName, excerpt, request, sb, title, url);
			Assert.Greater(responseText.IndexOf(entry.Title), 0, "Did not find the entry title.");
		}
		
		/// <summary>
		/// Sends an error message if the id in the url does not match an existing entry.
		/// </summary>
		[Test]
		[RollBack]
		public void TrackBackHandlerSendsErrorIfEntryIdInUrlDoesNotMatchEntry()
		{
			string url = "http://haacked.com/blah/";
			string title = "The Title";
			string excerpt = "Blah blah blah blah blah.";
			string blogName = "You've been haacked";
			
			string hostname = UnitTestHelper.GenerateUniqueString();
			UnitTestHelper.SetHttpContextWithBlogRequest(hostname, string.Empty, string.Empty);
			Config.CreateBlog("", "username", "password", hostname, string.Empty);
			Config.CurrentBlog.DuplicateCommentsEnabled = true;
			Config.CurrentBlog.DuplicateCommentsEnabled = true;
			
			Entry entry = UnitTestHelper.CreateEntryInstanceForSyndication("phil", "title", "body");
			entry.DateCreated = entry.DateSyndicated = entry.DateModified = DateTime.ParseExact("2006/05/25", "yyyy/MM/dd", CultureInfo.InvariantCulture);
			Entries.Create(entry);
			
			StringBuilder sb = new StringBuilder();
			TextWriter output = new StringWriter(sb);
			SimulatedHttpRequest request = UnitTestHelper.SetHttpContextWithBlogRequest(hostname, string.Empty, string.Empty, "/trackback/services/" + int.MaxValue + ".aspx", output, "POST");
			string responseText = GetTrackBackHandlerResponseText(blogName, excerpt, request, sb, title, url);
            Assert.IsTrue(responseText.IndexOf("EntryID is invalid or missing") > 0, "Could not find the expected text.");
		}
		
		/// <summary>
		/// Checks the error message returned when the trackback URL does not have an entry id.
		/// </summary>
		[Test]
		[RollBack]
		public void TrackBackHandlerSendsErrorResponseForUrlWithoutEntryId()
		{
			string url = "http://haacked.com/blog/";
			string title = "Ha ha. Title of the Trackback";
			string excerpt = "Blah aoeu taonsteuh aonsteuh blah blah.";
			string blogName = "You've been haacked";
			
			string hostname = UnitTestHelper.GenerateUniqueString();
			UnitTestHelper.SetHttpContextWithBlogRequest(hostname, string.Empty, string.Empty);
			Config.CreateBlog("", "username", "password", hostname, string.Empty);
			Config.CurrentBlog.DuplicateCommentsEnabled = true;
			
			Entry entry = UnitTestHelper.CreateEntryInstanceForSyndication("phil", "title", "body");
			entry.DateCreated = entry.DateSyndicated = entry.DateModified = DateTime.ParseExact("2006/05/25", "yyyy/MM/dd", CultureInfo.InvariantCulture);
			Entries.Create(entry);
			
			StringBuilder sb = new StringBuilder();
			TextWriter output = new StringWriter(sb);
			SimulatedHttpRequest request = UnitTestHelper.SetHttpContextWithBlogRequest(hostname, string.Empty, string.Empty, "/trackback/services/SomethingNotANumber.aspx", output, "POST");
			string responseText = GetTrackBackHandlerResponseText(blogName, excerpt, request, sb, title, url);
			Assert.IsTrue(responseText.IndexOf("EntryID is invalid or missing") > 0, "Did not find the correct error message.");
		}
		
		/// <summary>
		/// Makes sure the HTTP handler used to handle trackbacks sends a 
		/// response with an error message when a badly formatted trackback is sent.
		/// </summary>
		[Test]
		[RollBack]
		public void TrackBackHandlerSendsErrorResponseForInvalidTrackbackUrl()
		{
			string url = "NOT_A_VALID_URL";
			string title = "The Title of the Trackback";
			string excerpt = "Blah blah blah.";
			string blogName = "You've been haacked";
			
			string hostname = UnitTestHelper.GenerateUniqueString();
			UnitTestHelper.SetHttpContextWithBlogRequest(hostname, string.Empty, string.Empty);
			Config.CreateBlog("", "username", "password", hostname, string.Empty);
			Config.CurrentBlog.DuplicateCommentsEnabled = true;
			
			Entry entry = UnitTestHelper.CreateEntryInstanceForSyndication("phil", "title", "body");
			entry.DateCreated = entry.DateSyndicated = entry.DateModified = DateTime.ParseExact("2006/05/25", "yyyy/MM/dd", CultureInfo.InvariantCulture);
			int id = Entries.Create(entry);
            ICollection<FeedbackItem> feedback = Entries.GetFeedBack(entry);
			Assert.AreEqual(0, feedback.Count, "Something is wrong if a freshly created entry has feedback.");
			
			string responseText = GetTrackBackHandlerResponseText(blogName, excerpt, hostname, string.Empty, id, title, url);
			
			Assert.IsTrue(responseText.IndexOf("no url parameter found, please try harder!") > 0, "Did not receive the correct error message.");

			ICollection<FeedbackItem> trackbacks = Entries.GetFeedBack(entry);
			Assert.AreEqual(0, trackbacks.Count, "We did not expect to see a trackback created.");
		}

		private static string GetTrackBackHandlerResponseText(string blogName, string excerpt, string hostname, string subfolder, int id, string title, string url)
		{	
			StringBuilder sb = new StringBuilder();
			TextWriter output = new StringWriter(sb);
			BlogInfo blog = Config.CurrentBlog;
			//the next line resets the httpcontext.
			SimulatedHttpRequest request = UnitTestHelper.SetHttpContextWithBlogRequest(hostname, subfolder, string.Empty, "/trackback/services/" + id + ".aspx", output, "POST");
			HttpContext.Current.Items["BlogInfo-"] = blog;
			
			return GetTrackBackHandlerResponseText(blogName, excerpt, request, sb, title, url);
		}

		private static string GetTrackBackHandlerResponseText(string blogName, string excerpt, SimulatedHttpRequest request, StringBuilder sb, string title, string url)
		{
			return GetTrackBackHandlerResponseText(blogName, excerpt, request, sb, title, url, true);
		}

		private static string GetTrackBackHandlerResponseText(string blogName, string excerpt, SimulatedHttpRequest request, StringBuilder sb, string title, string url, bool enabled)
		{
			request.Form["url"] = url;
			request.Form["title"] = title;
			request.Form["excerpt"] = excerpt;
			request.Form["blog_name"] = blogName;
			
			TrackBackHandler handler = new TrackBackHandler();
			handler.SourceVerification += handler_SourceVerification;
			Config.CurrentBlog.TrackbacksEnabled = enabled;
			
			HttpContext.Current.Request.ContentType = "application/x-www-form-urlencoded";
			handler.ProcessRequest(HttpContext.Current);
			HttpContext.Current.Response.Flush();
			return sb.ToString();
		}

		/// <summary>
		/// Makes sure the HTTP handler used to handle trackbacks handles a proper trackback request 
		/// by creating a trackback record in the local system.
		/// </summary>
		[Test]
		[RollBack]
		public void TrackBackPostCreatesProperTrackback()
		{
			string url = "http://haacked.com/";
			string title = "This is the Title of the Trackback" + UnitTestHelper.GenerateUniqueString();
			string excerpt = "Blah blah blah blah blah." + UnitTestHelper.GenerateUniqueString();
			string blogName = "You've been haacked";
			
			string hostname = UnitTestHelper.GenerateUniqueString();
			UnitTestHelper.SetHttpContextWithBlogRequest(hostname, "blog", string.Empty);
			Config.CreateBlog("Some Title", "username", "password", hostname, "blog");
			Config.CurrentBlog.DuplicateCommentsEnabled = true;
			
			
			Entry entry = UnitTestHelper.CreateEntryInstanceForSyndication("phil", "title", "body");
			entry.DateCreated = entry.DateSyndicated = entry.DateModified = DateTime.ParseExact("2006/05/25", "yyyy/MM/dd", CultureInfo.InvariantCulture);
			int id = Entries.Create(entry);
			ICollection<FeedbackItem> feedback = Entries.GetFeedBack(entry);
			Assert.AreEqual(0, feedback.Count, "Something is wrong if a freshly created entry has feedback.");
			
			string responseText = GetTrackBackHandlerResponseText(blogName, excerpt, hostname, "blog", id, title, url);

            Assert.AreEqual(string.Empty, responseText, "Did not expect any error response.");

			ICollection<FeedbackItem> trackbacks = Entries.GetFeedBack(entry);
			Assert.AreEqual(1, trackbacks.Count, "We expect to see the one feedback we just created.");

			FeedbackItem trackback = null;
			foreach (FeedbackItem tb in trackbacks)
		    {
		        trackback = tb;
		        break;
		    }

			if (trackback == null)
			{
				Assert.Fail("Trackback is null.");
				return;
			}
			Assert.AreEqual(title, trackback.Title, "Somehow the title of the feedback doesn't match.");
		}

		private static void handler_SourceVerification(object sender, SourceVerificationEventArgs e)
		{
			e.Verified = true;
		}
	}
}
