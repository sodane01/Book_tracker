namespace Book_tracker.Models
{
    public class BookSearchResult
    {
        public string ExternalBookId { get; set; } = string.Empty;

        public string Title { get; set; } = string.Empty;

        public string Author { get; set; } = string.Empty;

        public string? CoverImageUrl { get; set; }

        public double? AverageRating { get; set; }
    }
}