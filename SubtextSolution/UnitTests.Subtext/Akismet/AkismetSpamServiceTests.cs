using System;
using System.Net;
using MbUnit.Framework;
using Rhino.Mocks;
using Subtext.Akismet;
using Subtext.Extensibility;
using Subtext.Framework;
using Subtext.Framework.Components;
using Subtext.Framework.Services;

namespace UnitTests.Subtext.Akismet
{
	[TestFixture]
	public class AkismetSpamServiceTests
	{
		[Test]
		public void CanInstantiate()
		{
			MockRepository mocks = new MockRepository();
			IBlogInfo blog = mocks.CreateMock<IBlogInfo>();
			Uri rootUrl = new Uri("http://localhost:2072");
			Expect.Call(blog.RootUrl).Return(rootUrl);
			mocks.ReplayAll();

			AkismetSpamService service = new AkismetSpamService("api-key", blog);
			Assert.IsNotNull(service);
		}

		[Test]
		public void CanVerifyApiKey()
		{
			MockRepository mocks = new MockRepository();
			IAkismetClient akismetClient = mocks.CreateMock<IAkismetClient>();
			Expect.Call(akismetClient.VerifyApiKey()).Return(true);
			Expect.Call(akismetClient.Proxy).PropertyBehavior();
			mocks.ReplayAll();

			AkismetSpamService service = new AkismetSpamService(akismetClient);
			Assert.IsTrue(service.VerifyApiKey());
		}

		[Test]
		public void VerifyApiKeyReturnsFalseWhenWebExceptionThrown()
		{
			MockRepository mocks = new MockRepository();
			IAkismetClient akismetClient = mocks.CreateMock<IAkismetClient>();
			Expect.Call(akismetClient.VerifyApiKey()).Throw(new WebException());
			Expect.Call(akismetClient.Proxy).PropertyBehavior();
			mocks.ReplayAll();

			AkismetSpamService service = new AkismetSpamService(akismetClient);
			Assert.IsFalse(service.VerifyApiKey());
		}

		[Test]
        [RollBack]
		public void IsSpamReturnsTrueForSpam()
		{
		    UnitTestHelper.SetupBlog();

			MockRepository mocks = new MockRepository();
			IAkismetClient akismetClient = mocks.CreateMock<IAkismetClient>();
			IComment comment = mocks.CreateMock<IComment>();
			Expect.Call(akismetClient.Proxy).PropertyBehavior();
			Expect.Call(akismetClient.CheckCommentForSpam(comment)).Return(true).IgnoreArguments();
			akismetClient.SubmitSpam(null);
			LastCall.IgnoreArguments();
			mocks.ReplayAll();

			AkismetSpamService service = new AkismetSpamService(akismetClient);
			Assert.IsTrue(service.IsSpam(new FeedbackItem(FeedbackType.PingTrack)));
		}

		[Test]
        [RollBack]
		public void IsSpamReturnsFalseIfAkismetClientDoesSo()
		{
            UnitTestHelper.SetupBlog();

			MockRepository mocks = new MockRepository();
			IAkismetClient akismetClient = mocks.CreateMock<IAkismetClient>();
			IComment comment = mocks.CreateMock<IComment>();
			Expect.Call(akismetClient.Proxy).PropertyBehavior();
			Expect.Call(akismetClient.CheckCommentForSpam(comment)).Return(false).IgnoreArguments();
			mocks.ReplayAll();

			AkismetSpamService service = new AkismetSpamService(akismetClient);
			Assert.IsFalse(service.IsSpam(new FeedbackItem(FeedbackType.PingTrack)));
		}

		[Test]
        [RollBack]
		public void IsSpamReturnsFalseIfAkismetClientThrowsInvalidResponseException()
		{
            UnitTestHelper.SetupBlog();

			MockRepository mocks = new MockRepository();
			IAkismetClient akismetClient = mocks.CreateMock<IAkismetClient>();
			IComment comment = mocks.CreateMock<IComment>();
			Expect.Call(akismetClient.Proxy).PropertyBehavior();
			Expect.Call(akismetClient.CheckCommentForSpam(comment)).Throw(new InvalidResponseException()).IgnoreArguments();
			mocks.ReplayAll();

			AkismetSpamService service = new AkismetSpamService(akismetClient);
			Assert.IsFalse(service.IsSpam(new FeedbackItem(FeedbackType.PingTrack)));
		}

		[Test]
        [RollBack]
		public void CanSubmitSpam()
		{
            UnitTestHelper.SetupBlog();

			MockRepository mocks = new MockRepository();
			IAkismetClient akismetClient = mocks.CreateMock<IAkismetClient>();
			IComment comment = mocks.CreateMock<IComment>();
			Expect.Call(akismetClient.Proxy).PropertyBehavior();
			akismetClient.SubmitSpam(comment);
			LastCall.IgnoreArguments();
			mocks.ReplayAll();

			AkismetSpamService service = new AkismetSpamService(akismetClient);
			service.SubmitSpam(new FeedbackItem(FeedbackType.Comment));
			mocks.VerifyAll();
		}

		[Test]
        [RollBack]
		public void CanSubmitGoodFeedback()
		{
            UnitTestHelper.SetupBlog();

			MockRepository mocks = new MockRepository();
			IAkismetClient akismetClient = mocks.CreateMock<IAkismetClient>();
			IComment comment = mocks.CreateMock<IComment>();
			Expect.Call(akismetClient.Proxy).PropertyBehavior();
			akismetClient.SubmitHam(comment);
			LastCall.IgnoreArguments();
			mocks.ReplayAll();

			AkismetSpamService service = new AkismetSpamService(akismetClient);
			service.SubmitGoodFeedback(new FeedbackItem(FeedbackType.Comment));
			mocks.VerifyAll();
		}
	}
}
