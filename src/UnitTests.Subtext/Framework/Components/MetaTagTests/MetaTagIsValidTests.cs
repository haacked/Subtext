#region Disclaimer/Info

///////////////////////////////////////////////////////////////////////////////////////////////////
// Subtext WebLog
// 
// Subtext is an open source weblog system that is a fork of the .TEXT
// weblog system.
//
// For updated news and information please visit http://subtextproject.com/
// Subtext is hosted at Google Code at http://code.google.com/p/subtext/
// The development mailing list is at subtext@googlegroups.com 
//
// This project is licensed under the BSD license.  See the License.txt file for more information.
///////////////////////////////////////////////////////////////////////////////////////////////////

#endregion

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Subtext.Framework.Components;

namespace UnitTests.Subtext.Framework.Components.MetaTagTests
{
    [TestClass]
    public class MetaTagIsValidTests
    {
        [DataTestMethod]
        [DataRow(null, null, false, "Must supply either a Name or HttpEquiv")]
        [DataRow("description", null, true, "Name should be enough")]
        [DataRow(null, "expires", true, "HttpEquiv should be enough")]
        [DataRow("description", "expires", false, "Can not have both a Name and HttpEquiv!")]
        public void MetaTagRequiresEitherNameOrHttpEquivAttribute(string nameValue, string httpEquivValue,
                                                                  bool isValidResult, string errMsg)
        {
            var tag = new MetaTag("Steve rocks it!");
            tag.Name = nameValue;
            tag.HttpEquiv = httpEquivValue;

            Assert.AreEqual(tag.IsValid, isValidResult, errMsg);
        }

        [DataTestMethod]
        [DataRow(null, false, "The Content attribute requires a value")]
        [DataRow("This is some text!", true, "The Content attribute should allow a value")]
        public void MetaTagRequiresContentAttribute(string contentValue, bool isValidResult, string errMsg)
        {
            var tag = new MetaTag(contentValue);
            tag.Name = "description";

            Assert.AreEqual(tag.IsValid, isValidResult);
        }
    }
}