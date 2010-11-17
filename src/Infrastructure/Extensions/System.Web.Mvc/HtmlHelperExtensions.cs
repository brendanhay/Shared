using Infrastructure.Constants;
using System.Linq;
using Infrastructure.Extensions;

namespace System.Web.Mvc
{
    public static class CssExtensions
    {
        public static string WriteIfControllerMatches(this HtmlHelper self, params string[] controllers)
        {
            return WriteIfControllerMatches(self,
                (ctrlr, action) => controllers.Any(str => str.EqualsIgnoreCase(ctrlr)));
        }

        public static string WriteIfControllerMatches(this HtmlHelper self,
            Func<string, string, bool> predicate)
        {
            return WriteIfControllerMatches(self, predicate, Css.CURRENT_CLASS);
        }

        public static string WriteIfControllerMatches(this HtmlHelper self,
            Func<string, string, bool> predicate, string @string)
        {
            var route = self.ViewContext.RequestContext.RouteData;
            var current = new {
                Controller = route.GetRequiredString("controller"),
                Action = route.GetRequiredString("action")
            };

            return predicate(current.Controller, current.Action)
                ? @string
                : "";
        }
    }
}
