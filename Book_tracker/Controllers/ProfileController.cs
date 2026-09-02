using Book_tracker.Models;
using Book_tracker.Models.Enums;
using Book_tracker.Services;
using Book_tracker.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Book_tracker.Controllers
{
    [Authorize(Policy = "ActiveUser")]
    public class ProfileController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserBookService _userBookService;

        public ProfileController(
            UserManager<ApplicationUser> userManager,
            IUserBookService userBookService)
        {
            _userManager = userManager;
            _userBookService = userBookService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var user =
                await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return Challenge();
            }

            var model =
                await BuildProfileViewModelAsync(user);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateAccount(
            ProfileViewModel model)
        {
            var user =
                await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return Challenge();
            }

            if (!ModelState.IsValid)
            {
                var viewModel =
                    await BuildProfileViewModelAsync(user);

                viewModel.Email = model.Email;

                return View("Index", viewModel);
            }

            var existingUser =
                await _userManager.FindByEmailAsync(model.Email);

            if (existingUser != null &&
                existingUser.Id != user.Id)
            {
                ModelState.AddModelError(
                    nameof(model.Email),
                    "That email address is already in use.");

                var viewModel =
                    await BuildProfileViewModelAsync(user);

                viewModel.Email = model.Email;

                return View("Index", viewModel);
            }

            var emailResult =
                await _userManager.SetEmailAsync(
                    user,
                    model.Email);

            if (!emailResult.Succeeded)
            {
                foreach (var error in emailResult.Errors)
                {
                    ModelState.AddModelError(
                        nameof(model.Email),
                        error.Description);
                }

                var viewModel =
                    await BuildProfileViewModelAsync(user);

                viewModel.Email = model.Email;

                return View("Index", viewModel);
            }

            var usernameResult =
                await _userManager.SetUserNameAsync(
                    user,
                    model.Email);

            if (!usernameResult.Succeeded)
            {
                foreach (var error in usernameResult.Errors)
                {
                    ModelState.AddModelError(
                        nameof(model.Email),
                        error.Description);
                }

                var viewModel =
                    await BuildProfileViewModelAsync(user);

                viewModel.Email = model.Email;

                return View("Index", viewModel);
            }

            TempData["ProfileMessage"] =
                "Account information updated.";

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleFavourite(
            int userBookId)
        {
            var user =
                await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return Challenge();
            }

            var success =
                await _userBookService.ToggleFavouriteAsync(
                    user.Id,
                    userBookId);

            if (!success)
            {
                return NotFound();
            }

            TempData["ProfileMessage"] =
                "Favourite removed.";

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateTheme(
            Theme theme)
        {
            var user =
                await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return Challenge();
            }

            if (!Enum.IsDefined(typeof(Theme), theme))
            {
                return BadRequest();
            }

            user.Theme = theme;

            var result =
                await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                TempData["ProfileError"] =
                    "The theme could not be updated.";

                return RedirectToAction(nameof(Index));
            }

            TempData["ProfileMessage"] =
                "Theme updated.";

            return RedirectToAction(nameof(Index));
        }

        private async Task<ProfileViewModel>
            BuildProfileViewModelAsync(
                ApplicationUser user)
        {
            var userBooks =
                await _userBookService.GetUserBooksAsync(user.Id);

            var readBooks = userBooks
                .Where(userBook =>
                    userBook.ReadingStatus == ReadingStatus.Read)
                .ToList();

            var favouriteBooks = userBooks
                .Where(userBook =>
                    userBook.IsFavourite)
                .Select(userBook =>
                    new ProfileFavouriteBookViewModel
                    {
                        UserBookId =
                            userBook.Id,

                        BookId =
                            userBook.BookId,

                        ExternalBookId =
                            userBook.Book.ExternalBookId,

                        Title =
                            userBook.Book.Title,

                        Author =
                            userBook.Book.AuthorDisplay,

                        CoverImageUrl =
                            userBook.Book.CoverImageUrl
                    })
                .ToList();

            return new ProfileViewModel
            {
                Username =
                    user.UserName ?? string.Empty,

                Email =
                    user.Email ?? string.Empty,

                BooksRead =
                    readBooks.Count,

                PagesRead =
                    readBooks
                        .Where(userBook =>
                            userBook.Book.PageCount.HasValue)
                        .Sum(userBook =>
                            userBook.Book.PageCount!.Value),

                FavouriteBooks =
                    favouriteBooks,

                Theme =
                    user.Theme
            };
        }
    }
}