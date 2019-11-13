using System.Xml;
using MbUnit.Framework;
using SubtextUpgrader;

namespace SubtextUpgraderTests
{
    [TestFixture]
    public class SkinConfigTests
    {
        [Test]
        public void Ctor_WithStream_SetsXmlAndTemplateFolder()
        {
            // arrange
            var xml = new XmlDocument();

            // act
            var config = new SkinConfig(xml, "Test");

            // assert
            Assert.AreEqual("Test", config.TemplateFolder);
            Assert.AreEqual(xml, config.Xml);
        }
    }
}
