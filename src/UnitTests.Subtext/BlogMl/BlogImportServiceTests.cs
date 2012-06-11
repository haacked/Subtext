using System;
using System.IO;
using BlogML;
using BlogML.Xml;
using MbUnit.Framework;
using Moq;
using Subtext.ImportExport;

namespace UnitTests.Subtext.BlogMl
{
    [TestFixture]
    public class BlogMlImportServiceTests
    {
        [Test]
        public void Import_SetsExtendedPropertiesOnBlog()
        {
            // arrange
            var blog = new BlogMLBlog();
            var repository = new Mock<IBlogImportRepository>();
            bool extendedPropertiesSet = false;
            repository.Setup(r => r.SetExtendedProperties(blog.ExtendedProperties)).Callback(() => extendedPropertiesSet = true);
            var service = new BlogImportService(repository.Object);

            // act
            service.Import(blog);

            // assert
            Assert.IsTrue(extendedPropertiesSet);
        }

        [Test]
        public void Import_WithBlogHavingCategories_CreatesCategories()
        {
            // arrange
            var blog = new BlogMLBlog();
            var repository = new Mock<IBlogImportRepository>();
            bool categoriesCreated = false;
            repository.Setup(r => r.CreateCategories(blog)).Callback(() => categoriesCreated = true);
            var service = new BlogImportService(repository.Object);

            // act
            service.Import(blog);

            // assert
            Assert.IsTrue(categoriesCreated);
        }

        [Test]
        public void Import_WithBlogPostHavingComments_CreatesCommentUsingPostId()
        {
            // arrange
            var blog = new BlogMLBlog();
            var post = new BlogMLPost();
            var comment = new BlogMLComment();
            post.Comments.Add(comment);
            blog.Posts.Add(post);
            var repository = new Mock<IBlogImportRepository>();
            repository.Setup(r => r.CreateBlogPost(blog, post)).Returns("98053");
            bool commentCreated = false;
            repository.Setup(r => r.CreateComment(comment, "98053")).Callback(() => commentCreated = true);
            var service = new BlogImportService(repository.Object);

            // act
            service.Import(blog);

            // assert
            Assert.IsTrue(commentCreated);
        }

        [Test]
        public void Import_WithBlogPostHavingBase64EncodedContentWithAttachments_ProperlyRewritesAttachments()
        {
            // arrange
            var blog = new BlogMLBlog();
            const string originalPostContent = @"<img src=""http://old.example.com/images/my-mug.jpg"" />";
            var post = new BlogMLPost { Content = BlogMLContent.Create(originalPostContent, ContentTypes.Base64) };
            var attachment = new BlogMLAttachment { Url = "http://old.example.com/images/my-mug.jpg", Embedded = false};
            post.Attachments.Add(attachment);
            blog.Posts.Add(post);
            var repository = new Mock<IBlogImportRepository>();
            repository.Setup(r => r.GetAttachmentDirectoryUrl()).Returns("http://new.example.com/images/");
            repository.Setup(r => r.GetAttachmentDirectoryPath()).Returns(@"c:\web\images");
            BlogMLPost publishedPost = null;
            repository.Setup(r => r.CreateBlogPost(blog, post)).Callback<BlogMLBlog, BlogMLPost>((b, p) => publishedPost = p);
            var service = new BlogImportService(repository.Object);

            // act
            service.Import(blog);

            // assert
            Assert.AreEqual(ContentTypes.Base64, publishedPost.Content.ContentType);
            Assert.AreEqual(@"<img src=""http://new.example.com/images/my-mug.jpg"" />", publishedPost.Content.UncodedText);
        }

        [Test]
        public void Import_WithBlogPostHavingTrackback_CreatesTrackbackUsingPostId()
        {
            // arrange
            var blog = new BlogMLBlog();
            var post = new BlogMLPost();
            var trackback = new BlogMLTrackback();
            post.Trackbacks.Add(trackback);
            blog.Posts.Add(post);
            var repository = new Mock<IBlogImportRepository>();
            repository.Setup(r => r.CreateBlogPost(blog, post)).Returns("98053");
            bool trackbackCreated = false;
            repository.Setup(r => r.CreateTrackback(trackback, "98053")).Callback(() => trackbackCreated = true);
            var service = new BlogImportService(repository.Object);

            // act
            service.Import(blog);

            // assert
            Assert.IsTrue(trackbackCreated);
        }

        [Test]
        public void Import_WithCreateCommentThrowingException_DoesNotPropagateException()
        {
            // arrange
            var blog = new BlogMLBlog();
            var post = new BlogMLPost();
            post.Comments.Add(new BlogMLComment());
            blog.Posts.Add(post);
            var repository = new Mock<IBlogImportRepository>();
            repository.Setup(r => r.CreateComment(It.IsAny<BlogMLComment>(), It.IsAny<string>())).Throws(new InvalidOperationException());
            var service = new BlogImportService(repository.Object);

            // act, assert
            service.Import(blog);
        }

        [Test]
        public void Import_WithCreateTrackbackThrowingException_DoesNotPropagateException()
        {
            // arrange
            var blog = new BlogMLBlog();
            var post = new BlogMLPost();
            post.Trackbacks.Add(new BlogMLTrackback());
            blog.Posts.Add(post);
            var repository = new Mock<IBlogImportRepository>();
            repository.Setup(r => r.CreateTrackback(It.IsAny<BlogMLTrackback>(), It.IsAny<string>())).Throws(new InvalidOperationException());
            var service = new BlogImportService(repository.Object);

            // act, assert
            service.Import(blog);
        }

