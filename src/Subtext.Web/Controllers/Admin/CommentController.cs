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

using System.Web.Mvc;
using Subtext.Framework.Components;
using Subtext.Framework.Services;

namespace Subtext.Web.Controllers
{
    [Authorize(Roles = "Admins")]
    public class CommentController : Controller
    {
        public CommentController(ICommentService commentService)
        {
            CommentService = commentService;
        }

        protected ICommentService CommentService
        {
            get;
            private set;
        }

        public ActionResult UpdateStatus(int id, FeedbackStatusFlag status)
        {
            var comment = CommentService.Get(id);
            string subject = string.Format("Comment by {0}", comment.Author);
            string predicate = null;
            switch (status)
            {
                case FeedbackStatusFlag.Approved:
                    predicate = "has been approved";
                    break;

                case FeedbackStatusFlag.Deleted:
                    predicate = "has been removed";
                    break;

                case FeedbackStatusFlag.FlaggedAsSpam:
                    predicate = "has been flagged as spam";
                    break;
            }
            CommentService.UpdateStatus(comment, status);

            return Json(new { subject, predicate });
        }

        public ActionResult Destroy(int id)
        {
            var feedback = CommentService.Get(id);
            string subject = string.Format("Comment by {0}", feedback.Author);
            const string predicate = "was destroyed (there is no undo)";
            CommentService.Destroy(id);

            return Json(new { subject, predicate });
        }
    }
}
