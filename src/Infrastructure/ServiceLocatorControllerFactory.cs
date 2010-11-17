using System;
using System.Web.Mvc;
using System.Web.Routing;

namespace Infrastructure
{
    public sealed class ServiceLocatorControllerFactory : DefaultControllerFactory
    {
        private readonly IServiceLocator _locator;

        public ServiceLocatorControllerFactory(IServiceLocator locator)
        {
            _locator = locator;
        }

        public override IController CreateController(RequestContext requestContext, string controllerName)
        {
            var key = controllerName.ToLowerInvariant();
            IController controller;

            try {
                controller = _locator.Get<IController>(key);
            } catch (InvalidOperationException) {
                controller = base.CreateController(requestContext, controllerName);
            }

            return controller;
        }
    }
}
