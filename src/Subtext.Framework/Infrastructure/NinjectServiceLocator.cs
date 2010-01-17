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

using System;
using System.Web;
using Ninject;
using Ninject.Activation.Caching;
using Subtext.Infrastructure;

namespace Subtext.Framework.Infrastructure
{
    public class NinjectServiceLocator : IServiceLocator
    {
        public NinjectServiceLocator(IKernel kernel)
        {
            Kernel = kernel;
        }

        public IKernel Kernel
        {
            get; 
            private set;
        }

        public TService GetService<TService>()
        {
            return Kernel.Get<TService>();
        }

        public object GetService(Type type)
        {
            return Kernel.Get(type);
        }

        public void DisposeRequestScoped(HttpContext httpContext)
        {
            var cache = Kernel.Components.Get<Ninject.Activation.Caching.ICache>() as Cache;
            if(cache != null)
            {
                cache.DisposeRequestScoped(httpContext);
            }
        }
    }
}
