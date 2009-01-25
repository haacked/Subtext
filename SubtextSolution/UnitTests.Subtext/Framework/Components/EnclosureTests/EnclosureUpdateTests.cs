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
using Subtext.Framework.Text;
using Subtext.Framework.Providers;

namespace UnitTests.Subtext.Framework.Components.EnclosureTests
{
    [TestFixture]
    public class EnclosureUpdateTests
    {
        private Blog blog;

        [RowTest]
        [Row("My wonderful podcast", "http://codeclimber.net.nz/podcast/mypodcast.mp3", "audio/mpeg", 123456789, true, false)]
        [Row("", "http://codeclimber.net.nz/podcast/mypodcast.mp3", "audio/mpeg", 123456789, true, false)]
        [RollBack2]
        public void CanUpdateEnclosure(string title, string url, string mimetype, long size, bool addToFeed, bool showWithPost)
        {
            this.blog = UnitTestHelper.CreateBlogAndSetupContext();
            Entry e = UnitTestHelper.CreateEntryInstanceForSyndication("Simone Chiaretta", "Post for testing Enclosures", "Listen to my great podcast");
            int entryId = Entries.Create(e);
            Enclosure enc = UnitTestHelper.BuildEnclosure(title, url, mimetype, entryId, size, addToFeed, showWithPost);

            Enclosures.Create(enc);

            string randomStr = UnitTestHelper.GenerateUniqueString().Left(20);
            enc.Url = url + randomStr;

            if (!string.IsNullOrEmpty(title))
                enc.Title = title + randomStr;
            
            enc.MimeType = mimetype + randomStr;

            int randomSize = new Random().Next(10, 100);
            enc.Size = size + randomSize;

            Assert.IsTrue(Enclosures.Update(enc),"Should have updated the Enclosure");

            Entry newEntry = ObjectProvider.Instance().GetEntry(entryId, false);

            UnitTestHelper.AssertEnclosures(enc, newEntry.Enclosure);
        }

        [RowTest]
        [Row(null, null, null, "All attributs are null, should not be valid.", ExpectedException = typeof(ArgumentException))]
        [Row("http://codeclimber.net.nz/podcast/mypodcast.mp3", "audio/mp3", 0, "Enclosures must have size greater then ZERO.", ExpectedException = typeof(ArgumentException))]
        [Row("http://codeclimber.net.nz/podcast/mypodcast.mp3", "", 123456, "Enclosures must have a mimetype.", ExpectedException = typeof(ArgumentException))]
        [Row("", "audio/mp3", 123456, "Enclosures must have a Url.", ExpectedException = typeof(ArgumentException))]
        [Row(null, "audio/mp3", 123456, "Enclosures must have a Url.", ExpectedException = typeof(ArgumentException))]
        [RollBack2]
        public void CantUpdateWithInvalidEnclosure(string url, string mimetype, long size, string errMsg)
        {
            this.blog = UnitTestHelper.CreateBlogAndSetupContext();

            Entry e = UnitTestHelper.CreateEntryInstanceForSyndication("Simone Chiaretta", "Post for testing Enclosures", "Listen to my great podcast");
            int entryId = Entries.Create(e);

            Enclosure enc = UnitTestHelper.BuildEnclosure("Nothing to see here.", "httP://blablabla.com", "audio/mp3", entryId, 12345678, true, true);
            Enclosures.Create(enc);

            enc.Url = url;
            enc.MimeType = mimetype;
            enc.Size = size;

            Enclosures.Update(enc);
        }

        [Test]
        [ExpectedArgumentNullException]
        public void CanNotUpdateNullEnclosure()
        {
            Enclosures.Update(null);
        }
    }
}
