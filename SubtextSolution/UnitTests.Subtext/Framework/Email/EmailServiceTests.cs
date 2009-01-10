using System;
using System.Net;
using MbUnit.Framework;
using Moq;
using Subtext.Extensibility;
using Subtext.Extensibility.Providers;
using Subtext.Framework;
using Subtext.Framework.Components;
using Subtext.Framework.Email;
using Subtext.Framework.Routing;

namespace UnitTests.Subtext.Framework.Email
{
    [TestFixture]
    public class EmailServiceTests
    {
        [Test]
        public void EmailCommentToBlogAuthor_WithCurrentUserIsAnAdmin_DoesNotSendEmail()
        {
            //arrange
            var comment = new FeedbackItem(FeedbackType.Comment) { };
            var blog = new Blog { Email = "cody@example.com", UserName = "cody" };
            var emailProvider = new Mock<EmailProvider>();
            var context = new Mock<ISubtextContext>();
            context.Expect(c => c.UrlHelper).Returns(new Mock<UrlHelper>().Object);
            context.Expect(c => c.Blog).Returns(blog);
            context.Expect(c => c.User.Identity.Name).Returns("cody");
            context.Expect(c => c.User.IsInRole("Admins")).Returns(true);
            var emailService = new EmailService(emailProvider.Object, new Mock<ITemplateEngine>().Object, context.Object);
            emailProvider.Expect(e => e.Send(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Never();

            //act
            emailService.EmailCommentToBlogAuthor(comment);
        }

        [Test]
        public void EmailCommentToBlogAuthor_WithBlogHavingNullEmail_DoesNotSendEmail()
        {
            //arrange
            var comment = new FeedbackItem(FeedbackType.Comment) { };
            var blog = new Blog { Email = string.Empty };
            var emailProvider = new Mock<EmailProvider>();
            var context = new Mock<ISubtextContext>();
            context.Expect(c => c.UrlHelper).Returns(new Mock<UrlHelper>().Object);
            context.Expect(c => c.Blog).Returns(blog);
            var emailService = new EmailService(emailProvider.Object, new Mock<ITemplateEngine>().Object, context.Object);
            emailProvider.Expect(e => e.Send(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Never();
            
            //act
            emailService.EmailCommentToBlogAuthor(comment);
        }

        [Test]
        public void EmailCommentToBlogAuthor_WithCommentThatIsTrackback_DoesNotSendEmail()
        {
            //arrange
            var comment = new FeedbackItem(FeedbackType.PingTrack) { };
            var blog = new Blog { Email = "foo@example.com" };
            var context = new Mock<ISubtextContext>();
            context.Expect(c => c.UrlHelper).Returns(new Mock<UrlHelper>().Object);
            context.Expect(c => c.Blog).Returns(blog);
            var emailProvider = new Mock<EmailProvider>();
            var emailService = new EmailService(emailProvider.Object, new Mock<ITemplateEngine>().Object, context.Object);
            emailProvider.Expect(e => e.Send(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Never();

            //act
            emailService.EmailCommentToBlogAuthor(comment);
        }

        [Test]
        public void EmailCommentToBlogAuthor_WithComment_UsesTitleForSubject() {
            //arrange
            var comment = new FeedbackItem(FeedbackType.Comment) { Id = 121, Author = "me", Title = "the subject", FlaggedAsSpam = false };
            var emailProvider = new Mock<EmailProvider>();
            var emailService = SetupEmailService(comment, emailProvider);
            string subject = null;
            emailProvider.Expect(e => e.Send(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Callback<string, string, string, string>((to, from, title, message) => subject = title);
            //act
            emailService.EmailCommentToBlogAuthor(comment);

            //assert
            Assert.AreEqual("Comment: the subject (via the blog)", subject);
        }

        [Test]
        public void EmailCommentToBlogAuthor_WithCommentFlaggedAsSpam_PrefacesSubjectWithSpamHeader()
        {
            //arrange
            var comment = new FeedbackItem(FeedbackType.Comment) { Id = 121, FlaggedAsSpam = true, Author = "me", Title = "the subject" };
            var emailProvider = new Mock<EmailProvider>();
            var emailService = SetupEmailService(comment, emailProvider);
            string subject = null;
            emailProvider.Expect(e => e.Send(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Callback<string, string, string, string>((to, from, title, message) => subject = title);
            //act
            emailService.EmailCommentToBlogAuthor(comment);

            //assert
            Assert.AreEqual("[SPAM Flagged] Comment: the subject (via the blog)", subject);
        }

        [Test]
        public void EmailCommentToBlogAuthor_WithCommentHavingEmail_UsesEmailAsFromEmail()
        {
            //arrange
            var comment = new FeedbackItem(FeedbackType.Comment) { Id = 121, Email="from@example.com", Author = "me", Title = "the subject", FlaggedAsSpam = true };
            var emailProvider = new Mock<EmailProvider>();
            var emailService = SetupEmailService(comment, emailProvider);
            string fromEmail = null;
            emailProvider.Expect(e => e.Send(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Callback<string, string, string, string>((to, from, title, message) => fromEmail = from);
            //act
            emailService.EmailCommentToBlogAuthor(comment);

            //assert
            Assert.AreEqual("from@example.com", fromEmail);
        }

        [Test]
        public void EmailCommentToBlogAuthor_WithCommentHavingNullEmail_UsesProviderEmail()
        {
            //arrange
            var comment = new FeedbackItem(FeedbackType.Comment) { Id = 121, Email = null, Author = "me", Title = "the subject", FlaggedAsSpam = true };
            var emailProvider = new Mock<EmailProvider>();
            emailProvider.Object.AdminEmail = "admin@example.com";
            var emailService = SetupEmailService(comment, emailProvider);
            string fromEmail = null;
            emailProvider.Expect(e => e.Send(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Callback<string, string, string, string>((to, from, title, message) => fromEmail = from);
            //act
            emailService.EmailCommentToBlogAuthor(comment);

            //assert
            Assert.AreEqual("admin@example.com", fromEmail);
        }

        private static EmailService SetupEmailService(FeedbackItem comment, Mock<EmailProvider> emailProvider)
        {
            var templateEngine = new Mock<ITemplateEngine>();
            var template = new Mock<ITextTemplate>();
            templateEngine.Expect(t => t.GetTemplate(It.IsAny<string>())).Returns(template.Object);
            template.Expect(t => t.Format(It.IsAny<object>())).Returns("message");
            var urlHelper = new Mock<UrlHelper>();
            urlHelper.Expect(u => u.FeedbackUrl(comment)).Returns("/");
            var context = new Mock<ISubtextContext>();
            context.Expect(c => c.UrlHelper).Returns(urlHelper.Object);
            context.Expect(c => c.Blog).Returns(new Blog { Email = "test@test.com", Author = "to", Host = "localhost", Title = "the blog" });

            var emailService = new EmailService(emailProvider.Object, templateEngine.Object, context.Object);
            return emailService;
        }

        [Test]
        public void EmailCommentToBlogAuthor_WithBlog_UsesBlogEmailForToEmail()
        {
            //arrange
            var comment = new FeedbackItem(FeedbackType.Comment) { Id = 121, Author = "me", Title = "the subject", FlaggedAsSpam = false };
            var emailProvider = new Mock<EmailProvider>();
            var templateEngine = new Mock<ITemplateEngine>();
            var template = new Mock<ITextTemplate>();
            templateEngine.Expect(t => t.GetTemplate(It.IsAny<string>())).Returns(template.Object);
            template.Expect(t => t.Format(It.IsAny<object>())).Returns("message");
            var urlHelper = new Mock<UrlHelper>();
            urlHelper.Expect(u => u.FeedbackUrl(comment)).Returns("/");
            var context = new Mock<ISubtextContext>();
            context.Expect(c => c.UrlHelper).Returns(urlHelper.Object);
            context.Expect(c => c.Blog).Returns(new Blog { Email = "test@test.com", Author = "to", Host = "localhost", Title = "the blog" });

            string toEmail = null;
            emailProvider.Expect(e => e.Send(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Callback<string, string, string, string>((to, from, title, message) => toEmail = to);
            var emailService = new EmailService(emailProvider.Object, templateEngine.Object, context.Object);

            //act
            emailService.EmailCommentToBlogAuthor(comment);

            //assert
            Assert.AreEqual("test@test.com", toEmail);
        }

        [Test]
        public void EmailCommentToBlogAuthor_WithCommentFlaggedAsSpam_SetsSpamField()
        {
            //arrange
            var comment = new FeedbackItem(FeedbackType.Comment) { Id = 121, Author = "me", Title = "subject", FlaggedAsSpam = true };
            string sentMessage = null;
            var emailService = SetupEmailService(comment, "{spamflag}", sent => sentMessage = sent);

            //act
            emailService.EmailCommentToBlogAuthor(comment);

            //assert
            Assert.AreEqual("Spam Flagged ", sentMessage);
        }

        [Test]
        public void EmailCommentToBlogAuthor_WithCommentHavingId_SetsSourceFieldWithUrlContainingId()
        {
            //arrange
            var comment = new FeedbackItem(FeedbackType.Comment) { Id=121, Author = "me", Title = "subject", FlaggedAsSpam = true };
            string sentMessage = null;
            var emailService = SetupEmailService(comment, "{comment.source}", sent => sentMessage = sent);

            //act
            emailService.EmailCommentToBlogAuthor(comment);

            //assert
            Assert.AreEqual("http://localhost/comment#121", sentMessage);
        }

        [Test]
        public void EmailCommentToBlogAuthor_WithEmail_SetsFromEmailAccordingly()
        {
            //arrange
            var comment = new FeedbackItem(FeedbackType.Comment) { Id = 121, Author = "me", Title = "subject", Email = "test@example.com"};
            string sentMessage = null;
            var emailService = SetupEmailService(comment, "{comment.email}", sent => sentMessage = sent);

            //act
            emailService.EmailCommentToBlogAuthor(comment);

            //assert
            Assert.AreEqual("test@example.com", sentMessage);
        }

        [Test]
        public void EmailCommentToBlogAuthor_WithoutEmail_SetsFromEmailToNoneProvided()
        {
            //arrange
            var comment = new FeedbackItem(FeedbackType.Comment) { Id = 121, Author = "me", Title = "subject", Email = null };
            string sentMessage = null;
            var emailService = SetupEmailService(comment, "{comment.email}", sent => sentMessage = sent);

            //act
            emailService.EmailCommentToBlogAuthor(comment);

            //assert
            Assert.AreEqual("none given", sentMessage);
        }

        [Test]
        public void EmailCommentToBlogAuthor_WithAuthor_SetsAuthorName()
        {
            //arrange
            var comment = new FeedbackItem(FeedbackType.Comment) { Id = 121, Author = "me", Title = "subject", Email = null };
            string sentMessage = null;
            var emailService = SetupEmailService(comment, "{comment.author}", sent => sentMessage = sent);

            //act
            emailService.EmailCommentToBlogAuthor(comment);

            //assert
            Assert.AreEqual("me", sentMessage);
        }

        [Test]
        public void EmailCommentToBlogAuthor_WithSourceUrlSpecified_SetsUrl()
        {
            //arrange
            var comment = new FeedbackItem(FeedbackType.Comment) { Id = 121, Author = "me", Title = "subject", SourceUrl = new Uri("http://example.com/") };
            string sentMessage = null;
            var emailService = SetupEmailService(comment, "{comment.authorUrl}", sent => sentMessage = sent);

            //act
            emailService.EmailCommentToBlogAuthor(comment);

            //assert
            Assert.AreEqual("http://example.com/", sentMessage);
        }

        [Test]
        public void EmailCommentToBlogAuthor_WithSourceIp_SetsIp()
        {
            //arrange
            var comment = new FeedbackItem(FeedbackType.Comment) { Id = 121, Author = "me", Title = "subject", IpAddress = IPAddress.Parse("127.0.0.1")};
            string sentMessage = null;
            var emailService = SetupEmailService(comment, "{comment.ip}", sent => sentMessage = sent);

            //act
            emailService.EmailCommentToBlogAuthor(comment);

            //assert
            Assert.AreEqual("127.0.0.1", sentMessage);
        }

        [Test]
        public void EmailCommentToBlogAuthor_WithBodyContainingHtml_CleansHtml()
        {
            //arrange
            var comment = new FeedbackItem(FeedbackType.Comment) { Id = 121, Author = "me", Title = "subject", Body = "This<br />is not&lt;br /&gt;right" };
            string sentMessage = null;
            var emailService = SetupEmailService(comment, "{comment.body}", sent => sentMessage = sent);

            //act
            emailService.EmailCommentToBlogAuthor(comment);

            //assert
            Assert.AreEqual("This" + Environment.NewLine + "is not" + Environment.NewLine + "right", sentMessage);
        }

        private EmailService SetupEmailService(FeedbackItem comment, string templateText, Action<string> messageCallback) { 
            var emailProvider = new Mock<EmailProvider>();
            var templateEngine = new Mock<ITemplateEngine>();
            var template = new NamedFormatTextTemplate(templateText);
            var urlHelper = new Mock<UrlHelper>();
            var context = new Mock<ISubtextContext>();
            context.Expect(c => c.UrlHelper).Returns(urlHelper.Object);
            context.Expect(c => c.Blog).Returns(new Blog { Email = "foo@example.com", Author = "to", Host = "localhost" });
            
            urlHelper.Expect(u => u.FeedbackUrl(comment)).Returns<FeedbackItem>(f => "/comment#" + f.Id);
            templateEngine.Expect(t => t.GetTemplate("CommentReceived")).Returns(template);
            emailProvider.Expect(e => e.Send(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Callback<string, string, string, string>((to, from, title, message) => messageCallback(message));
            
            return new EmailService(emailProvider.Object, templateEngine.Object, context.Object);
        }
    }
}
