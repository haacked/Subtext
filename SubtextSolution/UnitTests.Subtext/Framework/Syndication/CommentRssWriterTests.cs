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
	/// Unit tests of the <see cref="CommentRssWriter"/> class.
	/// </summary>
	[TestFixture]
	public class CommentRssWriterTests : SyndicationTestBase
	{
		const int PacificTimeZoneId = -2037797565;
		
		/// <summary>
		/// Tests that a valid feed is produced even if a post has no comments.
		/// </summary>
		[Test]
		[RollBack]
		public void CommentRssWriterProducesValidEmptyFeed()
		{
			UnitTestHelper.SetHttpContextWithBlogRequest("localhost", "blog");

			BlogInfo blogInfo = new BlogInfo();
			blogInfo.Host = "localhost";
			blogInfo.Subfolder = "blog";
			blogInfo.Email = "Subtext@example.com";
			blogInfo.RFC3229DeltaEncodingEnabled = true;
			blogInfo.Title = "My Blog Rulz";
			blogInfo.TimeZoneId = PacificTimeZoneId;
			
			HttpContext.Current.Items.Add("BlogInfo-", blogInfo);

			Entry entry = UnitTestHelper.CreateEntryInstanceForSyndication("haacked", "title of the post", "Body of the post.");
			entry.EntryName = "titleofthepost";
			entry.DateCreated = entry.DateSyndicated = entry.DateModified = DateTime.ParseExact("2006/04/01", "yyyy/MM/dd", CultureInfo.InvariantCulture);
			entry.Url = "/archive/2006/04/01/titleofthepost.aspx";
			CommentRssWriter writer = new CommentRssWriter(new List<FeedbackItem>(), entry);
			
			Assert.IsTrue(entry.HasEntryName, "This entry should have an entry name.");

			string expected = @"<rss version=""2.0"" " 
									+ @"xmlns:dc=""http://purl.org/dc/elements/1.1/"" " 
									+ @"xmlns:trackback=""http://madskills.com/public/xml/rss/module/trackback/"" " 
									+ @"xmlns:wfw=""http://wellformedweb.org/CommentAPI/"" " 
									+ @"xmlns:slash=""http://purl.org/rss/1.0/modules/slash/"" " 
									+ @"xmlns:copyright=""http://blogs.law.harvard.edu/tech/rss"" " 
									+ @"xmlns:image=""http://purl.org/rss/1.0/modules/image/"">" + Environment.NewLine
								+ indent() + @"<channel>" + Environment.NewLine
										+ indent(2) + @"<title>title of the post</title>" + Environment.NewLine
										+ indent(2) + @"<link>http://localhost/blog/archive/2006/04/01/titleofthepost.aspx</link>" + Environment.NewLine
										+ indent(2) + @"<description>Body of the post.</description>" + Environment.NewLine
										+ indent(2) + @"<language>en-US</language>" + Environment.NewLine
										+ indent(2) + @"<copyright>Subtext Weblog</copyright>" + Environment.NewLine
										+ indent(2) + @"<generator>{0}</generator>" + Environment.NewLine
										+ indent(2) + @"<image>" + Environment.NewLine
											+ indent(3) + @"<title>title of the post</title>" + Environment.NewLine
											+ indent(3) + @"<url>http://localhost/images/RSS2Image.gif</url>" + Environment.NewLine
											+ indent(3) + @"<link>http://localhost/blog/archive/2006/04/01/titleofthepost.aspx</link>" + Environment.NewLine
											+ indent(3) + @"<width>77</width>" + Environment.NewLine
											+ indent(3) + @"<height>60</height>" + Environment.NewLine
										+ indent(2) + @"</image>" + Environment.NewLine
								+ indent(1) + @"</channel>" + Environment.NewLine
			                  + @"</rss>";

			expected = string.Format(expected, VersionInfo.VersionDisplayText);
			
			Console.WriteLine("EXPECTED: " + expected);
			Console.WriteLine("ACTUAL  : " + writer.Xml);
			Assert.AreEqual(expected, writer.Xml);			
		}

		/// <summary>
		/// Tests that a valid feed is produced even if a post has no comments.
		/// </summary>
		[Test]
		[RollBack]
		public void CommentRssWriterProducesValidFeed()
		{
			UnitTestHelper.SetHttpContextWithBlogRequest("localhost", "", "Subtext.Web");

			BlogInfo blogInfo = new BlogInfo();
			blogInfo.Host = "localhost";
			blogInfo.Email = "Subtext@example.com";
			blogInfo.RFC3229DeltaEncodingEnabled = true;
			blogInfo.Title = "My Blog Rulz";
			blogInfo.TimeZoneId = PacificTimeZoneId;

			HttpContext.Current.Items.Add("BlogInfo-", blogInfo);

			Entry entry = UnitTestHelper.CreateEntryInstanceForSyndication("haacked", "title of the post", "Body of the post.");
			entry.EntryName = "titleofthepost";
			entry.DateCreated = entry.DateSyndicated = entry.DateModified = DateTime.ParseExact("2006/02/01", "yyyy/MM/dd", CultureInfo.InvariantCulture);
			entry.Url = "/archive/2006/02/01/titleofthepost.aspx";
			entry.Id = 1001;

			FeedbackItem comment = new FeedbackItem(FeedbackType.Comment);
			comment.Id = 1002;
			comment.DateCreated = comment.DateModified = DateTime.ParseExact("2006/02/01", "yyyy/MM/dd", CultureInfo.InvariantCulture);
			comment.Title = "re: titleofthepost";
			comment.ParentEntryName = entry.EntryName;
			comment.ParentDateCreated = entry.DateCreated;
			comment.Body = "<strong>I rule!</strong>";
			comment.Author = "Jane Schmane";
			comment.Email = "jane@example.com";
			comment.EntryId = entry.Id;

			List <FeedbackItem> comments = new List<FeedbackItem>();
			comments.Add(comment);
			
			CommentRssWriter writer = new CommentRssWriter(comments, entry);

			Assert.IsTrue(entry.HasEntryName, "This entry should have an entry name.");

			string expected = @"<rss version=""2.0"" "
									+ @"xmlns:dc=""http://purl.org/dc/elements/1.1/"" "
									+ @"xmlns:trackback=""http://madskills.com/public/xml/rss/module/trackback/"" "
									+ @"xmlns:wfw=""http://wellformedweb.org/CommentAPI/"" "
									+ @"xmlns:slash=""http://purl.org/rss/1.0/modules/slash/"" "
									+ @"xmlns:copyright=""http://blogs.law.harvard.edu/tech/rss"" "
									+ @"xmlns:image=""http://purl.org/rss/1.0/modules/image/"">" + Environment.NewLine
								+ indent() + @"<channel>" + Environment.NewLine
										+ indent(2) + @"<title>title of the post</title>" + Environment.NewLine
										+ indent(2) + @"<link>http://localhost/Subtext.Web/archive/2006/02/01/titleofthepost.aspx</link>" + Environment.NewLine
										+ indent(2) + @"<description>Body of the post.</description>" + Environment.NewLine
										+ indent(2) + @"<language>en-US</language>" + Environment.NewLine
										+ indent(2) + @"<copyright>Subtext Weblog</copyright>" + Environment.NewLine
										+ indent(2) + @"<generator>{0}</generator>" + Environment.NewLine
										+ indent(2) + @"<image>" + Environment.NewLine
											+ indent(3) + @"<title>title of the post</title>" + Environment.NewLine
											+ indent(3) + @"<url>http://localhost/Subtext.Web/images/RSS2Image.gif</url>" + Environment.NewLine
											+ indent(3) + @"<link>http://localhost/Subtext.Web/archive/2006/02/01/titleofthepost.aspx</link>" + Environment.NewLine
											+ indent(3) + @"<width>77</width>" + Environment.NewLine
											+ indent(3) + @"<height>60</height>" + Environment.NewLine
										+ indent(2) + @"</image>" + Environment.NewLine
										+ indent(2) + @"<item>" + Environment.NewLine
											+ indent(3) + @"<title>re: titleofthepost</title>" + Environment.NewLine
											+ indent(3) + @"<link>http://localhost/Subtext.Web/archive/2006/02/01/titleofthepost.aspx#1002</link>" + Environment.NewLine
											+ indent(3) + @"<description>&lt;strong&gt;I rule!&lt;/strong&gt;</description>" + Environment.NewLine
											+ indent(3) + @"<dc:creator>Jane Schmane</dc:creator>" + Environment.NewLine
											+ indent(3) + @"<guid>http://localhost/Subtext.Web/archive/2006/02/01/titleofthepost.aspx#1002</guid>" + Environment.NewLine
											+ indent(3) + @"<pubDate>Wed, 01 Feb 2006 08:00:00 GMT</pubDate>" + Environment.NewLine
										+ indent(2) + @"</item>" + Environment.NewLine
								+ indent() + @"</channel>" + Environment.NewLine
							  + @"</rss>";

			expected = string.Format(expected, VersionInfo.VersionDisplayText);

			Console.WriteLine("EXPECTED: " + expected);
			Console.WriteLine("ACTUAL  : " + writer.Xml);
			Assert.AreEqual(expected, writer.Xml);
		}
		
		#region ---- [Exception Cases] ------
		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void CommentRssWriterRequiresNonNullEntryCollection()
		{
			new CommentRssWriter(null, new Entry(PostType.BlogPost));
		}
		
		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void CommentRssWriterRequiresNonNullEntry()
		{
			new CommentRssWriter(new List<FeedbackItem>(), null);
		}
		#endregion
		
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
