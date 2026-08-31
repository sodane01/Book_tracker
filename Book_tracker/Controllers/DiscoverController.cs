using Book_tracker.Models.Enums;
using Book_tracker.Services;
using Book_tracker.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Book_tracker.Controllers
{
    public class DiscoverController : Controller
    {
        private readonly IGoogleBooksService _googleBooksService;

        public DiscoverController(
            IGoogleBooksService googleBooksService)
        {
            _googleBooksService = googleBooksService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(
            string? searchQuery,
            BookSearchType searchType = BookSearchType.Title)
        {
            var viewModel = new DiscoverViewModel
            {
                SearchQuery = searchQuery?.Trim() ?? string.Empty,
                SearchType = searchType
            };

            if (string.IsNullOrWhiteSpace(searchQuery))
            {
                return View(viewModel);
            }

            var books = await _googleBooksService.SearchBooksAsync(

            viewModel.SearchQuery,
            viewModel.SearchType);

            viewModel.SearchResults = books;
            viewModel.HasSearched = true;

            return View(viewModel);
        }
    }
}