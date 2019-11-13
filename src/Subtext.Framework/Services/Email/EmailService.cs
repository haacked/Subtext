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
using System.Globalization;
using Subtext.Extensibility;
using Subtext.Extensibility.Providers;
using Subtext.Framework.Components;
using Subtext.Framework.Properties;
using Subtext.Framework.Routing;
using Subtext.Framework.Security;

namespace Subtext.Framework.Email
{
    public class EmailService : IEmailService
    {
        public EmailService(EmailProvider provider, ITemplateEngine templateEngine, ISubtextContext context)
        {
            EmailProvider = provider;
            TemplateEngine = templateEngine;
            Url = context.UrlHelper;
            Blog = context.Blog;
            Context = context;
        }

        protected EmailProvider EmailProvider { get; private set; }

        protected ITemplateEngine TemplateEngine { get; private set; }

        protected Blog Blog { get; private set; }

        protected BlogUrlHelper Url { get; private set; }

        protected ISubtextContext Context { get; private set; }

        public void EmailCommentToBlogAuthor(FeedbackItem comment)
        {
            if (String.IsNullOrEmpty(Blog.Email)
               || comment.FeedbackType == FeedbackType.PingTrack
               || Context.User.IsAdministrator())
            {
                return;
            }

            string fromEmail = comment.Email;
            if (String.IsNullOrEmpty(fromEmail))
            {
                fromEmail = null;
            }

            var commentForTemplate = new
            {
                blog = Blog,
                comment = new
                {
                    author = comment.Author,
                    title = comment.Title,
                    source = Url.FeedbackUrl(comment).ToFullyQualifiedUrl(Blog),
                    email = fromEmail ?? "none given",
                    authorUrl = comment.SourceUrl,
                    ip = comment.IpAddress,
                    // we're sending plain text email by default, but body includes <br />s for crlf
                    body =
                        (comment.Body ?? string.Empty).Replace("<br />", Environment.NewLine).Replace("&lt;br /&gt;",
                                                                                                      Environment.
                                                                                                          NewLine)
                },
                spamFlag = comment.FlaggedAsSpam ? "Spam Flagged " : ""
            };

            ITextTemplate template = TemplateEngine.GetTemplate("CommentReceived");
            string message = template.Format(commentForTemplate);
            string subject = String.Format(CultureInfo.InvariantCulture, Resources.Email_CommentVia, comment.Title,
                                           Blog.Title);
            if (comment.FlaggedAsSpam)
            {
                subject = "[SPAM Flagged] " + subject;
            }
            string from = EmailProvider.UseCommentersEmailAsFromAddress
                              ? (fromEmail ?? EmailProvider.AdminEmail)
                              : EmailProvider.AdminEmail;

            EmailProvider.Send(Blog.Email, from, subject, message);
        }
    }
}