using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Hosting;
using MbUnit.Framework;
using Moq;
using Subtext.Framework.UI.Skinning;

namespace UnitTests.Subtext.Framework.Skinning
{
    [TestFixture]
    public class SkinEngineTests
    {
        [Test]
        public void GetSkinTemplates_WithFolders_ReturnsSkinPerFolder()
        {
            //arrange
            var directories = new List<VirtualDirectory>();
            for (int i = 0; i < 3; i++)
            {
                var skinDir = new Mock<VirtualDirectory>("~/skins/skin" + i);
                skinDir.Setup(d => d.Name).Returns("Skin" + i);
                directories.Add(skinDir.Object);
            }
            var skinsDir = new Mock<VirtualDirectory>("~/skins");
            skinsDir.Setup(s => s.Directories).Returns(directories);
            var vpp = new Mock<VirtualPathProvider>();
            vpp.Setup(v => v.GetDirectory("~/skins")).Returns(skinsDir.Object);
            var skins = new SkinEngine(vpp.Object);

            //act
            IDictionary<string, SkinTemplate> skinTemplates = skins.GetSkinTemplates(mobileOnly: false);

            //assert
            Assert.AreEqual(3, skinTemplates.Count);
            Assert.AreEqual("Skin0", skinTemplates.Values.First().Name);
            Assert.AreEqual("Skin0", skinTemplates.Values.First().TemplateFolder);
        }

        [Test]
        public void GetSkinTemplates_WithSpecialFolders_IgnoresSpecialFolders()
        {
            //arrange
            var directories = new List<VirtualDirectory>();
            var nonSkinDir = new Mock<VirtualDirectory>("~/skins/_system");
            nonSkinDir.Setup(d => d.Name).Returns("_system");
            directories.Add(nonSkinDir.Object);
            var skinDir = new Mock<VirtualDirectory>("~/skins/skin1");
            skinDir.Setup(d => d.Name).Returns("Skin1");
            directories.Add(skinDir.Object);
            var skinsDir = new Mock<VirtualDirectory>("~/skins");
            skinsDir.Setup(s => s.Directories).Returns(directories);
            var vpp = new Mock<VirtualPathProvider>();
            vpp.Setup(v => v.GetDirectory("~/skins")).Returns(skinsDir.Object);
            var skins = new SkinEngine(vpp.Object);

            //act
            IDictionary<string, SkinTemplate> skinTemplates = skins.GetSkinTemplates(mobileOnly: false);

            //assert
            Assert.AreEqual(1, skinTemplates.Count);
            Assert.AreEqual("Skin1", skinTemplates.Values.First().Name);
        }

        [Test]
        public void GetSkinTemplates_WithSkinConfigInFolder_AppliesConfig()
        {
            //arrange
            var virtualFile = new Mock<VirtualFile>("~/skins/skin1/skin.config");
            Stream stream =
                @"<?xml version=""1.0""?>
<SkinTemplates xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"">
    <SkinTemplate Name=""Skinny"" StyleMergeMode=""MergedAfter"" ScriptMergeMode=""Merge"">
      <Styles>
        <Style href=""~/skins/_System/commonstyle.css"" />
      </Styles>
    </SkinTemplate>
</SkinTemplates>"
                    .ToStream();
            virtualFile.Setup(vf => vf.Open()).Returns(stream);

            var directories = new List<VirtualDirectory>();
            var skinDir = new Mock<VirtualDirectory>("~/skins/skin1");
            skinDir.Setup(d => d.Name).Returns("Skin1");
            directories.Add(skinDir.Object);
            var skinsDir = new Mock<VirtualDirectory>("~/skins");
            skinsDir.Setup(s => s.Directories).Returns(directories);
            var vpp = new Mock<VirtualPathProvider>();
            vpp.Setup(v => v.GetDirectory("~/skins")).Returns(skinsDir.Object);
            vpp.Setup(v => v.FileExists("~/skins/Skin1/skin.config")).Returns(true);
            vpp.Setup(v => v.GetFile("~/skins/Skin1/skin.config")).Returns(virtualFile.Object);
            var skins = new SkinEngine(vpp.Object);

            //act
            IDictionary<string, SkinTemplate> skinTemplates = skins.GetSkinTemplates(mobileOnly: false);

            //assert
            Assert.AreEqual(1, skinTemplates.Count);
            SkinTemplate template = skinTemplates.Values.First();
            Assert.AreEqual("Skinny", template.Name);
            Assert.AreEqual("Skin1", template.TemplateFolder);
            Assert.AreEqual(StyleMergeMode.MergedAfter, template.StyleMergeMode);
            Assert.AreEqual(ScriptMergeMode.Merge, template.ScriptMergeMode);
            Assert.AreEqual(1, template.Styles.Count());
        }

