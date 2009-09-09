using MbUnit.Framework;
using Moq;
using Subtext.Akismet;
using Subtext.Extensibility;
using Subtext.Framework;
using Subtext.Framework.Components;
using Subtext.Framework.Routing;
using Subtext.Framework.Services;

namespace UnitTests.Subtext.Akismet
{
    [TestFixture]
    public class AkismetSpamServiceTests
    {
        [Test]
        public void Service_WithUrlHelper_UsesItForFeedbackUrl()
        {
            //arrange
            var akismetClient = new Mock<AkismetClient>();
            IComment submittedSpam = null;
            akismetClient.Setup(c => c.SubmitSpam(It.IsAny<IComment>())).Callback<IComment>(
                comment => submittedSpam = comment);

            var urlHelper = new Mock<UrlHelper>();
            urlHelper.Setup(helper => helper.FeedbackUrl(It.IsAny<FeedbackItem>())).Returns("/feedback-item");
            var service = new AkismetSpamService("apikey"
                                                 , new Blog {Host = "localhost"}
                                                 , akismetClient.Object
                                                 , urlHelper.Object);

            //act
            service.SubmitSpam(new FeedbackItem(FeedbackType.Comment));

            //assert
            Assert.AreEqual("http://localhost/feedback-item", submittedSpam.Permalink.ToString());
        }
    }
}