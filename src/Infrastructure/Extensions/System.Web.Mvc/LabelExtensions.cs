using System.Web.Routing;

namespace System.Web.Mvc.Html
{
    public static class LabelExtensions
    {
        public static MvcHtmlString LabelForModel(this HtmlHelper self, object htmlAttributes)
        {
            var metadata = self.ViewData.ModelMetadata;
            var text = metadata.DisplayName ?? metadata.PropertyName;

            var tag = new TagBuilder("label");
            tag.Attributes.Add("for", self.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldId(""));
            tag.MergeAttributes(new RouteValueDictionary(htmlAttributes));
            tag.SetInnerText(text);

            return MvcHtmlString.Create(tag.ToString());
        }
    }
}
