using Subtext.Framework.Providers;
using Subtext.Framework.Routing;

namespace Subtext.Framework.Web.Handlers
{
    public class SubtextDependencies : ISubtextDependencies
    {
        AdminUrlHelper _adminUrlHelper;

        public SubtextDependencies(ISubtextContext subtextContext)
        {
            SubtextContext = subtextContext;
        }

        public Blog Blog
        {
            get { return SubtextContext.Blog; }
        }

        public BlogUrlHelper Url
        {
            get { return SubtextContext.UrlHelper; }
        }

        public ObjectProvider Repository
        {
            get { return SubtextContext.Repository; }
        }

        public AdminUrlHelper AdminUrl
        {
            get
            {
                if (_adminUrlHelper == null)
                {
                    _adminUrlHelper = new AdminUrlHelper(Url);
                }
                return _adminUrlHelper;
            }
        }

        public ISubtextContext SubtextContext { get; protected set; }
    }
}
