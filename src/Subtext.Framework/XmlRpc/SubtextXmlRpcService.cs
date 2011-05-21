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

using System.Web.Routing;
using CookComputing.XmlRpc;
using Subtext.Framework.Providers;
using Subtext.Framework.Routing;

namespace Subtext.Framework.XmlRpc
{
    public abstract class SubtextXmlRpcService : XmlRpcService
    {
        protected SubtextXmlRpcService(ISubtextContext context)
        {
            SubtextContext = context;
        }

        protected ISubtextContext SubtextContext { get; private set; }

        protected Blog Blog
        {
            get { return SubtextContext.Blog; }
        }

        protected BlogUrlHelper Url
        {
            get { return SubtextContext.UrlHelper; }
        }

        protected ObjectProvider Repository
        {
            get { return SubtextContext.Repository; }
        }

        protected RouteValueDictionary RouteValues
        {
            get { return SubtextContext.RequestContext.RouteData.Values; }
        }
    }
}