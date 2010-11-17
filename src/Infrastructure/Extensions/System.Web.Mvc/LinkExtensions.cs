using System.Linq;
using System.Web.Mvc.Html;
using Infrastructure.Constants;

namespace System.Web.Mvc
{
    public static class LinkExtensions
    {
        public static MvcHtmlString ActionTab(this HtmlHelper self, string text, string action,
            string controller, object routeValues = null, bool matchOnControllerOnly = false)
        {
            return self.ActionTab(text, new[] { action }, controller, routeValues,
                matchOnControllerOnly);
        }

        public static MvcHtmlString ActionTab(this HtmlHelper self, string text, string[] actions,
            string controller, object routeValues = null, bool matchOnControllerOnly = false)
        {
            var route = self.ViewContext.RequestContext.RouteData;
            var current = new {
                Controller = route.GetRequiredString("controller"),
                Action = route.GetRequiredString("action")
            };

            var attributes = new object();

            if (current.Controller.Equals(controller, StringComparison.OrdinalIgnoreCase)) {
                if (matchOnControllerOnly || AnyActionsMatch(current.Action, actions)) {
                    attributes = new { @class = Css.CURRENT_CLASS };
                }
            }

            return self.ActionLink(text, actions[0], controller, routeValues, attributes);
        }

        private static bool AnyActionsMatch(string currentAction, string[] actions)
        {
            return actions.Any(action => action.Equals(currentAction, 
                StringComparison.OrdinalIgnoreCase));
        }
    }
}
