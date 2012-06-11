using System.Linq;
using System.Xml;
using MbUnit.Framework;
using SubtextUpgrader;

namespace SubtextUpgraderTests
{
    [TestFixture]
    public class XmlManipulationsTests
    {
        [Test]
        public void MoveUp_MovesNodeToSiblingOfParent()
        {
            // arrange
            const string xmlText = "<root><parent><child><sub-child /></child></parent></root>";
            var xml = xmlText.ToXml();
            var node = xml.SelectSingleNode("/root/parent/child");

            // act
            node.MoveUp();

            // assert
            Assert.AreEqual("<root><parent></parent><child><sub-child /></child></root>", xml.OuterXml);
        }

        [Test]
        public void MoveUp_WithChildOfRoot_DoesNotMove()
        {
            // arrange
            const string xmlText = "<root><parent><child><sub-child /></child></parent></root>";
            var xml = xmlText.ToXml();
            var node = xml.SelectSingleNode("/root/parent");

            // act
            node.MoveUp();

            // assert
            Assert.AreEqual(xmlText, xml.OuterXml);
        }

        [Test]
        public void ExtractNode_WithChildNode_ReturnsNewXmlDocumentFromNode()
        {
            // arrange
            const string xmlText = "<root><parent><template><stuff /></template></parent></root>";
            var xml = xmlText.ToXml();
            
            // act
            XmlDocument doc = xml.ExtractNodeAsDocument("/root/parent/template"); 

            // assert
            Assert.AreEqual("<template><stuff /></template>", doc.DocumentElement.OuterXml);
        }

        [Test]
        public void ExtractDocuments_WithXmlContainingMultipleNodes_ExtractEachAsDocument()
        {
            const string xmlText = "<root><parent><template><stuff /></template><template><otherstuff /></template></parent></root>";
            var xml = xmlText.ToXml();

            // act
            var docs = xml.ExtractDocuments("/root/parent/template");

            // assert
            Assert.AreEqual("<template><stuff /></template>", docs.First().DocumentElement.OuterXml);
            Assert.AreEqual("<template><otherstuff /></template>", docs.ElementAt(1).DocumentElement.OuterXml);
            Assert.AreEqual(2, docs.Count());
        }

        [Test]
        public void ExtractDocuments_WithTemplateXmlAndInsertionExpath_ExtractsNodesAsChildren()
        {
            const string xmlText = "<root><parent><template><stuff /></template><template><otherstuff /></template></parent></root>";
            var templateXml = "<templates><foo><bar /></foo></templates>".ToXml();
            var xml = xmlText.ToXml();

            // act
            var docs = xml.ExtractDocuments("/root/parent/template", templateXml, "/templates/foo/bar");

            // assert
            Assert.AreEqual("<templates><foo><bar><template><stuff /></template></bar></foo></templates>", docs.First().DocumentElement.OuterXml);
            Assert.AreEqual("<templates><foo><bar><template><otherstuff /></template></bar></foo></templates>", docs.ElementAt(1).DocumentElement.OuterXml);
            Assert.AreEqual(2, docs.Count());
        }


    }
}
