using System;
using System.Collections.Specialized;
using System.Globalization;
using System.Net;
using System.Web;
using MbUnit.Framework;
using Moq;
using Subtext.Akismet;

namespace UnitTests.Subtext.Akismet
{
    [TestFixture]
    public class AkismetApiTests
    {
        [Test]
        public void Ctor_WithNullApiKey_ThrowsArgumentNullException()
        {
            // act, assert
            UnitTestHelper.AssertThrowsArgumentNullException(() => new AkismetClient(null, new Uri("http://example.com/"), new HttpClient()));
        }

        [Test]
        public void Ctor_WithNullBlogUrl_ThrowsArgumentNullException()
        {
            // act, assert
            UnitTestHelper.AssertThrowsArgumentNullException(() => new AkismetClient("api-key", null, new HttpClient()));
        }

        [Test]
        public void Ctor_WithNullHttpClient_ThrowsArgumentNullException()
        {
            // act, assert
            UnitTestHelper.AssertThrowsArgumentNullException(() => new AkismetClient("api-key", new Uri("http://example.com/"), null));
        }

        [Test]
        public void CheckCommentForSpam_WithNullComment_ThrowsArgumentNullException()
        {
            // arrange
            var client = new AkismetClient("fake-key", new Uri("http://haacked.com/"), new HttpClient());
            
            // act, assert
            UnitTestHelper.AssertThrowsArgumentNullException(() => client.CheckCommentForSpam(null));
        }

        [Test]
        public void CanSetAndGetCommentProperties()
        {
            var comment = new Comment(IPAddress.Loopback, "Test");
            Assert.AreEqual("Test", comment.UserAgent);
            Assert.AreEqual(IPAddress.Loopback, comment.IPAddress);

            UnitTestHelper.AssertSimpleProperties(comment);

            comment.ServerEnvironmentVariables.Add("SomeVar", "SomeVal");
            Assert.AreEqual(1, comment.ServerEnvironmentVariables.Count);
        }

        [Test]
        public void ConstructorSetsApiKeyAndUrl()
        {
            var client = new AkismetClient("fake-key", new Uri("http://haacked.com/"), new HttpClient());
            Assert.AreEqual(new Uri("http://haacked.com/"), client.BlogUrl);
            Assert.AreEqual("fake-key", client.ApiKey);
            UnitTestHelper.AssertSimpleProperties(client, "ApiKey");
        }

        [Test]
        public void CanVerifyApiKey()
        {
            //arrange
            string userAgent = GetExpectedUserAgent();
            var verifyUrl = new Uri("http://rest.akismet.com/1.1/verify-key");
            string parameters = "key=" + HttpUtility.UrlEncode("fake-key") + "&blog=" +
                                HttpUtility.UrlEncode("http://haacked.com/");
            var httpClient = new Mock<HttpClient>();
            httpClient.Setup(hc => hc.PostRequest(verifyUrl, userAgent, 5000, parameters, null)).Returns("valid");
            var client = new AkismetClient("fake-key", new Uri("http://haacked.com/"), httpClient.Object);

            //act
            bool isVerified = client.VerifyApiKey();

            //assert
            Assert.IsTrue(isVerified, "If the request returns 'valid' we should return true.");
        }

        [Test]
        public void CanVerifyApiKeyIsWrong()
        {
            //act
            string userAgent = GetExpectedUserAgent();
            var verifyUrl = new Uri("http://rest.akismet.com/1.1/verify-key");
            string parameters = "key=" + HttpUtility.UrlEncode("wrong-key") + "&blog=" +
                                HttpUtility.UrlEncode("http://haacked.com/");
            var httpClient = new Mock<HttpClient>();
            httpClient.Setup(hc => hc.PostRequest(verifyUrl, userAgent, 5000, parameters, null)).Returns("invalid");
            var client = new AkismetClient("wrong-key", new Uri("http://haacked.com/"), httpClient.Object);

            //act
            bool isVerified = client.VerifyApiKey();

            //assert
            Assert.IsFalse(isVerified, "If the request returns 'invalid' then we should return false!");
        }

        [Test]
        public void CanCheckCommentForSpam()
        {
            string userAgent = GetExpectedUserAgent();
            var checkUrl = new Uri("http://myapikey.rest.akismet.com/1.1/comment-check");
            string parameters = "blog=" + HttpUtility.UrlEncode("http://haacked.com/")
                                + "&user_ip=10.0.0.1"
                                + "&user_agent=" + HttpUtility.UrlEncode("Mozilla (My Silly Browser)")
                                + "&referer=" + HttpUtility.UrlEncode("http://example.com/none-of-your-business/")
                                + "&permalink=" + HttpUtility.UrlEncode("http://example.com/i-am-right-you-are-wrong/")
                                + "&comment_type=comment"
                                + "&comment_author=Your+Mama"
                                + "&comment_author_email=" + HttpUtility.UrlEncode("nobody@example.com")
                                + "&comment_author_url=" + HttpUtility.UrlEncode("http://mysite.example.com/foo/")
                                + "&comment_content=" +
                                HttpUtility.UrlEncode("This is my rifle. There are many like it, but this one is MINE.");

            var httpClient = new Mock<HttpClient>();
            var comment = new Mock<IComment>();

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

            httpClient.Setup(hc => hc.PostRequest(checkUrl, userAgent, 5000, parameters)).Returns("true");

            var client = new AkismetClient("myapikey", new Uri("http://haacked.com/"), httpClient.Object);
            Assert.IsTrue(client.CheckCommentForSpam(comment.Object),
                          "If the request returns 'false' then we should return false!");
        }

