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
using System.Net;
using Ninject;
using Subtext.Framework;
using Subtext.Framework.Routing;

namespace Subtext.Providers.BlogEntryEditor.FCKeditor
{
    public class Uploader : FredCK.FCKeditorV2.Uploader, ISubtextHandler
    {
        protected override void OnInit(EventArgs e)
        {
            if (!SubtextContext.User.IsInRole("Admins"))
            {
                SubtextContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                SubtextContext.HttpContext.Response.End();
            }
            base.OnInit(e);
            base.OnInit(e);
        }

        [Inject]
        public ISubtextContext SubtextContext
        {
            get;
            set;
        }

        public UrlHelper Url
        {
            get
            {
                return SubtextContext.UrlHelper;
            }
        }

        public Subtext.Framework.Providers.ObjectProvider Repository
        {
            get
            {
                return SubtextContext.Repository;
            }
        }

        public AdminUrlHelper AdminUrl
        {
            get
            {
                return new AdminUrlHelper(Url);
            }
        }
    }
}
