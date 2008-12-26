using System;

namespace Subtext.Framework.Routing
{
    public interface ISubtextPageBuilder
    {
        object CreateInstanceFromVirtualPath(string virtualPath, Type type);
    }
}
