using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Book_tracker.Controllers
{
    public class AuthorizationTestController : Controller
    {
        [Authorize(Policy = "ActiveUser")]
        public IActionResult UserOnly()
        {
            return Content("You are an active user.");
        }

        [Authorize(Roles = "Admin")]
        public IActionResult AdminOnly()
        {
            return Content("You are an Admin.");
        }
    }
}