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
            string? searchType,
            string? sortType)
        {
            var model = new DiscoverViewModel();

            if (string.IsNullOrWhiteSpace(searchQuery))
            {
                return View(model);
            }

            model.SearchQuery = searchQuery;
            model.HasSearched = true;

            if (!Enum.TryParse<BookSearchType>(
                    searchType,
                    true,
                    out var selectedSearchType))
            {
                selectedSearchType = BookSearchType.Title;
            }

            model.SearchType = selectedSearchType;

            if (!Enum.TryParse<BookSortType>(
                    sortType,
                    true,
                    out var selectedSortType))
            {
                selectedSortType = BookSortType.Title;
            }

            model.SortType = selectedSortType;

            var searchResult =
                await _googleBooksService.SearchBooksAsync(
                    searchQuery,
                    selectedSearchType);

            if (!searchResult.IsSuccess)
            {
                model.HasSearchError = true;

                return View(model);
            }

            var results = searchResult.Books;

            if (selectedSortType == BookSortType.Author)
            {
                results = results
                    .OrderBy(
                        book => book.Author ?? string.Empty,
                        StringComparer.OrdinalIgnoreCase)
                    .ToList();
            }
            else
            {
                results = results
                    .OrderBy(
                        book => book.Title,
                        StringComparer.OrdinalIgnoreCase)
                    .ToList();
            }

            model.SearchResults = results;

            return View(model);
        }
    }
}