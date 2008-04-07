#region Disclaimer/Info
///////////////////////////////////////////////////////////////////////////////////////////////////
// Subtext WebLog
// 
// Subtext is an open source weblog system that is a fork of the .TEXT
// weblog system.
//
// For updated news and information please visit http://subtextproject.com/
// Subtext is hosted at SourceForge at http://sourceforge.net/projects/subtext
// The development mailing list is at subtext-devs@lists.sourceforge.net 
//
// This project is licensed under the BSD license.  See the License.txt file for more information.
///////////////////////////////////////////////////////////////////////////////////////////////////
#endregion

using System;
using MbUnit.Framework;
using Subtext.Framework;
using Subtext.Framework.Components;

namespace UnitTests.Subtext.Framework.Components.EnclosureTests
{
    [TestFixture]
    public class EnclosureInsertTests
    {
        private BlogInfo blog;

        [RowTest]
        [Row("My wonderful podcast", "http://codeclimber.net.nz/podcast/mypodcast.mp3", "audio/mpeg", 123456789, "Did not create enclusure")]
        [Row("", "http://codeclimber.net.nz/podcast/mypodcast.mp3", "audio/mpeg", 123456789, "Did not create enclusure")]
        [Row("My wonderful podcast", "http://codeclimber.net.nz/podcast/mypodcast.mp3", "audio/mpeg", 0, "Enclosure Invalid - Requires Size",ExpectedException = typeof(ArgumentException))]
        [Row("My wonderful podcast", "http://codeclimber.net.nz/podcast/mypodcast.mp3", "", 123456789, "Enclosure Invalid - Requires MimeType", ExpectedException = typeof(ArgumentException))]
        [Row("My wonderful podcast", "", "audio/mpeg", 123456789, "Enclosure Invalid - Requires Url", ExpectedException = typeof(ArgumentException))]
        [RollBack2]
        public void CanInsertEnclosure(string title, string url, string mimetype, long size, string errMsg)
        {
            this.blog = UnitTestHelper.CreateBlogAndSetupContext();
            Entry e = UnitTestHelper.CreateEntryInstanceForSyndication("Simone Chiaretta", "Post for testing Enclosures", "Listen to my great podcast");
            int entryId = Entries.Create(e);
            Enclosure enc = UnitTestHelper.BuildEnclosure(title, url, mimetype, entryId, size);

            Enclosures.Create(enc);

            Entry newEntry = Entries.GetEntry(entryId, false);

            Assert.IsNotNull(newEntry.Enclosure,errMsg);

            Assert.AreEqual(title, enc.Title, "Wrong title");
            Assert.AreEqual(url, enc.Url, "Wrong Url");
            Assert.AreEqual(mimetype, enc.MimeType, "Wrong MimeType");
            Assert.AreEqual(size, enc.Size, "Wrong Size");
        }

        [Test]
        [RollBack2]
        public void EntryWithNoEnclusureHasNullAsEnclosure()
        {
            this.blog = UnitTestHelper.CreateBlogAndSetupContext();
            Entry e = UnitTestHelper.CreateEntryInstanceForSyndication("Simone Chiaretta", "Post for testing Enclosures", "Listen to my great podcast");
            int entryId = Entries.Create(e);

            Entry newEntry = Entries.GetEntry(entryId, false);

            Assert.IsNull(newEntry.Enclosure, "Enclusure must be null");
        }


        [Test]
        [ExpectedArgumentNullException]
        public void CanNotInsertNullEnclosure()
        {
            Enclosures.Create(null);
        }
    }
}
