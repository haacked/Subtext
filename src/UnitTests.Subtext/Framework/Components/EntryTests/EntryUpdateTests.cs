using MbUnit.Framework;
using Moq;
using Subtext.Extensibility;
using Subtext.Framework;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;
using Subtext.Framework.Providers;
using Subtext.Framework.Web.HttpModules;

namespace UnitTests.Subtext.Framework.Components.EntryTests
{
    /// <summary>
    /// Some unit tests around updating entries.
    /// </summary>
    [TestFixture]
    public class EntryUpdateTests
    {
        string _hostName;

        [Test]
        [RollBack]
        public void CanDeleteEntry()
        {
            Config.CreateBlog("", "username", "password", _hostName, string.Empty);
            BlogRequest.Current.Blog = Config.GetBlog(_hostName, string.Empty);

            Entry entry = UnitTestHelper.CreateEntryInstanceForSyndication("Haacked", "Title Test", "Body Rocking");
            UnitTestHelper.Create(entry);

            Entry savedEntry = UnitTestHelper.GetEntry(entry.Id, PostConfig.None, false);
            Assert.IsNotNull(savedEntry);

            ObjectProvider.Instance().DeleteEntry(entry.Id);

            savedEntry = UnitTestHelper.GetEntry(entry.Id, PostConfig.None, false);
            Assert.IsNull(savedEntry, "Entry should now be null.");
        }

        /// <summary>
        /// Tests that setting the date syndicated to null removes the item from syndication.
        /// </summary>
        [Test]
        [RollBack]
        public void SettingDateSyndicatedToNullRemovesItemFromSyndication()
        {
            //arrange
            Config.CreateBlog("", "username", "password", _hostName, string.Empty);
            BlogRequest.Current.Blog = Config.GetBlog(_hostName, string.Empty);

            Entry entry = UnitTestHelper.CreateEntryInstanceForSyndication("Haacked", "Title Test", "Body Rocking");
            UnitTestHelper.Create(entry);

            Assert.IsTrue(entry.IncludeInMainSyndication,
                          "Failed to setup this test properly.  This entry should be included in the main syndication.");
            Assert.IsFalse(NullValue.IsNull(entry.DateSyndicated),
                           "Failed to setup this test properly. DateSyndicated should be null.");

            //act
            entry.DateSyndicated = NullValue.NullDateTime;

            //assert
            Assert.IsFalse(entry.IncludeInMainSyndication,
                           "Setting the DateSyndicated to a null date should have reset 'IncludeInMainSyndication'.");

            //save it
            var subtextContext = new Mock<ISubtextContext>();
            subtextContext.Setup(c => c.Blog).Returns(Config.CurrentBlog);
            subtextContext.Setup(c => c.Repository).Returns(ObjectProvider.Instance());
            UnitTestHelper.Update(entry, subtextContext.Object);
            Entry savedEntry = UnitTestHelper.GetEntry(entry.Id, PostConfig.None, false);

            //assert again
            Assert.IsFalse(savedEntry.IncludeInMainSyndication,
                           "This item should still not be included in main syndication.");
        }

        [SetUp]
        public void SetUp()
        {
            _hostName = UnitTestHelper.GenerateUniqueString();
            UnitTestHelper.SetHttpContextWithBlogRequest(_hostName, string.Empty);
        }

        [TearDown]
        public void TearDown()
        {
        }
    }
}