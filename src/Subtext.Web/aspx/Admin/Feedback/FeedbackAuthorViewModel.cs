using System.Net;
using System.Web;
using Subtext.Framework.Components;
using Subtext.Framework.Text;
using Subtext.Framework.Web;

namespace Subtext.Web
{
    public class FeedbackAuthorViewModel
    {
        public FeedbackAuthorViewModel(object comment)
            : this((FeedbackItem)comment)
        {
        }

        public FeedbackAuthorViewModel(FeedbackItem comment)
        {
            IpAddress = comment.IpAddress;
            Name = HttpUtility.HtmlEncode(comment.Author);
            Url = HttpUtility.HtmlEncode(comment.SourceUrl);
            if (!string.IsNullOrEmpty(Url))
            {
                SetUrlLink();
            }

            if (!string.IsNullOrEmpty(comment.Email) && comment.Email.IndexOf("@") > 0)
            {
                Email = HttpUtility.HtmlEncode(comment.Email);
                FormattedEmail = "&lt;" + Email + "&gt;";
                SetMailTo(comment);
            }
        }

        void SetMailTo(FeedbackItem feedback)
        {
            string safeAuthor = feedback.Author.EncodeForMailTo();
            string safeTitle = feedback.Title.EncodeForMailTo();
            string safeBody = feedback.Body.EncodeMailToBody();

            string mailToUrl = Email
                               + "&subject=re:" + safeTitle
                               + "&body=----------%0A"
                               + "From: " + safeAuthor + " (" + Email + ")%0A"
                               + "Sent: " + feedback.DateCreatedUtc.ToString().EncodeForMailTo() + "%0A"
                               + "Subject: " + safeTitle.Replace("+", " ") + "%0A%0A"
                               + safeBody;
            MailTo =
                string.Format(
                    @"<a href=""mailto:{0}"" title=""{1}""><img src=""{2}"" alt=""{1}"" border=""0"" class=""email"" /></a>",
                    mailToUrl, Email, HttpHelper.ExpandTildePath("~/images/email.gif"));
        }

        void SetUrlLink()
        {
            UrlLink = string.Format(@"<a href=""{0}"" title=""{1}""><img src=""{2}"" alt=""{1}"" border=""0"" /></a>",
                      Url, Url, HttpHelper.ExpandTildePath("~/images/permalink.gif"));

        }

        public bool HasEmail
        {
            get
            {
                return !string.IsNullOrEmpty(Email);
            }
        }

        public string Email
        {
            get;
            private set;
        }

        public string MailTo
        {
            get;
            private set;
        }

        public string FormattedEmail
        {
            get;
            private set;
        }

        public IPAddress IpAddress
        {
            get;
            private set;
        }

        public string Name
        {
            get;
            private set;
        }

        public string Url
        {
            get;
            private set;
        }

        public string UrlLink
        {
            get;
            private set;
        }
    }
}