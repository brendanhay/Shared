using System;
using System.Web.Mvc;
using System.Web.Routing;
using Infrastructure.Domain;

namespace Infrastructure.Data
{
    public class AggregateModelBinder<T> : DefaultModelBinder, ILocatableModelBinder
        where T : class, IAggregate, new()
    {
        public virtual Type Type { get { return typeof(T); } }

        protected override object CreateModel(ControllerContext controllerContext,
            ModelBindingContext bindingContext, Type modelType)
        {
            var controller = controllerContext.Controller as UnitOfWorkController;

            if (controller == null) {
                throw new NotSupportedException("Only Controllers derived from UnitOfWorkController are supported.");
            }

            var id = GetId(controller.RouteData);

            return controller.UnitOfWork.Repository<T>().Get(id);
        }

        protected virtual object GetId(RouteData routeData)
        {
            return ConvertId(routeData.GetRequiredString("id"));
        }

        protected virtual object ConvertId(string id)
        {
            return Convert.ToInt32(id);
        }
    }
}
