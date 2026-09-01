using Book_tracker.Models.Enums;

namespace Book_tracker.ViewModels
{
    public class AddBookToShelfViewModel
    {
        public string ExternalBookId { get; set; } = string.Empty;

        public ReadingStatus ReadingStatus { get; set; }

        public string SearchQuery { get; set; } = string.Empty;

        public BookSearchType SearchType { get; set; }
            = BookSearchType.Title;

        public BookSortType SortType { get; set; }
            = BookSortType.Title;
    }
}