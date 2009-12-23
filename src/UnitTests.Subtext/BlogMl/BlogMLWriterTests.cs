using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml;
using BlogML;
using BlogML.Xml;
using MbUnit.Framework;
using Moq;
using Subtext.ImportExport;

namespace UnitTests.Subtext.BlogMl
{
    [TestFixture]
    public class BlogMLWriterTests
    {
        [Test]
        public void Write_WithSourceReturningBlog_WritesBlogInfoToWriter()
        {
            // arrange
            var stringWriter = new StringWriter();
            var xmlWriter = new XmlTextWriter(stringWriter);
            var source = new Mock<IBlogMLSource>();
            var dateTime = DateTime.ParseExact("20090123", "yyyyMMdd", CultureInfo.InvariantCulture);
            source.Setup(s => s.GetBlog()).Returns(new BlogMLBlog { Title = "Subtext Blog", RootUrl = "http://subtextproject.com/", SubTitle = "A test blog", DateCreated = dateTime});
            var writer = new BlogMLWriter(source.Object, false /*embedAttachments*/);

            // act
            ((IBlogMLWriter)writer).Write(xmlWriter);

            // assert
            string output = stringWriter.ToString();
            Assert.Contains(output, @"<title type=""text""><![CDATA[Subtext Blog]]></title>");
            Assert.Contains(output, @"<sub-title type=""text""><![CDATA[A test blog]]></sub-title>");
            Assert.Contains(output, @"root-url=""http://subtextproject.com/""");
            Assert.Contains(output, @"date-created=""2009-01-23T08:00:00""");
        }

        [Test]
        public void Write_WithSourceReturningBlogWithNullSubtitle_RendersEmptyStringForUrl()
        {
            // arrange
            var stringWriter = new StringWriter();
            var xmlWriter = new XmlTextWriter(stringWriter);
            var source = new Mock<IBlogMLSource>();
            var dateTime = DateTime.ParseExact("20090123", "yyyyMMdd", CultureInfo.InvariantCulture);
            source.Setup(s => s.GetBlog()).Returns(new BlogMLBlog { Title = "Subtext Blog", RootUrl = "http://example.com/", SubTitle = null, DateCreated = dateTime });
            var writer = new BlogMLWriter(source.Object, false /*embedAttachments*/);

            // act
            ((IBlogMLWriter)writer).Write(xmlWriter);

            // assert
            string output = stringWriter.ToString();
            Assert.Contains(output, @"<sub-title type=""text""><![CDATA[]]></sub-title>");
        }

        [Test]
        public void Write_WithSourceReturningAuthors_WritesAuthorsToWriter()
        {
            // arrange
            var stringWriter = new StringWriter();
            var xmlWriter = new XmlTextWriter(stringWriter) {Formatting = Formatting.Indented};
            var source = new Mock<IBlogMLSource>();
            var dateTime = DateTime.ParseExact("20090123", "yyyyMMdd", CultureInfo.InvariantCulture);
            var blog = new BlogMLBlog {Title = "Subtext Blog", RootUrl = "http://subtextproject.com/", SubTitle = "A test blog", DateCreated = dateTime};
            blog.Authors.Add(new BlogMLAuthor{ ID = "112", Title = "Phineas", Email = "phineas@example.com", Approved = true});
            source.Setup(s => s.GetBlog()).Returns(blog);
            var writer = new BlogMLWriter(source.Object, false /*embedAttachments*/);

            // act
            ((IBlogMLWriter)writer).Write(xmlWriter);

            // assert
            string output = stringWriter.ToString();
            Assert.Contains(output, @"<author id=""112""");
            Assert.Contains(output, @"email=""phineas@example.com""");
            Assert.Contains(output, @"approved=""true""");
            Assert.Contains(output, @"<title type=""text""><![CDATA[Phineas]]></title>");
        }

        [Test]
        public void Write_WithBlogContainingExtendedProperties_WritesPropertiesToWriter()
        {
            // arrange
            var stringWriter = new StringWriter();
            var xmlWriter = new XmlTextWriter(stringWriter) {Formatting = Formatting.Indented};
            var source = new Mock<IBlogMLSource>();
            var dateTime = DateTime.ParseExact("20090123", "yyyyMMdd", CultureInfo.InvariantCulture);
            var blog = new BlogMLBlog { Title = "Subtext Blog", RootUrl = "http://subtextproject.com/", SubTitle = "A test blog", DateCreated = dateTime };
            blog.ExtendedProperties.Add(new Pair<string, string>("Color", "Blue"));
            source.Setup(s => s.GetBlog()).Returns(blog);
            var writer = new BlogMLWriter(source.Object, false /*embedAttachments*/);

            // act
            ((IBlogMLWriter)writer).Write(xmlWriter);

            // assert
            string output = stringWriter.ToString();
            Assert.Contains(output, @"<property name=""Color"" value=""Blue"" />");
        }

