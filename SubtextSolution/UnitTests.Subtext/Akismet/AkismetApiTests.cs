using System;
using System.Collections.Specialized;
using System.Net;
using System.Web;
using MbUnit.Framework;
using Rhino.Mocks;
using Subtext.Akismet;
using System.IO;
using System.Text;
using System.Runtime.Serialization;
using System.Reflection;

namespace UnitTests.Subtext.Akismet
{
	[TestFixture]
	public class AkismetApiTests
	{
		#region Exception Tests
		[RowTest]
		[Row(null, true, "http://haacked.com/shameless/self/promotion", ExpectedException = typeof(ArgumentNullException))]
		[Row("fake-key", true, null, ExpectedException = typeof(ArgumentNullException))]
		[Row("fake-key", false, "http://haacked.com/shameless/really/", ExpectedException = typeof(ArgumentNullException))]
		public void AkismetClientConstructorThrowsAppropriateExceptions(string apikey, bool createHttpClient, string blogUrl)
		{
			HttpClient httpClient = createHttpClient ? new HttpClient() : null;
			Uri blogUri = null;
			if(!String.IsNullOrEmpty(blogUrl))
				blogUri = new Uri(blogUrl);

			new AkismetClient(apikey, blogUri, httpClient);
		}
		
		[Test]
		[ExpectedArgumentNullException]
		public void CheckCommentThrowsArgumentNullException()
		{
			AkismetClient client = new AkismetClient("fake-key", new Uri("http://haacked.com/"), null);
			client.CheckCommentForSpam(null);
		}
		#endregion

        [Test]
        public void CanSetAndGetCommentProperties()
        {
            Comment comment = new Comment(IPAddress.Loopback, "Test");
            UnitTestHelper.AssertSimpleProperties(comment);
            Assert.AreEqual(IPAddress.Loopback, comment.IpAddress);
            Assert.AreEqual("Test", comment.UserAgent);
            comment.ServerEnvironmentVariables.Add("SomeVar", "SomeVal");
            Assert.AreEqual(1, comment.ServerEnvironmentVariables.Count);
        }

		[Test]
		public void ConstructorSetsApiKeyAndUrl()
		{
			AkismetClient client = new AkismetClient("fake-key", new Uri("http://haacked.com/"), new HttpClient());
			Assert.AreEqual(new Uri("http://haacked.com/"), client.BlogUrl);
			Assert.AreEqual("fake-key", client.ApiKey);
            UnitTestHelper.AssertSimpleProperties(client, "ApiKey");
		}

		[Test]
		public void CanVerifyApiKey()
		{
			string userAgent = GetExpectedUserAgent();
			Uri verifyUrl = new Uri("http://rest.akismet.com/1.1/verify-key");
			string parameters = "key=" + HttpUtility.UrlEncode("fake-key") + "&blog=" + HttpUtility.UrlEncode("http://haacked.com/");
			
			MockRepository mocks = new MockRepository();
			HttpClient httpClient = (HttpClient)mocks.CreateMock(typeof(HttpClient));
			Expect.Call(httpClient.PostRequest(verifyUrl, userAgent, 5000, parameters)).Return("valid");
			mocks.ReplayAll();	
			
			AkismetClient client = new AkismetClient("fake-key", new Uri("http://haacked.com/"), httpClient);
			Assert.IsTrue(client.VerifyApiKey(), "If the request returns 'valid' we should return true.");
			
			mocks.VerifyAll();
		}

		[Test]
		public void CanVerifyApiKeyIsWrong()
		{
			string userAgent = GetExpectedUserAgent();
			Uri verifyUrl = new Uri("http://rest.akismet.com/1.1/verify-key");
			string parameters = "key=" + HttpUtility.UrlEncode("wrong-key") + "&blog=" + HttpUtility.UrlEncode("http://haacked.com/");

			MockRepository mocks = new MockRepository();
			HttpClient httpClient = (HttpClient)mocks.CreateMock(typeof(HttpClient));
			Expect.Call(httpClient.PostRequest(verifyUrl, userAgent, 5000, parameters)).Return("invalid");
			mocks.ReplayAll();

			AkismetClient client = new AkismetClient("wrong-key", new Uri("http://haacked.com/"), httpClient);
			Assert.IsFalse(client.VerifyApiKey(), "If the request returns 'invalid' then we should return false!");

			mocks.VerifyAll();
		}
		
