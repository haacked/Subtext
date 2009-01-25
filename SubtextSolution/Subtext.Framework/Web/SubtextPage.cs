using System.Web.Routing;
using System.Web.UI;
using Subtext.Framework.Routing;

namespace Subtext.Framework.Web
{
    public abstract class SubtextPage : Page, ISubtextHandler {
        public ISubtextContext SubtextContext {
            get;
            set;
        }
        
        public RequestContext RequestContext {
            get {
                return SubtextContext.RequestContext;
            }
            set {
               //do nothing. 
            }
        }

        public UrlHelper Url
        {
            get {
                return SubtextContext.UrlHelper;
            }
        }
    }
}
