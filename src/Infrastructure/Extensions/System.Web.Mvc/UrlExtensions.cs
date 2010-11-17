using System.Web.Routing;
using Infrastructure.Constants;

namespace System.Web.Mvc
{
    public static class UrlExtensions
    {
        public static MvcHtmlString Javascript(this UrlHelper self, string name, object htmlAttributes = null)
        {
            var src = GetContentUrl(self, name, FileExtensions.JAVASCRIPT,
                DirectoryPaths.JAVASCRIPTS);

            var tag = new TagBuilder("script") {
                Attributes = { 
                    { "type", "text/javascript" },
                    { "src", src }
                }
            };

            var attributes = new RouteValueDictionary(htmlAttributes);

            foreach (var attribute in attributes) {
                tag.Attributes.Add(attribute.Key.Replace('_', '-'), attribute.Value.ToString());
            }

            return MvcHtmlString.Create(tag.ToString(TagRenderMode.Normal));
        }

        public static MvcHtmlString Stylesheet(this UrlHelper self, string name, string media = "all")
        {
            var href = GetContentUrl(self, name, FileExtensions.STYLESHEET,
                DirectoryPaths.STYLESHEETS);

            var tag = new TagBuilder("link") {
                Attributes = { 
                    { "rel", "stylesheet" },
                    { "type", "text/css" },
                    { "media", media },
                    { "href", href }
                }
            };

            return MvcHtmlString.Create(tag.ToString(TagRenderMode.SelfClosing));
        }

        private static string GetContentUrl(UrlHelper helper, string name, string extension,
            string directory)
        {
            name = name.EndsWith(extension)
                ? name
                : name + extension;

            return helper.Content(string.Format("~/Content/{0}/{1}", directory, name));
        }
    }
}
