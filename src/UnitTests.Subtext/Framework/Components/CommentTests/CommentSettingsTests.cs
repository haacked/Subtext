using MbUnit.Framework;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;

namespace UnitTests.Subtext.Framework.Components.CommentTests
{
    [TestFixture]
    public class CommentSettingsTests
    {
        string hostName;

        [Test]
        [RollBack]
        [ExpectedArgumentNullException]
        public void ApproveThrowsArgumentNullException()
        {
            Config.CreateBlog("", "username", "thePassword", hostName, "MyBlog1");
            FeedbackItem.Approve(null, null);
        }

        [SetUp]
        public void SetUp()
        {
            hostName = UnitTestHelper.GenerateUniqueString();
            UnitTestHelper.SetHttpContextWithBlogRequest(hostName, "MyBlog1");
        }

        [TearDown]
        public void TearDown()
        {
        }
    }
}