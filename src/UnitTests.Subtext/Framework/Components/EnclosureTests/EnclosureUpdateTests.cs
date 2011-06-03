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
using Subtext.Framework.Components;
using Subtext.Framework.Data;
using Subtext.Framework.Providers;
using Subtext.Framework.Text;

namespace UnitTests.Subtext.Framework.Components.EnclosureTests
{
    [TestFixture]
    public class EnclosureUpdateTests
    {
        [RowTest]
        [Row("My wonderful podcast", "http://codeclimber.net.nz/podcast/mypodcast.mp3", "audio/mpeg", 123456789, true,
            false)]
        [Row("", "http://codeclimber.net.nz/podcast/mypodcast.mp3", "audio/mpeg", 123456789, true, false)]
        [RollBack2]
        public void CanUpdateEnclosure(string title, string url, string mimetype, long size, bool addToFeed,
                                       bool showWithPost)
        {
            UnitTestHelper.SetupBlog(string.Empty);
            var repository = new DatabaseObjectProvider();
            Entry e = UnitTestHelper.CreateEntryInstanceForSyndication("Simone Chiaretta", "Post for testing Enclosures",
                                                                       "Listen to my great podcast");
            int entryId = UnitTestHelper.Create(e);
            Enclosure enc = UnitTestHelper.BuildEnclosure(title, url, mimetype, entryId, size, addToFeed, showWithPost);

            repository.Create(enc);

            string randomStr = UnitTestHelper.GenerateUniqueString().Left(20);
            enc.Url = url + randomStr;

            if (!string.IsNullOrEmpty(title))
            {
                enc.Title = title + randomStr;
            }

            enc.MimeType = mimetype + randomStr;

            int randomSize = new Random().Next(10, 100);
            enc.Size = size + randomSize;

            Assert.IsTrue(repository.Update(enc), "Should have updated the Enclosure");

            Entry newEntry = ObjectProvider.Instance().GetEntry(entryId, true, false);

            UnitTestHelper.AssertEnclosures(enc, newEntry.Enclosure);
        }

        [Test]
        public void Update_WithInvalidEnclosure_ThrowsArgumentException()
        {
            // arrange
            var enclosure = new Enclosure { EntryId = 0 };
            var repository = new DatabaseObjectProvider();

            // act, assert
            Assert.IsFalse(enclosure.IsValid);
            var exception = UnitTestHelper.AssertThrows<ArgumentException>(() => repository.Update(enclosure));
            Assert.AreEqual(enclosure.ValidationMessage, exception.Message);
        }

        [Test]
        public void Update_WithNullEnclosure_ThrowsArgumentNullException()
        {
            var repository = new DatabaseObjectProvider();
            UnitTestHelper.AssertThrowsArgumentNullException(() => repository.Update((Enclosure)null));
        }
    }
}