        [Test]
        public void Write_WithBlogContainingCategories_WritesCategoriesToWriter()
        {
            // arrange
            var stringWriter = new StringWriter();
            var xmlWriter = new XmlTextWriter(stringWriter) {Formatting = Formatting.Indented};
            var source = new Mock<IBlogMLSource>();
            var dateTime = DateTime.ParseExact("20090123", "yyyyMMdd", CultureInfo.InvariantCulture);
            var blog = new BlogMLBlog { Title = "Subtext Blog", RootUrl = "http://subtextproject.com/", SubTitle = "A test blog", DateCreated = dateTime };
            blog.Categories.Add(new BlogMLCategory { ID = "221", Title = "Test Category"});
            source.Setup(s => s.GetBlog()).Returns(blog);
            var writer = new BlogMLWriter(source.Object, false /*embedAttachments*/);

            // act
            ((IBlogMLWriter)writer).Write(xmlWriter);

            // assert
            string output = stringWriter.ToString();
            Assert.Contains(output, @"<category id=""221""");
            Assert.Contains(output, @"<title type=""text""><![CDATA[Test Category]]></title>");
        }

        [Test]
        public void Write_WithBlogContainingPosts_WritesPostsToWriter()
        {
            // arrange
            var stringWriter = new StringWriter();
            var xmlWriter = new XmlTextWriter(stringWriter) {Formatting = Formatting.Indented};
            var source = new Mock<IBlogMLSource>();
            var dateTime = DateTime.ParseExact("20090123", "yyyyMMdd", CultureInfo.InvariantCulture);
            var blog = new BlogMLBlog { Title = "Subtext Blog", RootUrl = "http://subtextproject.com/", SubTitle = "A test blog", DateCreated = dateTime };
            source.Setup(s => s.GetBlog()).Returns(blog);
            var posts = new List<BlogMLPost> {new BlogMLPost {Title = "This is a blog post"}};
            posts[0].Content.Text = "<p>Test</p>";
            source.Setup(s => s.GetBlogPosts(false /*embedAttachments*/)).Returns(posts);
            var writer = new BlogMLWriter(source.Object, false /*embedAttachments*/);

            // act
            ((IBlogMLWriter)writer).Write(xmlWriter);

            // assert
            string output = stringWriter.ToString();
            Assert.Contains(output, "<posts>");
            Assert.Contains(output, "</posts>");
            Assert.Contains(output, @"<title type=""text""><![CDATA[This is a blog post]]></title>");
            Assert.Contains(output, @"<content type=""text""><![CDATA[<p>Test</p>]]></content>");
        }

        [Test]
        public void Write_WithBlogContainingBase64EncodedPosts_WritesPostsToWriterAsBase64Encoded()
        {
            // arrange
            var stringWriter = new StringWriter();
            var xmlWriter = new XmlTextWriter(stringWriter) { Formatting = Formatting.Indented };
            var source = new Mock<IBlogMLSource>();
            var dateTime = DateTime.ParseExact("20090123", "yyyyMMdd", CultureInfo.InvariantCulture);
            var blog = new BlogMLBlog { Title = "Subtext Blog", RootUrl = "http://subtextproject.com/", SubTitle = "A test blog", DateCreated = dateTime };
            source.Setup(s => s.GetBlog()).Returns(blog);
            var post = new BlogMLPost { Content = BlogMLContent.Create("<p>This is a Test</p>", ContentTypes.Base64) };
            var posts = new List<BlogMLPost> { post };
            blog.Posts.Add(post);
            source.Setup(s => s.GetBlogPosts(false /*embedAttachments*/)).Returns(posts);
            var writer = new BlogMLWriter(source.Object, false /*embedAttachments*/);

            // act
            ((IBlogMLWriter)writer).Write(xmlWriter);

            // assert
            string output = stringWriter.ToString();
            Console.WriteLine(Convert.ToBase64String(Encoding.UTF8.GetBytes("<p>This is a Test</p>")));
            Assert.Contains(output, @"<content type=""base64""><![CDATA[PHA+VGhpcyBpcyBhIFRlc3Q8L3A+]]></content>");
        }

