using System.Xml;
using MbUnit.Framework;
using Moq;
using SubtextUpgrader;

namespace SubtextUpgraderTests
{
    [TestFixture]
    public class WebConfigUpgraderTests
    {
        [Test]
        public void UpgradeConfig_WithSourceAndDestinationDirectory_UpgradesWebConfig()
        {
            // arrange
            const string newConfig = @"<configuration><connectionStrings><connectionString name=""default"" /></connectionStrings><Foo /></configuration>";
            const string oldConfig = @"<configuration><connectionStrings><connectionString name=""mine"" /></connectionStrings></configuration>";
            var newConfigFile = new Mock<IFile>();
            
            var stream = new NonDisposableMemoryStream();
            newConfigFile.Setup(f => f.OpenWrite()).Returns(stream);
            newConfigFile.Setup(f => f.Contents).Returns(newConfig);
            var existingFileConfig = new Mock<IFile>();
            newConfigFile.Setup(f => f.Overwrite(existingFileConfig.Object));
            existingFileConfig.Setup(f => f.Contents).Returns(oldConfig);
            existingFileConfig.Setup(f => f.Exists).Returns(true);
            var sourceDirectory = new Mock<IDirectory>();
            sourceDirectory.Setup(s => s.CombineFile("Web.config")).Returns(newConfigFile.Object);
            var destinationDirectory = new Mock<IDirectory>();
            destinationDirectory.Setup(d => d.CombineFile("Web.config")).Returns(existingFileConfig.Object);
            var upgrader = new WebConfigUpgrader(sourceDirectory.Object);

            // act
            upgrader.UpgradeConfig(destinationDirectory.Object);

            // assert
            const string expected = @"<configuration>
  <connectionStrings>
    <connectionString name=""mine"" />
  </connectionStrings>
  <Foo />
</configuration>";
            Assert.AreEqual(expected, stream.ToStringContents());
        }

        [Test]
        public void OverwriteChildren_DoesJustThat()
        {
            // arrange
            const string sourceXml = @"<?xml version=""1.0""?>
<configuration>
	<configSections>
        <section name=""BlogConfigurationSettings"" type=""Subtext.Framework.Util.XmlSerializerSectionHandler, Subtext.Framework"" requirePermission=""false""/>
    </configSections>
	<connectionStrings>
		<clear/>
        <add name=""subtextData"" connectionString=""SOURCE-Connection-String""/>
        <add name=""subtextData"" connectionString=""SOURCE2-Connection-String""/>
	</connectionStrings>
</configuration>";
            const string destinationXml = @"<?xml version=""1.0""?>
<configuration>
	<configSections>
    </configSections>
	<connectionStrings>
		<clear/>
        <add name=""subtextData"" connectionString=""DEST-Connection-String""/>
	</connectionStrings>
</configuration>";
            var source = new XmlDocument();
            source.LoadXml(sourceXml);
            var destination = new XmlDocument();
            destination.LoadXml(destinationXml);

            // act
            WebConfigUpgrader.OverwriteChildren(doc => doc.SelectConnectionStringsNode(), source, destination);

            // assert
            Assert.AreEqual(@"<?xml version=""1.0""?><configuration><configSections></configSections><connectionStrings><add name=""subtextData"" connectionString=""SOURCE2-Connection-String"" /></connectionStrings></configuration>", destination.OuterXml);
        }
    }
}
