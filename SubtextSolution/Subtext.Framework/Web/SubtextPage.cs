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
using System.Web;
using System.Web.Routing;
using System.Web.UI;
using Ninject;
using Subtext.Framework.Providers;
using Subtext.Framework.Routing;
using Subtext.Infrastructure;

namespace Subtext.Framework.Web.Handlers
{
    public class SubtextPage : Page, ISubtextHandler {
        protected override void OnInit(EventArgs e) {
            //TODO: Remove the necessity for this code.
            if (SubtextContext == null) {
                Bootstrapper.RequestContext = new RequestContext(new HttpContextWrapper(HttpContext.Current), new RouteData());
                Bootstrapper.Kernel.Inject(this);
            }
            base.OnInit(e);
        }

        [Inject]
        public ISubtextContext SubtextContext {
            get;
            set;
        }

        public Blog Blog {
            get {
                return SubtextContext.Blog;
            }
        }

        public UrlHelper Url {
            get {
                return SubtextContext.UrlHelper;
            }
        }

        public ObjectProvider Repository {
            get {
                return SubtextContext.Repository;
            }
        }

        public AdminUrlHelper AdminUrl {
            get {
                if (_adminUrlHelper == null) {
                    _adminUrlHelper = new AdminUrlHelper(Url);
                }
                return _adminUrlHelper;
            }
        }
        AdminUrlHelper _adminUrlHelper;
    }
}