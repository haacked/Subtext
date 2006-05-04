using System;
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
			UnitTestHelper.SetHttpContextWithBlogRequest("localhost", "Subtext.Web");

			BlogInfo blogInfo = new BlogInfo();
			blogInfo.Host = "localhost";
			blogInfo.Subfolder = "Subtext.Web";
			blogInfo.Email = "Subtext@example.com";
			blogInfo.RFC3229DeltaEncodingEnabled = true;
			blogInfo.Title = "My Blog Rulz";

			HttpContext.Current.Items.Add("BlogInfo-", blogInfo);

			Entry entry = UnitTestHelper.CreateEntryInstanceForSyndication("haacked", "title of the post", "Body of the post.");
			entry.Link = "http://localhost/Subtext.Web/2006/04/01/titleofthepost.aspx";
			Console.WriteLine("LINK: " + entry.Link);
			CommentRssWriter writer = new CommentRssWriter(new EntryCollection(), entry);
			
			string expected = @"<rss version=""2.0"" " 
									+ @"xmlns:dc=""http://purl.org/dc/elements/1.1/"" " 
									+ @"xmlns:trackback=""http://madskills.com/public/xml/rss/module/trackback/"" " 
									+ @"xmlns:wfw=""http://wellformedweb.org/CommentAPI/"" " 
									+ @"xmlns:slash=""http://purl.org/rss/1.0/modules/slash/"" " 
									+ @"xmlns:copyright=""http://blogs.law.harvard.edu/tech/rss"" " 
									+ @"xmlns:image=""http://purl.org/rss/1.0/modules/image/"">" 
								+ @"<channel>" 
										+ @"<title>title of the post</title>" 
										+ @"<link>http://localhost/Subtext.Web/2006/04/01/titleofthepost.aspx</link>" 
										+ @"<description>Body of the post.</description>" 
										+ @"<language>en-US</language>" 
										+ @"<copyright>Subtext Weblog</copyright>" 
										+ @"<generator>{0}</generator>" 
										+ @"<image>" 
											+ @"<title>My Blog Rulz</title>" 
											+ @"<url>http://localhost/Subtext.Web/RSS2Image.gif</url>" 
											+ @"<link>http://localhost/Subtext.Web/Default.aspx</link>" 
											+ @"<width>77</width>" 
											+ @"<height>60</height>" 
											+ @"<description />" 
										+ @"</image>" 
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
			new CommentRssWriter(new EntryCollection(), null);
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
