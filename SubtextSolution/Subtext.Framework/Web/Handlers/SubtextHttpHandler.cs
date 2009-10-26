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

using System.Web;
using Subtext.Framework.Providers;
using Subtext.Framework.Routing;

namespace Subtext.Framework.Web.Handlers
{
    public abstract class SubtextHttpHandler : ISubtextHandler
    {
        AdminUrlHelper _adminUrlHelper;

        protected SubtextHttpHandler(ISubtextContext subtextContext)
        {
            SubtextContext = subtextContext;
        }

        public Blog Blog
        {
            get { return SubtextContext.Blog; }
        }

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

        public abstract void ProcessRequest();
    }
}