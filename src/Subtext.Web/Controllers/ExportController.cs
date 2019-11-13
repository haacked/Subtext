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
using Subtext.Framework;
using Subtext.Framework.Web;
using Subtext.ImportExport;
using Subtext.Infrastructure.ActionResults;

namespace Subtext.Web.Controllers
{
    public class ExportController : Controller
    {
        public ExportController(IBlogMLSource source, Blog blog)
        {
            Source = source;
            Blog = blog;
        }

        public IBlogMLSource Source
        {
            get;
            private set;
        }

        public Blog Blog
        {
            get;
            private set;
        }

        public ActionResult BlogML(bool? embed)
        {
            var writer = new BlogMLWriter(Source, embed.Value);
            return new ExportActionResult(writer, Blog.Title.GetSafeFileName() + "-Export.xml");
        }
    }
}
