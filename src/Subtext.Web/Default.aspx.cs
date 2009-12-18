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
using System.Web.Routing;
using System.Web.UI;
using Subtext.Framework;
using Subtext.Framework.Pipeline;
using Subtext.Framework.Routing;
using Subtext.Infrastructure;

namespace Subtext.Web
{
    public class _Default : Page
    {
        public void Page_Load(object sender, EventArgs e)
        {
            // Workaround for Cassini issue with request to /, IIS 6 and IIS 7 Classic mode.
            // In IIS7 Integrated mode, Default.aspx can be deleted.
            
            var serviceLocator = Bootstrapper.ServiceLocator;
            var pipelineService = new PipelineService(new HttpContextWrapper(HttpContext.Current), serviceLocator);
            
            var route = new RootRoute(HostInfo.Instance.BlogAggregationEnabled, serviceLocator);
            IRouteHandler routeHandler =
                new PageRouteHandler(
                    HostInfo.Instance.BlogAggregationEnabled ? "~/pages/AggDefault.aspx" : "~/pages/Dtp.aspx",
                    serviceLocator.GetService<ISubtextPageBuilder>(), serviceLocator);

            pipelineService.ProcessRootRequest(route, routeHandler);
        }
    }
}