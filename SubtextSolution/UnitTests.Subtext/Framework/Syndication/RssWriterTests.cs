#region Disclaimer/Info
///////////////////////////////////////////////////////////////////////////////////////////////////
// Subtext WebLog
// 
// Subtext is an open source weblog system that is a fork of the .TEXT
// weblog system.
//
// For updated news and information please visit http://subtextproject.com/
// Subtext is hosted at SourceForge at http://sourceforge.net/projects/subtext
// The development mailing list is at subtext-devs@lists.sourceforge.net 
//
// This project is licensed under the BSD license.  See the License.txt file for more information.
///////////////////////////////////////////////////////////////////////////////////////////////////
#endregion

using System;
using System.Globalization;
using System.Web;
using MbUnit.Framework;
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
		[RollBack]
		public void RssWriterProducesValidFeed()
		{
			UnitTestHelper.SetHttpContextWithBlogRequest("localhost", "Subtext.Web");

			BlogInfo blogInfo = new BlogInfo();
			blogInfo.Host = "localhost";
			blogInfo.Title = "My Blog Is Better Than Yours";
			blogInfo.Subfolder = "Subtext.Web";
			blogInfo.Email = "Subtext@example.com";
			blogInfo.RFC3229DeltaEncodingEnabled = true;

			HttpContext.Current.Items.Add("BlogInfo-", blogInfo);

			EntryCollection entries = new EntryCollection(CreateSomeEntries());		
			RssWriter writer = new RssWriter(entries, NullValue.NullDateTime, false);

			string expected = @"<rss version=""2.0"" " 
									+ @"xmlns:dc=""http://purl.org/dc/elements/1.1/"" " 
									+ @"xmlns:trackback=""http://madskills.com/public/xml/rss/module/trackback/"" " 
									+ @"xmlns:wfw=""http://wellformedweb.org/CommentAPI/"" " 
									+ @"xmlns:slash=""http://purl.org/rss/1.0/modules/slash/"" " 
									+ @"xmlns:copyright=""http://blogs.law.harvard.edu/tech/rss"" " 
									+ @"xmlns:image=""http://purl.org/rss/1.0/modules/image/"">" 
								+ @"<channel>" 
										+ @"<title>My Blog Is Better Than Yours</title>" 
										+ @"<link>http://localhost/Subtext.Web/Default.aspx</link>" 
										+ @"<description />" 
										+ @"<language>en-US</language>" 
										+ @"<copyright>Subtext Weblog</copyright>" 
										+ @"<managingEditor>Subtext@example.com</managingEditor>" 
										+ @"<generator>{0}</generator>" 
										+ @"<image>" 
											+ @"<title>My Blog Is Better Than Yours</title>" 
											+ @"<url>http://localhost/Subtext.Web/RSS2Image.gif</url>" 
											+ @"<link>http://localhost/Subtext.Web/Default.aspx</link>" 
											+ @"<width>77</width>" 
											+ @"<height>60</height>" 
											+ @"<description />" 
										+ @"</image>" 
										+ @"<item><title>Title of 1001.</title><link>http://localhost/Subtext.Web/archive/1975/01/23/1001.aspx</link><description>Body of 1001&lt;img src=""http://localhost/Subtext.Web/aggbug/1001.aspx"" width=""1"" height=""1"" /&gt;</description><guid>http://localhost/Subtext.Web/archive/1975/01/23/1001.aspx</guid><pubDate>Thu, 23 Jan 1975 00:00:00 GMT</pubDate><comments>http://localhost/Subtext.Web/archive/1975/01/23/1001.aspx#feedback</comments><wfw:commentRss>http://localhost/Subtext.Web/comments/commentRss/1001.aspx</wfw:commentRss></item>" 
										+ @"<item><title>Title of 1002.</title><link>http://localhost/Subtext.Web/archive/1976/05/25/1002.aspx</link><description>Body of 1002&lt;img src=""http://localhost/Subtext.Web/aggbug/1002.aspx"" width=""1"" height=""1"" /&gt;</description><guid>http://localhost/Subtext.Web/archive/1976/05/25/1002.aspx</guid><pubDate>Tue, 25 May 1976 00:00:00 GMT</pubDate><comments>http://localhost/Subtext.Web/archive/1976/05/25/1002.aspx#feedback</comments><wfw:commentRss>http://localhost/Subtext.Web/comments/commentRss/1002.aspx</wfw:commentRss></item>" 
										+ @"<item><title>Title of 1003.</title><link>http://localhost/Subtext.Web/archive/1979/09/16/1003.aspx</link><description>Body of 1003&lt;img src=""http://localhost/Subtext.Web/aggbug/1003.aspx"" width=""1"" height=""1"" /&gt;</description><guid>http://localhost/Subtext.Web/archive/1979/09/16/1003.aspx</guid><pubDate>Sun, 16 Sep 1979 00:00:00 GMT</pubDate><comments>http://localhost/Subtext.Web/archive/1979/09/16/1003.aspx#feedback</comments><wfw:commentRss>http://localhost/Subtext.Web/comments/commentRss/1003.aspx</wfw:commentRss></item>" 
										+ @"<item><title>Title of 1004.</title><link>http://localhost/Subtext.Web/archive/2003/06/14/1004.aspx</link><description>Body of 1004&lt;img src=""http://localhost/Subtext.Web/aggbug/1004.aspx"" width=""1"" height=""1"" /&gt;</description><guid>http://localhost/Subtext.Web/archive/2003/06/14/1004.aspx</guid><pubDate>Sat, 14 Jun 2003 00:00:00 GMT</pubDate><comments>http://localhost/Subtext.Web/archive/2003/06/14/1004.aspx#feedback</comments><wfw:commentRss>http://localhost/Subtext.Web/comments/commentRss/1004.aspx</wfw:commentRss></item>" 
								+ @"</channel>" 
			                  + @"</rss>";

			expected = string.Format(expected, VersionInfo.VersionDisplayText);
			
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
		[RollBack]
		public void RssWriterHandlesRFC3229DeltaEncoding()
		{
			UnitTestHelper.SetHttpContextWithBlogRequest("localhost", "Subtext.Web");

			BlogInfo blogInfo = new BlogInfo();
			blogInfo.Host = "localhost";
			blogInfo.Subfolder = "Subtext.Web";
			blogInfo.Email = "Subtext@example.com";
			blogInfo.RFC3229DeltaEncodingEnabled = true;

			HttpContext.Current.Items.Add("BlogInfo-", blogInfo);

			EntryCollection entries = new EntryCollection(CreateSomeEntriesDescending());
			// Tell the write we already received 1002 published 5/25/1976.
			RssWriter writer = new RssWriter(entries, DateTime.ParseExact("05/25/1976","MM/dd/yyyy",CultureInfo.InvariantCulture), true);
			
			// We only expect 1003 and 1004
			string expected = @"<rss version=""2.0"" xmlns:dc=""http://purl.org/dc/elements/1.1/"" xmlns:trackback=""http://madskills.com/public/xml/rss/module/trackback/"" xmlns:wfw=""http://wellformedweb.org/CommentAPI/"" xmlns:slash=""http://purl.org/rss/1.0/modules/slash/"" xmlns:copyright=""http://blogs.law.harvard.edu/tech/rss"" xmlns:image=""http://purl.org/rss/1.0/modules/image/""><channel><title /><link>http://localhost/Subtext.Web/Default.aspx</link><description /><language>en-US</language><copyright>Subtext Weblog</copyright><managingEditor>Subtext@example.com</managingEditor><generator>{0}</generator><image><title /><url>http://localhost/Subtext.Web/RSS2Image.gif</url><link>http://localhost/Subtext.Web/Default.aspx</link><width>77</width><height>60</height><description /></image>" 
							+ @"<item><title>Title of 1004.</title><link>http://localhost/Subtext.Web/archive/2003/06/14/1004.aspx</link><description>Body of 1004&lt;img src=""http://localhost/Subtext.Web/aggbug/1004.aspx"" width=""1"" height=""1"" /&gt;</description><guid>http://localhost/Subtext.Web/archive/2003/06/14/1004.aspx</guid><pubDate>Sat, 14 Jun 2003 00:00:00 GMT</pubDate><comments>http://localhost/Subtext.Web/archive/2003/06/14/1004.aspx#feedback</comments><wfw:commentRss>http://localhost/Subtext.Web/comments/commentRss/1004.aspx</wfw:commentRss></item>" 
							+ @"<item><title>Title of 1003.</title><link>http://localhost/Subtext.Web/archive/1979/09/16/1003.aspx</link><description>Body of 1003&lt;img src=""http://localhost/Subtext.Web/aggbug/1003.aspx"" width=""1"" height=""1"" /&gt;</description><guid>http://localhost/Subtext.Web/archive/1979/09/16/1003.aspx</guid><pubDate>Sun, 16 Sep 1979 00:00:00 GMT</pubDate><comments>http://localhost/Subtext.Web/archive/1979/09/16/1003.aspx#feedback</comments><wfw:commentRss>http://localhost/Subtext.Web/comments/commentRss/1003.aspx</wfw:commentRss></item></channel></rss>";
			
			expected = string.Format(expected, VersionInfo.VersionDisplayText);

			Assert.AreEqual(expected, writer.Xml);

			Assert.AreEqual(DateTime.ParseExact("05/25/1976","MM/dd/yyyy",CultureInfo.InvariantCulture), writer.DateLastViewedFeedItemPublished, "The Item ID Last Viewed (according to If-None-Since is wrong.");
			Assert.AreEqual(DateTime.ParseExact("06/14/2003","MM/dd/yyyy",CultureInfo.InvariantCulture), writer.LatestPublishDate, "The Latest Feed Item ID sent to the client is wrong.");
		}

		/// <summary>
		/// Tests writing a simple RSS feed.
		/// </summary>
		[Test]
		[RollBack]
		public void RssWriterSendsWholeFeedWhenRFC3229Disabled()
		{
			UnitTestHelper.SetHttpContextWithBlogRequest("localhost", "Subtext.Web");

			BlogInfo blogInfo = new BlogInfo();
			blogInfo.Host = "localhost";
			blogInfo.Subfolder = "Subtext.Web";
			blogInfo.Email = "Subtext@example.com";
			blogInfo.RFC3229DeltaEncodingEnabled = false;

			HttpContext.Current.Items.Add("BlogInfo-", blogInfo);

			EntryCollection entries = new EntryCollection(CreateSomeEntriesDescending());		
			RssWriter writer = new RssWriter(entries, DateTime.ParseExact("06/14/2003", "MM/dd/yyyy", CultureInfo.InvariantCulture), false);

			string expected = @"<rss version=""2.0"" xmlns:dc=""http://purl.org/dc/elements/1.1/"" xmlns:trackback=""http://madskills.com/public/xml/rss/module/trackback/"" xmlns:wfw=""http://wellformedweb.org/CommentAPI/"" xmlns:slash=""http://purl.org/rss/1.0/modules/slash/"" xmlns:copyright=""http://blogs.law.harvard.edu/tech/rss"" xmlns:image=""http://purl.org/rss/1.0/modules/image/""><channel><title /><link>http://localhost/Subtext.Web/Default.aspx</link><description /><language>en-US</language><copyright>Subtext Weblog</copyright><managingEditor>Subtext@example.com</managingEditor><generator>{0}</generator><image><title /><url>http://localhost/Subtext.Web/RSS2Image.gif</url><link>http://localhost/Subtext.Web/Default.aspx</link><width>77</width><height>60</height><description /></image>" 
							+ @"<item><title>Title of 1004.</title><link>http://localhost/Subtext.Web/archive/2003/06/14/1004.aspx</link><description>Body of 1004&lt;img src=""http://localhost/Subtext.Web/aggbug/1004.aspx"" width=""1"" height=""1"" /&gt;</description><guid>http://localhost/Subtext.Web/archive/2003/06/14/1004.aspx</guid><pubDate>Sat, 14 Jun 2003 00:00:00 GMT</pubDate><comments>http://localhost/Subtext.Web/archive/2003/06/14/1004.aspx#feedback</comments><wfw:commentRss>http://localhost/Subtext.Web/comments/commentRss/1004.aspx</wfw:commentRss></item>"
							+ @"<item><title>Title of 1003.</title><link>http://localhost/Subtext.Web/archive/1979/09/16/1003.aspx</link><description>Body of 1003&lt;img src=""http://localhost/Subtext.Web/aggbug/1003.aspx"" width=""1"" height=""1"" /&gt;</description><guid>http://localhost/Subtext.Web/archive/1979/09/16/1003.aspx</guid><pubDate>Sun, 16 Sep 1979 00:00:00 GMT</pubDate><comments>http://localhost/Subtext.Web/archive/1979/09/16/1003.aspx#feedback</comments><wfw:commentRss>http://localhost/Subtext.Web/comments/commentRss/1003.aspx</wfw:commentRss></item>" 
							+ @"<item><title>Title of 1002.</title><link>http://localhost/Subtext.Web/archive/1976/05/25/1002.aspx</link><description>Body of 1002&lt;img src=""http://localhost/Subtext.Web/aggbug/1002.aspx"" width=""1"" height=""1"" /&gt;</description><guid>http://localhost/Subtext.Web/archive/1976/05/25/1002.aspx</guid><pubDate>Tue, 25 May 1976 00:00:00 GMT</pubDate><comments>http://localhost/Subtext.Web/archive/1976/05/25/1002.aspx#feedback</comments><wfw:commentRss>http://localhost/Subtext.Web/comments/commentRss/1002.aspx</wfw:commentRss></item>" 
							+ @"<item><title>Title of 1001.</title><link>http://localhost/Subtext.Web/archive/1975/01/23/1001.aspx</link><description>Body of 1001&lt;img src=""http://localhost/Subtext.Web/aggbug/1001.aspx"" width=""1"" height=""1"" /&gt;</description><guid>http://localhost/Subtext.Web/archive/1975/01/23/1001.aspx</guid><pubDate>Thu, 23 Jan 1975 00:00:00 GMT</pubDate><comments>http://localhost/Subtext.Web/archive/1975/01/23/1001.aspx#feedback</comments><wfw:commentRss>http://localhost/Subtext.Web/comments/commentRss/1001.aspx</wfw:commentRss></item></channel></rss>";
			expected = string.Format(expected, VersionInfo.VersionDisplayText);
			Assert.AreEqual(expected, writer.Xml);			
		}

		Entry[] CreateSomeEntries()
		{
			return new Entry[]
			{
				CreateEntry(1001, "Title of 1001.", "Body of 1001", DateTime.ParseExact("01/23/1975","MM/dd/yyyy",CultureInfo.InvariantCulture))
				,CreateEntry(1002, "Title of 1002.", "Body of 1002", DateTime.ParseExact("05/25/1976","MM/dd/yyyy",CultureInfo.InvariantCulture))
				,CreateEntry(1003, "Title of 1003.", "Body of 1003", DateTime.ParseExact("09/16/1979","MM/dd/yyyy",CultureInfo.InvariantCulture))
				,CreateEntry(1004, "Title of 1004.", "Body of 1004", DateTime.ParseExact("06/14/2003","MM/dd/yyyy",CultureInfo.InvariantCulture))
			};
		}

		Entry[] CreateSomeEntriesDescending()
		{
			return new Entry[]
			{
				CreateEntry(1004, "Title of 1004.", "Body of 1004", DateTime.ParseExact("06/14/2003","MM/dd/yyyy",CultureInfo.InvariantCulture))
				,CreateEntry(1003, "Title of 1003.", "Body of 1003", DateTime.ParseExact("09/16/1979","MM/dd/yyyy",CultureInfo.InvariantCulture))
				,CreateEntry(1002, "Title of 1002.", "Body of 1002", DateTime.ParseExact("05/25/1976","MM/dd/yyyy",CultureInfo.InvariantCulture))
				,CreateEntry(1001, "Title of 1001.", "Body of 1001", DateTime.ParseExact("01/23/1975","MM/dd/yyyy",CultureInfo.InvariantCulture))
				
			};
		}

		Entry CreateEntry(int id, string title, string body, DateTime dateCreated)
		{
			return CreateEntry(id, title, body, null, dateCreated);
		}

		Entry CreateEntry(int id, string title, string body, string entryName, DateTime dateCreated)
		{
			Entry entry = new Entry(PostType.BlogPost);
			if(entryName != null)
			{
				entry.EntryName = entryName;
			}
			entry.DateCreated = dateCreated;
			entry.DateUpdated = entry.DateCreated;
			entry.Title = title;
			entry.Author = "Phil Haack";
			entry.Body = body;
			entry.EntryID = id;
			entry.Url = string.Format(CultureInfo.InvariantCulture, "http://localhost/Subtext.Web/archive/{0:yyyy/MM/dd}/{1}", dateCreated, id);
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
			//Confirm app settings
            Assert.AreEqual("~/Admin/Resources/PageTemplate.ascx", System.Configuration.ConfigurationManager.AppSettings["Admin.DefaultTemplate"]);
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
