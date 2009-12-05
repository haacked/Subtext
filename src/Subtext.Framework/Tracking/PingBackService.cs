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

#region Notes

///////////////////////////////////////////////////////////////////////////////////////////////////
// The code in this file is freely distributable.
// 
// ASPNetWeblog isnot responsible for, shall have no liability for 
// and disclaims all warranties whatsoever, expressed or implied, related to this code,
// including without limitation any warranties related to performance, security, stability,
// or non-infringement of title of the control.
// 
// If you have any questions, comments or concerns, please contact
// Scott Watermasysk, Scott@TripleASP.Net.
// 
// For more information on this control, updates, and other tools to integrate blogging 
// into your existing applications, please visit, http://aspnetweblog.com
// 
// Originally based off of code by Simon Fell http://www.pocketsoap.com/weblog/ 
// 
///////////////////////////////////////////////////////////////////////////////////////////////////

#endregion

using System;
using System.Globalization;
using CookComputing.XmlRpc;
using Subtext.Extensibility.Providers;
using Subtext.Framework.Components;
using Subtext.Framework.Email;
using Subtext.Framework.Properties;
using Subtext.Framework.Services;
using Subtext.Framework.Text;
using Subtext.Framework.Util;
using Subtext.Framework.XmlRpc;

namespace Subtext.Framework.Tracking
{
    /// <summary>
    /// Service used to receive pingbacks from remote clients.
    /// </summary>
    public class PingBackService : SubtextXmlRpcService
    {
        public PingBackService(ISubtextContext context)
            : base(context)
        {
        }

        /// <summary>
        /// Method called by a remote client to ping this server.
        /// </summary>
        /// <param name="sourceURI">Source URI.</param>
        /// <param name="targetURI">Target URI.</param>
        /// <returns></returns>
        [XmlRpcMethod("pingback.ping", Description = "Pingback server implementation")]
        public string pingBack(string sourceURI, string targetURI)
        {
            if(!Blog.TrackbacksEnabled)
            {
                return "Pingbacks are not enabled for this site.";
            }

            string pageTitle;

            // GetPostIDFromUrl returns the postID
            int? id = SubtextContext.RequestContext.GetIdFromRequest();
            if(id == null)
            {
                throw new XmlRpcFaultException(33, Resources.XmlRcpFault_DidNotLinkToPermalink);
            }

            Uri sourceUrl = sourceURI.ParseUri();
            Uri targetUrl = targetURI.ParseUri();

            // does the sourceURI actually contain the permalink ?
            if(sourceUrl == null || targetUrl == null ||
               !Verifier.SourceContainsTarget(sourceUrl, targetUrl, out pageTitle))
            {
                throw new XmlRpcFaultException(17, Resources.XmlRcpFault_InvalidLink);
            }

            //PTR = Pingback - TrackBack - Referral
            var trackback = new Trackback(id.Value, HtmlHelper.SafeFormat(pageTitle, SubtextContext.HttpContext.Server),
                                          new Uri(sourceURI), string.Empty,
                                          HtmlHelper.SafeFormat(pageTitle, SubtextContext.HttpContext.Server),
                                          Blog.TimeZone.Now);
            ICommentSpamService feedbackService = null;
            if(Blog.FeedbackSpamServiceEnabled)
            {
                feedbackService = new AkismetSpamService(Blog.FeedbackSpamServiceKey, Blog, null, Url);
            }
            var commentService = new CommentService(SubtextContext, new CommentFilter(SubtextContext, feedbackService));
            commentService.Create(trackback, true/*runFilters*/);

            //TODO: Create this using IoC container
            var emailService = new EmailService(EmailProvider.Instance(), new EmbeddedTemplateEngine(), SubtextContext);
            emailService.EmailCommentToBlogAuthor(trackback);

            return String.Format(CultureInfo.InvariantCulture, Resources.XmlRpcMessage_ThanksForThePingback, sourceURI);
        }
    }
}