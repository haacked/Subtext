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
using System.Web.Compilation;
using Ninject;

namespace Subtext.Framework.Routing
{
    public class SubtextPageBuilder : ISubtextPageBuilder
    {
        public SubtextPageBuilder(IKernel kernel)
        {
            Kernel = kernel;
        }

        public IKernel Kernel { get; private set; }

        #region ISubtextPageBuilder Members

        public object CreateInstanceFromVirtualPath(string virtualPath, Type type)
        {
            object instance = BuildManager.CreateInstanceFromVirtualPath(virtualPath, type);
            Kernel.Inject(instance);
            return instance;
        }

        #endregion
    }
}