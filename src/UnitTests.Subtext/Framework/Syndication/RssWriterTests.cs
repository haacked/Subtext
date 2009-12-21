#region Disclaimer/Info

///////////////////////////////////////////////////////////////////////////////////////////////////
// Subtext WebLog
// 
// Subtext is an open source weblog system that is a fork of the .TEXT
// weblog system.
//
// For updated news and information please visit http://subtextproject.com/
// Subtext is hosted at Google Code at http://code.google.com/p/subtext/
// The development mailing list is at subtext@googlegroups.com 
//
// This project is licensed under the BSD license.  See the License.txt file for more information.
///////////////////////////////////////////////////////////////////////////////////////////////////

#endregion

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Web;
using MbUnit.Framework;
using Moq;
using Subtext.Extensibility;
using Subtext.Framework;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;
using Subtext.Framework.Routing;
using Subtext.Framework.Syndication;
using Subtext.Framework.Web.HttpModules;
using UnitTests.Subtext.Framework.Util;

namespace UnitTests.Subtext.Framework.Syndication
{
    /// <summary>
    /// Unit tests for the RSSWriter classes.
    /// </summary>
    [TestFixture]
    public class RssWriterTests : SyndicationTestBase
    {
        [RowTest]
        [Row("Subtext.Web", "", "http://localhost/Subtext.Web/images/RSS2Image.gif")]
        [Row("Subtext.Web", "blog", "http://localhost/Subtext.Web/images/RSS2Image.gif")]
        [Row("", "", "http://localhost/images/RSS2Image.gif")]
        [Row("", "blog", "http://localhost/images/RSS2Image.gif")]
        [RollBack]
        public void RssImageUrlConcatenatedProperly(string application, string subfolder, string expected)
        {
            UnitTestHelper.SetHttpContextWithBlogRequest("localhost", subfolder, application);
            var blogInfo = new Blog();
            BlogRequest.Current.Blog = blogInfo;
            blogInfo.Host = "localhost";
            blogInfo.Subfolder = subfolder;
            blogInfo.Title = "My Blog Is Better Than Yours";
            blogInfo.Email = "Subtext@example.com";
            blogInfo.RFC3229DeltaEncodingEnabled = true;

            HttpContext.Current.Items.Add("BlogInfo-", blogInfo);
            var subtextContext = new Mock<ISubtextContext>();
            subtextContext.FakeSyndicationContext(blogInfo, "/", application, null);
            Mock<HttpContextBase> httpContext = Mock.Get(subtextContext.Object.RequestContext.HttpContext);
            httpContext.Setup(h => h.Request.ApplicationPath).Returns(application);

            var writer = new RssWriter(new StringWriter(), new List<Entry>(), DateTime.Now, false, subtextContext.Object);
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

            var blogInfo = new Blog();
            BlogRequest.Current.Blog = blogInfo;
            blogInfo.Host = "localhost";
            blogInfo.Title = "My Blog Is Better Than Yours";
            blogInfo.Email = "Subtext@example.com";
            blogInfo.RFC3229DeltaEncodingEnabled = true;
            blogInfo.TimeZoneId = TimeZonesTest.PacificTimeZoneId;
            blogInfo.ShowEmailAddressInRss = true;
            blogInfo.TrackbacksEnabled = true;

            HttpContext.Current.Items.Add("BlogInfo-", blogInfo);

            var entries = new List<Entry>(CreateSomeEntries());
            entries[0].Categories.AddRange(new[] {"Category1", "Category2"});
            entries[0].Email = "nobody@example.com";
            entries[2].Categories.Add("Category 3");

            var enc = new Enclosure();

            enc.Url = "http://perseus.franklins.net/hanselminutes_0107.mp3";
            enc.Title = "<Digital Photography Explained (for Geeks) with Aaron Hockley/>";
            enc.Size = 26707573;
            enc.MimeType = "audio/mp3";
            enc.AddToFeed = true;
            entries[2].Enclosure = enc;

            var enc1 = new Enclosure();

            enc1.Url = "http://perseus.franklins.net/hanselminutes_0107.mp3";
            enc1.Title = "<Digital Photography Explained (for Geeks) with Aaron Hockley/>";
            enc1.Size = 26707573;
            enc1.MimeType = "audio/mp3";
            enc1.AddToFeed = false;

            entries[3].Enclosure = enc1;

            var subtextContext = new Mock<ISubtextContext>();
            subtextContext.FakeSyndicationContext(blogInfo, "/", "Subtext.Web", null);
            Mock<UrlHelper> urlHelper = Mock.Get(subtextContext.Object.UrlHelper);
            urlHelper.Setup(u => u.BlogUrl()).Returns("/Subtext.Web/");
            urlHelper.Setup(u => u.EntryUrl(It.IsAny<Entry>())).Returns<Entry>(
                e => "/Subtext.Web/whatever/" + e.Id + ".aspx");

            var writer = new RssWriter(new StringWriter(), entries, NullValue.NullDateTime, false, subtextContext.Object);

            string expected = @"<rss version=""2.0"" "
                              + @"xmlns:dc=""http://purl.org/dc/elements/1.1/"" "
                              + @"xmlns:trackback=""http://madskills.com/public/xml/rss/module/trackback/"" "
                              + @"xmlns:wfw=""http://wellformedweb.org/CommentAPI/"" "
                              + @"xmlns:slash=""http://purl.org/rss/1.0/modules/slash/"" "
                              + @"xmlns:copyright=""http://blogs.law.harvard.edu/tech/rss"" "
                              + @"xmlns:image=""http://purl.org/rss/1.0/modules/image/"">" + Environment.NewLine
                              + indent() + @"<channel>" + Environment.NewLine
                              + indent(2) + @"<title>My Blog Is Better Than Yours</title>" + Environment.NewLine
                              + indent(2) + @"<link>http://localhost/Subtext.Web/Default.aspx</link>" +
                              Environment.NewLine
                              + indent(2) + @"<description />" + Environment.NewLine
                              + indent(2) + @"<language>en-US</language>" + Environment.NewLine
                              + indent(2) + @"<copyright>Subtext Weblog</copyright>" + Environment.NewLine
                              + indent(2) + @"<managingEditor>Subtext@example.com</managingEditor>" +
                              Environment.NewLine
                              + indent(2) + @"<generator>{0}</generator>" + Environment.NewLine
                              + indent(2) + @"<image>" + Environment.NewLine
                              + indent(3) + @"<title>My Blog Is Better Than Yours</title>" + Environment.NewLine
                              + indent(3) + @"<url>http://localhost/Subtext.Web/images/RSS2Image.gif</url>" +
                              Environment.NewLine
                              + indent(3) + @"<link>http://localhost/Subtext.Web/Default.aspx</link>" +
                              Environment.NewLine
                              + indent(3) + @"<width>77</width>" + Environment.NewLine
                              + indent(3) + @"<height>60</height>" + Environment.NewLine
                              + indent(2) + @"</image>" + Environment.NewLine
                              + indent(2) + @"<item>" + Environment.NewLine
                              + indent(3) + @"<title>Title of 1001.</title>" + Environment.NewLine
                              + indent(3) + @"<category>Category1</category>" + Environment.NewLine
                              + indent(3) + @"<category>Category2</category>" + Environment.NewLine
                              + indent(3) + @"<link>http://localhost/Subtext.Web/whatever/1001.aspx</link>" +
                              Environment.NewLine
                              + indent(3) +
                              @"<description>Body of 1001&lt;img src=""http://localhost/Subtext.Web/aggbug/1001.aspx"" width=""1"" height=""1"" /&gt;</description>" +
                              Environment.NewLine
                              + indent(3) + @"<dc:creator>Phil Haack</dc:creator>" + Environment.NewLine
                              + indent(3) + @"<guid>http://localhost/Subtext.Web/whatever/1001.aspx</guid>" +
                              Environment.NewLine
                              + indent(3) + @"<pubDate>Sun, 23 Feb 1975 08:00:00 GMT</pubDate>" + Environment.NewLine
                              + indent(3) +
                              @"<comments>http://localhost/Subtext.Web/whatever/1001.aspx#feedback</comments>" +
                              Environment.NewLine
                              + indent(3) +
                              @"<wfw:commentRss>http://localhost/Subtext.Web/comments/commentRss/1001.aspx</wfw:commentRss>" +
                              Environment.NewLine
                              + indent(3) +
                              @"<trackback:ping>http://localhost/Subtext.Web/services/trackbacks/1001.aspx</trackback:ping>" +
                              Environment.NewLine
                              + indent(2) + @"</item>" + Environment.NewLine
                              + indent(2) + @"<item>" + Environment.NewLine
                              + indent(3) + @"<title>Title of 1002.</title>" + Environment.NewLine
                              + indent(3) + @"<link>http://localhost/Subtext.Web/whatever/1002.aspx</link>" +
                              Environment.NewLine
                              + indent(3) +
                              @"<description>Body of 1002&lt;img src=""http://localhost/Subtext.Web/aggbug/1002.aspx"" width=""1"" height=""1"" /&gt;</description>" +
                              Environment.NewLine
                              + indent(3) + @"<dc:creator>Phil Haack</dc:creator>" + Environment.NewLine
                              + indent(3) + @"<guid>http://localhost/Subtext.Web/whatever/1002.aspx</guid>" +
                              Environment.NewLine
                              + indent(3) + @"<pubDate>Fri, 25 Jun 1976 07:00:00 GMT</pubDate>" + Environment.NewLine
                              + indent(3) +
                              @"<comments>http://localhost/Subtext.Web/whatever/1002.aspx#feedback</comments>" +
                              Environment.NewLine
                              + indent(3) +
                              @"<wfw:commentRss>http://localhost/Subtext.Web/comments/commentRss/1002.aspx</wfw:commentRss>" +
                              Environment.NewLine
                              + indent(3) +
                              @"<trackback:ping>http://localhost/Subtext.Web/services/trackbacks/1002.aspx</trackback:ping>" +
                              Environment.NewLine
                              + indent(2) + @"</item>" + Environment.NewLine
                              + indent(2) + @"<item>" + Environment.NewLine
                              + indent(3) + @"<title>Title of 1003.</title>" + Environment.NewLine
                              + indent(3) + @"<category>Category 3</category>" + Environment.NewLine
                              + indent(3) + @"<link>http://localhost/Subtext.Web/whatever/1003.aspx</link>" +
                              Environment.NewLine
                              + indent(3) +
                              @"<description>Body of 1003&lt;img src=""http://localhost/Subtext.Web/aggbug/1003.aspx"" width=""1"" height=""1"" /&gt;</description>" +
                              Environment.NewLine
                              + indent(3) + @"<dc:creator>Phil Haack</dc:creator>" + Environment.NewLine
                              + indent(3) + @"<guid>http://localhost/Subtext.Web/whatever/1003.aspx</guid>" +
                              Environment.NewLine
                              + indent(3) + @"<pubDate>Tue, 16 Oct 1979 07:00:00 GMT</pubDate>" + Environment.NewLine
                              + indent(3) +
                              @"<comments>http://localhost/Subtext.Web/whatever/1003.aspx#feedback</comments>" +
                              Environment.NewLine
                              + indent(3) +
                              @"<wfw:commentRss>http://localhost/Subtext.Web/comments/commentRss/1003.aspx</wfw:commentRss>" +
                              Environment.NewLine
                              + indent(3) +
                              @"<trackback:ping>http://localhost/Subtext.Web/services/trackbacks/1003.aspx</trackback:ping>" +
                              Environment.NewLine
                              + indent(3) +
                              @"<enclosure url=""http://perseus.franklins.net/hanselminutes_0107.mp3"" length=""26707573"" type=""audio/mp3"" />" +
                              Environment.NewLine
                              + indent(2) + @"</item>" + Environment.NewLine
                              + indent(2) + @"<item>" + Environment.NewLine
                              + indent(3) + @"<title>Title of 1004.</title>" + Environment.NewLine
                              + indent(3) + @"<link>http://localhost/Subtext.Web/whatever/1004.aspx</link>" +
                              Environment.NewLine
                              + indent(3) +
                              @"<description>Body of 1004&lt;img src=""http://localhost/Subtext.Web/aggbug/1004.aspx"" width=""1"" height=""1"" /&gt;</description>" +
                              Environment.NewLine
                              + indent(3) + @"<dc:creator>Phil Haack</dc:creator>" + Environment.NewLine
                              + indent(3) + @"<guid>http://localhost/Subtext.Web/whatever/1004.aspx</guid>" +
                              Environment.NewLine
                              + indent(3) + @"<pubDate>Mon, 14 Jul 2003 07:00:00 GMT</pubDate>" + Environment.NewLine
                              + indent(3) +
                              @"<comments>http://localhost/Subtext.Web/whatever/1004.aspx#feedback</comments>" +
                              Environment.NewLine
                              + indent(3) +
                              @"<wfw:commentRss>http://localhost/Subtext.Web/comments/commentRss/1004.aspx</wfw:commentRss>" +
                              Environment.NewLine
                              + indent(3) +
                              @"<trackback:ping>http://localhost/Subtext.Web/services/trackbacks/1004.aspx</trackback:ping>" +
                              Environment.NewLine
                              + indent(2) + @"</item>" + Environment.NewLine
                              + indent() + @"</channel>" + Environment.NewLine
                              + @"</rss>";

            expected = string.Format(expected, VersionInfo.VersionDisplayText);

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

            var blogInfo = new Blog();
            BlogRequest.Current.Blog = blogInfo;
            blogInfo.Host = "localhost";
            blogInfo.Subfolder = "";
            blogInfo.Email = "Subtext@example.com";
            blogInfo.RFC3229DeltaEncodingEnabled = true;
            blogInfo.TimeZoneId = TimeZonesTest.PacificTimeZoneId;

            HttpContext.Current.Items.Add("BlogInfo-", blogInfo);

            var entries = new List<Entry>(CreateSomeEntriesDescending());
            var subtextContext = new Mock<ISubtextContext>();
            subtextContext.FakeSyndicationContext(blogInfo, "/", "Subtext.Web", null);
            Mock<UrlHelper> urlHelper = Mock.Get(subtextContext.Object.UrlHelper);
            urlHelper.Setup(u => u.BlogUrl()).Returns("/Subtext.Web/");
            urlHelper.Setup(u => u.EntryUrl(It.IsAny<Entry>())).Returns("/Subtext.Web/whatever");

            // Tell the write we already received 1002 published 6/25/1976.
            var writer = new RssWriter(new StringWriter(), entries,
                                       DateTime.ParseExact("06/25/1976", "MM/dd/yyyy", CultureInfo.InvariantCulture),
                                       true, subtextContext.Object);

            // We only expect 1003 and 1004
            string expected =
                @"<rss version=""2.0"" xmlns:dc=""http://purl.org/dc/elements/1.1/"" xmlns:trackback=""http://madskills.com/public/xml/rss/module/trackback/"" xmlns:wfw=""http://wellformedweb.org/CommentAPI/"" xmlns:slash=""http://purl.org/rss/1.0/modules/slash/"" xmlns:copyright=""http://blogs.law.harvard.edu/tech/rss"" xmlns:image=""http://purl.org/rss/1.0/modules/image/"">" +
                Environment.NewLine
                + indent() + "<channel>" + Environment.NewLine
                + indent(2) + "<title />" + Environment.NewLine
                + indent(2) + "<link>http://localhost/Subtext.Web/Default.aspx</link>" + Environment.NewLine
                + indent(2) + "<description />" + Environment.NewLine
                + indent(2) + "<language>en-US</language>" + Environment.NewLine
                + indent(2) + "<copyright>Subtext Weblog</copyright>" + Environment.NewLine
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
                + indent(3) + @"<link>http://localhost/Subtext.Web/whatever</link>" + Environment.NewLine
                + indent(3) +
                @"<description>Body of 1004&lt;img src=""http://localhost/Subtext.Web/aggbug/1004.aspx"" width=""1"" height=""1"" /&gt;</description>" +
                Environment.NewLine
                + indent(3) + @"<dc:creator>Phil Haack</dc:creator>" + Environment.NewLine
                + indent(3) + @"<guid>http://localhost/Subtext.Web/whatever</guid>" + Environment.NewLine
                + indent(3) + @"<pubDate>Mon, 14 Jul 2003 07:00:00 GMT</pubDate>" + Environment.NewLine
                + indent(3) + @"<comments>http://localhost/Subtext.Web/whatever#feedback</comments>" +
                Environment.NewLine
                + indent(3) +
                @"<wfw:commentRss>http://localhost/Subtext.Web/comments/commentRss/1004.aspx</wfw:commentRss>" +
                Environment.NewLine
                + indent(2) + @"</item>" + Environment.NewLine
                + indent(2) + @"<item>" + Environment.NewLine
                + indent(3) + "<title>Title of 1003.</title>" + Environment.NewLine
                + indent(3) + "<link>http://localhost/Subtext.Web/whatever</link>" + Environment.NewLine
                + indent(3) +
                @"<description>Body of 1003&lt;img src=""http://localhost/Subtext.Web/aggbug/1003.aspx"" width=""1"" height=""1"" /&gt;</description>" +
                Environment.NewLine
                + indent(3) + @"<dc:creator>Phil Haack</dc:creator>" + Environment.NewLine
                + indent(3) + @"<guid>http://localhost/Subtext.Web/whatever</guid>" + Environment.NewLine
                + indent(3) + @"<pubDate>Tue, 16 Oct 1979 07:00:00 GMT</pubDate>" + Environment.NewLine
                + indent(3) + @"<comments>http://localhost/Subtext.Web/whatever#feedback</comments>" +
                Environment.NewLine
                + indent(3) +
                @"<wfw:commentRss>http://localhost/Subtext.Web/comments/commentRss/1003.aspx</wfw:commentRss>" +
                Environment.NewLine
                + indent(2) + "</item>" + Environment.NewLine
                + indent() + "</channel>" + Environment.NewLine
                + "</rss>";

            expected = string.Format(expected, VersionInfo.VersionDisplayText);

            Assert.AreEqual(expected, writer.Xml);

            Assert.AreEqual(DateTime.ParseExact("06/25/1976", "MM/dd/yyyy", CultureInfo.InvariantCulture),
                            writer.DateLastViewedFeedItemPublished,
                            "The Item ID Last Viewed (according to If-None-Since is wrong.");
            Assert.AreEqual(DateTime.ParseExact("07/14/2003", "MM/dd/yyyy", CultureInfo.InvariantCulture),
                            writer.LatestPublishDate, "The Latest Feed Item ID sent to the client is wrong.");
        }

