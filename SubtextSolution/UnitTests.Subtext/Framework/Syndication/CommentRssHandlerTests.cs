using System;
using System.IO;
using System.Text;
using System.Web;
using System.Xml;
using MbUnit.Framework;
using Subtext.Framework.Syndication;
using Subtext.Framework;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;

namespace UnitTests.Subtext.Framework.Syndication
{
	/// <summary>
	/// Tests the CommentRSS HttpHandler.
	/// </summary>
	[TestFixture]
	public class CommentRssHandlerTests
	{
		/// <summary>
		/// <para>
		/// Makes sure that the CommentRssHandler produces a valid RSS feed even if 
		/// the entry has no feed items.
		/// This test is in response to case [1446934] 
		/// </para>
		/// <para>
		/// https://sourceforge.net/tracker/index.php?func=detail&amp;aid=1446934&amp;group_id=137896&amp;atid=739979"
		/// </para>
		/// </summary>
		[Test]
		[RollBack]
		public void CommentRssHandlerProducesValidEmptyFeed()
		{
			string hostName = UnitTestHelper.GenerateUniqueHostname();
			Config.CreateBlog("Test", "username", "password", hostName, string.Empty);

			StringBuilder sb = new StringBuilder();
			TextWriter output = new StringWriter(sb);
			
			DateTime dateCreated = DateTime.Now;
			
			UnitTestHelper.SetHttpContextWithBlogRequest(hostName, "", "", "", output);
			Config.CurrentBlog.Email = "Subtext@example.com";
			Config.CurrentBlog.RFC3229DeltaEncodingEnabled = false;
			
			Entry entry = UnitTestHelper.CreateEntryInstanceForSyndication("Author", "Best post EVER", "testbody", null, dateCreated);
			int id = Entries.Create(entry); //persist to db.
			UnitTestHelper.SetHttpContextWithBlogRequest(hostName, "", "", string.Format("/2006/04/01/{0}.aspx", id), output);

			RssCommentHandler handler = new RssCommentHandler();
			handler.ProcessRequest(HttpContext.Current);
			HttpContext.Current.Response.Flush();

			string rssOutput = sb.ToString();
			XmlDocument doc = new XmlDocument();
			doc.LoadXml(rssOutput);
			Console.Write(rssOutput);
			
			XmlNodeList titleNodes = doc.SelectNodes("/rss/channel/title");
			Assert.IsNotNull(titleNodes, "The title node should not be null.");
			Assert.AreEqual("Best post EVER", titleNodes[0].InnerText, "Did not get the expected value of the title node.");
		}
	}
}
