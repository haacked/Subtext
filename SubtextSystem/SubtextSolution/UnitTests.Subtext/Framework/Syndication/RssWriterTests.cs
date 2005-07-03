using System;
using System.Web;
using NUnit.Framework;
using Subtext.Common.Syndication;
using Subtext.Extensibility;
using Subtext.Framework;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;

namespace UnitTests.Subtext.Framework.Syndication
{
	/// <summary>
	/// Unit tests for the RSSWriter classes.
	/// </summary>
	[TestFixture]
	public class RssWriterTests
	{
		/// <summary>
		/// Tests writing a simple RSS feed.
		/// </summary>
		[Test]
		public void RssWriterProducesValidFeed()
		{
			UnitTestHelper.SetHttpContextWithBlogRequest("localhost", "Subtext.Web");

			BlogInfo blogInfo = new BlogInfo();
			blogInfo.Host = "localhost";
			blogInfo.Application = "Subtext.Web";
			blogInfo.Email = "Subtext@example.com";
			blogInfo.RFC3229DeltaEncodingEnabled = true;

			HttpContext.Current.Items.Add("BlogInfo-", blogInfo);

			EntryCollection entries = new EntryCollection(CreateSomeEntries());		
			RssWriter writer = new RssWriter(entries, DateTime.MinValue, false);

			string expected = @"<rss version=""2.0"" xmlns:dc=""http://purl.org/dc/elements/1.1/"" xmlns:trackback=""http://madskills.com/public/xml/rss/module/trackback/"" xmlns:wfw=""http://wellformedweb.org/CommentAPI/"" xmlns:slash=""http://purl.org/rss/1.0/modules/slash/"" xmlns:copyright=""http://blogs.law.harvard.edu/tech/rss"" xmlns:image=""http://purl.org/rss/1.0/modules/image/"">" 
				+ @"<channel><title /><link>http://localhost/Subtext.Web/</link><description /><managingEditor>Subtext@example.com</managingEditor><dc:language>en-US</dc:language><generator>" + VersionInfo.Version + @"</generator><copyright>Subtext Weblog</copyright><image><title /><url>http://localhost/Subtext.Web/RSS2Image.gif</url><link>http://localhost/Subtext.Web/</link><width>77</width><height>60</height><description /></image>" 
				+ @"<item><dc:creator>Phil Haack</dc:creator><title>Title of 1001.</title><link>http://localhost/Subtext.Web/archive/1975/01/23/1001</link><pubDate>Thu, 23 Jan 1975 00:00:00 GMT</pubDate><guid>http://localhost/Subtext.Web/archive/1975/01/23/1001</guid><description>Body of 1001&lt;img src =""http://localhost/Subtext.Web/aggbug/1001.aspx"" width = ""1"" height = ""1"" /&gt;</description></item><item><dc:creator>Phil Haack</dc:creator><title>Title of 1002.</title><link>http://localhost/Subtext.Web/archive/1976/05/25/1002</link><pubDate>Tue, 25 May 1976 00:00:00 GMT</pubDate><guid>http://localhost/Subtext.Web/archive/1976/05/25/1002</guid><description>Body of 1002&lt;img src =""http://localhost/Subtext.Web/aggbug/1002.aspx"" width = ""1"" height = ""1"" /&gt;</description></item><item><dc:creator>Phil Haack</dc:creator><title>Title of 1003.</title><link>http://localhost/Subtext.Web/archive/1979/09/16/1003</link><pubDate>Sun, 16 Sep 1979 00:00:00 GMT</pubDate><guid>http://localhost/Subtext.Web/archive/1979/09/16/1003</guid><description>Body of 1003&lt;img src =""http://localhost/Subtext.Web/aggbug/1003.aspx"" width = ""1"" height = ""1"" /&gt;</description></item><item><dc:creator>Phil Haack</dc:creator><title>Title of 1004.</title><link>http://localhost/Subtext.Web/archive/2003/06/14/1004</link><pubDate>Sat, 14 Jun 2003 00:00:00 GMT</pubDate><guid>http://localhost/Subtext.Web/archive/2003/06/14/1004</guid><description>Body of 1004&lt;img src =""http://localhost/Subtext.Web/aggbug/1004.aspx"" width = ""1"" height = ""1"" /&gt;</description></item></channel></rss>";
			Console.WriteLine(expected);
			Console.WriteLine(writer.Xml);
			Assert.AreEqual(expected, writer.Xml);			
		}