        /// <summary>
        /// Tests writing a simple RSS feed.
        /// </summary>
        [Test]
        [RollBack]
        public void RssWriterSendsWholeFeedWhenRFC3229Disabled()
        {
            UnitTestHelper.SetHttpContextWithBlogRequest("localhost", "", "Subtext.Web");

            var blogInfo = new Blog();
            BlogRequest.Current.Blog = blogInfo;
            blogInfo.Host = "localhost";
            blogInfo.Subfolder = "";
            blogInfo.Email = "Subtext@example.com";
            blogInfo.RFC3229DeltaEncodingEnabled = false;
            blogInfo.TimeZoneId = TimeZonesTest.PacificTimeZoneId;

            HttpContext.Current.Items.Add("BlogInfo-", blogInfo);

            var entries = new List<Entry>(CreateSomeEntriesDescending());
            var subtextContext = new Mock<ISubtextContext>();
            subtextContext.FakeSyndicationContext(blogInfo, "/Subtext.Web/", "Subtext.Web", null);
            Mock<UrlHelper> urlHelper = Mock.Get(subtextContext.Object.UrlHelper);
            urlHelper.Setup(u => u.BlogUrl()).Returns("/Subtext.Web/");
            urlHelper.Setup(u => u.EntryUrl(It.IsAny<Entry>())).Returns<Entry>(e => "/Subtext.Web/whatever/" + e.Id);

            var writer = new RssWriter(new StringWriter(), entries,
                                       DateTime.ParseExact("07/14/2003", "MM/dd/yyyy", CultureInfo.InvariantCulture),
                                       false, subtextContext.Object);

            string expected =
                @"<rss version=""2.0"" xmlns:dc=""http://purl.org/dc/elements/1.1/"" xmlns:trackback=""http://madskills.com/public/xml/rss/module/trackback/"" xmlns:wfw=""http://wellformedweb.org/CommentAPI/"" xmlns:slash=""http://purl.org/rss/1.0/modules/slash/"" xmlns:copyright=""http://blogs.law.harvard.edu/tech/rss"" xmlns:image=""http://purl.org/rss/1.0/modules/image/"">" +
                Environment.NewLine
                + indent() + "<channel>" + Environment.NewLine
                + indent(2) + "<title />" + Environment.NewLine
                + indent(2) + "<link>http://localhost/Subtext.Web/Default.aspx</link>" + Environment.NewLine
                + indent(2) + "<description />" + Environment.NewLine
                + indent(2) + "<language>en-US</language>" + Environment.NewLine
                + indent(2) + "<copyright>Subtext Weblog</copyright>" + Environment.NewLine
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
                + indent(3) + "<link>http://localhost/Subtext.Web/whatever/1004</link>" + Environment.NewLine
                + indent(3) +
                @"<description>Body of 1004&lt;img src=""http://localhost/Subtext.Web/aggbug/1004.aspx"" width=""1"" height=""1"" /&gt;</description>" +
                Environment.NewLine
                + indent(3) + "<dc:creator>Phil Haack</dc:creator>" + Environment.NewLine
                + indent(3) + "<guid>http://localhost/Subtext.Web/whatever/1004</guid>" + Environment.NewLine
                + indent(3) + "<pubDate>Mon, 14 Jul 2003 07:00:00 GMT</pubDate>" + Environment.NewLine
                + indent(3) + "<comments>http://localhost/Subtext.Web/whatever/1004#feedback</comments>" +
                Environment.NewLine
                + indent(3) +
                "<wfw:commentRss>http://localhost/Subtext.Web/comments/commentRss/1004.aspx</wfw:commentRss>" +
                Environment.NewLine
                + indent(2) + "</item>" + Environment.NewLine
                + indent(2) + "<item>" + Environment.NewLine
                + indent(3) + "<title>Title of 1003.</title>" + Environment.NewLine
                + indent(3) + @"<link>http://localhost/Subtext.Web/whatever/1003</link>" + Environment.NewLine
                + indent(3) +
                @"<description>Body of 1003&lt;img src=""http://localhost/Subtext.Web/aggbug/1003.aspx"" width=""1"" height=""1"" /&gt;</description>" +
                Environment.NewLine
                + indent(3) + "<dc:creator>Phil Haack</dc:creator>" + Environment.NewLine
                + indent(3) + "<guid>http://localhost/Subtext.Web/whatever/1003</guid>" + Environment.NewLine
                + indent(3) + "<pubDate>Tue, 16 Oct 1979 07:00:00 GMT</pubDate>" + Environment.NewLine
                + indent(3) + "<comments>http://localhost/Subtext.Web/whatever/1003#feedback</comments>" +
                Environment.NewLine
                + indent(3) +
                "<wfw:commentRss>http://localhost/Subtext.Web/comments/commentRss/1003.aspx</wfw:commentRss>" +
                Environment.NewLine
                + indent(2) + "</item>" + Environment.NewLine
                + indent(2) + @"<item>" + Environment.NewLine
                + indent(3) + "<title>Title of 1002.</title>" + Environment.NewLine
                + indent(3) + "<link>http://localhost/Subtext.Web/whatever/1002</link>" + Environment.NewLine
                + indent(3) +
                @"<description>Body of 1002&lt;img src=""http://localhost/Subtext.Web/aggbug/1002.aspx"" width=""1"" height=""1"" /&gt;</description>" +
                Environment.NewLine
                + indent(3) + "<dc:creator>Phil Haack</dc:creator>" + Environment.NewLine
                + indent(3) + "<guid>http://localhost/Subtext.Web/whatever/1002</guid>" + Environment.NewLine
                + indent(3) + "<pubDate>Fri, 25 Jun 1976 07:00:00 GMT</pubDate>" + Environment.NewLine
                + indent(3) + "<comments>http://localhost/Subtext.Web/whatever/1002#feedback</comments>" +
                Environment.NewLine
                + indent(3) +
                "<wfw:commentRss>http://localhost/Subtext.Web/comments/commentRss/1002.aspx</wfw:commentRss>" +
                Environment.NewLine
                + indent(2) + "</item>" + Environment.NewLine
                + indent(2) + @"<item>" + Environment.NewLine
                + indent(3) + "<title>Title of 1001.</title>" + Environment.NewLine
                + indent(3) + "<link>http://localhost/Subtext.Web/whatever/1001</link>" + Environment.NewLine
                + indent(3) +
                @"<description>Body of 1001&lt;img src=""http://localhost/Subtext.Web/aggbug/1001.aspx"" width=""1"" height=""1"" /&gt;</description>" +
                Environment.NewLine
                + indent(3) + "<dc:creator>Phil Haack</dc:creator>" + Environment.NewLine
                + indent(3) + "<guid>http://localhost/Subtext.Web/whatever/1001</guid>" + Environment.NewLine
                + indent(3) + "<pubDate>Sun, 23 Feb 1975 08:00:00 GMT</pubDate>" + Environment.NewLine
                + indent(3) + "<comments>http://localhost/Subtext.Web/whatever/1001#feedback</comments>" +
                Environment.NewLine
                + indent(3) +
                "<wfw:commentRss>http://localhost/Subtext.Web/comments/commentRss/1001.aspx</wfw:commentRss>" +
                Environment.NewLine
                + indent(2) + "</item>" + Environment.NewLine
                + indent() + "</channel>" + Environment.NewLine
                + "</rss>";
            expected = string.Format(expected, VersionInfo.VersionDisplayText);
            UnitTestHelper.AssertStringsEqualCharacterByCharacter(expected, writer.Xml);
        }

