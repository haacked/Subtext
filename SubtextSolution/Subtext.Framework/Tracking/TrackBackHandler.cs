#region Disclaimer/Info
///////////////////////////////////////////////////////////////////////////////////////////////////
// Subtext WebLog
// 
// Subtext is an open source weblog system that is a fork of the .TEXT
// weblog system.
//
// For updated news and information please visit http://subtextproject.com/
// Subtext is hosted at Google Code at http://code.google.com/p/subtext/
// The development mailing list is at subtext-devs@lists.sourceforge.net 
//
// This project is licensed under the BSD license.  See the License.txt file for more information.
///////////////////////////////////////////////////////////////////////////////////////////////////
#endregion

using System;
using System.Globalization;
using System.Web;
using System.Xml;
using Subtext.Extensibility.Providers;
using Subtext.Framework.Components;
using Subtext.Framework.Email;
using Subtext.Framework.Exceptions;
using Subtext.Framework.Logging;
using Subtext.Framework.Properties;
using Subtext.Framework.Services;
using Subtext.Framework.Text;
using Subtext.Framework.Util;
using Subtext.Framework.Web.Handlers;

namespace Subtext.Framework.Tracking
{
    /// <summary>
    /// Service used to receive trackbacks from remote clients.
    /// </summary>
    public class TrackBackHandler : SubtextHttpHandler
    {
        static Log Log = new Log();

        public TrackBackHandler(ISubtextContext subtextContext) : base(subtextContext)
        {
        }

        public override void ProcessRequest()
        {
            if (!SubtextContext.Blog.TrackbacksEnabled)
            {
                return;
            }

            try
            {
                HandleTrackback(SubtextContext);
            }
            catch (BaseCommentException e)
            {
                Log.Info("Comment exception occurred.", e);
            }
        }

        private void HandleTrackback(ISubtextContext subtextContext)
        {
            var httpContext = subtextContext.RequestContext.HttpContext;
            httpContext.Response.ContentType = "text/xml";

            Entry entry;

            int? id = subtextContext.RequestContext.GetIdFromRequest();
            if (id != null)
            {
                entry = subtextContext.Repository.GetEntry(id.Value, true /* activeOnly */, false /* includeCategories */);
            }
            else
            {
                string slug = subtextContext.RequestContext.GetSlugFromRequest();
                entry = subtextContext.Repository.GetEntry(slug, true /* activeOnly */, false /* includeCategories */);
            }

            if (entry == null)
            {
                Log.Info(string.Format(CultureInfo.InvariantCulture, Resources.Log_CouldNotExtractEntryId, httpContext.Request.Path));
                SendTrackbackResponse(httpContext, 1, Resources.TrackbackResponse_EntryIdMissing);
                return;
            }

            if (httpContext.Request.HttpMethod == "POST")
            {
                CreateTrackbackAndSendResponse(subtextContext, entry, entry.Id);
            }
            else
            {
                SendTrackbackRss(subtextContext, entry, entry.Id);
            }
        }

        private static void SendTrackbackRss(ISubtextContext context, Entry entry, int postId)
        {
            XmlTextWriter w = new XmlTextWriter(context.RequestContext.HttpContext.Response.Output);
            w.Formatting = Formatting.Indented;

            string url = context.UrlHelper.TrackbacksUrl(postId).ToFullyQualifiedUrl(context.Blog).ToString();

            w.WriteStartDocument();
            w.WriteStartElement("response");
            w.WriteElementString("error", "0");
            w.WriteStartElement("rss");
            w.WriteAttributeString("version", "0.91");
            w.WriteStartElement("channel");
            w.WriteElementString("title", entry.Title);
            w.WriteElementString("link", url);
            w.WriteElementString("description", string.Empty);
            w.WriteElementString("language", "en-us");

            w.WriteEndElement(); // channel
            w.WriteEndElement(); // rss 
            w.WriteEndElement(); // response
            w.WriteEndDocument();
        }

        private void CreateTrackbackAndSendResponse(ISubtextContext subtextContext, Entry entry, int entryId)
        {
            var context = subtextContext.RequestContext.HttpContext;
            string title = SafeParam(context, "title");
            string excerpt = SafeParam(context, "excerpt");
            string urlText = SafeParam(context, "url");
            string blog_name = SafeParam(context, "blog_name");

            Uri url = HtmlHelper.ParseUri(urlText);
            if (url == null)
            {
                SendTrackbackResponse(context, 1, Resources.TrackbackResponse_NoUrl);
                return;
            }

            if (entry == null || !IsSourceVerification(url, subtextContext.UrlHelper.EntryUrl(entry).ToFullyQualifiedUrl(subtextContext.Blog)))
            {
                SendTrackbackResponse(context, 2, String.Format(CultureInfo.InvariantCulture, Resources.TrackbackResponse_NoRelevantLink, url));
                return;
            }

            Trackback trackback = new Trackback(entryId, title, url, blog_name, excerpt, Blog.TimeZone.Now);
            ICommentSpamService feedbackService = null;
            Blog blog = subtextContext.Blog;
            if (blog.FeedbackSpamServiceEnabled)
            {
                feedbackService = new AkismetSpamService(blog.FeedbackSpamServiceKey, blog, null, Url);
            }
            CommentService commentService = new CommentService(SubtextContext, new CommentFilter(SubtextContext, feedbackService));
            commentService.Create(trackback);
            //TODO: Create this using IoC container
            var emailService = new EmailService(EmailProvider.Instance(), new EmbeddedTemplateEngine(), subtextContext);
            emailService.EmailCommentToBlogAuthor(trackback);
        }

        private static void SendTrackbackResponse(HttpContextBase context, int errorNumber, string errorMessage)
        {
            XmlDocument d = new XmlDocument();
            XmlElement root = d.CreateElement("response");
            d.AppendChild(root);
            XmlElement er = d.CreateElement("error");
            root.AppendChild(er);
            er.AppendChild(d.CreateTextNode(errorNumber.ToString(CultureInfo.InvariantCulture)));
            if (errorMessage.Length > 0)
            {
                XmlElement msg = d.CreateElement("message");
                root.AppendChild(msg);
                msg.AppendChild(d.CreateTextNode(errorMessage));
            }
            d.Save(context.Response.Output);
            context.Response.Output.Flush();
        }

        private static string SafeParam(HttpContextBase context, string pName)
        {
            if (context.Request.Form[pName] != null)
                return HtmlHelper.SafeFormat(context.Request.Form[pName], context.Server);
            return string.Empty;
        }

        public event EventHandler<SourceVerificationEventArgs> SourceVerification;

        private bool IsSourceVerification(Uri sourceUrl, Uri entryUrl)
        {
            var handler = SourceVerification;
            if (handler != null)
            {
                SourceVerificationEventArgs args = new SourceVerificationEventArgs(sourceUrl, entryUrl);
                handler(this, args);
                return args.Verified;
            }
            else
            {
                return Verifier.SourceContainsTarget(sourceUrl, entryUrl);
            }
        }
    }
}