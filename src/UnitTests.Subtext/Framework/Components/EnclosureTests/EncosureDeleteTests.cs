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

using MbUnit.Framework;
using Subtext.Framework;
using Subtext.Framework.Components;
using Subtext.Framework.Data;
using Subtext.Framework.Providers;

namespace UnitTests.Subtext.Framework.Components.EnclosureTests
{
    [TestFixture]
    public class EnclosureDeleteTests
    {
        [Test]
        [RollBack2]
        public void CanDeleteEnclosure()
        {
            Blog blog = UnitTestHelper.CreateBlogAndSetupContext();
            var repository = new DatabaseObjectProvider();
            Entry e = UnitTestHelper.CreateEntryInstanceForSyndication("Simone Chiaretta", "Post for testing Enclosures",
                                                                       "Listen to my great podcast");
            int entryId = UnitTestHelper.Create(e);

            Enclosure enc = UnitTestHelper.BuildEnclosure("Nothing to see here.", "httP://blablabla.com", "audio/mp3",
                                                          entryId, 12345678, true, true);
            repository.Create(enc);

            Entry newEntry = repository.GetEntry(entryId, true, false);

            Assert.IsNotNull(newEntry.Enclosure, "Did not create enclosure.");

            repository.DeleteEnclosure(enc.Id);

            Entry newEntry1 = repository.GetEntry(entryId, true, false);

            Assert.IsNull(newEntry1.Enclosure, "Did not delete enclosure.");
        }
    }
}