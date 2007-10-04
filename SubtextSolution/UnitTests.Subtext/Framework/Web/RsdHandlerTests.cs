using System;
using System.Text;
using System.Web;
using System.Xml;
using MbUnit.Framework;
using Subtext.Framework;
using Subtext.Framework.Web.Handlers;
using Subtext.Framework.Web.HttpModules;
using Subtext.TestLibrary;

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
			
			BlogInfo blog = new BlogInfo();
			blog.Id = 8675309;
			blog.Subfolder = subfolder;
			blog.Host = host;

			using (BlogRequestSimulator.SimulateRequest(blog, host, application, subfolder))
			{
				RsdHandler.WriteRsd(writer, blog);

				//Now lets assert some things.
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
				Assert.AreEqual(expected, node.Attributes["apiLink"].Value, "Api link is wrong");

				Assert.AreEqual("8675309", node.Attributes["blogID"].Value, "Blog Id is not set.");
			}
		}
	}
}
