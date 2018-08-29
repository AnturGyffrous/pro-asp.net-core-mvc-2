using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Internal;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace UrlsAndRoutes.Infrastructure
{
    public class LegacyRoute : IRouter
    {
        private readonly List<string> _urls;

        private readonly IRouter _mvcRoute;

        public LegacyRoute(IServiceProvider services, params string[] targetUrls)
        {
            _urls = new List<string>(targetUrls);
            _mvcRoute = services.GetRequiredService<MvcRouteHandler>();
        }

        public async Task RouteAsync(RouteContext context)
        {
            var requestedUrl = context.HttpContext.Request.Path.Value.TrimEnd('/');
            if (_urls.Contains(requestedUrl, StringComparer.OrdinalIgnoreCase))
            {
                //context.Handler = async ctx => {
                //    HttpResponse response = ctx.Response;
                //    byte[] bytes = Encoding.ASCII.GetBytes($"URL: {requestedUrl}");
                //    await response.Body.WriteAsync(bytes, 0, bytes.Length);
                //};
                context.RouteData.Values["controller"] = "Legacy";
                context.RouteData.Values["action"] = "GetLegacyUrl";
                context.RouteData.Values["legacyUrl"] = requestedUrl;
                await _mvcRoute.RouteAsync(context);
            }
            //return Task.CompletedTask;
        }

        public VirtualPathData GetVirtualPath(VirtualPathContext context)
        {
            if (context.Values.ContainsKey("legacyUrl"))
            {
                var url = context.Values["legacyUrl"] as string;
                if (_urls.Contains(url))
                {
                    return new VirtualPathData(this, url);
                }
            }

            return null;
        }
    }
}