		/// <summary>
		/// Makes sure the RSS Writer can write the delta of a feed based 
		/// on the RFC3229 with feeds 
		/// <see href="http://bobwyman.pubsub.com/main/2004/09/using_rfc3229_w.html"/>.
		/// </summary>
		[Test]
		[Rollback]
		public void RssWriterHandlesRFC3229DeltaEncoding()
		{
			UnitTestHelper.SetHttpContextWithBlogRequest("localhost", "Subtext.Web");

			BlogInfo blogInfo = new BlogInfo();
			blogInfo.Host = "localhost";
			blogInfo.Application = "Subtext.Web";
			blogInfo.Email = "Subtext@example.com";
			blogInfo.RFC3229DeltaEncodingEnabled = true;

			HttpContext.Current.Items.Add("BlogInfo-", blogInfo);

			EntryCollection entries = new EntryCollection(CreateSomeEntriesDescending());
			// Tell the write we already received 1002 published 5/25/1976.
			RssWriter writer = new RssWriter(entries, DateTime.Parse("5/25/1976"), true);
			
			// We only expect 1003 and 1004
			string expected = @"<rss version=""2.0"" xmlns:dc=""http://purl.org/dc/elements/1.1/"" xmlns:trackback=""http://madskills.com/public/xml/rss/module/trackback/"" xmlns:wfw=""http://wellformedweb.org/CommentAPI/"" xmlns:slash=""http://purl.org/rss/1.0/modules/slash/"" xmlns:copyright=""http://blogs.law.harvard.edu/tech/rss"" xmlns:image=""http://purl.org/rss/1.0/modules/image/""><channel><title /><link>http://localhost/Subtext.Web/</link><description /><managingEditor>Subtext@example.com</managingEditor><dc:language>en-US</dc:language><generator>" + VersionInfo.Version + @"</generator><copyright>Subtext Weblog</copyright><image><title /><url>http://localhost/Subtext.Web/RSS2Image.gif</url><link>http://localhost/Subtext.Web/</link><width>77</width><height>60</height><description /></image><item><dc:creator>Phil Haack</dc:creator><title>Title of 1004.</title><link>http://localhost/Subtext.Web/archive/2003/06/14/1004</link><pubDate>Sat, 14 Jun 2003 00:00:00 GMT</pubDate><guid>http://localhost/Subtext.Web/archive/2003/06/14/1004</guid><description>Body of 1004&lt;img src =""http://localhost/Subtext.Web/aggbug/1004.aspx"" width = ""1"" height = ""1"" /&gt;</description></item><item><dc:creator>Phil Haack</dc:creator><title>Title of 1003.</title><link>http://localhost/Subtext.Web/archive/1979/09/16/1003</link><pubDate>Sun, 16 Sep 1979 00:00:00 GMT</pubDate><guid>http://localhost/Subtext.Web/archive/1979/09/16/1003</guid><description>Body of 1003&lt;img src =""http://localhost/Subtext.Web/aggbug/1003.aspx"" width = ""1"" height = ""1"" /&gt;</description></item></channel></rss>";
			
			Console.WriteLine(expected);
			Console.WriteLine(writer.Xml);

			Assert.AreEqual(expected, writer.Xml);

			Assert.AreEqual(DateTime.Parse("5/25/1976"), writer.DateLastViewedFeedItemPublished, "The Item ID Last Viewed (according to If-None-Since is wrong.");
			Assert.AreEqual(DateTime.Parse("6/14/2003"), writer.LatestPublishDate, "The Latest Feed Item ID sent to the client is wrong.");
		}

