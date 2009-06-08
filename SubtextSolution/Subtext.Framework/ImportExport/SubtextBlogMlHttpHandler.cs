using Ninject;
using Subtext.BlogML;
using Subtext.BlogML.Interfaces;
using Subtext.Framework.Configuration;
using Subtext.Framework.Providers;
using Subtext.Framework.Routing;
using Subtext.Framework.Services;
using Subtext.ImportExport;

namespace Subtext.Framework.ImportExport
{
    //TODO: Fix this. We need to pull BlogMLHttpHandler into Subtext.Framework
    public class SubtextBlogMlHttpHandler : BlogMLHttpHandler, ISubtextHandler {
        public override IBlogMLProvider GetBlogMLProvider() {
            var handler = new SubtextBlogMLProvider(Config.ConnectionString, SubtextContext, new CommentService(SubtextContext, null));
            handler.PageSize = 100;
            return handler;
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

        public AdminUrlHelper AdminUrl
        {
            get {
                if (_adminUrlHelper == null) {
                    _adminUrlHelper = new AdminUrlHelper(Url);
                }
                return _adminUrlHelper;
            }
        }
        AdminUrlHelper _adminUrlHelper;

        [Inject]
        public ISubtextContext SubtextContext {
            get;
            set;
        }
    }
}
