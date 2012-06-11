using System.Xml;
using MbUnit.Framework;
using SubtextUpgrader;

namespace SubtextUpgraderTests
{
    [TestFixture]
    public class XmlExtensionTests
    {
        [Test]
        public void SelectConnectionStringsNode_GrabsConnectionStringsNode_FromWebConfig()
        {
            // arrange
            const string configXml = @"<?xml version=""1.0""?>
<configuration>
	<configSections>
    </configSections>
	<connectionStrings>
		<clear/>
        <add name=""subtextData"" connectionString=""Connection-String""/>
	</connectionStrings>
</configuration>";
            var xml = new XmlDocument();
            xml.LoadXml(configXml);
            
            // act
            var node = xml.SelectConnectionStringsNode();

            // assert
            Assert.AreEqual(@"<connectionStrings><clear /><add name=""subtextData"" connectionString=""Connection-String"" /></connectionStrings>", node.OuterXml);
        }
    }
}
