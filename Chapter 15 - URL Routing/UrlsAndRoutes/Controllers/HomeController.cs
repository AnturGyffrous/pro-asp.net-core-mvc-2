using Microsoft.AspNetCore.Mvc;
using UrlsAndRoutes.Models;

namespace UrlsAndRoutes.Controllers
{
    public class HomeController : Controller
    {
        public ViewResult Index() =>
            View("Result", new Result {Controller = nameof(HomeController), Action = nameof(Index)});

        public ViewResult CustomVariable(string id)
        {
            var r = new Result
            {
                Controller = nameof(HomeController),
                Action = nameof(CustomVariable)
            };
            r.Data["Id"] = id ?? "<no value>";
            r.Data["catchall"] = RouteData.Values["catchall"];
            return View("Result", r);
        }
    }
}