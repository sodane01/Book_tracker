using Book_tracker.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Book_tracker.Controllers
{
    public class BookController : Controller
    {
        private readonly IGoogleBooksService _googleBooksService;
        private readonly IReviewService _reviewService;

        public BookController(
            IGoogleBooksService googleBooksService,
            IReviewService reviewService)
        {
            _googleBooksService = googleBooksService;
            _reviewService = reviewService;
        }

        [HttpGet]
        public async Task<IActionResult> Details(string? id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return NotFound();
            }

            var book =
                await _googleBooksService.GetBookByIdAsync(id);

            if (book == null)
            {
                return NotFound();
            }

            var currentUserId =
                User.Identity?.IsAuthenticated == true
                    ? User.FindFirstValue(ClaimTypes.NameIdentifier)
                    : null;

            book.Reviews =
                await _reviewService.GetReviewsForBookAsync(
                    id,
                    currentUserId);

            return View(book);
        }
    }
}