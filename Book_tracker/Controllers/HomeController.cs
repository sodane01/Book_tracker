using Book_tracker.Models;
using Book_tracker.Models.Enums;
using Book_tracker.Services;
using Book_tracker.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Book_tracker.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserBookService _userBookService;

        public HomeController(
            UserManager<ApplicationUser> userManager,
            IUserBookService userBookService)
        {
            _userManager = userManager;
            _userBookService = userBookService;
        }

        public async Task<IActionResult> Index()
        {
            var isAuthenticated =
                User.Identity?.IsAuthenticated ?? false;

            var viewModel = new HomeViewModel
            {
                IsAuthenticated = isAuthenticated,
                UserName = User.Identity?.Name
            };

            if (!isAuthenticated)
            {
                return View(viewModel);
            }

            var user =
                await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return View(viewModel);
            }

            var userBooks =
                await _userBookService.GetUserBooksAsync(user.Id);

            viewModel.CurrentlyReadingCount =
                userBooks.Count(userBook =>
                    userBook.ReadingStatus ==
                    ReadingStatus.CurrentlyReading);

            viewModel.BooksReadCount =
                userBooks.Count(userBook =>
                    userBook.ReadingStatus ==
                    ReadingStatus.Read);

            viewModel.FavouriteBooksCount =
                userBooks.Count(userBook =>
                    userBook.IsFavourite);

            // Notifications are not connected to the Home statistics yet.
            viewModel.UnreadNotificationCount = 0;

            return View(viewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(
            Duration = 0,
            Location = ResponseCacheLocation.None,
            NoStore = true)]
        public IActionResult Error()
        {
            return View(
                new ErrorViewModel
                {
                    RequestId =
                        Activity.Current?.Id ??
                        HttpContext.TraceIdentifier
                });
        }
    }
}