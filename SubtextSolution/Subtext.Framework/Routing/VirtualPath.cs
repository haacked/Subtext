using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Subtext.Framework.Text;

namespace Subtext.Framework.Routing
{
    public class VirtualPath
    {
        public VirtualPath(string virtualPath) {
            if (virtualPath == null) {
                throw new ArgumentNullException("virtualPath");
            }

            _virtualPath = new Uri(virtualPath, UriKind.Relative);
        }

        Uri _virtualPath;

        public static implicit operator String(VirtualPath vp) {
            if (vp == null)
                return null;
            return vp.ToString();
        }

        public static implicit operator VirtualPath(string virtualPath)
        {
            if (String.IsNullOrEmpty(virtualPath)) {
                return null;
            }
            return new VirtualPath(virtualPath);
        }

        public override string ToString()
        {
            return _virtualPath.ToString();
        }

        public Uri ToUri() {
            return _virtualPath;
        }

        public virtual Uri ToFullyQualifiedUrl(Blog blog) {
            UriBuilder builder = new UriBuilder();
            builder.Scheme = "http";
            builder.Host = blog.Host;
            return new Uri(builder.Uri, _virtualPath);
        }
    }
}
