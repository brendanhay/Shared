using System;
using System.Web.Mvc;

namespace Infrastructure.ActionFilters
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public sealed class CompressAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
#if !DEBUG
            var request = filterContext.HttpContext.Request;
            var acceptEncoding = request.Headers["Accept-Encoding"];

            if (string.IsNullOrEmpty(acceptEncoding)) {
                return;
            }

            acceptEncoding = acceptEncoding.ToUpperInvariant();

            var response = filterContext.HttpContext.Response;

            if (acceptEncoding.Contains("GZIP")) {
                response.AppendHeader("Content-encoding", "gzip");
                response.Filter = new GZipStream(response.Filter, CompressionMode.Compress);
            } else if (acceptEncoding.Contains("DEFLATE")) {
                response.AppendHeader("Content-encoding", "deflate");
                response.Filter = new DeflateStream(response.Filter, CompressionMode.Compress);
            }
#endif
        }
    }
}
