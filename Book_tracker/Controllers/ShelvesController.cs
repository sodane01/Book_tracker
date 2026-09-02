using System.Security.Claims;
using Book_tracker.Services;
using Book_tracker.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Book_tracker.Controllers
{
    [Authorize(Policy = "ActiveUser")]
    public class ShelvesController : Controller
    {
        private readonly IUserBookService _userBookService;

        public ShelvesController(
            IUserBookService userBookService)
        {
            _userBookService = userBookService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userId =
                User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrWhiteSpace(userId))
            {
                return Challenge();
            }

            var userBooks =
                await _userBookService.GetUserBooksAsync(userId);

            var model = new ShelvesViewModel
            {
                WantToReadBooks = userBooks
                    .Where(userBook =>
                        userBook.ReadingStatus ==
                        Models.Enums.ReadingStatus.WantToRead)
                    .Select(CreateShelfBookViewModel)
                    .ToList(),

                CurrentlyReadingBooks = userBooks
                    .Where(userBook =>
                        userBook.ReadingStatus ==
                        Models.Enums.ReadingStatus.CurrentlyReading)
                    .Select(CreateShelfBookViewModel)
                    .ToList(),

                ReadBooks = userBooks
                    .Where(userBook =>
                        userBook.ReadingStatus ==
                        Models.Enums.ReadingStatus.Read)
                    .Select(CreateShelfBookViewModel)
                    .ToList()
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(
            AddBookToShelfViewModel model)
        {
            var userId =
                User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrWhiteSpace(userId))
            {
                return Challenge();
            }

            if (string.IsNullOrWhiteSpace(model.ExternalBookId))
            {
                return BadRequest();
            }

            var book =
                await _userBookService.GetOrCreateBookAsync(
                    model.ExternalBookId);

            if (book == null)
            {
                return NotFound();
            }

            await _userBookService.CreateUserBookAsync(
                userId,
                book,
                model.ReadingStatus);

            TempData["ShelfMessage"] =
                $"\"{book.Title}\" was saved to {GetShelfDisplayName(model.ReadingStatus)}.";

            return RedirectToAction(
                "Index",
                "Discover",
                new
                {
                    searchQuery = model.SearchQuery,
                    searchType = model.SearchType.ToString(),
                    sortType = model.SortType.ToString()
                });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeStatus(
            int userBookId,
            Models.Enums.ReadingStatus readingStatus)
        {
            var userId =
                User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrWhiteSpace(userId))
            {
                return Challenge();
            }

            if (!Enum.IsDefined(
                typeof(Models.Enums.ReadingStatus),
                readingStatus))
            {
                return BadRequest();
            }

            var success =
                await _userBookService.ChangeReadingStatusAsync(
                    userId,
                    userBookId,
                    readingStatus);

            if (!success)
            {
                return NotFound();
            }

            TempData["ShelfMessage"] =
                $"Reading status changed to {GetShelfDisplayName(readingStatus)}.";

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleFavourite(
            int userBookId)
        {
            var userId =
                User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrWhiteSpace(userId))
            {
                return Challenge();
            }

            var success =
                await _userBookService.ToggleFavouriteAsync(
                    userId,
                    userBookId);

            if (!success)
            {
                return NotFound();
            }

            TempData["ShelfMessage"] =
                "Favourite status updated.";

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Remove(
            int userBookId)
        {
            var userId =
                User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrWhiteSpace(userId))
            {
                return Challenge();
            }

            var success =
                await _userBookService.RemoveUserBookAsync(
                    userId,
                    userBookId);

            if (!success)
            {
                return NotFound();
            }

            TempData["ShelfMessage"] =
                "Book removed from your shelves.";

            return RedirectToAction(nameof(Index));
        }

        private static ShelfBookViewModel CreateShelfBookViewModel(
            Models.UserBook userBook)
        {
            return new ShelfBookViewModel
            {
                UserBookId = userBook.Id,
                BookId = userBook.BookId,
                ExternalBookId = userBook.Book.ExternalBookId,
                Title = userBook.Book.Title,
                Author = userBook.Book.AuthorDisplay,
                CoverImageUrl = userBook.Book.CoverImageUrl,
                ReadingStatus = userBook.ReadingStatus,
                IsFavourite = userBook.IsFavourite
            };
        }

        private static string GetShelfDisplayName(
            Models.Enums.ReadingStatus readingStatus)
        {
            return readingStatus switch
            {
                Models.Enums.ReadingStatus.WantToRead =>
                    "Want to Read",

                Models.Enums.ReadingStatus.CurrentlyReading =>
                    "Currently Reading",

                Models.Enums.ReadingStatus.Read =>
                    "Read",

                _ => "your shelf"
            };
        }
    }
}