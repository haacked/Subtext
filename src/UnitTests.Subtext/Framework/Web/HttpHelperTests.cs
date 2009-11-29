using System;
using System.Collections.Specialized;
using System.Net;
using System.Web;
using MbUnit.Framework;
using Moq;
using Subtext.Framework;
using Subtext.Framework.Util;
using Subtext.Framework.Web;
using UnitTests.Subtext.Framework.Util;

namespace UnitTests.Subtext.Framework.Web
{
    /// <summary>
    /// Contains tests of our handling of Http.
    /// </summary>
    [TestFixture]
    public class HttpHelperTests
    {
        /// <summary>
        /// Tests that we can create a proxy. This is based on some 
        /// settings in Web.config, which we populated in App.config 
        /// for this unit test.
        /// </summary>
        [Test]
        public void CanCreateProxy()
        {
            WebRequest request = WebRequest.Create("http://subtextproject.com/");
            HttpHelper.SetProxy(request);
            Assert.IsNotNull(request.Proxy, "Proxy should not be null.");
        }

        /// <summary>
        /// Tests that we correctly parse if-modified-since from the request.
        /// Unfortunately, this unit test is time-zone sensitive.
        /// </summary>
        [RowTest]
        [Row("4/12/2006", "04/11/2006 5:00 PM")]
        [Row("12 Apr 2006 06:59:33 GMT", "4/11/2006 11:59:33 PM")]
        [Row("Wed, 12 Apr 2006 06:59:33 GMT", "04-11-2006 23:59:33")]
        public void TestIfModifiedSinceExtraction(string received, string expected)
        {
            var headers = new NameValueCollection();
            headers.Add("If-Modified-Since", received);
            var httpRequest = new Mock<HttpRequestBase>();
            httpRequest.Setup(r => r.Headers).Returns(headers);

            DateTime expectedDate = DateTimeHelper.ParseUnknownFormatUtc(expected);
            Console.WriteLine("{0}\t{1}\t{2}", received, expected, expectedDate.ToUniversalTime());

            DateTime result = HttpHelper.GetIfModifiedSinceDateUtc(httpRequest.Object);
            //Convert to PST:
            TimeZoneInfo timeZone = TimeZones.GetTimeZones().GetById(TimeZonesTest.PacificTimeZoneId);
            result = TimeZoneInfo.ConvertTimeFromUtc(result, timeZone);

            Assert.AreEqual(expectedDate, result);
        }

        [RowTest]
        [Row("test.css", true)]
        [Row("test.js", true)]
        [Row("test.png", true)]
        [Row("test.gif", true)]
        [Row("test.jpg", true)]
        [Row("test.html", true)]
        [Row("test.xml", true)]
        [Row("test.htm", true)]
        [Row("test.txt", true)]
        [Row("test.aspx", false)]
        [Row("test.asmx", false)]
        [Row("test.ashx", false)]
        public void CanDeterimineIsStaticFileRequest(string filename, bool expected)
        {
            // arrange
            var request = new Mock<HttpRequestBase>();
            request.Setup(r => r.Url).Returns(new Uri("http://localhost:1234/whatever/" + filename));

            Assert.AreEqual(expected, request.Object.IsStaticFileRequest());
        }

        [Test]
        public void GetMimeType_WithPngExtension_ReturnsImagePng()
        {
            // arrange, act
            string mimeType = "/foo.png".GetMimeType();

            Assert.AreEqual("image/png", mimeType);
        }

        [Test]
        public void GetMimeType_WithJpgExtension_ReturnsImageJPEG()
        {
            // arrange, act
            string mimeType = "/foo.Jpg".GetMimeType();

            Assert.AreEqual("image/jpeg", mimeType);
        }

        [Test]
        public void GetMimeType_WithJpegExtension_ReturnsImageJPEG()
        {
            // arrange, act
            string mimeType = "/foo.Jpeg".GetMimeType();

            Assert.AreEqual("image/jpeg", mimeType);
        }

        [Test]
        public void GetMimeType_WithBmpExtension_ReturnsImageBmp()
        {
            // arrange, act
            string mimeType = "/foo.bmp".GetMimeType();

            Assert.AreEqual("image/bmp", mimeType);
        }

        [Test]
        public void GetMimeType_WithGifExtension_ReturnsImageGif()
        {
            // arrange, act
            string mimeType = "/foo.gif".GetMimeType();

            Assert.AreEqual("image/gif", mimeType);
        }

        [Test]
        public void GetMimeType_WithUnknownExtension_ReturnsNone()
        {
            // arrange, act
            string mimeType = "/foo.bif".GetMimeType();

            Assert.AreEqual("none", mimeType);
        }

        [Test]
        public void GetSafeFileName_WithTextContainingInvalidText_RemovesInvalidChars()
        {
            // arrange
            var text = @"This \|/ : contains bad chars";

            // act
            var fileName = text.GetSafeFileName();

            // assert
            Assert.AreEqual("This   contains bad chars", fileName);
        }

        [Test]
        public void GetSafeFileName_WithNullText_ThrowsArgumentNullException()
        {
            // arrange
            string text = null;

            // act
            UnitTestHelper.AssertThrowsArgumentNullException(() => text.GetSafeFileName());
        }

        [Test]
        public void GetSafeFileName_WithEmptyText_ThrowsArgumentNullException()
        {
            // arrange
            string text = string.Empty;

            // act
            UnitTestHelper.AssertThrowsArgumentNullException(() => text.GetSafeFileName());
        }
    }
}