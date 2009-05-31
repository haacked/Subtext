using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Xml;
using Subtext.Framework.ModelBinders;
using Subtext.Framework.Services;
using Subtext.Framework.Components;
using Subtext.Extensibility;
using Subtext.Framework;
using Subtext.Framework.Format;

namespace Subtext.Web.Controllers
{
    public class CommentApiController : Controller
    {
        public CommentApiController(ISubtextContext context, ICommentService commentService) {
            CommentService = commentService;
        }

        public ICommentService CommentService { 
            get; 
            private set; 
        }

        public ISubtextContext SubtextContext {
            get;
            private set;
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public void Create(int id, [ModelBinder(typeof(XmlModelBinder))]XmlDocument xml) {
            if (xml == null) {
                throw new ArgumentNullException("xml");
            }
            FeedbackItem comment = new FeedbackItem(FeedbackType.Comment);
            comment.CreatedViaCommentAPI = true;
            
            string name = (xml.SelectSingleNode("//item/author") ?? Empty).InnerText;
            if (name.IndexOf("<") != -1)
            {
                name = name.Substring(0, name.IndexOf("<"));
            }
            comment.Author = name.Trim();
            comment.Body = (xml.SelectSingleNode("//item/description") ?? Empty).InnerText;
            comment.Title = (xml.SelectSingleNode("//item/title") ?? Empty).InnerText;
            comment.SourceUrl = Subtext.Framework.Text.HtmlHelper.CheckForUrl((xml.SelectSingleNode("//item/link") ?? Empty).InnerText);
            comment.EntryId = id;
            CommentService.Create(comment);
        }

        private static XmlNode Empty = new XmlDocument();
    }
}
