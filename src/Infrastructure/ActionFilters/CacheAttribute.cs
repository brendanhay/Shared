using System;
using System.Web;
using System.Web.Mvc;

namespace Infrastructure.ActionFilters
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public sealed class CacheAttribute : ActionFilterAttribute
    {
        public CacheAttribute()
        {
            Duration = 10;
        }

        public int Duration { get; set; }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (Duration <= 0) {
                return;
            }

            var cache = filterContext.HttpContext.Response.Cache;

#if DEBUG
            cache.SetLastModified(DateTime.Now);
            cache.SetNoStore();
            cache.SetCacheability(HttpCacheability.NoCache);
#else
            var cacheDuration = TimeSpan.FromSeconds(Duration);

            cache.SetCacheability(HttpCacheability.Public);
            cache.SetExpires(DateTime.Now.Add(cacheDuration));
            cache.SetMaxAge(cacheDuration);
            cache.AppendCacheExtension("must-revalidate, proxy-revalidate");
#endif
        }
    }
}