        [Test]
        public void Write_WithBlogContainingPostsWithCategories_WritesPostCategoriesToWriter()
        {
            // arrange
            var stringWriter = new StringWriter();
            var xmlWriter = new XmlTextWriter(stringWriter) {Formatting = Formatting.Indented};
            var source = new Mock<IBlogMLSource>();
            var dateTime = DateTime.ParseExact("20090123", "yyyyMMdd", CultureInfo.InvariantCulture);
            var blog = new BlogMLBlog { Title = "Subtext Blog", RootUrl = "http://subtextproject.com/", SubTitle = "A test blog", DateCreated = dateTime };
            blog.Categories.Add(new BlogMLCategory { ID = "221", Title = "Test Category" });
            source.Setup(s => s.GetBlog()).Returns(blog);
            var post = new BlogMLPost {Title = "This is a blog post"};
            var posts = new List<BlogMLPost> { post };
            post.Categories.Add("221");
            source.Setup(s => s.GetBlogPosts(false /*embedAttachments*/)).Returns(posts);
            var writer = new BlogMLWriter(source.Object, false /*embedAttachments*/);

            // act
            ((IBlogMLWriter)writer).Write(xmlWriter);

            // assert
            string output = stringWriter.ToString();
            Assert.Contains(output, @"<categories>");
            Assert.Contains(output, @"<category ref=""221"" />");
            Assert.Contains(output, @"</categories>");
        }

        [Test]
        public void Write_WithBlogContainingPostsWithComments_WritesPostCommentsToWriter()
        {
            // arrange
            var stringWriter = new StringWriter();
            var xmlWriter = new XmlTextWriter(stringWriter) {Formatting = Formatting.Indented};
            var source = new Mock<IBlogMLSource>();
            var dateTime = DateTime.ParseExact("20090123", "yyyyMMdd", CultureInfo.InvariantCulture);
            var blog = new BlogMLBlog { Title = "Subtext Blog", RootUrl = "http://subtextproject.com/", SubTitle = "A test blog", DateCreated = dateTime };
            source.Setup(s => s.GetBlog()).Returns(blog);
            var post = new BlogMLPost { Title = "This is a blog post" };
            var posts = new List<BlogMLPost> { post };
            var comment = new BlogMLComment {Title = "Test Comment Title", Content = {Text = "<p>Comment Body</p>"}};
            post.Comments.Add(comment);
            source.Setup(s => s.GetBlogPosts(false /*embedAttachments*/)).Returns(posts);
            var writer = new BlogMLWriter(source.Object, false /*embedAttachments*/);

            // act
            ((IBlogMLWriter)writer).Write(xmlWriter);

            // assert
            string output = stringWriter.ToString();
            Assert.Contains(output, @"<title type=""text""><![CDATA[Test Comment Title]]></title>");
            Assert.Contains(output, @"<content type=""text""><![CDATA[<p>Comment Body</p>]]></content>");
        }

        [Test]
        public void Write_WithBlogContainingTrackbacksWithComments_WritesPostTrackbacksToWriter()
        {
            // arrange
            var stringWriter = new StringWriter();
            var xmlWriter = new XmlTextWriter(stringWriter) {Formatting = Formatting.Indented};
            var source = new Mock<IBlogMLSource>();
            var dateTime = DateTime.ParseExact("20090123", "yyyyMMdd", CultureInfo.InvariantCulture);
            var blog = new BlogMLBlog { Title = "Subtext Blog", RootUrl = "http://subtextproject.com/", SubTitle = "A test blog", DateCreated = dateTime };
            source.Setup(s => s.GetBlog()).Returns(blog);
            var post = new BlogMLPost { Title = "This is a blog post" };
            post.Trackbacks.Add(new BlogMLTrackback { Title = "Post Test Trackback", Url = "http://example.com/trackback-source"});
            var posts = new List<BlogMLPost> { post };

            source.Setup(s => s.GetBlogPosts(false /*embedAttachments*/)).Returns(posts);
            var writer = new BlogMLWriter(source.Object, false /*embedAttachments*/);

            // act
            ((IBlogMLWriter)writer).Write(xmlWriter);

            // assert
            string output = stringWriter.ToString();
            Assert.Contains(output, @"<title type=""text""><![CDATA[Post Test Trackback]]></title>");
            Assert.Contains(output, @"url=""http://example.com/trackback-source""");
        }

