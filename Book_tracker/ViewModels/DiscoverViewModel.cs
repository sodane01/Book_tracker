using Book_tracker.Models;
using Book_tracker.Models.Enums;

namespace Book_tracker.ViewModels
{
    public class DiscoverViewModel
    {
        public string SearchQuery { get; set; } = string.Empty;

        public BookSearchType SearchType { get; set; } = BookSearchType.Title;

        public List<BookSearchResult> SearchResults { get; set; } = new();

        public bool HasSearched { get; set; }
    }
}