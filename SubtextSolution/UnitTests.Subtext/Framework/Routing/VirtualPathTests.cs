using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using Subtext.Framework.Routing;
using Subtext.Framework;

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
            catch (ArgumentNullException) {
                return;
            }

            //assert
            Assert.Fail();
        }

        [Test]
        public void VirtualPath_WithFragment_SetsFragmentPropertyWithFragment()
        {
            //arrange
            VirtualPath vp = new VirtualPath("/foo#bar");

            //assert
            Assert.AreEqual("bar", vp.Fragment);
            Assert.AreEqual("/foo#bar", vp.ToString());
        }

        [Test]
        public void VirtualPathHasImplicitConversionToString() { 
            //arrange
            VirtualPath vp = new VirtualPath("/foo");

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
        public void ToFullyQualifiedUrl_WithBlogInfo_ReturnsUri()
        {
            //arrange
            VirtualPath vp = "/foo";

            //act
            vp.ToFullyQualifiedUrl(new BlogInfo { Host = "localhost" });

            //assert
            Assert.AreEqual("/foo", (string)vp);
        }
    }
}
