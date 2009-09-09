using System.Web;
using Subtext.Framework.Providers;
using Subtext.Framework.Routing;

namespace Subtext.Framework.Web.Handlers
{
    public abstract class SubtextHttpHandler : ISubtextHandler
    {
        AdminUrlHelper _adminUrlHelper;

        public SubtextHttpHandler(ISubtextContext subtextContext)
        {
            SubtextContext = subtextContext;
        }

        public Blog Blog
        {
            get { return SubtextContext.Blog; }
        }

        #region ISubtextHandler Members

        public UrlHelper Url
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
                if(_adminUrlHelper == null)
                {
                    _adminUrlHelper = new AdminUrlHelper(Url);
                }
                return _adminUrlHelper;
            }
        }

        public ISubtextContext SubtextContext { get; protected set; }

        public bool IsReusable
        {
            get { return false; }
        }

        public void ProcessRequest(HttpContext context)
        {
            ProcessRequest();
        }

        #endregion

        public abstract void ProcessRequest();
    }
}