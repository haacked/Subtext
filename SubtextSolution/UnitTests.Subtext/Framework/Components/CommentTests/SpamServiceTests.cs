using System;
using System.Security.Principal;
using System.Threading;
using System.Web;
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

namespace UnitTests.Subtext.Framework.Components.CommentTests
{
	[TestFixture]
	public class SpamServiceTests
	{
		/// <summary>
		/// Make sure when we create feedback, that it calls the comment service 
		/// if enabled.
		/// </summary>
		[RowTest]
		[Row(true, false)]
		[Row(false, false)]
		[RollBack2]
		public void FeedbackCreateCallsCommentService(bool isSpam, bool isAdmin)
		{
			UnitTestHelper.SetupBlog();
			//Need to set our user to a non-admin
			HttpContext.Current.User = new GenericPrincipal(new GenericIdentity("NotAnAdmin"), new string[] { "Anonymous" });
			
			MockRepository mocks = new MockRepository();
			IFeedbackSpamService service = (IFeedbackSpamService)mocks.CreateMock(typeof(IFeedbackSpamService));
			Config.CurrentBlog.FeedbackSpamService = service;
			Config.CurrentBlog.DuplicateCommentsEnabled = true;
			Config.CurrentBlog.FeedbackSpamServiceKey = "my-secret-key";
			Config.CurrentBlog.ModerationEnabled = false;
			FeedbackItem feedback = new FeedbackItem(FeedbackType.Comment);
			Expect.Call(service.IsSpam(feedback)).Return(isSpam);
			feedback.Title = "blah";
			feedback.Body = UnitTestHelper.GenerateRandomString();
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
		
		[TearDown]
		public void TearDown()
		{
			Config.ConfigurationProvider = null;
		}
	}
}
