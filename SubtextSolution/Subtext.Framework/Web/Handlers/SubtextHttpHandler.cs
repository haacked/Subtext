using System.Web;
using Subtext.Framework.Providers;
using Subtext.Framework.Routing;

namespace Subtext.Framework.Web.Handlers
{
    public abstract class SubtextHttpHandler : ISubtextHandler {
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


        public ISubtextContext SubtextContext {
            get;
            set;
        }

        public bool IsReusable {
            get {
                return false;
            }
        }

        public void ProcessRequest(HttpContext context) {
            ProcessRequest();
        }

        public abstract void ProcessRequest();
    }
}
