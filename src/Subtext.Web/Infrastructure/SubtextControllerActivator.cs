using System;
using System.Web.Mvc;
using System.Web.Routing;

namespace Subtext.Web.Infrastructure
{
    public class SubtextControllerActivator : IControllerActivator
    {
        IDependencyResolver _resolver;

        public SubtextControllerActivator(IDependencyResolver resolver)
        {
            _resolver = resolver;
        }

        public IController Create(RequestContext requestContext, Type controllerType)
        {
            IController controller = (_resolver.GetService(controllerType) ?? Activator.CreateInstance(controllerType)) as IController;
            var fullController = controller as Controller;
            if (fullController != null)
            {
                fullController.TempDataProvider = new EmptyTempDataProvider();
            }

            return controller;

        }
    }
}