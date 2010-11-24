using Infrastructure.Constants;
using Infrastructure.Extensions;

namespace System.Web.Mvc
{
    public static class LinkExtensions
    {
        public static MvcHtmlString ActionTab(this HtmlHelper self, string url, string text,
            bool root = false)
        {
            var current = HttpContext.Current.Request.Url;
            var tag = new TagBuilder("a") {
                InnerHtml = text
            };
            tag.Attributes.Add("href", url);

            if (root) {
                if (current.AbsolutePath.EqualsIgnoreCase(url)) {
                    tag.AddCssClass(Css.CURRENT_CLASS);
                }
            } else if (current.AbsolutePath.StartsWith(url)) {
                tag.AddCssClass(Css.CURRENT_CLASS);
            }

            return MvcHtmlString.Create(tag.ToString());
        }

        //public static MvcHtmlString ActionTab(this HtmlHelper self, string text, string action,
        //    string controller, string area, bool controllerOnly = false, bool areaOnly = false)
        //{
        //    var route = self.ViewContext.RouteData;
        //    var current = new {
        //        Area = route.DataTokens["area"] ?? "",
        //        Controller = route.GetRequiredString("controller"),
        //        Action = route.GetRequiredString("action")
        //    };

        //    var attributes = new object();

        //    if (areaOnly && area.EqualsIgnoreCase(current.Area.ToString())) {
        //        attributes = new { @class = Css.CURRENT_CLASS };
        //    }

        //    if (current.Controller.EqualsIgnoreCase(controller)) {
        //        if (controllerOnly || current.Action.EqualsIgnoreCase(action)) {
        //            attributes = new { @class = Css.CURRENT_CLASS };
        //        }
        //    }

        //    return self.ActionLink(text, action, controller,
        //        new RouteValueDictionary(new { area }), attributes);
        //}
    }
}