		[Test]
		public void CanCheckCommentForSpam()
		{
			string userAgent = GetExpectedUserAgent();
			Uri checkUrl = new Uri("http://myapikey.rest.akismet.com/1.1/comment-check");
			string parameters = "blog=" + HttpUtility.UrlEncode("http://haacked.com/")
								+ "&user_ip=10.0.0.1"
								+ "&user_agent=" + HttpUtility.UrlEncode("Mozilla (My Silly Browser)")
								+ "&referer=" + HttpUtility.UrlEncode("http://example.com/none-of-your-business/")
								+ "&permalink=" + HttpUtility.UrlEncode("http://example.com/i-am-right-you-are-wrong/")
								+ "&comment_type=comment"
								+ "&comment_author=Your+Mama"
								+ "&comment_author_email=" + HttpUtility.UrlEncode("nobody@example.com")
								+ "&comment_author_url=" + HttpUtility.UrlEncode("http://mysite.example.com/foo/")
								+ "&comment_content=" + HttpUtility.UrlEncode("This is my rifle. There are many like it, but this one is MINE.");

			MockRepository mocks = new MockRepository();
			HttpClient httpClient = (HttpClient)mocks.CreateMock(typeof(HttpClient));
			IComment comment = (IComment)mocks.CreateMock(typeof(IComment));

			SetupCallsAnComment(comment
			                    , "Your Mama"
			                    , "nobody@example.com"
			                    , IPAddress.Parse("10.0.0.1")
			                    , "Mozilla (My Silly Browser)"
			                    , "http://example.com/none-of-your-business/"
								, new Uri("http://example.com/i-am-right-you-are-wrong/")
								, "comment"
			                    , new Uri("http://mysite.example.com/foo/")
			                    , "This is my rifle. There are many like it, but this one is MINE."
								, null);

			Expect.Call(httpClient.PostRequest(checkUrl, userAgent, 5000, parameters)).Return("true");
			mocks.ReplayAll();

			AkismetClient client = new AkismetClient("myapikey", new Uri("http://haacked.com/"), httpClient);
			Assert.IsTrue(client.CheckCommentForSpam(comment), "If the request returns 'false' then we should return false!");

			mocks.VerifyAll();
		}

		[Test]
		public void CanCheckCommentForSpamWithoutOptionalParams()
		{
			string userAgent = GetExpectedUserAgent();
			Uri checkUrl = new Uri("http://myapikey.rest.akismet.com/1.1/comment-check");
			string parameters = "blog=" + HttpUtility.UrlEncode("http://haacked.com/")
			                    + "&user_ip=192.168.200.201"
			                    + "&user_agent=" + HttpUtility.UrlEncode("Mozilla (My Silly Browser)");

			MockRepository mocks = new MockRepository();
			HttpClient httpClient = (HttpClient)mocks.CreateMock(typeof(HttpClient));
			IComment comment = (IComment)mocks.CreateMock(typeof(IComment));
			
			//We'll try a mix of nulls and empty strings.
			SetupCallsAnComment(comment
								, string.Empty
								, string.Empty
								, IPAddress.Parse("192.168.200.201")
								, "Mozilla (My Silly Browser)"
								, null
								, null
								, null
								, null
								, string.Empty
								, null);
			
			Expect.Call(httpClient.PostRequest(checkUrl, userAgent, 5000, parameters)).Return("true");
			mocks.ReplayAll();

			AkismetClient client = new AkismetClient("myapikey", new Uri("http://haacked.com/"), httpClient);
			Assert.IsTrue(client.CheckCommentForSpam(comment), "If the request returns 'false' then we should return false!");

			mocks.VerifyAll();
		}

		[Test]
		public void CanCheckCommentWithArbitraryServerParams()
		{
			string userAgent = GetExpectedUserAgent();
			Uri checkUrl = new Uri("http://myapikey.rest.akismet.com/1.1/comment-check");
			string parameters = "blog=" + HttpUtility.UrlEncode("http://haacked.com/")
								+ "&user_ip=192.168.200.201"
								+ "&user_agent=" + HttpUtility.UrlEncode("Mozilla (My Silly Browser)")
								+ "&Making=" + HttpUtility.UrlEncode("This-Stuff")
								+ "&Up=" + HttpUtility.UrlEncode("As I-Go-Along");

			MockRepository mocks = new MockRepository();
			HttpClient httpClient = (HttpClient)mocks.CreateMock(typeof(HttpClient));
			IComment comment = (IComment)mocks.CreateMock(typeof(IComment));

			NameValueCollection extendedProps = new NameValueCollection();
			
			extendedProps.Add("Making", "This-Stuff");
			extendedProps.Add("Up", "As I-Go-Along");
			
			//We'll try a mix of nulls and empty strings.
			SetupCallsAnComment(comment
								, string.Empty
								, string.Empty
								, IPAddress.Parse("192.168.200.201")
								, "Mozilla (My Silly Browser)"
								, null
								, null
								, null
								, null
								, string.Empty
								, extendedProps);
			
			
			Expect.Call(httpClient.PostRequest(checkUrl, userAgent, 5000, parameters)).Return("false");
			mocks.ReplayAll();

			AkismetClient client = new AkismetClient("myapikey", new Uri("http://haacked.com/"), httpClient);
			Assert.IsFalse(client.CheckCommentForSpam(comment), "If the request returns 'false' then we should return false!");

			mocks.VerifyAll();
		}

