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
using System.Collections.Generic;
using System.Globalization;
using System.Web;
using MbUnit.Framework;
using Subtext.Framework.Syndication;
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
	public class RssWriterTests : SyndicationTestBase
	{
		const int PacificTimeZoneId = -2037797565;
		
		[RowTest]
		[Row("Subtext.Web", "", "http://localhost/Subtext.Web/images/RSS2Image.gif")]
		[Row("Subtext.Web", "blog", "http://localhost/Subtext.Web/images/RSS2Image.gif")]
		[Row("", "", "http://localhost/images/RSS2Image.gif")]
		[Row("", "blog", "http://localhost/images/RSS2Image.gif")]
		[RollBack]
		public void RssImageUrlConcatenatedProperly(string application, string subfolder, string expected)
		{
			UnitTestHelper.SetHttpContextWithBlogRequest("localhost", subfolder, application);
			BlogInfo blogInfo = new BlogInfo();
			blogInfo.Host = "localhost";
			blogInfo.Subfolder = subfolder;
			blogInfo.Title = "My Blog Is Better Than Yours";
			blogInfo.Email = "Subtext@example.com";
			blogInfo.RFC3229DeltaEncodingEnabled = true;

			HttpContext.Current.Items.Add("BlogInfo-", blogInfo);
			
			RssWriter writer = new RssWriter(new List<Entry>(), DateTime.Now, false);
			Uri rssImageUrl = writer.GetRssImage();
			Assert.AreEqual(expected, rssImageUrl.ToString(), "not the expected url.");			
		}
		
		/// <summary>
		/// Tests writing a simple RSS feed.
		/// </summary>
		[Test]
		[RollBack]
		public void RssWriterProducesValidFeed()
		{
			UnitTestHelper.SetHttpContextWithBlogRequest("localhost", "", "Subtext.Web");

			BlogInfo blogInfo = new BlogInfo();
			blogInfo.Host = "localhost";
			blogInfo.Title = "My Blog Is Better Than Yours";
			blogInfo.Email = "Subtext@example.com";
			blogInfo.RFC3229DeltaEncodingEnabled = true;
			blogInfo.TimeZoneId = PacificTimeZoneId;
			
			HttpContext.Current.Items.Add("BlogInfo-", blogInfo);

		    List<Entry> entries = new List<Entry>(CreateSomeEntries());
            entries[0].Categories.AddRange(new string[] { "Category1", "Category2" });
			entries[0].Email = "nobody@example.com";
            entries[2].Categories.Add("Category 3");

            Enclosure enc = new Enclosure();

		    enc.Url = "http://perseus.franklins.net/hanselminutes_0107.mp3";
		    enc.Title = "<Digital Photography Explained (for Geeks) with Aaron Hockley/>";
		    enc.Size = 26707573;
		    enc.MimeType = "audio/mp3";
		    enc.AddToFeed = true;


		    entries[2].Enclosure = enc;

            Enclosure enc1 = new Enclosure();

            enc1.Url = "http://perseus.franklins.net/hanselminutes_0107.mp3";
            enc1.Title = "<Digital Photography Explained (for Geeks) with Aaron Hockley/>";
            enc1.Size = 26707573;
            enc1.MimeType = "audio/mp3";
            enc1.AddToFeed = false;

		    entries[3].Enclosure = enc1;

			RssWriter writer = new RssWriter(entries, NullValue.NullDateTime, false);

			string expected = @"<rss version=""2.0"" "
									+ @"xmlns:dc=""http://purl.org/dc/elements/1.1/"" "
									+ @"xmlns:trackback=""http://madskills.com/public/xml/rss/module/trackback/"" "
									+ @"xmlns:wfw=""http://wellformedweb.org/CommentAPI/"" "
									+ @"xmlns:slash=""http://purl.org/rss/1.0/modules/slash/"" "
									+ @"xmlns:copyright=""http://blogs.law.harvard.edu/tech/rss"" "
									+ @"xmlns:image=""http://purl.org/rss/1.0/modules/image/"">" + Environment.NewLine
								+ indent() + @"<channel>" + Environment.NewLine
										+ indent(2) + @"<title>My Blog Is Better Than Yours</title>" + Environment.NewLine
										+ indent(2) + @"<link>http://localhost/Subtext.Web/Default.aspx</link>" + Environment.NewLine
										+ indent(2) + @"<description />" + Environment.NewLine
										+ indent(2) + @"<language>en-US</language>" + Environment.NewLine
										+ indent(2) + @"<copyright>Subtext Weblog</copyright>" + Environment.NewLine
										+ indent(2) + @"<managingEditor>Subtext@example.com</managingEditor>" + Environment.NewLine
										+ indent(2) + @"<generator>{0}</generator>" + Environment.NewLine
										+ indent(2) + @"<image>" + Environment.NewLine
											+ indent(3) + @"<title>My Blog Is Better Than Yours</title>" + Environment.NewLine
											+ indent(3) + @"<url>http://localhost/Subtext.Web/images/RSS2Image.gif</url>" + Environment.NewLine
											+ indent(3) + @"<link>http://localhost/Subtext.Web/Default.aspx</link>" + Environment.NewLine
											+ indent(3) + @"<width>77</width>" + Environment.NewLine
											+ indent(3) + @"<height>60</height>" + Environment.NewLine
										+ indent(2) + @"</image>" + Environment.NewLine
										+ indent(2) + @"<item>" + Environment.NewLine
											+ indent(3) + @"<title>Title of 1001.</title>" + Environment.NewLine
											+ indent(3) + @"<category>Category1</category>" + Environment.NewLine
											+ indent(3) + @"<category>Category2</category>" + Environment.NewLine
											+ indent(3) + @"<link>http://localhost/Subtext.Web/archive/1975/01/23/1001.aspx</link>" + Environment.NewLine
											+ indent(3) + @"<description>Body of 1001&lt;img src=""http://localhost/Subtext.Web/aggbug/1001.aspx"" width=""1"" height=""1"" /&gt;</description>" + Environment.NewLine
											+ indent(3) + @"<dc:creator>Phil Haack</dc:creator>" + Environment.NewLine
											+ indent(3) + @"<guid>http://localhost/Subtext.Web/archive/1975/01/23/1001.aspx</guid>" + Environment.NewLine
											+ indent(3) + @"<pubDate>Thu, 23 Jan 1975 08:00:00 GMT</pubDate>" + Environment.NewLine
											+ indent(3) + @"<comments>http://localhost/Subtext.Web/archive/1975/01/23/1001.aspx#feedback</comments>" + Environment.NewLine
											+ indent(3) + @"<wfw:commentRss>http://localhost/Subtext.Web/comments/commentRss/1001.aspx</wfw:commentRss>" + Environment.NewLine
										+ indent(2) + @"</item>" + Environment.NewLine
										+ indent(2) + @"<item>" + Environment.NewLine
											+ indent(3) + @"<title>Title of 1002.</title>" + Environment.NewLine
											+ indent(3) + @"<link>http://localhost/Subtext.Web/archive/1976/05/25/1002.aspx</link>" + Environment.NewLine
											+ indent(3) + @"<description>Body of 1002&lt;img src=""http://localhost/Subtext.Web/aggbug/1002.aspx"" width=""1"" height=""1"" /&gt;</description>" + Environment.NewLine
											+ indent(3) + @"<dc:creator>Phil Haack</dc:creator>" + Environment.NewLine
											+ indent(3) + @"<guid>http://localhost/Subtext.Web/archive/1976/05/25/1002.aspx</guid>" + Environment.NewLine
											+ indent(3) + @"<pubDate>Tue, 25 May 1976 07:00:00 GMT</pubDate>" + Environment.NewLine
											+ indent(3) + @"<comments>http://localhost/Subtext.Web/archive/1976/05/25/1002.aspx#feedback</comments>" + Environment.NewLine
											+ indent(3) + @"<wfw:commentRss>http://localhost/Subtext.Web/comments/commentRss/1002.aspx</wfw:commentRss>" + Environment.NewLine
										+ indent(2) + @"</item>" + Environment.NewLine
										+ indent(2) + @"<item>" + Environment.NewLine
											+ indent(3) + @"<title>Title of 1003.</title>" + Environment.NewLine
											+ indent(3) + @"<category>Category 3</category>" + Environment.NewLine
											+ indent(3) + @"<link>http://localhost/Subtext.Web/archive/1979/09/16/1003.aspx</link>" + Environment.NewLine
											+ indent(3) + @"<description>Body of 1003&lt;img src=""http://localhost/Subtext.Web/aggbug/1003.aspx"" width=""1"" height=""1"" /&gt;</description>" + Environment.NewLine
											+ indent(3) + @"<dc:creator>Phil Haack</dc:creator>" + Environment.NewLine
											+ indent(3) + @"<guid>http://localhost/Subtext.Web/archive/1979/09/16/1003.aspx</guid>" + Environment.NewLine
											+ indent(3) + @"<pubDate>Sun, 16 Sep 1979 07:00:00 GMT</pubDate>" + Environment.NewLine
											+ indent(3) + @"<comments>http://localhost/Subtext.Web/archive/1979/09/16/1003.aspx#feedback</comments>" + Environment.NewLine
											+ indent(3) + @"<wfw:commentRss>http://localhost/Subtext.Web/comments/commentRss/1003.aspx</wfw:commentRss>" + Environment.NewLine
                                            + indent(3) + @"<enclosure url=""http://perseus.franklins.net/hanselminutes_0107.mp3"" length=""26707573"" type=""audio/mp3"" />" + Environment.NewLine
										+ indent(2) + @"</item>" + Environment.NewLine
										+ indent(2) + @"<item>" + Environment.NewLine
											+ indent(3) + @"<title>Title of 1004.</title>" + Environment.NewLine
											+ indent(3) + @"<link>http://localhost/Subtext.Web/archive/2003/06/14/1004.aspx</link>" + Environment.NewLine
											+ indent(3) + @"<description>Body of 1004&lt;img src=""http://localhost/Subtext.Web/aggbug/1004.aspx"" width=""1"" height=""1"" /&gt;</description>" + Environment.NewLine
											+ indent(3) + @"<dc:creator>Phil Haack</dc:creator>" + Environment.NewLine
											+ indent(3) + @"<guid>http://localhost/Subtext.Web/archive/2003/06/14/1004.aspx</guid>" + Environment.NewLine
											+ indent(3) + @"<pubDate>Sat, 14 Jun 2003 07:00:00 GMT</pubDate>" + Environment.NewLine
											+ indent(3) + @"<comments>http://localhost/Subtext.Web/archive/2003/06/14/1004.aspx#feedback</comments>" + Environment.NewLine
											+ indent(3) + @"<wfw:commentRss>http://localhost/Subtext.Web/comments/commentRss/1004.aspx</wfw:commentRss>" + Environment.NewLine
										+ indent(2) + @"</item>" + Environment.NewLine
								+ indent() + @"</channel>" + Environment.NewLine
							  + @"</rss>";

			expected = string.Format(expected, VersionInfo.VersionDisplayText);
			
			Console.WriteLine(expected);
			Console.WriteLine(writer.Xml);
			UnitTestHelper.AssertStringsEqualCharacterByCharacter(expected, writer.Xml);
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
			UnitTestHelper.SetHttpContextWithBlogRequest("localhost", "", "Subtext.Web");

			BlogInfo blogInfo = new BlogInfo();
			blogInfo.Host = "localhost";
			blogInfo.Subfolder = "";
			blogInfo.Email = "Subtext@example.com";
			blogInfo.RFC3229DeltaEncodingEnabled = true;
			blogInfo.TimeZoneId = PacificTimeZoneId;

			HttpContext.Current.Items.Add("BlogInfo-", blogInfo);

            List<Entry> entries = new List<Entry>(CreateSomeEntriesDescending());
			// Tell the write we already received 1002 published 5/25/1976.
			RssWriter writer = new RssWriter(entries, DateTime.ParseExact("05/25/1976","MM/dd/yyyy",CultureInfo.InvariantCulture), true);

			// We only expect 1003 and 1004
			string expected = @"<rss version=""2.0"" xmlns:dc=""http://purl.org/dc/elements/1.1/"" xmlns:trackback=""http://madskills.com/public/xml/rss/module/trackback/"" xmlns:wfw=""http://wellformedweb.org/CommentAPI/"" xmlns:slash=""http://purl.org/rss/1.0/modules/slash/"" xmlns:copyright=""http://blogs.law.harvard.edu/tech/rss"" xmlns:image=""http://purl.org/rss/1.0/modules/image/"">" + Environment.NewLine
								+ indent() + "<channel>" + Environment.NewLine
									+ indent(2) + "<title />" + Environment.NewLine
									+ indent(2) + "<link>http://localhost/Subtext.Web/Default.aspx</link>" + Environment.NewLine
									+ indent(2) + "<description />" + Environment.NewLine
									+ indent(2) + "<language>en-US</language>" + Environment.NewLine
									+ indent(2) + "<copyright>Subtext Weblog</copyright>" + Environment.NewLine
									+ indent(2) + "<managingEditor>Subtext@example.com</managingEditor>" + Environment.NewLine
									+ indent(2) + "<generator>{0}</generator>" + Environment.NewLine
									+ indent(2) + "<image>" + Environment.NewLine
										+ indent(3) + "<title />" + Environment.NewLine
										+ indent(3) + "<url>http://localhost/Subtext.Web/images/RSS2Image.gif</url>" + Environment.NewLine
										+ indent(3) + "<link>http://localhost/Subtext.Web/Default.aspx</link>" + Environment.NewLine
										+ indent(3) + "<width>77</width>" + Environment.NewLine
										+ indent(3) + "<height>60</height>" + Environment.NewLine
									+ indent(2) + "</image>" + Environment.NewLine
									+ indent(2) + @"<item>" + Environment.NewLine
										+ indent(3) + @"<title>Title of 1004.</title>" + Environment.NewLine
										+ indent(3) + @"<link>http://localhost/Subtext.Web/archive/2003/06/14/1004.aspx</link>" + Environment.NewLine
										+ indent(3) + @"<description>Body of 1004&lt;img src=""http://localhost/Subtext.Web/aggbug/1004.aspx"" width=""1"" height=""1"" /&gt;</description>" + Environment.NewLine
										+ indent(3) + @"<dc:creator>Phil Haack</dc:creator>" + Environment.NewLine
										+ indent(3) + @"<guid>http://localhost/Subtext.Web/archive/2003/06/14/1004.aspx</guid>" + Environment.NewLine
										+ indent(3) + @"<pubDate>Sat, 14 Jun 2003 07:00:00 GMT</pubDate>" + Environment.NewLine
										+ indent(3) + @"<comments>http://localhost/Subtext.Web/archive/2003/06/14/1004.aspx#feedback</comments>" + Environment.NewLine
										+ indent(3) + @"<wfw:commentRss>http://localhost/Subtext.Web/comments/commentRss/1004.aspx</wfw:commentRss>" + Environment.NewLine
									+ indent(2) + @"</item>" + Environment.NewLine
									+ indent(2) + @"<item>" + Environment.NewLine
										+ indent(3) + "<title>Title of 1003.</title>" + Environment.NewLine
										+ indent(3) + "<link>http://localhost/Subtext.Web/archive/1979/09/16/1003.aspx</link>" + Environment.NewLine
										+ indent(3) + @"<description>Body of 1003&lt;img src=""http://localhost/Subtext.Web/aggbug/1003.aspx"" width=""1"" height=""1"" /&gt;</description>" + Environment.NewLine
										+ indent(3) + @"<dc:creator>Phil Haack</dc:creator>" + Environment.NewLine
										+ indent(3) + @"<guid>http://localhost/Subtext.Web/archive/1979/09/16/1003.aspx</guid>" + Environment.NewLine
										+ indent(3) + @"<pubDate>Sun, 16 Sep 1979 07:00:00 GMT</pubDate>" + Environment.NewLine
										+ indent(3) + @"<comments>http://localhost/Subtext.Web/archive/1979/09/16/1003.aspx#feedback</comments>" + Environment.NewLine
										+ indent(3) + @"<wfw:commentRss>http://localhost/Subtext.Web/comments/commentRss/1003.aspx</wfw:commentRss>" + Environment.NewLine
									+ indent(2) + "</item>" + Environment.NewLine
								+ indent() + "</channel>" + Environment.NewLine
			                  + "</rss>";
			
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
			UnitTestHelper.SetHttpContextWithBlogRequest("localhost", "", "Subtext.Web");

			BlogInfo blogInfo = new BlogInfo();
			blogInfo.Host = "localhost";
			blogInfo.Subfolder = "";
			blogInfo.Email = "Subtext@example.com";
			blogInfo.RFC3229DeltaEncodingEnabled = false;
			blogInfo.TimeZoneId = PacificTimeZoneId;
			
			HttpContext.Current.Items.Add("BlogInfo-", blogInfo);

            List<Entry> entries = new List<Entry>(CreateSomeEntriesDescending());		
			RssWriter writer = new RssWriter(entries, DateTime.ParseExact("06/14/2003", "MM/dd/yyyy", CultureInfo.InvariantCulture), false);

			string expected = @"<rss version=""2.0"" xmlns:dc=""http://purl.org/dc/elements/1.1/"" xmlns:trackback=""http://madskills.com/public/xml/rss/module/trackback/"" xmlns:wfw=""http://wellformedweb.org/CommentAPI/"" xmlns:slash=""http://purl.org/rss/1.0/modules/slash/"" xmlns:copyright=""http://blogs.law.harvard.edu/tech/rss"" xmlns:image=""http://purl.org/rss/1.0/modules/image/"">" + Environment.NewLine 
								+ indent() + "<channel>" + Environment.NewLine
									+ indent(2) + "<title />" + Environment.NewLine
									+ indent(2) + "<link>http://localhost/Subtext.Web/Default.aspx</link>" + Environment.NewLine
									+ indent(2) + "<description />" + Environment.NewLine
									+ indent(2) + "<language>en-US</language>" + Environment.NewLine
									+ indent(2) + "<copyright>Subtext Weblog</copyright>" + Environment.NewLine
									+ indent(2) + "<managingEditor>Subtext@example.com</managingEditor>" + Environment.NewLine
									+ indent(2) + "<generator>{0}</generator>" + Environment.NewLine
									+ indent(2) + "<image>" + Environment.NewLine
										+ indent(3) + "<title />" + Environment.NewLine
										+ indent(3) + "<url>http://localhost/Subtext.Web/images/RSS2Image.gif</url>" + Environment.NewLine
										+ indent(3) + "<link>http://localhost/Subtext.Web/Default.aspx</link>" + Environment.NewLine
										+ indent(3) + "<width>77</width>" + Environment.NewLine
										+ indent(3) + "<height>60</height>" + Environment.NewLine
									+ indent(2) + "</image>" + Environment.NewLine
									+ indent(2) + @"<item>" + Environment.NewLine
										+ indent(3) + "<title>Title of 1004.</title>" + Environment.NewLine
										+ indent(3) + "<link>http://localhost/Subtext.Web/archive/2003/06/14/1004.aspx</link>" + Environment.NewLine
										+ indent(3) + @"<description>Body of 1004&lt;img src=""http://localhost/Subtext.Web/aggbug/1004.aspx"" width=""1"" height=""1"" /&gt;</description>" + Environment.NewLine
										+ indent(3) + "<dc:creator>Phil Haack</dc:creator>" + Environment.NewLine
										+ indent(3) + "<guid>http://localhost/Subtext.Web/archive/2003/06/14/1004.aspx</guid>" + Environment.NewLine
										+ indent(3) + "<pubDate>Sat, 14 Jun 2003 07:00:00 GMT</pubDate>" + Environment.NewLine
										+ indent(3) + "<comments>http://localhost/Subtext.Web/archive/2003/06/14/1004.aspx#feedback</comments>" + Environment.NewLine
										+ indent(3) + "<wfw:commentRss>http://localhost/Subtext.Web/comments/commentRss/1004.aspx</wfw:commentRss>" + Environment.NewLine
									+ indent(2) + "</item>" + Environment.NewLine
									+ indent(2) + "<item>" + Environment.NewLine
										+ indent(3) + "<title>Title of 1003.</title>" + Environment.NewLine
										+ indent(3) + @"<link>http://localhost/Subtext.Web/archive/1979/09/16/1003.aspx</link>" + Environment.NewLine
										+ indent(3) + @"<description>Body of 1003&lt;img src=""http://localhost/Subtext.Web/aggbug/1003.aspx"" width=""1"" height=""1"" /&gt;</description>" + Environment.NewLine
										+ indent(3) + "<dc:creator>Phil Haack</dc:creator>" + Environment.NewLine
										+ indent(3) + "<guid>http://localhost/Subtext.Web/archive/1979/09/16/1003.aspx</guid>" + Environment.NewLine
										+ indent(3) + "<pubDate>Sun, 16 Sep 1979 07:00:00 GMT</pubDate>" + Environment.NewLine
										+ indent(3) + "<comments>http://localhost/Subtext.Web/archive/1979/09/16/1003.aspx#feedback</comments>" + Environment.NewLine
										+ indent(3) + "<wfw:commentRss>http://localhost/Subtext.Web/comments/commentRss/1003.aspx</wfw:commentRss>" + Environment.NewLine
									+ indent(2) + "</item>" + Environment.NewLine
									+ indent(2) + @"<item>" + Environment.NewLine
										+ indent(3) + "<title>Title of 1002.</title>" + Environment.NewLine
										+ indent(3) + "<link>http://localhost/Subtext.Web/archive/1976/05/25/1002.aspx</link>" + Environment.NewLine
										+ indent(3) + @"<description>Body of 1002&lt;img src=""http://localhost/Subtext.Web/aggbug/1002.aspx"" width=""1"" height=""1"" /&gt;</description>" + Environment.NewLine
										+ indent(3) + "<dc:creator>Phil Haack</dc:creator>" + Environment.NewLine
										+ indent(3) + "<guid>http://localhost/Subtext.Web/archive/1976/05/25/1002.aspx</guid>" + Environment.NewLine
										+ indent(3) + "<pubDate>Tue, 25 May 1976 07:00:00 GMT</pubDate>" + Environment.NewLine
										+ indent(3) + "<comments>http://localhost/Subtext.Web/archive/1976/05/25/1002.aspx#feedback</comments>" + Environment.NewLine
										+ indent(3) + "<wfw:commentRss>http://localhost/Subtext.Web/comments/commentRss/1002.aspx</wfw:commentRss>" + Environment.NewLine
									+ indent(2) + "</item>" + Environment.NewLine
									+ indent(2) + @"<item>" + Environment.NewLine
										+ indent(3) + "<title>Title of 1001.</title>" + Environment.NewLine
										+ indent(3) + "<link>http://localhost/Subtext.Web/archive/1975/01/23/1001.aspx</link>" + Environment.NewLine
										+ indent(3) + @"<description>Body of 1001&lt;img src=""http://localhost/Subtext.Web/aggbug/1001.aspx"" width=""1"" height=""1"" /&gt;</description>" + Environment.NewLine
										+ indent(3) + "<dc:creator>Phil Haack</dc:creator>" + Environment.NewLine
										+ indent(3) + "<guid>http://localhost/Subtext.Web/archive/1975/01/23/1001.aspx</guid>" + Environment.NewLine
										+ indent(3) + "<pubDate>Thu, 23 Jan 1975 08:00:00 GMT</pubDate>" + Environment.NewLine
										+ indent(3) + "<comments>http://localhost/Subtext.Web/archive/1975/01/23/1001.aspx#feedback</comments>" + Environment.NewLine
										+ indent(3) + "<wfw:commentRss>http://localhost/Subtext.Web/comments/commentRss/1001.aspx</wfw:commentRss>" + Environment.NewLine
									+ indent(2) + "</item>" + Environment.NewLine
								+ indent() + "</channel>" + Environment.NewLine
			                  + "</rss>";
			expected = string.Format(expected, VersionInfo.VersionDisplayText);
		    UnitTestHelper.AssertStringsEqualCharacterByCharacter(expected, writer.Xml);
		}

		Entry[] CreateSomeEntries()
		{
			return new Entry[]
			{
				CreateEntry(1001, "Title of 1001.", "Body of 1001", DateTime.ParseExact("01/23/1975", "MM/dd/yyyy", CultureInfo.InvariantCulture))
				,CreateEntry(1002, "Title of 1002.", "Body of 1002", DateTime.ParseExact("05/25/1976", "MM/dd/yyyy", CultureInfo.InvariantCulture))
				,CreateEntry(1003, "Title of 1003.", "Body of 1003", DateTime.ParseExact("09/16/1979", "MM/dd/yyyy", CultureInfo.InvariantCulture))
				,CreateEntry(1004, "Title of 1004.", "Body of 1004", DateTime.ParseExact("06/14/2003", "MM/dd/yyyy", CultureInfo.InvariantCulture))
			};
		}

		Entry[] CreateSomeEntriesDescending()
		{
			return new Entry[]
			{
				CreateEntry(1004, "Title of 1004.", "Body of 1004", DateTime.ParseExact("06/14/2003", "MM/dd/yyyy", CultureInfo.InvariantCulture))
				,CreateEntry(1003, "Title of 1003.", "Body of 1003", DateTime.ParseExact("09/16/1979", "MM/dd/yyyy", CultureInfo.InvariantCulture))
				,CreateEntry(1002, "Title of 1002.", "Body of 1002", DateTime.ParseExact("05/25/1976", "MM/dd/yyyy", CultureInfo.InvariantCulture))
				,CreateEntry(1001, "Title of 1001.", "Body of 1001", DateTime.ParseExact("01/23/1975", "MM/dd/yyyy", CultureInfo.InvariantCulture))
				
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
			entry.DateModified = entry.DateCreated;
			entry.Title = title;
			entry.Author = "Phil Haack";
			entry.Body = body;
			entry.Id = id;
			entry.Url = string.Format(CultureInfo.InvariantCulture, "http://localhost/Subtext.Web/archive/{0:yyyy/MM/dd}/{1}", dateCreated, id);
			entry.DateSyndicated = entry.DateCreated;
			
			return entry;
		}

		/// <summary>
		/// Sets the up test fixture.  This is called once for 
		/// this test fixture before all the tests run.
		/// </summary>
		[TestFixtureSetUp]
		public void SetUpTestFixture()
		{
			//Confirm app settings
            UnitTestHelper.AssertAppSettings();
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
