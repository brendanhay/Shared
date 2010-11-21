using System;
using System.Web.Mvc;
using Infrastructure.Domain;

namespace Infrastructure.Data
{
    public class AggregateModelBinder<T> : DefaultModelBinder, ILocatableModelBinder
        where T : class, IAggregate, new()
    {
        public Type Type { get { return typeof(T); } }

        protected override object CreateModel(ControllerContext controllerContext,
            ModelBindingContext bindingContext, Type modelType)
        {
            var controller = controllerContext.Controller as UnitOfWorkController;

            if (controller == null) {
                throw new NotSupportedException("Only Controllers derived from UnitOfWorkController are supported.");
            }

            var id = Convert.ToInt32(controller.RouteData.GetRequiredString("id"));

            return id > 0
                ? controller.UnitOfWork.Repository<T>().Get(id)
                : new T();
        }
    }
}