        [Test]
        public void ImportBlog_WithStream_DeserializesBlog()
        {
            // arrange
            var stream = @"<?xml version=""1.0"" encoding=""utf-8""?>
                                <blog root-url=""http://localhost:1608/SUBWebV2/"" 
                                    date-created=""2006-05-06T23:06:32"" 
                                    xmlns=""http://www.blogml.com/2006/09/BlogML"" 
                                    xmlns:xs=""http://www.w3.org/2001/XMLSchema"">
                                  <title type=""text""><![CDATA[Blog Title]]></title>
                                  <sub-title type=""text""><![CDATA[Blog Subtitle]]></sub-title>
                                  <authors>
                                    <author id=""2100"" 
                                        date-created=""2006-08-10T08:44:35"" 
                                        date-modified=""2006-09-04T13:46:38"" 
                                        approved=""true"" 
                                        email=""someone@blogml.com"">
                                      <title type=""text""><![CDATA[The Author]]></title>
                                    </author>
                                  </authors>
                                  <posts>
                                    <post id=""b0e03eec-ab81-4dc4-a69b-374d57cfad5e"" 
                                        date-created=""2006-01-07T03:31:32"" 
                                        date-modified=""2006-01-07T03:31:32"" 
                                        approved=""true"" 
                                        post-url=""http://example.com/whatever"">
                                      <title type=""text""><![CDATA[Post Title]]></title>
                                      <content type=""base64"">
                                        <![CDATA[Q29udGVudCBvZiB0aGUgcG9zdA==]]>
                                      </content>
                                      <authors>
                                        <author ref=""2100"" />
                                      </authors>
                                    </post>
                                  </posts>
                                </blog>".ToStream();
            var repository = new Mock<IBlogImportRepository>();
            BlogMLPost deserializedPost = null;
            repository.Setup(r => r.CreateBlogPost(It.IsAny<BlogMLBlog>(), It.IsAny<BlogMLPost>())).Callback<BlogMLBlog, BlogMLPost>((blog, post) => deserializedPost = post);
            var service = new BlogImportService(repository.Object);
            
            // act
            service.ImportBlog(stream);

            // assert
            Assert.IsNotNull(deserializedPost);
            Assert.AreEqual("Post Title", deserializedPost.Title);
            Assert.AreEqual(ContentTypes.Base64, deserializedPost.Content.ContentType);
            Assert.AreEqual("Content of the post", deserializedPost.Content.UncodedText);
            Assert.AreEqual(1, deserializedPost.Authors.Count);
        }

        [Test]
        public void CreateFileFromAttachment_WithEmbeddedAttachment_CreatesFile()
        {
            // arrange
            var data = new byte[] {1, 2, 3};
            var attachment = new BlogMLAttachment {Url = "http://old.example.com/images/my-mug.jpg", Embedded = true, Data = data};
            string attachmentDirectoryPath = Path.Combine(Environment.CurrentDirectory, "images");
            Directory.CreateDirectory(ImageDirectory);

            // act
            BlogImportService.CreateFileFromAttachment(attachment, 
                attachmentDirectoryPath, 
                "http://example.com/images/", 
                "Some Content");

            // assert
            Assert.IsTrue(File.Exists(Path.Combine(ImageDirectory, "my-mug.jpg")));
        }

        [Test]
        public void CreateFileFromAttachment_WithOutEmbeddedAttachment_RewritesPostContent()
        {
            // arrange
            var attachment = new BlogMLAttachment { Url = "http://old.example.com/images/my-mug.jpg", Embedded = false};
            string attachmentDirectoryPath = ImageDirectory;
            Directory.CreateDirectory(attachmentDirectoryPath);
            const string originalPostContent = @"<img src=""http://old.example.com/images/my-mug.jpg"" />";

            // act
            string postContent = BlogImportService.CreateFileFromAttachment(attachment, 
                attachmentDirectoryPath, 
                "http://example.com/images/", 
                originalPostContent);

            // assert
            Assert.AreEqual(@"<img src=""http://example.com/images/my-mug.jpg"" />", postContent);
        }

        [Test]
        public void Import_WithEmbeddedAttachments_CreatesFilesForAttachmentsAndRewritesBlogPost()
        {
            // arrange
            var data = new byte[] { 1, 2, 3 };
            var attachment = new BlogMLAttachment { Url = "http://old.example.com/images/my-mug.jpg", Embedded = true, Data = data };
            var post = new BlogMLPost { Content = new BlogMLContent { Text = @"<img src=""http://old.example.com/images/my-mug.jpg"" />" } };
            post.Attachments.Add(attachment);
            var blog = new BlogMLBlog();
            blog.Posts.Add(post);
            var repository = new Mock<IBlogImportRepository>();
            repository.Setup(r => r.GetAttachmentDirectoryPath()).Returns(ImageDirectory + "/wlw");
            repository.Setup(r => r.GetAttachmentDirectoryUrl()).Returns("http://example.com/images/wlw/");
            var service = new BlogImportService(repository.Object);
            
            // act
            service.Import(blog);

            // assert
            Assert.IsTrue(File.Exists(Path.Combine(ImageDirectory, @"wlw\my-mug.jpg")));
            Assert.AreEqual(@"<img src=""http://example.com/images/wlw/my-mug.jpg"" />", post.Content.UncodedText);
        }

        private static string ImageDirectory
        {
            get
            {
                return Path.Combine(Environment.CurrentDirectory, "images");
            }
        }

        [SetUp]
        public void Setup()
        {
            //Make sure no files are left over from last time.
            TearDown();
        }

        [TearDown]
        public void TearDown()
        {
            if(Directory.Exists(ImageDirectory))
            {
                try
                {
                    Directory.Delete(ImageDirectory, true);
                }
                catch(Exception)
                {
                    Console.WriteLine("Could not delete " + ImageDirectory);
                }
            }
        }
    }
}