        Entry[] CreateSomeEntries()
        {
            return new[]
            {
                CreateEntry(1001, "Title of 1001.", "Body of 1001",
                            DateTime.ParseExact("01/23/1975", "MM/dd/yyyy", CultureInfo.InvariantCulture))
                ,
                CreateEntry(1002, "Title of 1002.", "Body of 1002",
                            DateTime.ParseExact("05/25/1976", "MM/dd/yyyy", CultureInfo.InvariantCulture))
                ,
                CreateEntry(1003, "Title of 1003.", "Body of 1003",
                            DateTime.ParseExact("09/16/1979", "MM/dd/yyyy", CultureInfo.InvariantCulture))
                ,
                CreateEntry(1004, "Title of 1004.", "Body of 1004",
                            DateTime.ParseExact("06/14/2003", "MM/dd/yyyy", CultureInfo.InvariantCulture))
            };
        }

        Entry[] CreateSomeEntriesDescending()
        {
            return new[]
            {
                CreateEntry(1004, "Title of 1004.", "Body of 1004",
                            DateTime.ParseExact("06/14/2003", "MM/dd/yyyy", CultureInfo.InvariantCulture))
                ,
                CreateEntry(1003, "Title of 1003.", "Body of 1003",
                            DateTime.ParseExact("09/16/1979", "MM/dd/yyyy", CultureInfo.InvariantCulture))
                ,
                CreateEntry(1002, "Title of 1002.", "Body of 1002",
                            DateTime.ParseExact("05/25/1976", "MM/dd/yyyy", CultureInfo.InvariantCulture))
                ,
                CreateEntry(1001, "Title of 1001.", "Body of 1001",
                            DateTime.ParseExact("01/23/1975", "MM/dd/yyyy", CultureInfo.InvariantCulture))
            };
        }

        static Entry CreateEntry(int id, string title, string body, DateTime dateCreated)
        {
            var entry = new Entry(PostType.BlogPost)
                            {
                                DateCreated = dateCreated,
                                Title = title,
                                Author = "Phil Haack",
                                Body = body,
                                Id = id
                            };
            entry.DateModified = entry.DateCreated;
            entry.DateSyndicated = entry.DateCreated.AddMonths(1);

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
        }
    }
}