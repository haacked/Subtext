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

using System.Globalization;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Subtext.Framework.Components;
using Subtext.Framework.Properties;

namespace UnitTests.Subtext.Framework.Components.EnclosureTests
{
    [TestClass]
    public class EnclosureGenericTests
    {
        [MultipleCultureTestMethod("it-IT,en-US")]
        [DataRow(100, "100 bytes")]
        [DataRow(1024, "1 KB")]
        [DataRow(1162, "1,13 KB")]
        [DataRow(7862732, "7,5 MB")]
        public void SizeIsFormattedCorrectly(long size, string expected)
        {
            var enc = new Enclosure {Size = size};
            Thread.CurrentThread.CurrentCulture = new CultureInfo("it-IT");
            Assert.AreEqual(expected, enc.FormattedSize, "Not the right formatting");
        }

        [TestMethod]
        public void IsValid_WithZeroEntryId_ReturnsFalse()
        {
            // arrange
            var enclosure = new Enclosure {EntryId = 0};

            // act
            bool valid = enclosure.IsValid;

            // assert
            Assert.IsFalse(valid);
            Assert.AreEqual(Resources.Enclosure_NeedsAnEntry, enclosure.ValidationMessage);
        }

        [TestMethod]
        public void IsValid_WithNullUrl_ReturnsFalse()
        {
            // arrange
            var enclosure = new Enclosure { EntryId = 1, Url = null };

            // act
            bool valid = enclosure.IsValid;

            // assert
            Assert.IsFalse(valid);
            Assert.AreEqual(Resources.Enclosure_UrlRequired, enclosure.ValidationMessage);
        }

        [TestMethod]
        public void IsValid_WithEmptyUrl_ReturnsFalse()
        {
            // arrange
            var enclosure = new Enclosure { EntryId = 1, Url = string.Empty};

            // act
            bool valid = enclosure.IsValid;

            // assert
            Assert.IsFalse(valid);
            Assert.AreEqual(Resources.Enclosure_UrlRequired, enclosure.ValidationMessage);
        }

        [TestMethod]
        public void IsValid_WithNullMimeType_ReturnsFalse()
        {
            // arrange
            var enclosure = new Enclosure { EntryId = 1, Url = "http://example.com", MimeType = null };

            // act
            bool valid = enclosure.IsValid;

            // assert
            Assert.IsFalse(valid);
            Assert.AreEqual(Resources.Enclosure_MimeTypeRequired, enclosure.ValidationMessage);
        }

        [TestMethod]
        public void IsValid_WithEmptyMimeType_ReturnsFalse()
        {
            // arrange
            var enclosure = new Enclosure { EntryId = 1, Url = "http://example.com", MimeType = string.Empty};

            // act
            bool valid = enclosure.IsValid;

            // assert
            Assert.IsFalse(valid);
            Assert.AreEqual(Resources.Enclosure_MimeTypeRequired, enclosure.ValidationMessage);
        }

        [TestMethod]
        public void IsValid_WithZeroSize_ReturnsFalse()
        {
            // arrange
            var enclosure = new Enclosure { EntryId = 1, Url = "http://example.com", MimeType = "image/jpg", Size = 0};

            // act
            bool valid = enclosure.IsValid;

            // assert
            Assert.IsFalse(valid);
            Assert.AreEqual(Resources.Enclosure_SizeGreaterThanZero, enclosure.ValidationMessage);
        }

        [TestMethod]
        public void IsValid_WithValidEnclosure_ReturnsTrue()
        {
            // arrange
            var enclosure = new Enclosure { EntryId = 1, Url = "http://example.com", MimeType = "image/jpg", Size = 100};

            // act
            bool valid = enclosure.IsValid;

            // assert
            Assert.IsTrue(valid);
            Assert.IsNull(enclosure.ValidationMessage);
        }
    }
}