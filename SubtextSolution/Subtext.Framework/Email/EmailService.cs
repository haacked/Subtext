using System;
using System.Globalization;
using Subtext.Extensibility.Providers;
using Subtext.Framework.Components;
using Subtext.Framework.Routing;
using Subtext.Framework.Security;

namespace Subtext.Framework.Email
{
    public class EmailService : IEmailService {
        public EmailService(EmailProvider provider, ITemplateEngine templateEngine, ISubtextContext context) {
            EmailProvider = provider;
            TemplateEngine = templateEngine;
            Url = context.UrlHelper;
            Blog = context.Blog;
            Context = context;
        }

        protected EmailProvider EmailProvider {
            get;
            private set;
        }

        protected ITemplateEngine TemplateEngine {
            get;
            private set;
        }

        protected Blog Blog {
            get;
            private set;
        }

        protected UrlHelper Url {
            get;
            private set;
        }

        protected ISubtextContext Context {
            get;
            private set;
        }

        public void EmailCommentToBlogAuthor(FeedbackItem comment) {
            if (String.IsNullOrEmpty(Blog.Email) 
                || comment.FeedbackType == Subtext.Extensibility.FeedbackType.PingTrack
                || Context.User.IsInAdminRole(Blog)) {
                return;
            }

            string fromEmail = comment.Email;
            if (String.IsNullOrEmpty(fromEmail))
                fromEmail = null;
            
            var template = TemplateEngine.GetTemplate("CommentReceived");
            var commentForTemplate = new {
                blog = Blog,
                comment = new { 
                    author = comment.Author,
                    title = comment.Title,
                    source = Url.FeedbackUrl(comment).ToFullyQualifiedUrl(Blog),
                    email = fromEmail ?? "none given",
                    authorUrl = comment.SourceUrl,
                    ip = comment.IpAddress,
                    // we're sending plain text email by default, but body includes <br />s for crlf
                    body = (comment.Body ?? string.Empty).Replace("<br />", Environment.NewLine).Replace("&lt;br /&gt;", Environment.NewLine)
                },
                spamFlag = comment.FlaggedAsSpam ? "Spam Flagged " : ""
            };
            string message = template.Format(commentForTemplate);
            string subject = String.Format(CultureInfo.InvariantCulture, "Comment: {0} (via {1})", comment.Title, Blog.Title);
            if (comment.FlaggedAsSpam) {
                subject = "[SPAM Flagged] " + subject;
            }
            string from = fromEmail ?? EmailProvider.AdminEmail;
            EmailProvider.Send(Blog.Email, from, subject, message);
        }
    }
}
