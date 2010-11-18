using System.Web.Mvc.Html;
using System.Web.Routing;
using Infrastructure.Constants;
using Infrastructure.Extensions;

namespace System.Web.Mvc
{
    public static class LinkExtensions
    {
        public static MvcHtmlString ActionTab(this HtmlHelper self, string text, string action,
            string controller, object routeValues = null, bool controllerOnly = false)
        {
            return self.ActionTab(text, action, controller, routeValues, controllerOnly, false);
        }

        public static MvcHtmlString ActionTab(this HtmlHelper self, string text, string action,
            string controller, object routeValues = null, bool controllerOnly = false, bool areaOnly = false)
        {
            var route = self.ViewContext.RequestContext.RouteData;
            var current = new {
                Area = route.DataTokens["area"],
                Controller = route.GetRequiredString("controller"),
                Action = route.GetRequiredString("action")
            };

            var values = new RouteValueDictionary(routeValues);
            var attributes = new object();

            if (areaOnly && current.Area != null && current.Area.Equals(values["area"])) {
                attributes = new { @class = Css.CURRENT_CLASS };
            } else if (current.Controller.EqualsIgnoreCase(controller)) {
                if (controllerOnly || current.Action.EqualsIgnoreCase(action)) {
                    attributes = new { @class = Css.CURRENT_CLASS };
                }
            }

            return self.ActionLink(text, action, controller, routeValues, attributes);
        }
    }
}
