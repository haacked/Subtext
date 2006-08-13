using System;
using System.Text;
using System.Xml;
using MbUnit.Framework;
using Subtext.Framework;
using Subtext.Framework.Web.Handlers;

namespace UnitTests.Subtext.Framework.Web
{
	[TestFixture]
	public class RsdHandlerTests
	{
		[RowTest]
		[Row("localhost", "", "blog", "http://localhost/blog/services/metablogapi.aspx")]
		[Row("localhost", "Subtext.Web", "blog", "http://localhost/Subtext.Web/blog/services/metablogapi.aspx")]
		[Row("localhost", "Subtext.Web", "", "http://localhost/Subtext.Web/services/metablogapi.aspx")]
		[Row("localhost", "", "", "http://localhost/services/metablogapi.aspx")]
		public void WriteRsdWritesTheCorrectRSD(string host, string application, string subfolder, string expected)
		{
			StringBuilder builder = new StringBuilder();
			XmlWriter writer = XmlWriter.Create(builder);
			RsdHandler handler = new RsdHandler();

			BlogInfo blog = new BlogInfo();
			blog.Subfolder = subfolder;
			blog.Host = host;
			UnitTestHelper.SetHttpContextWithBlogRequest(host, subfolder, application);
			handler.WriteRsd(writer, blog);
			
			//Now lets assert some things.
			XmlDocument xml = new XmlDocument();
			xml.LoadXml(builder.ToString());

			XmlNamespaceManager nsmgr = new XmlNamespaceManager(xml.NameTable);
			nsmgr.AddNamespace("rsd", "http://archipelago.phrasewise.com/rsd");

			XmlNode engineNameNode = xml.SelectSingleNode("/rsd:rsd/rsd:service/rsd:engineName", nsmgr);
			Assert.IsNotNull(engineNameNode, "Could not find the engineName node.");
			Assert.AreEqual(engineNameNode.InnerText, "Subtext");
			XmlNode node = xml.SelectSingleNode("/rsd:rsd/rsd:service/rsd:apis/rsd:api[@name='MetaWeblog']", nsmgr);
			Assert.IsNotNull(node, "Could not find the metaweblog node.");
			Assert.AreEqual(expected, node.Attributes["apiLink"].Value, "Api link is wrong");
		}
	}
}
