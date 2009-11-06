using System.Xml;
using MbUnit.Framework;
using SubtextUpgrader;

namespace SubtextUpgraderTests
{
    [TestFixture]
    public class SkinUpgraderTests
    {
        [Test]
        public void TransformTemplateToSkinConfig_TransformsOldSkinToNewSkinConfig()
        {
            // arrange
            const string config = @"<?xml version=""1.0""?>
<SkinTemplates xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"">
  <Skins>
    <SkinTemplate Name=""AnotherEon001"" TemplateFolder=""AnotherEon001"" StyleMergeMode=""MergedAfter"">
      <Styles>
        <Style href=""~/skins/_System/csharp.css"" />
      </Styles>
    </SkinTemplate>
  </Skins>
</SkinTemplates>";
            var xml = new XmlDocument();
            xml.LoadXml(config);

            // act
            var skinConfigNode = SkinUpgrader.TransformTemplateToSkinConfig(xml);

            // assert
            const string expected = @"<?xml version=""1.0""?><SkinTemplates><SkinTemplate Name=""AnotherEon001"" StyleMergeMode=""MergedAfter""><Styles><Style href=""~/skins/_System/csharp.css"" /></Styles></SkinTemplate></SkinTemplates>";
            Assert.AreEqual(expected, skinConfigNode.OuterXml);
        }
    }
}