        [Test]
        public void CanCheckCommentForSpamWithoutOptionalParams()
        {
            string userAgent = GetExpectedUserAgent();
            var checkUrl = new Uri("http://myapikey.rest.akismet.com/1.1/comment-check");
            string parameters = "blog=" + HttpUtility.UrlEncode("http://haacked.com/")
                                + "&user_ip=192.168.200.201"
                                + "&user_agent=" + HttpUtility.UrlEncode("Mozilla (My Silly Browser)");


            var httpClient = new Mock<HttpClient>();
            var comment = new Mock<IComment>();

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

            httpClient.Setup(hc => hc.PostRequest(checkUrl, userAgent, 5000, parameters)).Returns("true");


            var client = new AkismetClient("myapikey", new Uri("http://haacked.com/"), httpClient.Object);
            Assert.IsTrue(client.CheckCommentForSpam(comment.Object),
                          "If the request returns 'false' then we should return false!");
        }

        [Test]
        public void CanCheckCommentWithArbitraryServerParams()
        {
            string userAgent = GetExpectedUserAgent();
            var checkUrl = new Uri("http://myapikey.rest.akismet.com/1.1/comment-check");
            string parameters = "blog=" + HttpUtility.UrlEncode("http://haacked.com/")
                                + "&user_ip=192.168.200.201"
                                + "&user_agent=" + HttpUtility.UrlEncode("Mozilla (My Silly Browser)")
                                + "&Making=" + HttpUtility.UrlEncode("This-Stuff")
                                + "&Up=" + HttpUtility.UrlEncode("As I-Go-Along");


            var httpClient = new Mock<HttpClient>();
            var comment = new Mock<IComment>();

            var extendedProps = new NameValueCollection();

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


            httpClient.Setup(hc => hc.PostRequest(checkUrl, userAgent, 5000, parameters)).Returns("false");


            var client = new AkismetClient("myapikey", new Uri("http://haacked.com/"), httpClient.Object);
            Assert.IsFalse(client.CheckCommentForSpam(comment.Object),
                           "If the request returns 'false' then we should return false!");
        }

        [RowTest]
        [Row("submit-ham", true)]
        [Row("submit-spam", false)]
        public void SubmitHamTest(string urlEnding, bool isHam)
        {
            string userAgent = GetExpectedUserAgent();
            var checkUrl = new Uri("http://myapikey.rest.akismet.com/1.1/" + urlEnding);
            string parameters = "blog=" + HttpUtility.UrlEncode("http://haacked.com/")
                                + "&user_ip=192.168.200.201"
                                + "&user_agent=" + HttpUtility.UrlEncode("Mozilla (My Silly Browser)");


            var httpClient = new Mock<HttpClient>();
            var comment = new Mock<IComment>();

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

            httpClient.Setup(hc => hc.PostRequest(checkUrl, userAgent, 5000, parameters)).Returns(string.Empty);


            var client = new AkismetClient("myapikey", new Uri("http://haacked.com/"), httpClient.Object);
            if(isHam)
            {
                client.SubmitHam(comment.Object);
            }
            else
            {
                client.SubmitSpam(comment.Object);
            }
        }

        /// <summary>
        /// The comment check call returns "invalid" if the api key is not valid 
        /// on the URL. The apikey must be the first part of the url.
        /// </summary>
        [Test]
        public void ThrowsInvalidResponseWhenApiKeyInvalid()
        {
            // arrange
            string userAgent = GetExpectedUserAgent();
            var checkUrl = new Uri("http://myapikey.rest.akismet.com/1.1/comment-check");
            string parameters = "blog=" + HttpUtility.UrlEncode("http://haacked.com/")
                                + "&user_ip=192.168.200.201"
                                + "&user_agent=" + HttpUtility.UrlEncode("Mozilla (My Silly Browser)");


            var httpClient = new Mock<HttpClient>();
            var comment = new Mock<IComment>();

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

            httpClient.Setup(hc => hc.PostRequest(checkUrl, userAgent, 5000, parameters)).Returns("invalid");
            var client = new AkismetClient("myapikey", new Uri("http://haacked.com/"), httpClient.Object);
            
            // act, assert
            UnitTestHelper.AssertThrows<InvalidResponseException>(() => client.CheckCommentForSpam(comment.Object));
        }

        private static void SetupCallsAnComment(Mock<IComment> comment, string author, string email, IPAddress ip,
                                                string userAgent, string referer, Uri permalink, string commentType,
                                                Uri authorUrl, string content, NameValueCollection extendedProperties)
        {
            comment.Setup(c => c.Author).Returns(author);
            comment.Setup(c => c.AuthorEmail).Returns(email);
            comment.Setup(c => c.IPAddress).Returns(ip);
            comment.Setup(c => c.UserAgent).Returns(userAgent);
            comment.Setup(c => c.Referrer).Returns(referer);
            comment.Setup(c => c.Permalink).Returns(permalink);
            comment.Setup(c => c.CommentType).Returns(commentType);
            comment.Setup(c => c.AuthorUrl).Returns(authorUrl);
            comment.Setup(c => c.Content).Returns(content);
            comment.Setup(c => c.ServerEnvironmentVariables).Returns(extendedProperties);
        }

        static string GetExpectedUserAgent()
        {
            return string.Format(CultureInfo.InvariantCulture, "Subtext/{0} | Akismet/1.11",
                                 typeof(HttpClient).Assembly.GetName().Version);
        }
    }
}