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

using System;
using MbUnit.Framework;
using Subtext.Framework;
using Subtext.Framework.Components;
using Subtext.Framework.Providers;

namespace UnitTests.Subtext.Framework.Components.EnclosureTests
{
    [TestFixture]
    public class EnclosureInsertTests
    {
        [RowTest]
        [Row("My wonderful podcast", "http://codeclimber.net.nz/podcast/mypodcast.mp3", "audio/mpeg", 123456789, true,
            true, "Did not create enclosure")]
        [Row("", "http://codeclimber.net.nz/podcast/mypodcast.mp3", "audio/mpeg", 123456789, true, false,
            "Did not create enclosure")]
        [Row("", "http://codeclimber.net.nz/podcast/mypodcast.mp3", "audio/mpeg", 123456789, false, true,
            "Did not create enclosure")]
        [RollBack2]
        public void CanInsertEnclosure(string title, string url, string mimetype, long size, bool addToFeed,
                                       bool showWithPost, string errMsg)
        {
            UnitTestHelper.SetupBlog();
            Entry e = UnitTestHelper.CreateEntryInstanceForSyndication("Simone Chiaretta", "Post for testing Enclosures",
                                                                       "Listen to my great podcast");
            int entryId = UnitTestHelper.Create(e);
            Enclosure enc = UnitTestHelper.BuildEnclosure(title, url, mimetype, entryId, size, addToFeed, showWithPost);

            Enclosures.Create(enc);

            Entry newEntry = ObjectProvider.Instance().GetEntry(entryId, true, false);

            Assert.IsNotNull(newEntry.Enclosure, errMsg);

            UnitTestHelper.AssertEnclosures(enc, newEntry.Enclosure);
        }

        [Test]
        public void Create_WithNullEnclosure_ThrowsArgumentNullException()
        {
            UnitTestHelper.AssertThrowsArgumentNullException(() => Enclosures.Create(null));
        }

        [Test]
        public void Create_WithInvalidEntry_ThrowsArgumentException()
        {
            // arrange
            var enclosure = new Enclosure{ EntryId = 0};

            // act, assert
            Assert.IsFalse(enclosure.IsValid);
            UnitTestHelper.AssertThrows<ArgumentException>(() => Enclosures.Create(enclosure));
        }

        [Test]
        [RollBack2]
        public void EntryWithNoenclosureHasNullAsEnclosure()
        {
            UnitTestHelper.SetupBlog();
            Entry e = UnitTestHelper.CreateEntryInstanceForSyndication("Simone Chiaretta", "Post for testing Enclosures",
                                                                       "Listen to my great podcast");
            int entryId = UnitTestHelper.Create(e);

            Entry newEntry = ObjectProvider.Instance().GetEntry(entryId, true, false);

            Assert.IsNull(newEntry.Enclosure, "enclosure must be null");
        }
   }
}