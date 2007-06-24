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
using System.Security.Principal;
using System.Threading;
using System.Web;
using System.Web.Security;
using MbUnit.Framework;
using Subtext.Extensibility;
using Subtext.Framework;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;
using Subtext.Framework.Exceptions;
using Subtext.Framework.Security;

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
		[RollBack2]
		[ExpectedException(typeof(CommentFrequencyException))]
		public void CannotCreateMoreThanOneCommentWithinDelay()
		{
			UnitTestHelper.SetupBlog();
			//Need to set our user to a non-admin
			HttpContext.Current.User = new GenericPrincipal(new GenericIdentity("NotAnAdmin"), new string[] { "Anonymous" });
			
			BlogInfo blog = Config.CurrentBlog;
			blog.CommentDelayInMinutes = 1;

			FeedbackItem trackback = new FeedbackItem(FeedbackType.PingTrack);
			trackback.DateCreated = DateTime.Now;
			trackback.SourceUrl = new Uri("http://localhost/ThisUrl/");
			trackback.Title = "Some Title";
			trackback.Body = "Some Body Some Body";
			FeedbackItem.Create(trackback, new CommentFilter(HttpContext.Current.Cache));
			
			Thread.Sleep(100);

			FeedbackItem comment = new FeedbackItem(FeedbackType.Comment);
			comment.DateCreated = DateTime.Now;
			comment.SourceUrl = new Uri("http://localhost/ThisUrl/");
			comment.Title = "Some Title";
			comment.Body = "Some Body Else";
			FeedbackItem.Create(comment, new CommentFilter(HttpContext.Current.Cache));
		}

		/// <summary>
		/// Make sure that duplicate comments are blocked.
		/// </summary>
		[Test]
		[RollBack2]
		[ExpectedException(typeof(CommentDuplicateException))]
		public void CannotCreateDuplicateComments()
		{
			UnitTestHelper.SetupBlog();
			
			//Need to set our user to a non-admin
			HttpContext.Current.User = new GenericPrincipal(new GenericIdentity("NotAnAdmin"), new string[] { "Anonymous" });
			
			BlogInfo blog = Config.CurrentBlog;
			blog.CommentDelayInMinutes = 0;

			FeedbackItem feedbackItem = new FeedbackItem(FeedbackType.Comment);
			feedbackItem.DateCreated = DateTime.Now;
			feedbackItem.SourceUrl = new Uri("http://localhost/ThisUrl/");
			feedbackItem.Title = "Some Title";
			feedbackItem.Body = "Some Body";
			FeedbackItem.Create(feedbackItem, new CommentFilter(HttpContext.Current.Cache));
			FeedbackItem.Create(feedbackItem, new CommentFilter(HttpContext.Current.Cache));
		}
	    
	    /// <summary>
	    /// Make sure that comments and Track/Pingbacks generated 
	    /// by the blog owner (logged in Administrator) don't get 
	    /// filtered.
	    /// </summary>
	    [Test]
	    [RollBack2]
	    public void CommentsFromAdminNotFiltered()
	    {
			UnitTestHelper.SetupBlog();
            BlogInfo blog = Config.CurrentBlog;
			Config.UpdateConfigData(blog);

            blog.CommentDelayInMinutes = 0;
	        /*
             * Need to add the authentication ticket to the context (cookie), and then 
             * read that ticket to set the HttpContext.Current.User's Principle.
             */
			Membership.ValidateUser(UnitTestHelper.MembershipTestUsername, UnitTestHelper.MembershipTestPassword);
	        UnitTestHelper.AuthenticateFormsAuthenticationCookie();
	        Assert.IsTrue(SecurityHelper.IsAdmin, "Not able to login to the current blog.");

            FeedbackItem trackback = new FeedbackItem(FeedbackType.PingTrack);
            trackback.DateCreated = DateTime.Now;
            trackback.SourceUrl = new Uri("http://localhost/ThisUrl/");
            trackback.Title = "Some Title";
            trackback.Body = "Some Body";
			FeedbackItem.Create(trackback, new CommentFilter(HttpContext.Current.Cache));
			FeedbackItem.Create(trackback, new CommentFilter(HttpContext.Current.Cache));
	    }

		[Test]
		[ExpectedArgumentNullException]
		public void FilterAfterPersistThrowsArgumentNullException()
		{
			CommentFilter.FilterAfterPersist(null);
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
		
		private IDisposable blogRequest;
		[SetUp]
		public void SetUp()
		{
			_hostName = UnitTestHelper.GenerateRandomString();
			blogRequest = BlogRequestSimulator.SimulateRequest(_hostName, "", "MyBlog");
			new CommentFilter(HttpContext.Current.Cache).ClearCommentCache();
		}

		[TearDown]
		public void TearDown()
		{
			Config.ConfigurationProvider = null;
			if (blogRequest != null)
				blogRequest.Dispose();
		}
	}
}