		/// <summary>
		/// Tests writing a simple RSS feed.
		/// </summary>
		[Test]
		public void RssWriterSendsWholeFeedWhenRFC3229Disabled()
		{
			UnitTestHelper.SetHttpContextWithBlogRequest("localhost", "Subtext.Web");

			BlogInfo blogInfo = new BlogInfo();
			blogInfo.Host = "localhost";
			blogInfo.Application = "Subtext.Web";
			blogInfo.Email = "Subtext@example.com";
			blogInfo.RFC3229DeltaEncodingEnabled = false;

			HttpContext.Current.Items.Add("BlogInfo-", blogInfo);

			EntryCollection entries = new EntryCollection(CreateSomeEntriesDescending());		
			RssWriter writer = new RssWriter(entries, DateTime.Parse("6/14/2003"), false);

			string expected = @"<rss version=""2.0"" xmlns:dc=""http://purl.org/dc/elements/1.1/"" xmlns:trackback=""http://madskills.com/public/xml/rss/module/trackback/"" xmlns:wfw=""http://wellformedweb.org/CommentAPI/"" xmlns:slash=""http://purl.org/rss/1.0/modules/slash/"" xmlns:copyright=""http://blogs.law.harvard.edu/tech/rss"" xmlns:image=""http://purl.org/rss/1.0/modules/image/"">" 
				+ @"<channel><title /><link>http://localhost/Subtext.Web/</link><description /><managingEditor>Subtext@example.com</managingEditor><dc:language>en-US</dc:language><generator>" + VersionInfo.Version + @"</generator><copyright>Subtext Weblog</copyright><image><title /><url>http://localhost/Subtext.Web/RSS2Image.gif</url><link>http://localhost/Subtext.Web/</link><width>77</width><height>60</height><description /></image>" 
				+ @"<item><dc:creator>Phil Haack</dc:creator><title>Title of 1004.</title><link>http://localhost/Subtext.Web/archive/2003/06/14/1004</link><pubDate>Sat, 14 Jun 2003 00:00:00 GMT</pubDate><guid>http://localhost/Subtext.Web/archive/2003/06/14/1004</guid><description>Body of 1004&lt;img src =""http://localhost/Subtext.Web/aggbug/1004.aspx"" width = ""1"" height = ""1"" /&gt;</description></item>" 
				+ @"<item><dc:creator>Phil Haack</dc:creator><title>Title of 1003.</title><link>http://localhost/Subtext.Web/archive/1979/09/16/1003</link><pubDate>Sun, 16 Sep 1979 00:00:00 GMT</pubDate><guid>http://localhost/Subtext.Web/archive/1979/09/16/1003</guid><description>Body of 1003&lt;img src =""http://localhost/Subtext.Web/aggbug/1003.aspx"" width = ""1"" height = ""1"" /&gt;</description></item>" 
				+ @"<item><dc:creator>Phil Haack</dc:creator><title>Title of 1002.</title><link>http://localhost/Subtext.Web/archive/1976/05/25/1002</link><pubDate>Tue, 25 May 1976 00:00:00 GMT</pubDate><guid>http://localhost/Subtext.Web/archive/1976/05/25/1002</guid><description>Body of 1002&lt;img src =""http://localhost/Subtext.Web/aggbug/1002.aspx"" width = ""1"" height = ""1"" /&gt;</description></item>" 
				+ @"<item><dc:creator>Phil Haack</dc:creator><title>Title of 1001.</title><link>http://localhost/Subtext.Web/archive/1975/01/23/1001</link><pubDate>Thu, 23 Jan 1975 00:00:00 GMT</pubDate><guid>http://localhost/Subtext.Web/archive/1975/01/23/1001</guid><description>Body of 1001&lt;img src =""http://localhost/Subtext.Web/aggbug/1001.aspx"" width = ""1"" height = ""1"" /&gt;</description></item>" 
				+ @"</channel></rss>";
			Console.WriteLine(expected);
			Console.WriteLine(writer.Xml);
			Assert.AreEqual(expected, writer.Xml);			
		}

		Entry[] CreateSomeEntries()
		{
			return new Entry[]
			{
				CreateEntry(1001, "Title of 1001.", "Body of 1001", DateTime.Parse("1/23/1975"))
				,CreateEntry(1002, "Title of 1002.", "Body of 1002", DateTime.Parse("5/25/1976"))
				,CreateEntry(1003, "Title of 1003.", "Body of 1003", DateTime.Parse("9/16/1979"))
				,CreateEntry(1004, "Title of 1004.", "Body of 1004", DateTime.Parse("6/14/2003"))
			};
		}

		Entry[] CreateSomeEntriesDescending()
		{
			return new Entry[]
			{
				CreateEntry(1004, "Title of 1004.", "Body of 1004", DateTime.Parse("6/14/2003"))
				,CreateEntry(1003, "Title of 1003.", "Body of 1003", DateTime.Parse("9/16/1979"))
				,CreateEntry(1002, "Title of 1002.", "Body of 1002", DateTime.Parse("5/25/1976"))
				,CreateEntry(1001, "Title of 1001.", "Body of 1001", DateTime.Parse("1/23/1975"))
				
			};
		}

		Entry CreateEntry(int id, string title, string body, DateTime dateCreated)
		{
			Entry entry = new Entry(PostType.BlogPost);
			entry.DateCreated = dateCreated;
			entry.DateUpdated = entry.DateCreated;
			entry.Title = title;
			entry.Author = "Phil Haack";
			entry.Body = body;
			entry.EntryID = id;
			entry.Link = "http://localhost/Subtext.Web/archive/" + dateCreated.ToString("yyyy/MM/dd") + "/" + id;
			entry.DateSyndicated = entry.DateCreated;
			
			return entry;
		}

		/// <summary>
		/// Sets the up test fixture.  This is called once for 
		/// this test fixture before all the tests run.  It 
		/// essentially copies the App.config file to the 
		/// run directory.
		/// </summary>
		[TestFixtureSetUp]
		public void SetUpTestFixture()
		{
			UnitTestHelper.UnpackEmbeddedResource("App.config", "UnitTests.Subtext.dll.config");
			
			//Confirm app settings
			Assert.AreEqual("~/Admin/Resources/PageTemplate.ascx", System.Configuration.ConfigurationSettings.AppSettings["Admin.DefaultTemplate"]) ;
		}

		[SetUp]
		public void SetUp()
		{
		}

		[TearDown]
		public void TearDown()
		{
			Config.ConfigurationProvider = null;
		}
	}
}
