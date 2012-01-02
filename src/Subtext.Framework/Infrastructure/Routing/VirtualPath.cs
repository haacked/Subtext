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

namespace Subtext.Framework.Routing
{
    public class VirtualPath
    {
        readonly Uri _virtualPath;

        public VirtualPath(string virtualPath)
        {
            if (virtualPath == null)
            {
                throw new ArgumentNullException("virtualPath");
            }
            virtualPath = virtualPath.Replace("%7B:#:%7D", "%23");
            _virtualPath = new Uri(virtualPath, UriKind.Relative);
        }

        public static implicit operator String(VirtualPath vp)
        {
            if (vp == null)
            {
                return null;
            }
            return vp.ToString();
        }

        public static implicit operator VirtualPath(string virtualPath)
        {
            if (String.IsNullOrEmpty(virtualPath))
            {
                return null;
            }
            return new VirtualPath(virtualPath);
        }

        public override string ToString()
        {
            return _virtualPath.ToString();
        }

        public Uri ToUri()
        {
            return _virtualPath;
        }
    }
}