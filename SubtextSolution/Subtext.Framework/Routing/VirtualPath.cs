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

            _virtualPath = virtualPath.LeftBefore("#", StringComparison.Ordinal);
            if (virtualPath.Contains("#")) {
                Fragment = virtualPath.RightAfter("#", StringComparison.Ordinal);
            }
        }

        string _virtualPath;

        public string Fragment {
            get;
            private set;
        }

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
            return _virtualPath + (String.IsNullOrEmpty(Fragment) ? "" : "#" + Fragment);
        }

        public virtual Uri ToFullyQualifiedUrl(BlogInfo blog) {
            UriBuilder builder = new UriBuilder("http", blog.Host);
            builder.Path = _virtualPath;
            builder.Fragment = Fragment;
            return builder.Uri;
        }
    }
}
