using Book_tracker.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Book_tracker.Controllers
{
    [Authorize(Policy = "ActiveUser")]
    public class ReviewsController : Controller
    {
        private readonly IReviewService _reviewService;
        private readonly IUserBookService _userBookService;

        public ReviewsController(
            IReviewService reviewService,
            IUserBookService userBookService)
        {
            _reviewService = reviewService;
            _userBookService = userBookService;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            string externalBookId,
            string reviewText)
        {
            var userId =
                User.FindFirstValue(
                    ClaimTypes.NameIdentifier);

            if (string.IsNullOrWhiteSpace(userId))
            {
                return Challenge();
            }

            if (string.IsNullOrWhiteSpace(externalBookId))
            {
                return BadRequest();
            }

            if (string.IsNullOrWhiteSpace(reviewText))
            {
                TempData["ReviewError"] =
                    "Review text is required.";

                return RedirectToAction(
                    "Details",
                    "Book",
                    new { id = externalBookId });
            }

            var book =
                await _userBookService.GetOrCreateBookAsync(
                    externalBookId);

            if (book == null)
            {
                return NotFound();
            }

            var success =
                await _reviewService.CreateReviewAsync(
                    userId,
                    book.Id,
                    reviewText);

            if (!success)
            {
                TempData["ReviewError"] =
                    "You have already reviewed this book.";

                return RedirectToAction(
                    "Details",
                    "Book",
                    new { id = externalBookId });
            }

            TempData["ReviewMessage"] =
                "Review added.";

            return RedirectToAction(
                "Details",
                "Book",
                new { id = externalBookId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int reviewId,
            string externalBookId,
            string reviewText)
        {
            var userId =
                User.FindFirstValue(
                    ClaimTypes.NameIdentifier);

            if (string.IsNullOrWhiteSpace(userId))
            {
                return Challenge();
            }

            if (string.IsNullOrWhiteSpace(externalBookId))
            {
                return BadRequest();
            }

            if (string.IsNullOrWhiteSpace(reviewText))
            {
                TempData["ReviewError"] =
                    "Review text is required.";

                return RedirectToAction(
                    "Details",
                    "Book",
                    new { id = externalBookId });
            }

            var success =
                await _reviewService.UpdateReviewAsync(
                    userId,
                    reviewId,
                    reviewText);

            if (!success)
            {
                TempData["ReviewError"] =
                    "The review could not be updated.";

                return RedirectToAction(
                    "Details",
                    "Book",
                    new { id = externalBookId });
            }

            TempData["ReviewMessage"] =
                "Review updated.";

            return RedirectToAction(
                "Details",
                "Book",
                new { id = externalBookId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(
            int reviewId,
            string externalBookId)
        {
            var userId =
                User.FindFirstValue(
                    ClaimTypes.NameIdentifier);

            if (string.IsNullOrWhiteSpace(userId))
            {
                return Challenge();
            }

            if (string.IsNullOrWhiteSpace(externalBookId))
            {
                return BadRequest();
            }

            var success =
                await _reviewService.DeleteReviewAsync(
                    userId,
                    reviewId);

            if (!success)
            {
                TempData["ReviewError"] =
                    "The review could not be deleted.";

                return RedirectToAction(
                    "Details",
                    "Book",
                    new { id = externalBookId });
            }

            TempData["ReviewMessage"] =
                "Review deleted.";

            return RedirectToAction(
                "Details",
                "Book",
                new { id = externalBookId });
        }
    }
}