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
    public class EncosureDeleteTests
    {
        private BlogInfo blog;

        [Test]
        [RollBack2]
        public void CanDeleteEnclosure()
        {
            blog = UnitTestHelper.CreateBlogAndSetupContext();

            Entry e = UnitTestHelper.CreateEntryInstanceForSyndication("Simone Chiaretta", "Post for testing Enclosures", "Listen to my great podcast");
            int entryId = Entries.Create(e);

            Enclosure enc = UnitTestHelper.BuildEnclosure("Nothing to see here.", "httP://blablabla.com", "audio/mp3", entryId, 12345678, true, true);
            Enclosures.Create(enc);

            Entry newEntry = Entries.GetEntry(entryId, false);

            Assert.IsNotNull(newEntry.Enclosure, "Did not create enclosure.");

            Enclosures.Delete(enc.Id);

            Entry newEntry1 = Entries.GetEntry(entryId, false);

            Assert.IsNull(newEntry1.Enclosure, "Did not delete enclosure.");
        }
    }
}
