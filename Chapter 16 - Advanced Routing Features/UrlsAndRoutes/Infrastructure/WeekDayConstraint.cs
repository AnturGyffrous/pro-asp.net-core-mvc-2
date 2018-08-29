using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace UrlsAndRoutes.Infrastructure
{
    public class WeekDayConstraint : IRouteConstraint
    {
        private static readonly List<string> Days = new List<string>(new[] {"mon", "tue", "wed", "thu", "fri", "sat", "sun"});

        public bool Match(HttpContext httpContext, IRouter route, string routeKey, RouteValueDictionary values,
            RouteDirection routeDirection)
        {
            return Days.Contains(values[routeKey]?.ToString().ToLowerInvariant());
        }
    }
}