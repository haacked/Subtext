using System.Web;
using System.Web.Routing;
using Ninject;
using Ninject.Modules;

namespace Subtext.Infrastructure
{
    public static class Bootstrapper
    {
        public static IKernel Kernel { get; set; }

        public static RequestContext RequestContext
        {
            get
            {
                if(HttpContext.Current != null && HttpContext.Current.Items != null)
                {
                    return HttpContext.Current.Items["__Subtext_RequestContext"] as RequestContext;
                }
                return null;
            }
            set
            {
                if(HttpContext.Current != null && HttpContext.Current.Items != null)
                {
                    HttpContext.Current.Items["__Subtext_RequestContext"] = value;
                }
            }
        }

        public static void InitializeKernel(params INinjectModule[] modules)
        {
            Kernel = new StandardKernel(modules);
        }
    }
}