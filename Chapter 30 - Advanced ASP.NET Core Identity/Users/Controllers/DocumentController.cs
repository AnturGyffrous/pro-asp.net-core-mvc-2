using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Users.Models;

namespace Users.Controllers
{
    public class DocumentController : Controller
    {
        private readonly IAuthorizationService _authService;

        private readonly ProtectedDocument[] _docs =
        {
            new ProtectedDocument {Title = "Q3 Budget", Author = "Alice", Editor = "Joe"},
            new ProtectedDocument {Title = "Project Plan", Author = "Bob", Editor = "Alice"}
        };

        public DocumentController(IAuthorizationService authService)
        {
            _authService = authService;
        }

        public ViewResult Index() => View(_docs);

        public async Task<IActionResult> Edit(string title)
        {
            var doc = _docs.FirstOrDefault(d => d.Title == title);
            var authorized = await _authService.AuthorizeAsync(User, doc, "AuthorsAndEditors");
            if (authorized.Succeeded)
            {
                return View("Index", doc);
            }

            return new ChallengeResult();
        }
    }
}