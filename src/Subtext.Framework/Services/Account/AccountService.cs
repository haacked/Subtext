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
using System.Web;
using System.Web.Security;
using log4net;
using Subtext.Framework.Logging;
using Subtext.Framework.Security;

namespace Subtext.Framework.Services.Account
{
    public class AccountService : IAccountService
    {
        private readonly static ILog Log = new Log();

        public void Logout(ISubtextContext context)
        {
            var request = context.HttpContext.Request;
            var response = context.HttpContext.Response;
            var authCookie = new HttpCookie(request.GetFullCookieName(context.Blog)) { Expires = DateTime.Now.AddYears(-30) };
            response.Cookies.Add(authCookie);

            if(Log.IsDebugEnabled)
            {
                string username = context.HttpContext.User.Identity.Name;
                if(Log.IsDebugEnabled)
                {
                    Log.Debug("Logging out " + username);
                    Log.Debug("the code MUST call a redirect after this");
                }
            }

            FormsAuthentication.SignOut();
        }
    }
}
