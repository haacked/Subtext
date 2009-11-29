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

using System.Collections.Specialized;
using System.Configuration;
using System.Web.Mvc;
using Subtext.Framework.Services.Identicon;
using Subtext.Identicon;
using Subtext.Infrastructure.ActionResults;

namespace Subtext.Web.Controllers
{
    public class IdenticonController : Controller
    {
        public IdenticonController() : this(null)
        {
        }

        public IdenticonController(NameValueCollection appSettings)
        {
            var settings = appSettings ?? ConfigurationManager.AppSettings;
            int identiconSize;
            Size = int.TryParse(settings["IdenticonSize"], out identiconSize) ? identiconSize : 40;
        }

        public int Size
        {
            get; 
            private set;
        }

        public ActionResult Image(int? code)
        {
            if(code == null)
            {
                code = IdenticonUtil.Code(HttpContext.Request.UserHostAddress);
            }

            string etag = IdenticonUtil.ETag(code.Value, Size);

            if(HttpContext != null && HttpContext.Request != null && HttpContext.Request.Headers != null && HttpContext.Request.Headers["If-None-Match"] == etag)
            {
                // browser already has the image cached
                return new NotModifiedResult();
            }
            
            return new IdenticonResult(code.Value, Size, etag);
        }
    }
}
