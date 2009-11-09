using System.IO;
using System.Linq;
using MbUnit.Framework;
using Moq;
using SubtextUpgrader;

namespace SubtextUpgraderTests
{
    [TestFixture]
    public class LegacySkinsConfigTests
    {
        [Test]
        public void GetNewSkinConfigs_WithLegacySkinConfigXml_ExtractsSkinConfigs()
        {
            // arrange
            var configXml = @"<?xml version=""1.0""?>
<SkinTemplates xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"">
  <Skins>
    <SkinTemplate Name=""Foo"" TemplateFolder=""Foo"" StyleMergeMode=""MergedAfter"">
      <Styles>
        <Style href=""csharp.css"" />
      </Styles>
    </SkinTemplate>
    <SkinTemplate Name=""Bar"" TemplateFolder=""Bar"">
      <Styles>
        <Style href=""bar.css"" />
      </Styles>
    </SkinTemplate>
  </Skins>
</SkinTemplates>";
            var file = new Mock<IFile>();
            file.Setup(f => f.Contents).Returns(configXml);
            var oldConfig = new LegacySkinsConfig(file.Object);

            // act
            var configs = oldConfig.GetNewSkinConfigs();

            // assert
            Assert.AreEqual(2, configs.Count());
            const string expectedFirstXml =
                @"<SkinTemplates><SkinTemplate Name=""Foo"" StyleMergeMode=""MergedAfter""><Styles><Style href=""csharp.css"" /></Styles></SkinTemplate></SkinTemplates>";
            Assert.AreEqual(expectedFirstXml, configs.First().Xml.OuterXml);
            Assert.AreEqual("Foo", configs.First().TemplateFolder);
            const string expectedSecondXml =
                @"<SkinTemplates><SkinTemplate Name=""Bar""><Styles><Style href=""bar.css"" /></Styles></SkinTemplate></SkinTemplates>";
            Assert.AreEqual(expectedSecondXml, configs.ElementAt(1).Xml.OuterXml);
            Assert.AreEqual("Bar", configs.ElementAt(1).TemplateFolder);
        }

        [Test]
        public void UpgradeSkins_WritesNewSkins_ToSkinTemplateDirectory()
        {
            // arrange
            var configXml = @"<?xml version=""1.0""?>
<SkinTemplates xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"">
  <Skins>
    <SkinTemplate Name=""Foo"" TemplateFolder=""Foo"" StyleMergeMode=""MergedAfter"">
      <Styles>
        <Style href=""csharp.css"" />
      </Styles>
    </SkinTemplate>
  </Skins>
</SkinTemplates>".ToXml();
            var oldConfig = new LegacySkinsConfig(configXml);
            var memoryStream = new NonDisposableMemoryStream();
            var skinFile = new Mock<IFile>();
            skinFile.Setup(f => f.OpenWrite()).Returns(memoryStream);
            var skinDirectory = new Mock<IDirectory>();
            skinDirectory.Setup(d => d.Exists).Returns(false);
            skinDirectory.Setup(d => d.CombineFile("skin.config")).Returns(skinFile.Object);
            skinDirectory.Setup(d => d.Ensure()).Returns(skinDirectory.Object);
            var skinsDirectory = new Mock<IDirectory>();
            skinsDirectory.Setup(d => d.Combine("Foo")).Returns(skinDirectory.Object);
            skinsDirectory.Setup(d => d.Ensure()).Returns(skinDirectory.Object);

            // act
            oldConfig.UpgradeSkins(skinsDirectory.Object);

            // assert
            const string expected =
                @"<SkinTemplates>
  <SkinTemplate Name=""Foo"" StyleMergeMode=""MergedAfter"">
    <Styles>
      <Style href=""csharp.css"" />
    </Styles>
  </SkinTemplate>
</SkinTemplates>";
            Assert.AreEqual(expected, memoryStream.ToStringContents());
        }
    }
}
