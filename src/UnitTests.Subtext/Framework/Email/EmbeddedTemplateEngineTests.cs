using Microsoft.VisualStudio.TestTools.UnitTesting;
using Subtext.Framework.Email;

namespace UnitTests.Subtext.Framework.Email
{
    [TestClass]
    public class EmbeddedTemplateEngineTests
    {
        [TestMethod]
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