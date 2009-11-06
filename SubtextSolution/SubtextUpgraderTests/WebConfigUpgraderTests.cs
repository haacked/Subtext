using System.Xml;
using MbUnit.Framework;
using SubtextUpgrader;

namespace SubtextUpgraderTests
{
    [TestFixture]
    public class WebConfigUpgraderTests
    {
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
