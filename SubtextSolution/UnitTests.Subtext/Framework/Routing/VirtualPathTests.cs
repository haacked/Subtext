using System;
using MbUnit.Framework;
using Subtext.Framework;
using Subtext.Framework.Routing;

namespace UnitTests.Subtext.Framework.Routing
{
    [TestFixture]
    public class VirtualPathTests
    {
        [Test]
        public void Ctor_WithNull_ThrowsArgumentNullException()
        {
            //arrange, act
            try
            {
                new VirtualPath(null);
            }
            catch(ArgumentNullException)
            {
                return;
            }

            //assert
            Assert.Fail();
        }

        [Test]
        public void VirtualPath_WithFragment_SetsFragmentPropertyWithFragment()
        {
            //arrange
            var vp = new VirtualPath("/foo#bar");

            //assert
            Assert.AreEqual("/foo#bar", vp.ToString());
            Assert.AreEqual("#bar", vp.ToFullyQualifiedUrl(new Blog {Host = "localhost"}).Fragment);
        }

        [Test]
        public void VirtualPath_WithSpecialPoundSequence_ReplacesSequenceWithEncodedPoundSign()
        {
            //arrange
            var vp = new VirtualPath("/foo%7B:#:%7Dbar/");

            //assert
            Assert.AreEqual("/foo%23bar/", vp.ToString());
        }

        [Test]
        public void VirtualPathHasImplicitConversionToString()
        {
            //arrange
            var vp = new VirtualPath("/foo");

            //act
            string s = vp;

            //assert
            Assert.AreEqual("/foo", s);
        }

        [Test]
        public void NullString_ConvertsToNullVirtualPath()
        {
            //arrange, act
            VirtualPath vp = (string)null;

            //assert
            Assert.IsNull(vp);
        }

        [Test]
        public void EmptyString_ConvertsToNullVirtualPath()
        {
            //arrange, act
            VirtualPath vp = string.Empty;

            //assert
            Assert.IsNull(vp);
        }

        [Test]
        public void VirtualPathHasImplicitConversionFromString()
        {
            //arrange, act
            VirtualPath vp = "/foo";

            //assert
            Assert.AreEqual("/foo", (string)vp);
        }

        [Test]
        public void ToFullyQualifiedUrl_WithBlog_ReturnsUri()
        {
            //arrange
            VirtualPath vp = "/foo";

            //act
            vp.ToFullyQualifiedUrl(new Blog {Host = "localhost"});

            //assert
            Assert.AreEqual("/foo", (string)vp);
        }

        [Test]
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
    }
}