        [Test]
        public void Write_WithBlogContainingEmbeddedAttachmentsWithComments_WritesPostAttachmentsToWriter()
        {
            // arrange
            var stringWriter = new StringWriter();
            var xmlWriter = new XmlTextWriter(stringWriter) {Formatting = Formatting.Indented};
            var source = new Mock<IBlogMLSource>();
            var dateTime = DateTime.ParseExact("20090123", "yyyyMMdd", CultureInfo.InvariantCulture);
            var blog = new BlogMLBlog { Title = "Subtext Blog", RootUrl = "http://subtextproject.com/", SubTitle = "A test blog", DateCreated = dateTime };
            source.Setup(s => s.GetBlog()).Returns(blog);
            var post = new BlogMLPost { Title = "This is a blog post" };
            var attachment = new BlogMLAttachment
            {
                Data = new byte[] {1, 2, 3, 4, 5},
                Path = @"c:\\path-to-attachment.jpg",
                Url = "/foo/path-to-attachment.jpg",
                Embedded = true,
                MimeType = "img/jpeg"
            };
            post.Attachments.Add(attachment);
            var posts = new List<BlogMLPost> { post };

            source.Setup(s => s.GetBlogPosts(false /*embedAttachments*/)).Returns(posts);
            var writer = new BlogMLWriter(source.Object, false /*embedAttachments*/);

            // act
            ((IBlogMLWriter)writer).Write(xmlWriter);

            // assert
            string output = stringWriter.ToString();
            Assert.Contains(output, @"external-uri=""c:\\path-to-attachment.jpg""");
            Assert.Contains(output, @"url=""/foo/path-to-attachment.jpg""");
            Assert.Contains(output, @"mime-type=""img/jpeg""");
            Assert.Contains(output, @"embedded=""true""");
            Assert.Contains(output, @"AQIDBAU=</attachment>");
        }

        [Test]
        public void Write_WithBlogContainingNonEmbeddedAttachmentsWithComments_WritesPostAttachmentsToWriter()
        {
            // arrange
            var stringWriter = new StringWriter();
            var xmlWriter = new XmlTextWriter(stringWriter) {Formatting = Formatting.Indented};
            var source = new Mock<IBlogMLSource>();
            var dateTime = DateTime.ParseExact("20090123", "yyyyMMdd", CultureInfo.InvariantCulture);
            var blog = new BlogMLBlog { Title = "Subtext Blog", RootUrl = "http://subtextproject.com/", SubTitle = "A test blog", DateCreated = dateTime };
            source.Setup(s => s.GetBlog()).Returns(blog);
            var post = new BlogMLPost { Title = "This is a blog post" };
            var attachment = new BlogMLAttachment
            {
                Path = @"c:\\path-to-attachment.jpg",
                Url = "/foo/path-to-attachment.jpg",
                Embedded = false,
                MimeType = "img/jpeg"
            };
            post.Attachments.Add(attachment);
            var posts = new List<BlogMLPost> { post };

            source.Setup(s => s.GetBlogPosts(false /*embedAttachments*/)).Returns(posts);
            var writer = new BlogMLWriter(source.Object, false /*embedAttachments*/);

            // act
            ((IBlogMLWriter)writer).Write(xmlWriter);

            // assert
            string output = stringWriter.ToString();
            Assert.Contains(output, @"<attachment url=""/foo/path-to-attachment.jpg"" mime-type=""img/jpeg"" external-uri=""c:\\path-to-attachment.jpg"" embedded=""false"" />");
        }

        [Test]
        public void Write_WithBlogContainingMultipleAuthors_WritesPostAuthorsToWriter()
        {
            // arrange
            var stringWriter = new StringWriter();
            var xmlWriter = new XmlTextWriter(stringWriter) {Formatting = Formatting.Indented};
            var source = new Mock<IBlogMLSource>();
            var dateTime = DateTime.ParseExact("20090123", "yyyyMMdd", CultureInfo.InvariantCulture);
            var blog = new BlogMLBlog { Title = "Subtext Blog", RootUrl = "http://subtextproject.com/", SubTitle = "A test blog", DateCreated = dateTime };
            blog.Authors.Add(new BlogMLAuthor { ID = "10"});
            blog.Authors.Add(new BlogMLAuthor { ID = "20" });
            source.Setup(s => s.GetBlog()).Returns(blog);
            var post = new BlogMLPost { Title = "This is a blog post" };
            post.Authors.Add(new BlogMLAuthorReference { Ref = "20" });
            var posts = new List<BlogMLPost> { post };

            source.Setup(s => s.GetBlogPosts(false /*embedAttachments*/)).Returns(posts);
            var writer = new BlogMLWriter(source.Object, false /*embedAttachments*/);

            // act
            ((IBlogMLWriter)writer).Write(xmlWriter);

            // assert
            string output = stringWriter.ToString();
            Assert.Contains(output, @"<author ref=""20"" />");
        }
    }
}
