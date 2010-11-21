using System.Web.Mvc;

namespace Infrastructure
{
    public sealed class ResfultRoutingRazorViewEngine : RazorViewEngine
    {
        public ResfultRoutingRazorViewEngine()
        {
            AreaMasterLocationFormats = new[] {
                "~/Views/{2}/{1}/{0}.cshtml",
                "~/Views/{2}/Shared/{0}.cshtml",
            };

            AreaViewLocationFormats = new[] {
                "~/Views/{2}/{1}/{0}.cshtml",
                "~/Views/{2}/{1}/{0}.cshtml",
                "~/Views/{2}/Shared/{0}.cshtml",
                "~/Views/{2}/Shared/{0}.cshtml",
            };

            AreaPartialViewLocationFormats = AreaViewLocationFormats;
        }
    }
}
