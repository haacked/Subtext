using System;
using System.Text;
using System.Xml;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Subtext.Framework;
using Subtext.Framework.Routing;
using Subtext.Framework.Web.Handlers;

namespace UnitTests.Subtext.Framework.Web
{
    [TestClass]
    public class RsdHandlerTests
    {
        [TestMethod]
        public void WriteRsdWritesTheCorrectRSD()
        {
            //arrange
            var blog = new Blog();
            blog.Id = 8675309;
            blog.Subfolder = "sub";
            blog.Host = "example.com";

            var urlHelper = new Mock<BlogUrlHelper>();
            urlHelper.Setup(u => u.BlogUrl()).Returns("/");
            urlHelper.Setup(u => u.MetaWeblogApiUrl(blog)).Returns(
                new Uri("http://example.com/sub/services/metablogapi.aspx"));

            var builder = new StringBuilder();
            XmlWriter writer = XmlWriter.Create(builder);
            var handler = new RsdHandler(null);

            //act
            handler.WriteRsd(writer, blog, urlHelper.Object);

            //assert
            var xml = new XmlDocument();
            Console.WriteLine(builder);
            xml.LoadXml(builder.ToString());

            var nsmgr = new XmlNamespaceManager(xml.NameTable);
            nsmgr.AddNamespace("rsd", "http://archipelago.phrasewise.com/rsd");

            XmlNode rootRsdNode = xml.SelectSingleNode("/rsd:rsd", nsmgr);
            Assert.IsNotNull(rootRsdNode, "Could not find the root RSD node.");
            Assert.AreEqual("1.0", rootRsdNode.Attributes["version"].InnerText,
                            "Expected the version attribute to be '1.0'");

            XmlNode engineNameNode = xml.SelectSingleNode("/rsd:rsd/rsd:service/rsd:engineName", nsmgr);
            Assert.IsNotNull(engineNameNode, "Could not find the engineName node.");
            Assert.AreEqual(engineNameNode.InnerText, "Subtext");
            XmlNode node = xml.SelectSingleNode("/rsd:rsd/rsd:service/rsd:apis/rsd:api[@name='MetaWeblog']", nsmgr);
            Assert.IsNotNull(node, "Could not find the metaweblog node.");
            Assert.AreEqual("http://example.com/sub/services/metablogapi.aspx", node.Attributes["apiLink"].Value,
                            "Api link is wrong");

            Assert.AreEqual("8675309", node.Attributes["blogID"].Value, "Blog Id is not set.");
        }
    }
}