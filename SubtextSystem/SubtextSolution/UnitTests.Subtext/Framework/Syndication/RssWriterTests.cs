using System;
using NUnit.Framework;
using Subtext.Common.Syndication;
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
			EntryCollection entries = new EntryCollection(CreateSomeEntries());		
			RssWriter writer = new RssWriter(entries, int.MinValue);

			string expected = @"<rss version=""2.0"" xmlns:dc=""http://purl.org/dc/elements/1.1/"" xmlns:trackback=""http://madskills.com/public/xml/rss/module/trackback/"" xmlns:wfw=""http://wellformedweb.org/CommentAPI/"" xmlns:slash=""http://purl.org/rss/1.0/modules/slash/"" xmlns:copyright=""http://blogs.law.harvard.edu/tech/rss"" xmlns:image=""http://purl.org/rss/1.0/modules/image/"">" 
				+ @"<channel><title /><link /><description /><managingEditor>Subtext Weblog</managingEditor><dc:language>en-US</dc:language><generator>" + VersionInfo.Version + @"</generator><copyright>Subtext Weblog</copyright><image><title /><url>RSS2Image.gif</url><link /><width>77</width><height>60</height><description /></image><item><dc:creator>Phil Haack</dc:creator><title>Title of 1001.</title><link>http://localhost/Subtext.Web/archive/YYYY/00/23/1001</link><pubDate>Thu, 23 Jan 1975 00:00:00 GMT</pubDate><guid>http://localhost/Subtext.Web/archive/YYYY/00/23/1001</guid><description>Body of 1001&lt;img src =""aggbug/1001.aspx"" width = ""1"" height = ""1"" /&gt;</description></item><item><dc:creator>Phil Haack</dc:creator><title>Title of 1002.</title><link>http://localhost/Subtext.Web/archive/YYYY/00/25/1002</link><pubDate>Tue, 25 May 1976 00:00:00 GMT</pubDate><guid>http://localhost/Subtext.Web/archive/YYYY/00/25/1002</guid><description>Body of 1002&lt;img src =""aggbug/1002.aspx"" width = ""1"" height = ""1"" /&gt;</description></item><item><dc:creator>Phil Haack</dc:creator><title>Title of 1003.</title><link>http://localhost/Subtext.Web/archive/YYYY/00/16/1003</link><pubDate>Sun, 16 Sep 1979 00:00:00 GMT</pubDate><guid>http://localhost/Subtext.Web/archive/YYYY/00/16/1003</guid><description>Body of 1003&lt;img src =""aggbug/1003.aspx"" width = ""1"" height = ""1"" /&gt;</description></item><item><dc:creator>Phil Haack</dc:creator><title>Title of 1004.</title><link>http://localhost/Subtext.Web/archive/YYYY/00/14/1004</link><pubDate>Sat, 14 Jun 2003 00:00:00 GMT</pubDate><guid>http://localhost/Subtext.Web/archive/YYYY/00/14/1004</guid><description>Body of 1004&lt;img src =""aggbug/1004.aspx"" width = ""1"" height = ""1"" /&gt;</description></item></channel></rss>";
			Assert.AreEqual(expected, writer.Xml);			
		}

		/// <summary>
		/// Makes sure the RSS Writer can write the delta of a feed based 
		/// on the RFC3229 with feeds 
		/// <see href="http://bobwyman.pubsub.com/main/2004/09/using_rfc3229_w.html"/>.
		/// </summary>
		[Test]
		public void RssWriterHandlesRFC3229DeltaEncoding()
		{
			EntryCollection entries = new EntryCollection(CreateSomeEntries());		
			RssWriter writer = new RssWriter(entries, 1002);
			
			string expected = @"<rss version=""2.0"" xmlns:dc=""http://purl.org/dc/elements/1.1/"" xmlns:trackback=""http://madskills.com/public/xml/rss/module/trackback/"" xmlns:wfw=""http://wellformedweb.org/CommentAPI/"" xmlns:slash=""http://purl.org/rss/1.0/modules/slash/"" xmlns:copyright=""http://blogs.law.harvard.edu/tech/rss"" xmlns:image=""http://purl.org/rss/1.0/modules/image/"">" 
				+ @"<channel><title /><link /><description /><managingEditor>Subtext Weblog</managingEditor><dc:language>en-US</dc:language><generator>" + VersionInfo.Version + @"</generator><copyright>Subtext Weblog</copyright><image><title /><url>RSS2Image.gif</url><link /><width>77</width><height>60</height><description /></image><item><dc:creator>Phil Haack</dc:creator><title>Title of 1003.</title><link>http://localhost/Subtext.Web/archive/YYYY/00/16/1003</link><pubDate>Sun, 16 Sep 1979 00:00:00 GMT</pubDate><guid>http://localhost/Subtext.Web/archive/YYYY/00/16/1003</guid><description>Body of 1003&lt;img src =""aggbug/1003.aspx"" width = ""1"" height = ""1"" /&gt;</description></item><item><dc:creator>Phil Haack</dc:creator><title>Title of 1004.</title><link>http://localhost/Subtext.Web/archive/YYYY/00/14/1004</link><pubDate>Sat, 14 Jun 2003 00:00:00 GMT</pubDate><guid>http://localhost/Subtext.Web/archive/YYYY/00/14/1004</guid><description>Body of 1004&lt;img src =""aggbug/1004.aspx"" width = ""1"" height = ""1"" /&gt;</description></item></channel></rss>";
			Assert.AreEqual(expected, writer.Xml);

			Assert.AreEqual(1002, writer.LastViewedFeedItemId, "The Item ID Last Viewed (according to If-None-Since is wrong.");
			Assert.AreEqual(1004, writer.LatestFeedItemId, "The Latest Feed Item ID sent to the client is wrong.");
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

		Entry CreateEntry(int id, string title, string body, DateTime dateCreated)
		{
			Entry entry = new Entry(PostType.BlogPost);
			entry.DateCreated = dateCreated;
			entry.DateUpdated = entry.DateCreated;
			entry.Title = title;
			entry.Author = "Phil Haack";
			entry.Body = body;
			entry.EntryID = id;
			entry.Link = "http://localhost/Subtext.Web/archive/" + dateCreated.ToString("YYYY/mm/dd") + "/" + id;
			
			return entry;
		}

		[SetUp]
		public void SetUp()
		{
			Config.ConfigurationProvider = new UnitTestConfigProvider();

			//This file needs to be there already.
			UnitTestHelper.UnpackEmbeddedResource("App.config", "UnitTests.Subtext.dll.config");
			
			//Confirm app settings
			Assert.AreEqual("~/Admin/Resources/PageTemplate.ascx", System.Configuration.ConfigurationSettings.AppSettings["Admin.DefaultTemplate"]) ;
		}

		[TearDown]
		public void TearDown()
		{
		}
	}
}
