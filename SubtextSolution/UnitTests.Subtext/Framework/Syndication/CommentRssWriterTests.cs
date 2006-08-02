using System;
using System.Collections.Generic;
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
	/// Unit tests of the <see cref="CommentRssWriter"/> class.
	/// </summary>
	[TestFixture]
	public class CommentRssWriterTests
	{
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

			HttpContext.Current.Items.Add("BlogInfo-", blogInfo);

			Entry entry = UnitTestHelper.CreateEntryInstanceForSyndication("haacked", "title of the post", "Body of the post.");
			entry.EntryName = "titleofthepost";
			entry.DateCreated = entry.DateSyndicated = entry.DateUpdated = DateTime.ParseExact("2006/04/01", "yyyy/MM/dd", CultureInfo.InvariantCulture);
			entry.Url = "/archive/2006/04/01/titleofthepost.aspx";
			CommentRssWriter writer = new CommentRssWriter(new List<Entry>(), entry);
			
			Assert.IsTrue(entry.HasEntryName, "This entry should have an entry name.");
			
			string expected = @"<rss version=""2.0"" " 
									+ @"xmlns:dc=""http://purl.org/dc/elements/1.1/"" " 
									+ @"xmlns:trackback=""http://madskills.com/public/xml/rss/module/trackback/"" " 
									+ @"xmlns:wfw=""http://wellformedweb.org/CommentAPI/"" " 
									+ @"xmlns:slash=""http://purl.org/rss/1.0/modules/slash/"" " 
									+ @"xmlns:copyright=""http://blogs.law.harvard.edu/tech/rss"" " 
									+ @"xmlns:image=""http://purl.org/rss/1.0/modules/image/"">" 
								+ @"<channel>" 
										+ @"<title>title of the post</title>" 
										+ @"<link>http://localhost/blog/archive/2006/04/01/titleofthepost.aspx</link>" 
										+ @"<description>Body of the post.</description>" 
										+ @"<language>en-US</language>" 
										+ @"<copyright>Subtext Weblog</copyright>" 
										+ @"<generator>{0}</generator>" 
										+ @"<image>" 
											+ @"<title>title of the post</title>" 
											+ @"<url>http://localhost/RSS2Image.gif</url>"
											+ @"<link>http://localhost/blog/archive/2006/04/01/titleofthepost.aspx</link>" 
											+ @"<width>77</width>" 
											+ @"<height>60</height>" 
										+ @"</image>" 
								+ @"</channel>" 
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

			HttpContext.Current.Items.Add("BlogInfo-", blogInfo);

			Entry entry = UnitTestHelper.CreateEntryInstanceForSyndication("haacked", "title of the post", "Body of the post.");
			entry.EntryName = "titleofthepost";
			entry.DateCreated = entry.DateSyndicated = entry.DateUpdated = DateTime.ParseExact("2006/04/01", "yyyy/MM/dd", CultureInfo.InvariantCulture);
			entry.Url = "/archive/2006/04/01/titleofthepost.aspx";
			entry.Id = 1001;
			
			Entry comment = new Entry(PostType.Comment);
			comment.Id = 1002;
			comment.DateCreated = comment.DateSyndicated = comment.DateUpdated = DateTime.ParseExact("2006/04/01", "yyyy/MM/dd", CultureInfo.InvariantCulture);
			comment.Title = "re: titleofthepost";
			comment.Url = Config.CurrentBlog.UrlFormats.CommentUrl(entry, comment);
			comment.Body = "<strong>I rule!</strong>";
			comment.Author = "Jane Schmane";
			comment.Email = "jane@example.com";
			comment.ParentId = entry.Id;

			List <Entry> comments = new List<Entry>();
			comments.Add(comment);
			
			CommentRssWriter writer = new CommentRssWriter(comments, entry);

			Assert.IsTrue(entry.HasEntryName, "This entry should have an entry name.");

			string expected = @"<rss version=""2.0"" "
									+ @"xmlns:dc=""http://purl.org/dc/elements/1.1/"" "
									+ @"xmlns:trackback=""http://madskills.com/public/xml/rss/module/trackback/"" "
									+ @"xmlns:wfw=""http://wellformedweb.org/CommentAPI/"" "
									+ @"xmlns:slash=""http://purl.org/rss/1.0/modules/slash/"" "
									+ @"xmlns:copyright=""http://blogs.law.harvard.edu/tech/rss"" "
									+ @"xmlns:image=""http://purl.org/rss/1.0/modules/image/"">"
								+ @"<channel>"
										+ @"<title>title of the post</title>"
										+ @"<link>http://localhost/Subtext.Web/archive/2006/04/01/titleofthepost.aspx</link>"
										+ @"<description>Body of the post.</description>"
										+ @"<language>en-US</language>"
										+ @"<copyright>Subtext Weblog</copyright>"
										+ @"<generator>{0}</generator>"
										+ @"<image>"
											+ @"<title>title of the post</title>"
											+ @"<url>http://localhost/Subtext.Web/RSS2Image.gif</url>"
											+ @"<link>http://localhost/Subtext.Web/archive/2006/04/01/titleofthepost.aspx</link>"
											+ @"<width>77</width>"
											+ @"<height>60</height>"
										+ @"</image>"
										+ @"<item>"
											+ @"<title>re: titleofthepost</title>"
											+ @"<link>http://localhost/Subtext.Web/archive/2006/04/01/titleofthepost.aspx#1002</link>"
											+ @"<description>&lt;strong&gt;I rule!&lt;/strong&gt;</description>"
											+ @"<dc:creator>Jane Schmane</dc:creator>"
											+ @"<guid>http://localhost/Subtext.Web/archive/2006/04/01/titleofthepost.aspx#1002</guid>"
											+ @"<pubDate>Sat, 01 Apr 2006 00:00:00 GMT</pubDate>"
										+ @"</item>" 
								+ @"</channel>"
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
			new CommentRssWriter(null, new Entry(PostType.Comment));
		}
		
		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void CommentRssWriterRequiresNonNullEntry()
		{
			new CommentRssWriter(new List<Entry>(), null);
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
