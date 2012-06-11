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
using Subtext.Framework.Routing;
using Subtext.Framework.Security;

namespace Subtext.Web.Controllers
{
    public class AccountController : Controller
    {
        public AccountController(BlogUrlHelper urlHelper, IAccountService service)
        {
            BlogUrlHelper = urlHelper;
            AccountService = service;
        }

        protected BlogUrlHelper BlogUrlHelper
        {
            get;
            set;
        }

        protected IAccountService AccountService
        {
            get;
            set;
        }

        public ActionResult Logout()
        {
            AccountService.Logout();
            return Redirect(BlogUrlHelper.BlogUrl());
        }
    }
}
