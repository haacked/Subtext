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
using System.Web.Mvc;
using System.Web.Routing;
using Subtext.Infrastructure;

namespace Subtext.Web.Infrastructure
{
    public class SubtextControllerFactory : IControllerFactory
    {
        public SubtextControllerFactory(IServiceLocator serviceLocator)
        {
            ServiceLocator = serviceLocator;
        }

        protected IServiceLocator ServiceLocator { get; private set; }

        public IController CreateController(RequestContext requestContext, string controllerName)
        {
            Bootstrapper.RequestContext = requestContext;

            IController instance = null;
            Type controllerType = Type.GetType(string.Format("Subtext.Web.Controllers.{0}Controller", controllerName), false /*ignoreOnError*/, true /*ignoreCase*/);
            if(controllerType != null)
            {
                instance = ServiceLocator.GetService(controllerType) as IController;
                var controller = instance as Controller;
                if(controller != null)
                {
                    controller.TempDataProvider = new EmptyTempDataProvider();
                }
            }

            return instance;
        }

        public void ReleaseController(IController controller)
        {
            var disposable = controller as IDisposable;
            if(disposable != null)
            {
                disposable.Dispose();
            }
        }
    }
}