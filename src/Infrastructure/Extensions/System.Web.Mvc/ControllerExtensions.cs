using System.IO;
using System.Text.RegularExpressions;

namespace System.Web.Mvc
{
    public static class ControllerExtensions
    {
        private static readonly Regex _stripper = new Regex(@"(?<extraneous>[\r\n\t])|(?<brackets>\>\s+)",
            RegexOptions.Multiline | RegexOptions.Compiled | RegexOptions.CultureInvariant);

        public static string RenderPartial(this ControllerBase self, string viewName, object model,
            bool strip = true)
        {
            var context = self.ControllerContext;

            if (string.IsNullOrEmpty(viewName)) {
                viewName = context.RouteData.GetRequiredString("action");
            }

            self.ViewData.Model = model;

            //try {
            using (var writer = new StringWriter()) {
                var viewResult = ViewEngines.Engines.FindPartialView(context, viewName);
                var viewContext = new ViewContext(context, viewResult.View, self.ViewData,
                    self.TempData, writer);
                viewResult.View.Render(viewContext, writer);

                var result = writer.GetStringBuilder().ToString();
                return strip ? _stripper.Replace(result, StripInvalidChars) : result;
            }
            //} catch (ArgumentNullException ex) { }

            //return "";
        }

        private static string StripInvalidChars(Match match)
        {
            if (match.Groups["brackets"].Success) {
                return ">";
            }

            return match.Groups["extraneous"].Success ? "" : match.Value;
        }
    }
}
