using System;
using System.Web.Mvc;

namespace Infrastructure.ActionFilters
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public sealed class LayoutAttribute : ActionFilterAttribute
    {
        private readonly string _path;

        public LayoutAttribute(string path)
        {
            _path = path;
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (filterContext.Result is ViewResult) {
                ((ViewResult)filterContext.Result).MasterName = _path;
            }

            base.OnActionExecuted(filterContext);
        }
    }
}
