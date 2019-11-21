using System;
using System.Web;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Subtext.Framework;
using Subtext.Framework.Routing;

namespace UnitTests.Subtext.Framework.Routing
{
    [TestClass]
    public class VirtualPathTests
    {
        [TestMethod]
        public void Ctor_WithNull_ThrowsArgumentNullException()
        {
            //arrange, act, assert
            UnitTestHelper.AssertThrowsArgumentNullException(() => new VirtualPath(null));
        }

        [TestMethod]
        public void VirtualPath_WithFragment_SetsFragmentPropertyWithFragment()
        {
            //arrange
            var vp = new VirtualPath("/foo#bar");

            //assert
            Assert.AreEqual("/foo#bar", vp.ToString());
            Assert.AreEqual("#bar", vp.ToFullyQualifiedUrl(new Blog {Host = "localhost"}).Fragment);
        }

        [TestMethod]
        public void VirtualPath_WithSpecialPoundSequence_ReplacesSequenceWithEncodedPoundSign()
        {
            //arrange
            var vp = new VirtualPath("/foo%7B:#:%7Dbar/");

            //assert
            Assert.AreEqual("/foo%23bar/", vp.ToString());
        }

        [TestMethod]
        public void VirtualPathHasImplicitConversionToString()
        {
            //arrange
            var vp = new VirtualPath("/foo");

            //act
            string s = vp;

            //assert
            Assert.AreEqual("/foo", s);
        }

        [TestMethod]
        public void NullString_ConvertsToNullVirtualPath()
        {
            //arrange, act
            VirtualPath vp = (string)null;

            //assert
            Assert.IsNull(vp);
        }

        [TestMethod]
        public void EmptyString_ConvertsToNullVirtualPath()
        {
            //arrange, act
            VirtualPath vp = string.Empty;

            //assert
            Assert.IsNull(vp);
        }

        [TestMethod]
        public void VirtualPathHasImplicitConversionFromString()
        {
            //arrange, act
            VirtualPath vp = "/foo";

            //assert
            Assert.AreEqual("/foo", (string)vp);
        }

        [TestMethod]
        public void ToFullyQualifiedUrl_WithBlog_ReturnsUri()
        {
            //arrange
            VirtualPath vp = "/foo";

            //act
            Uri fullyQualified = vp.ToFullyQualifiedUrl(new Blog {Host = "localhost"});

            //assert
            Assert.AreEqual("http://localhost/foo", fullyQualified.ToString());
        }

        [TestMethod]
        public void ToFullyQualifiedUrl_WithBlogAndPort_ReturnsUriWithPort()
        {
            //arrange
            try
            {
                VirtualPath vp = "/foo";
                UnitTestHelper.SetHttpContextWithBlogRequest("localhost", 8080, "", "");

                //act
                Uri fullyQualified = vp.ToFullyQualifiedUrl(new Blog {Host = "localhost"});

                //assert
                Assert.AreEqual("http://localhost:8080/foo", fullyQualified.ToString());
            }
            finally
            {
                HttpContext.Current = null;
            }
        }

        [TestMethod]
        public void ToFullyQualifiedUrl_WithQueryString_ReturnsUriWithQueryString()
        {
            var x = new Uri("/foo", UriKind.Relative);
            Console.WriteLine(x.ToString());
            //arrange
            VirtualPath vp = "/foo?a=b";

            //act
            Uri uri = vp.ToFullyQualifiedUrl(new Blog {Host = "localhost"});

            //assert
            Assert.AreEqual("?a=b", uri.Query);
        }

        [TestMethod]
        public void ToFullyQualifiedUrl_WithNullVirtualPath_ReturnsNull()
        {
            VirtualPath vp = null;
            Assert.IsNull(vp.ToFullyQualifiedUrl(new Blog()));
            Assert.IsNull(vp.ToFullyQualifiedUrl(null));
        }

        [TestMethod]
        public void ToFullyQualifiedUrl_WithNullBlog_ThrowsArgumentNullException()
        {
            VirtualPath vp = "/";
            UnitTestHelper.AssertThrowsArgumentNullException(() => vp.ToFullyQualifiedUrl(null));
        }
    }
}