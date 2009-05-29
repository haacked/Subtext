using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Ninject;
using Ninject.Modules;
using Subtext.Infrastructure;

namespace Subtext.Web.Infrastructure
{
    public class SubtextControllerFactory : DefaultControllerFactory
    {
        public SubtextControllerFactory(IKernel kernel) {
            Kernel = kernel;
        }

        protected IKernel Kernel {
            get;
            private set;
        }

        public override IController CreateController(RequestContext requestContext, string controllerName)
        {
            Bootstrapper.RequestContext = requestContext;

            IController instance = null;
            Type controllerType = base.GetControllerType(controllerName);
            if (controllerType != null) {
                instance = Kernel.Get(controllerType) as IController;
                var controller = instance as Controller;
                if (controller != null)
                {
                    controller.TempDataProvider = new EmptyTempDataProvider();
                }
            }
            
            return instance;
        }

    
    }
}
