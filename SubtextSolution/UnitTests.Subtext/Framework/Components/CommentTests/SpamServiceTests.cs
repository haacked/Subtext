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
            BlogRequest.Current.Blog = Config.GetBlog(_hostName, string.Empty);

			MockRepository mocks = new MockRepository();
			IFeedbackSpamService service = (IFeedbackSpamService)mocks.CreateMock(typeof(IFeedbackSpamService));
			Config.CurrentBlog.FeedbackSpamService = service;
			Config.CurrentBlog.DuplicateCommentsEnabled = true;
			Config.CurrentBlog.FeedbackSpamServiceKey = "my-secret-key";
			Config.CurrentBlog.ModerationEnabled = false;
			FeedbackItem feedback = new FeedbackItem(FeedbackType.Comment);
			Expect.Call(service.IsSpam(feedback)).Return(isSpam);
			feedback.Title = "blah";
			feedback.Body = UnitTestHelper.GenerateUniqueString();
			mocks.ReplayAll();

			Assert.AreEqual(isAdmin, SecurityHelper.IsAdmin);

			try
			{
				FeedbackItem.Create(feedback, new CommentFilter(new Cache()));
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
