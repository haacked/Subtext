using System;
using System.Globalization;
using System.Web;
using System.Xml;
using MbUnit.Framework;
using Moq;
using Subtext.Framework;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;
using Subtext.Framework.Routing;
using Subtext.Framework.Syndication;
using Subtext.Framework.Web.HttpModules;

namespace UnitTests.Subtext.Framework.Syndication
{
	/// <summary>
	/// Summary description for AtomHandlerTests.
	/// </summary>
	[TestFixture]
	public class AtomHandlerTests
	{
		/// <summary>
		/// Tests writing a simple RSS feed from some database entries.
		/// </summary>
		[Test]
		[RollBack]
		public void AtomWriterProducesValidFeedFromDatabase()
		{
			string hostName = UnitTestHelper.GenerateUniqueString();
			Config.CreateBlog("Test", "username", "password", hostName, string.Empty);

			UnitTestHelper.SetHttpContextWithBlogRequest(hostName, "");
            BlogRequest.Current.Blog = Config.GetBlog(hostName, string.Empty);
			Config.CurrentBlog.Email = "Subtext@example.com";
			Config.CurrentBlog.RFC3229DeltaEncodingEnabled = false;

            var dateCreated = DateTime.ParseExact("2008/01/23", "yyyy/MM/dd", CultureInfo.InvariantCulture);
            Entry entry = UnitTestHelper.CreateEntryInstanceForSyndication("Author", "testtitle", "testbody", null, dateCreated);
            
			int id = UnitTestHelper.Create(entry); //persist to db.

			AtomHandler handler = new AtomHandler();

            var subtextContext = new Mock<ISubtextContext>();
            string rssOutput = null;
            subtextContext.FakeSyndicationContext(Config.CurrentBlog, "/", s => rssOutput = s);
            var urlHelper = Mock.Get<UrlHelper>(subtextContext.Object.UrlHelper);
            urlHelper.Setup(u => u.BlogUrl()).Returns("/");
            urlHelper.Setup(u => u.EntryUrl(It.IsAny<Entry>())).Returns("/archive/2008/01/23/testtitle.aspx");

			handler.ProcessRequest(subtextContext.Object);
			HttpContext.Current.Response.Flush();

			XmlDocument doc = new XmlDocument();
			doc.LoadXml(rssOutput);
			XmlNamespaceManager nsmanager = new XmlNamespaceManager(doc.NameTable);
			nsmanager.AddNamespace("atom", "http://www.w3.org/2005/Atom");

			XmlNodeList itemNodes = doc.SelectNodes("/atom:feed/atom:entry", nsmanager);
			Assert.AreEqual(1, itemNodes.Count, "expected one entry node.");

			Assert.AreEqual("testtitle", itemNodes[0].SelectSingleNode("atom:title", nsmanager).InnerText, "Not what we expected for the title.");
			string urlFormat = "http://{0}/archive/2008/01/23/{1}.aspx";

			string expectedUrl = string.Format(urlFormat, hostName, "testtitle");

			Assert.AreEqual(expectedUrl, itemNodes[0].SelectSingleNode("atom:id", nsmanager).InnerText, "Not what we expected for the link.");
			Assert.AreEqual(expectedUrl, itemNodes[0].SelectSingleNode("atom:link/@href", nsmanager).InnerText, "Not what we expected for the link.");
		}
	}
}
