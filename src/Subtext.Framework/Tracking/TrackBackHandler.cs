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
using System.Web;
using System.Xml;
using Subtext.Extensibility.Interfaces;
using Subtext.Extensibility.Providers;
using Subtext.Framework.Components;
using Subtext.Framework.Email;
using Subtext.Framework.Exceptions;
using Subtext.Framework.Logging;
using Subtext.Framework.Properties;
using Subtext.Framework.Routing;
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
        static readonly Log Log = new Log();

        public TrackBackHandler(ISubtextContext subtextContext) : base(subtextContext)
        {
        }

        public override void ProcessRequest()
        {
            if(!SubtextContext.Blog.TrackbacksEnabled)
            {
                return;
            }

            try
            {
                HandleTrackback(SubtextContext);
            }
            catch(BaseCommentException e)
            {
                Log.Info("Comment exception occurred.", e);
            }
        }

        private void HandleTrackback(ISubtextContext subtextContext)
        {
            HttpContextBase httpContext = subtextContext.RequestContext.HttpContext;
            httpContext.Response.ContentType = "text/xml";

            Entry entry;

            int? id = subtextContext.RequestContext.GetIdFromRequest();
            if(id != null)
            {
                entry = subtextContext.Repository.GetEntry(id.Value, true /* activeOnly */, false
                    /* includeCategories */);
            }
            else
            {
                string slug = subtextContext.RequestContext.GetSlugFromRequest();
                entry = subtextContext.Repository.GetEntry(slug, true /* activeOnly */, false /* includeCategories */);
            }

            if(entry == null)
            {
                Log.Info(string.Format(CultureInfo.InvariantCulture, Resources.Log_CouldNotExtractEntryId,
                                       httpContext.Request.Path));
                SendTrackbackResponse(httpContext, 1, Resources.TrackbackResponse_EntryIdMissing);
                return;
            }

            if(httpContext.Request.HttpMethod == "POST")
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
            var writer = new XmlTextWriter(context.RequestContext.HttpContext.Response.Output) {Formatting = Formatting.Indented};

            string url = context.UrlHelper.TrackbacksUrl(postId).ToFullyQualifiedUrl(context.Blog).ToString();

            writer.WriteStartDocument();
            writer.WriteStartElement("response");
            writer.WriteElementString("error", "0");
            writer.WriteStartElement("rss");
            writer.WriteAttributeString("version", "0.91");
            writer.WriteStartElement("channel");
            writer.WriteElementString("title", entry.Title);
            writer.WriteElementString("link", url);
            writer.WriteElementString("description", string.Empty);
            writer.WriteElementString("language", "en-us");

            writer.WriteEndElement(); // channel
            writer.WriteEndElement(); // rss 
            writer.WriteEndElement(); // response
            writer.WriteEndDocument();
        }

        private void CreateTrackbackAndSendResponse(ISubtextContext subtextContext, IEntryIdentity entry, int entryId)
        {
            HttpContextBase context = subtextContext.RequestContext.HttpContext;
            string title = SafeParam(context, "title");
            string excerpt = SafeParam(context, "excerpt");
            string urlText = SafeParam(context, "url");
            string blogName = SafeParam(context, "blog_name");

            Uri url = urlText.ParseUri();
            if(url == null)
            {
                SendTrackbackResponse(context, 1, Resources.TrackbackResponse_NoUrl);
                return;
            }

            if(entry == null ||
               !IsSourceVerification(url,
                                     subtextContext.UrlHelper.EntryUrl(entry).ToFullyQualifiedUrl(subtextContext.Blog)))
            {
                SendTrackbackResponse(context, 2,
                                      String.Format(CultureInfo.InvariantCulture,
                                                    Resources.TrackbackResponse_NoRelevantLink, url));
                return;
            }

            var trackback = new Trackback(entryId, title, url, blogName, excerpt, Blog.TimeZone.Now);
            ICommentSpamService feedbackService = null;
            Blog blog = subtextContext.Blog;
            if(blog.FeedbackSpamServiceEnabled)
            {
                feedbackService = new AkismetSpamService(blog.FeedbackSpamServiceKey, blog, null, Url);
            }
            var commentService = new CommentService(SubtextContext, new CommentFilter(SubtextContext, feedbackService));
            commentService.Create(trackback, true/*runFilters*/);
            //TODO: Create this using IoC container
            var emailService = new EmailService(EmailProvider.Instance(), new EmbeddedTemplateEngine(), subtextContext);
            emailService.EmailCommentToBlogAuthor(trackback);
        }

        private static void SendTrackbackResponse(HttpContextBase context, int errorNumber, string errorMessage)
        {
            var d = new XmlDocument();
            XmlElement root = d.CreateElement("response");
            d.AppendChild(root);
            XmlElement er = d.CreateElement("error");
            root.AppendChild(er);
            er.AppendChild(d.CreateTextNode(errorNumber.ToString(CultureInfo.InvariantCulture)));
            if(errorMessage.Length > 0)
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
            if(context.Request.Form[pName] != null)
            {
                return HtmlHelper.SafeFormat(context.Request.Form[pName], context.Server);
            }
            return string.Empty;
        }

        public event EventHandler<SourceVerificationEventArgs> SourceVerification;

        private bool IsSourceVerification(Uri sourceUrl, Uri entryUrl)
        {
            EventHandler<SourceVerificationEventArgs> handler = SourceVerification;
            if(handler != null)
            {
                var args = new SourceVerificationEventArgs(sourceUrl, entryUrl);
                handler(this, args);
                return args.Verified;
            }
            return Verifier.SourceContainsTarget(sourceUrl, entryUrl);
        }
    }
}