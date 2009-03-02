using System;
using System.Web.Caching;
using MbUnit.Framework;
using Rhino.Mocks;
using Subtext.Extensibility;
using Subtext.Framework;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;
using Subtext.Framework.Exceptions;
using Subtext.Framework.Services;
using UnitTests.Subtext;
using Subtext.Framework.Security;
using Subtext.Framework.Web.HttpModules;
using Moq;
using Subtext.Framework.Data;

namespace UnitTests.Subtext.Framework.Components.CommentTests
{
	[TestFixture]
	public class SpamServiceTests
	{
		string _hostName = string.Empty;
		
		/// <summary>
		/// Make sure when we create feedback, that it calls the comment service 
		/// if enabled.
		/// </summary>
		[RowTest]
		[Row(true, false)]
		[Row(false, false)]
		[RollBack]
		public void FeedbackCreateCallsCommentService(bool isSpam, bool isAdmin)
		{
            Config.CreateBlog("", "username", "password", _hostName, string.Empty);
            BlogRequest.Current = new BlogRequest(_hostName, string.Empty, new Uri("http://example.com/"), false);
            Blog blog = Config.GetBlog(_hostName, string.Empty);
            BlogRequest.Current.Blog = blog;
            blog.DuplicateCommentsEnabled = true;
            blog.FeedbackSpamServiceKey = "my-secret-key";
            blog.ModerationEnabled = false;
			
            var service = new Mock<IFeedbackSpamService>();
            var cache = new Mock<ICache>();
            service.Setup(s => s.IsSpam(It.IsAny<FeedbackItem>())).Returns(isSpam);
			FeedbackItem feedback = new FeedbackItem(FeedbackType.Comment);
			feedback.Title = "blah";
			feedback.Body = UnitTestHelper.GenerateUniqueString();

			Assert.AreEqual(isAdmin, SecurityHelper.IsAdmin);

			try {
                FeedbackItem.Create(feedback, new CommentFilter(cache.Object, service.Object, blog), blog);
			}
			catch(BaseCommentException)
			{
			}
			Assert.AreEqual(!isSpam, feedback.Approved);
		}
		
		[SetUp]
		public void SetUp()
		{
			_hostName = UnitTestHelper.GenerateUniqueString();
			UnitTestHelper.SetHttpContextWithBlogRequest(_hostName, string.Empty);
		}

		[TearDown]
		public void TearDown()
		{
			Config.ConfigurationProvider = null;
		}
	}
}
