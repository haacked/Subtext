using System;
using System.Text;
using System.Web;
using System.Xml;
using MbUnit.Framework;
using Subtext.Framework;
using Subtext.Framework.Web.Handlers;
using Moq;
using Subtext.Framework.Routing;

namespace UnitTests.Subtext.Framework.Web
{
	[TestFixture]
	public class RsdHandlerTests
	{
		[Test]
		public void WriteRsdWritesTheCorrectRSD()
		{
            //arrange
            Blog blog = new Blog();
            blog.Id = 8675309;
            blog.Subfolder = "sub";
            blog.Host = "example.com";

            var urlHelper = new Mock<UrlHelper>();
            urlHelper.Setup(u => u.BlogUrl()).Returns("/");
            urlHelper.Setup(u => u.MetaWeblogApiUrl(blog)).Returns(new Uri("http://example.com/sub/services/metablogapi.aspx"));

			StringBuilder builder = new StringBuilder();
			XmlWriter writer = XmlWriter.Create(builder);
			RsdHandler handler = new RsdHandler();

            //act
			handler.WriteRsd(writer, blog, urlHelper.Object);
			
			//assert
			XmlDocument xml = new XmlDocument();
			Console.WriteLine(builder);
			xml.LoadXml(builder.ToString());

			XmlNamespaceManager nsmgr = new XmlNamespaceManager(xml.NameTable);
			nsmgr.AddNamespace("rsd", "http://archipelago.phrasewise.com/rsd");

			XmlNode rootRsdNode = xml.SelectSingleNode("/rsd:rsd", nsmgr);
			Assert.IsNotNull(rootRsdNode, "Could not find the root RSD node.");
			Assert.AreEqual("1.0", rootRsdNode.Attributes["version"].InnerText, "Expected the version attribute to be '1.0'");
			
			XmlNode engineNameNode = xml.SelectSingleNode("/rsd:rsd/rsd:service/rsd:engineName", nsmgr);
			Assert.IsNotNull(engineNameNode, "Could not find the engineName node.");
			Assert.AreEqual(engineNameNode.InnerText, "Subtext");
			XmlNode node = xml.SelectSingleNode("/rsd:rsd/rsd:service/rsd:apis/rsd:api[@name='MetaWeblog']", nsmgr);
			Assert.IsNotNull(node, "Could not find the metaweblog node.");
            Assert.AreEqual("http://example.com/sub/services/metablogapi.aspx", node.Attributes["apiLink"].Value, "Api link is wrong");

			Assert.AreEqual("8675309", node.Attributes["blogID"].Value, "Blog Id is not set.");
		}
	}
}