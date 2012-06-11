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
using System.Web.Mvc;
using System.Xml;
using Subtext.Extensibility;
using Subtext.Framework;
using Subtext.Framework.Components;
using Subtext.Framework.ModelBinders;
using Subtext.Framework.Services;
using HtmlHelper = Subtext.Framework.Text.HtmlHelper;

namespace Subtext.Web.Controllers
{
    public class CommentApiController : Controller
    {
        private static readonly XmlNode Empty = new XmlDocument();

        public CommentApiController(ISubtextContext context, ICommentService commentService)
        {
            SubtextContext = context;
            CommentService = commentService;
        }

        public ICommentService CommentService { get; private set; }

        public ISubtextContext SubtextContext { get; private set; }

        [AcceptVerbs(HttpVerbs.Post)]
        public void Create(int id, [ModelBinder(typeof(XmlModelBinder))] XmlDocument xml)
        {
            if (xml == null)
            {
                throw new ArgumentNullException("xml");
            }
            var comment = new FeedbackItem(FeedbackType.Comment) { CreatedViaCommentApi = true };

            string name = (xml.SelectSingleNode("//item/author") ?? Empty).InnerText;
            if (name.IndexOf("<") != -1)
            {
                name = name.Substring(0, name.IndexOf("<"));
            }
            comment.Author = name.Trim();
            comment.Body = (xml.SelectSingleNode("//item/description") ?? Empty).InnerText;
            comment.Title = (xml.SelectSingleNode("//item/title") ?? Empty).InnerText;
            comment.SourceUrl = HtmlHelper.EnsureUrl((xml.SelectSingleNode("//item/link") ?? Empty).InnerText);
            comment.EntryId = id;
            CommentService.Create(comment, true/*runFilters*/);
        }
    }
}