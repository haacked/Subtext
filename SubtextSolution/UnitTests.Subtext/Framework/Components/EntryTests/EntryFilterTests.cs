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
using System.Collections.Specialized;
using MbUnit.Framework;
using Moq;
using Subtext.Extensibility;
using Subtext.Framework;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;
using Subtext.Framework.Data;
using Subtext.Framework.Exceptions;
using Subtext.Framework.Security;
using Subtext.Framework.Web.HttpModules;
using System.Web.Caching;

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
			Config.CreateBlog("", "username", "password", _hostName, string.Empty);
            BlogRequest.Current.Blog = Config.GetBlog(_hostName, string.Empty);
			Blog blog = Config.CurrentBlog;
			blog.CommentDelayInMinutes = 1;
            FeedbackItem trackback = new FeedbackItem(FeedbackType.PingTrack);
			trackback.DateCreated = DateTime.Now;
			trackback.SourceUrl = new Uri("http://localhost/ThisUrl/");
			trackback.Title = "Some Title";
			trackback.Body = "Some Body Some Body";
            var cache = new TestCache();
            FeedbackItem.Create(trackback, new CommentFilter(cache, null, blog), blog);
			
			FeedbackItem comment = new FeedbackItem(FeedbackType.Comment);
			comment.DateCreated = DateTime.Now;
			comment.SourceUrl = new Uri("http://localhost/ThisUrl/");
			comment.Title = "Some Title";
			comment.Body = "Some Body Else";
            FeedbackItem.Create(comment, new CommentFilter(cache, null, blog), blog);
		}

		/// <summary>
		/// Make sure that duplicate comments are blocked.
		/// </summary>
		[Test]
		[RollBack]
		[ExpectedException(typeof(CommentDuplicateException))]
		public void CannotCreateDuplicateComments()
		{
			Config.CreateBlog("", "username", "password", _hostName, string.Empty);
            BlogRequest.Current.Blog = Config.GetBlog(_hostName, string.Empty);
			Blog blog = Config.CurrentBlog;
			blog.CommentDelayInMinutes = 0;
            var cache = new TestCache();
            
			FeedbackItem feedbackItem = new FeedbackItem(FeedbackType.Comment);
			feedbackItem.DateCreated = DateTime.Now;
			feedbackItem.SourceUrl = new Uri("http://localhost/ThisUrl/");
			feedbackItem.Title = "Some Title";
			feedbackItem.Body = "Some Body";
            FeedbackItem.Create(feedbackItem, new CommentFilter(cache, null, blog), blog);
            FeedbackItem.Create(feedbackItem, new CommentFilter(cache, null, blog), blog);
		}
	    
	    /// <summary>
	    /// Make sure that comments and Track/Pingbacks generated 
	    /// by the blog owner (logged in Administrator) don't get 
	    /// filtered.
	    /// </summary>
	    [Test]
	    [RollBack]
	    public void CommentsFromAdminNotFiltered()
	    {
            Config.CreateBlog("", "username", "some-password", _hostName, string.Empty);
            BlogRequest.Current.Blog = Config.GetBlog(_hostName, string.Empty);
            Blog blog = Config.CurrentBlog;
            blog.CommentDelayInMinutes = 0;
            var cache = new Mock<ICache>();
	        
	        /*
             * Need to add the authentication ticket to the context (cookie), and then 
             * read that ticket to set the HttpContext.Current.User's Principle.
             */
	        SecurityHelper.Authenticate("username", "some-password", true);
	        UnitTestHelper.AuthenticateFormsAuthenticationCookie();
	        Assert.IsTrue(SecurityHelper.IsAdmin, "Not able to login to the current blog.");

            FeedbackItem trackback = new FeedbackItem(FeedbackType.PingTrack);
            trackback.DateCreated = DateTime.Now;
            trackback.SourceUrl = new Uri("http://localhost/ThisUrl/");
            trackback.Title = "Some Title";
            trackback.Body = "Some Body";
            FeedbackItem.Create(trackback, new CommentFilter(cache.Object, null, blog), blog);
            FeedbackItem.Create(trackback, new CommentFilter(cache.Object, null, blog), blog);
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

		[SetUp]
		public void SetUp()
		{
			_hostName = UnitTestHelper.GenerateUniqueString();
			UnitTestHelper.SetHttpContextWithBlogRequest(_hostName, string.Empty);
            var cache = new Mock<ICache>();
			new CommentFilter(cache.Object, null, Config.CurrentBlog).ClearCommentCache();
		}

		[TearDown]
		public void TearDown()
		{
			Config.ConfigurationProvider = null;
		}
	}

    internal class TestCache : NameObjectCollectionBase, ICache
    {
        public object this[string key]
        {
            get
            {
                return BaseGet(key);
            }
            set
            {
                BaseSet(key, value);
            }
        }

        public void Insert(string key, object value, System.Web.Caching.CacheDependency dependency)
        {
            this[key] = value;
        }

        public void Insert(string key, object value, System.Web.Caching.CacheDependency dependency, DateTime absoluteExpiration, TimeSpan slidingExpiration)
        {
            this[key] = value;
        }

        public void Remove(string key)
        {
            BaseRemove(key);
        }

        public void Insert(string key, object value)
        {
            this[key] = value;
        }

        public void Insert(string key, object value, CacheDependency dependencies, DateTime absoluteExpiration, TimeSpan slidingExpiration, System.Web.Caching.CacheItemPriority priority, System.Web.Caching.CacheItemRemovedCallback onRemoveCallback)
        {
            this[key] = value;
        }
    }
}
