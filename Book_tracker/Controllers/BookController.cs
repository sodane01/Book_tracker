using Book_tracker.Services;
using Microsoft.AspNetCore.Mvc;

namespace Book_tracker.Controllers
{
    public class BookController : Controller
    {
        private readonly IGoogleBooksService _googleBooksService;

        public BookController(
            IGoogleBooksService googleBooksService)
        {
            _googleBooksService = googleBooksService;
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

            return View(book);
        }
    }
}