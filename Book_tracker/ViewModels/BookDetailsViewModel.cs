namespace Book_tracker.ViewModels
{
    public class BookDetailsViewModel
    {
        public string ExternalBookId { get; set; } = string.Empty;

        public string Title { get; set; } = string.Empty;

        public string Author { get; set; } = string.Empty;

        public string? CoverImageUrl { get; set; }

        public string? Description { get; set; }

        public string? Publisher { get; set; }

        public string? PublishedDate { get; set; }

        public double? AverageRating { get; set; }

        public List<ReviewViewModel> Reviews { get; set; } = new();
    }
}