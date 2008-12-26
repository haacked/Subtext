using System;
using System.Web.Compilation;

namespace Subtext.Framework.Routing
{
    public class SubtextPageBuilder : ISubtextPageBuilder
    {
        public object CreateInstanceFromVirtualPath(string virtualPath, Type type)
        {
            return BuildManager.CreateInstanceFromVirtualPath(virtualPath, type);
        }
    }
}