        [Test]
        public void GetSkinTemplates_WithMobileOnlyTrue_ReturnsSkinWithMobileSupportSetToMobileOnly()
        {
            //arrange
            var virtualFile = new Mock<VirtualFile>("~/skins/skin1/skin.config");
            Stream stream =
                @"<?xml version=""1.0""?>
<SkinTemplates xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"">
    <SkinTemplate Name=""Mobile"" MobileSupport=""MobileOnly"">
      <Styles>
        <Style href=""~/skins/_System/commonstyle.css"" />
      </Styles>
    </SkinTemplate>
</SkinTemplates>"
                    .ToStream();
            virtualFile.Setup(vf => vf.Open()).Returns(stream);

            var directories = new List<VirtualDirectory>();
            var skinDir = new Mock<VirtualDirectory>("~/skins/skin1");
            skinDir.Setup(d => d.Name).Returns("Skin1");
            directories.Add(skinDir.Object);
            var skinsDir = new Mock<VirtualDirectory>("~/skins");
            skinsDir.Setup(s => s.Directories).Returns(directories);
            var vpp = new Mock<VirtualPathProvider>();
            vpp.Setup(v => v.GetDirectory("~/skins")).Returns(skinsDir.Object);
            vpp.Setup(v => v.FileExists("~/skins/Skin1/skin.config")).Returns(true);
            vpp.Setup(v => v.GetFile("~/skins/Skin1/skin.config")).Returns(virtualFile.Object);
            var skins = new SkinEngine(vpp.Object);

            //act
            IDictionary<string, SkinTemplate> skinTemplates = skins.GetSkinTemplates(mobileOnly: true);

            //assert
            Assert.AreEqual(1, skinTemplates.Count);
            SkinTemplate template = skinTemplates.Values.First();
            Assert.AreEqual("Mobile", template.Name);
        }

        [Test]
        public void GetSkinTemplates_WithMobileOnlyTrue_ReturnsSkinWithMobileSupportSetToSupported()
        {
            //arrange
            var virtualFile = new Mock<VirtualFile>("~/skins/skin1/skin.config");
            Stream stream =
                @"<?xml version=""1.0""?>
<SkinTemplates xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"">
    <SkinTemplate Name=""Mobile"" MobileSupport=""Supported"">
      <Styles>
        <Style href=""~/skins/_System/commonstyle.css"" />
      </Styles>
    </SkinTemplate>
</SkinTemplates>"
                    .ToStream();
            virtualFile.Setup(vf => vf.Open()).Returns(stream);

            var directories = new List<VirtualDirectory>();
            var skinDir = new Mock<VirtualDirectory>("~/skins/skin1");
            skinDir.Setup(d => d.Name).Returns("Skin1");
            directories.Add(skinDir.Object);
            var skinsDir = new Mock<VirtualDirectory>("~/skins");
            skinsDir.Setup(s => s.Directories).Returns(directories);
            var vpp = new Mock<VirtualPathProvider>();
            vpp.Setup(v => v.GetDirectory("~/skins")).Returns(skinsDir.Object);
            vpp.Setup(v => v.FileExists("~/skins/Skin1/skin.config")).Returns(true);
            vpp.Setup(v => v.GetFile("~/skins/Skin1/skin.config")).Returns(virtualFile.Object);
            var skins = new SkinEngine(vpp.Object);

            //act
            IDictionary<string, SkinTemplate> skinTemplates = skins.GetSkinTemplates(mobileOnly: true);

            //assert
            Assert.AreEqual(1, skinTemplates.Count);
            SkinTemplate template = skinTemplates.Values.First();
            Assert.AreEqual("Mobile", template.Name);
        }

