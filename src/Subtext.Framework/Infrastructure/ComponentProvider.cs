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
using System.Web.Routing;
using Ninject;
using Ninject.Modules;

namespace Subtext.Infrastructure
{
    public static class Bootstrapper
    {
        public static IServiceLocator ServiceLocator { get; set; }

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
            var kernel = new StandardKernel(modules);
            ServiceLocator = kernel.Get<IServiceLocator>();
        }
    }
}