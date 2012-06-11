using MbUnit.Framework;
using Subtext.Framework.Email;

namespace UnitTests.Subtext.Framework.Email
{
    [TestFixture]
    public class EmbeddedTemplateEngineTests
    {
        [Test]
        public void GetTemplate_WithCommentReceveid_ReturnsPropertTemplate()
        {
            //arrange
            var templateEngine = new EmbeddedTemplateEngine();

            //act
            ITextTemplate template = templateEngine.GetTemplate("CommentReceived");

            //assert
            Assert.IsTrue(template.ToString().StartsWith("{spamflag}"));
        }
    }
}