using System.Web.Mvc;

namespace Infrastructure.Data
{
    public abstract class ModelBindingController : Controller
    {
        protected ModelBindingController(params ILocatableModelBinder[] binders)
        {
            Binders.Clear();

            foreach (var binder in binders) {
                Binders.Add(binder.Type, binder);
            }
        }
    }
}