		[RowTest]
		[Row("submit-ham", true)]
		[Row("submit-spam", false)]
		public void SubmitHamTest(string urlEnding, bool isHam)
		{
			string userAgent = GetExpectedUserAgent();
			Uri checkUrl = new Uri("http://myapikey.rest.akismet.com/1.1/" + urlEnding);
			string parameters = "blog=" + HttpUtility.UrlEncode("http://haacked.com/")
								+ "&user_ip=192.168.200.201"
								+ "&user_agent=" + HttpUtility.UrlEncode("Mozilla (My Silly Browser)");

			MockRepository mocks = new MockRepository();
			HttpClient httpClient = (HttpClient)mocks.CreateMock(typeof(HttpClient));
			IComment comment = (IComment)mocks.CreateMock(typeof(IComment));

			//We'll try a mix of nulls and empty strings.
			SetupCallsAnComment(comment
								, string.Empty
								, string.Empty
								, IPAddress.Parse("192.168.200.201")
								, "Mozilla (My Silly Browser)"
								, null
								, null
								, null
								, null
								, string.Empty
								, null);

			Expect.Call(httpClient.PostRequest(checkUrl, userAgent, 5000, parameters)).Return(string.Empty);
			mocks.ReplayAll();

			AkismetClient client = new AkismetClient("myapikey", new Uri("http://haacked.com/"), httpClient);
			if (isHam)
				client.SubmitHam(comment);
			else
				client.SubmitSpam(comment);

			mocks.VerifyAll();
		}

		/// <summary>
		/// The comment check call returns "invalid" if the api key is not valid 
		/// on the URL. The apikey must be the first part of the url.
		/// </summary>
		[Test]
		[ExpectedException(typeof(InvalidResponseException))]
		public void ThrowsInvalidResponseWhenApiKeyInvalid()
		{
			string userAgent = GetExpectedUserAgent();
			Uri checkUrl = new Uri("http://myapikey.rest.akismet.com/1.1/comment-check");
			string parameters = "blog=" + HttpUtility.UrlEncode("http://haacked.com/")
								+ "&user_ip=192.168.200.201"
								+ "&user_agent=" + HttpUtility.UrlEncode("Mozilla (My Silly Browser)");

			MockRepository mocks = new MockRepository();
			HttpClient httpClient = (HttpClient)mocks.CreateMock(typeof(HttpClient));
			IComment comment = (IComment)mocks.CreateMock(typeof(IComment));

			//We'll try a mix of nulls and empty strings.
			SetupCallsAnComment(comment
								, string.Empty
								, string.Empty
								, IPAddress.Parse("192.168.200.201")
								, "Mozilla (My Silly Browser)"
								, null
								, null
								, null
								, null
								, string.Empty
								, null);

			Expect.Call(httpClient.PostRequest(checkUrl, userAgent, 5000, parameters)).Return("invalid");
			mocks.ReplayAll();

			AkismetClient client = new AkismetClient("myapikey", new Uri("http://haacked.com/"), httpClient);
			Assert.IsTrue(client.CheckCommentForSpam(comment), "If the request returns 'false' then we should return false!");
		}

		private static void SetupCallsAnComment(IComment comment, string author, string email, IPAddress ip, string userAgent, string referer, Uri permalink, string commentType, Uri authorUrl, string content, NameValueCollection extendedProperties)
		{
			Expect.Call(comment.Author).Return(author).Repeat.Any();
			Expect.Call(comment.AuthorEmail).Return(email).Repeat.Any();
			Expect.Call(comment.IpAddress).Return(ip).Repeat.Any();
			Expect.Call(comment.UserAgent).Return(userAgent).Repeat.Any();
			Expect.Call(comment.Referer).Return(referer).Repeat.Any();
			Expect.Call(comment.Permalink).Return(permalink).Repeat.Any();
			Expect.Call(comment.CommentType).Return(commentType).Repeat.Any();
			Expect.Call(comment.AuthorUrl).Return(authorUrl).Repeat.Any();
			Expect.Call(comment.Content).Return(content).Repeat.Any();
			Expect.Call(comment.ServerEnvironmentVariables).Return(extendedProperties).Repeat.Any();
		}

		static string GetExpectedUserAgent()
		{
			return string.Format("Subtext/{0} | Akismet/1.11", typeof(HttpClient).Assembly.GetName().Version.ToString());
		}
	}
}
