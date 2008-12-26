using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using Subtext.Framework.Services;
using Subtext.Framework;
using Moq;
using Subtext.Framework.Routing;
using Subtext.Framework.Components;
using Subtext.Extensibility;
using Subtext.Akismet;
using System.Web.Routing;
using System.Web;

namespace UnitTests.Subtext.Akismet
{
    [TestFixture]
    public class AkismetSpamServiceTests
    {
        [Test]
        public void Service_WithUrlHelper_UsesItForFeedbackUrl() {
            //arrange
            var akismetClient = new Mock<AkismetClient>();
            IComment submittedSpam = null;
            akismetClient.Expect(c => c.SubmitSpam(It.IsAny<IComment>())).Callback<IComment>(comment => submittedSpam = comment);

            var urlHelper = new Mock<UrlHelper>();
            urlHelper.Expect(helper => helper.FeedbackUrl(It.IsAny<FeedbackItem>())).Returns("/feedback-item");
            var service = new AkismetSpamService("apikey"
                , new BlogInfo { Host = "localhost" }
                , akismetClient.Object
                , urlHelper.Object);

            //act
            service.SubmitSpam(new FeedbackItem(FeedbackType.Comment));

            //assert
            Assert.AreEqual("http://localhost/feedback-item", submittedSpam.Permalink.ToString());
        }
    }
}