        [Test]
        public void GetSkinTemplates_WithMobileOnlyFalse_ReturnsSkinWithMobileSupportSetToSupported()
        {
            //arrange
            var virtualFile = new Mock<VirtualFile>("~/skins/skin1/skin.config");
            Stream stream =
                @"<?xml version=""1.0""?>
<SkinTemplates xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"">
    <SkinTemplate Name=""Mobile"" MobileSupport=""Supported"">
      <Styles>
        <Style href=""~/skins/_System/commonstyle.css"" />
      </Styles>
    </SkinTemplate>
</SkinTemplates>"
                    .ToStream();
            virtualFile.Setup(vf => vf.Open()).Returns(stream);

            var directories = new List<VirtualDirectory>();
            var skinDir = new Mock<VirtualDirectory>("~/skins/skin1");
            skinDir.Setup(d => d.Name).Returns("Skin1");
            directories.Add(skinDir.Object);
            var skinsDir = new Mock<VirtualDirectory>("~/skins");
            skinsDir.Setup(s => s.Directories).Returns(directories);
            var vpp = new Mock<VirtualPathProvider>();
            vpp.Setup(v => v.GetDirectory("~/skins")).Returns(skinsDir.Object);
            vpp.Setup(v => v.FileExists("~/skins/Skin1/skin.config")).Returns(true);
            vpp.Setup(v => v.GetFile("~/skins/Skin1/skin.config")).Returns(virtualFile.Object);
            var skins = new SkinEngine(vpp.Object);

            //act
            IDictionary<string, SkinTemplate> skinTemplates = skins.GetSkinTemplates(mobileOnly: false);

            //assert
            Assert.AreEqual(1, skinTemplates.Count);
            SkinTemplate template = skinTemplates.Values.First();
            Assert.AreEqual("Mobile", template.Name);
        }

        [Test]
        public void GetSkinTemplates_WithMobileOnlyTrue_DoesNotReturnSkinThatDoesNotSupportMobile()
        {
            //arrange
            var virtualFile = new Mock<VirtualFile>("~/skins/skin1/skin.config");
            Stream stream =
                @"<?xml version=""1.0""?>
    <SkinTemplates xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"">
        <SkinTemplate Name=""Skinny"" MobileSupported=""None"">
          <Styles>
            <Style href=""~/skins/_System/commonstyle.css"" />
          </Styles>
        </SkinTemplate>
    </SkinTemplates>"
                    .ToStream();
            virtualFile.Setup(vf => vf.Open()).Returns(stream);

            var directories = new List<VirtualDirectory>();
            var skinDir = new Mock<VirtualDirectory>("~/skins/skin1");
            skinDir.Setup(d => d.Name).Returns("Skin1");
            directories.Add(skinDir.Object);
            var skinsDir = new Mock<VirtualDirectory>("~/skins");
            skinsDir.Setup(s => s.Directories).Returns(directories);
            var vpp = new Mock<VirtualPathProvider>();
            vpp.Setup(v => v.GetDirectory("~/skins")).Returns(skinsDir.Object);
            vpp.Setup(v => v.FileExists("~/skins/Skin1/skin.config")).Returns(true);
            vpp.Setup(v => v.GetFile("~/skins/Skin1/skin.config")).Returns(virtualFile.Object);
            var skins = new SkinEngine(vpp.Object);

            //act
            IDictionary<string, SkinTemplate> skinTemplates = skins.GetSkinTemplates(mobileOnly: true);

            //assert
            Assert.AreEqual(0, skinTemplates.Count);
        }
    }
}