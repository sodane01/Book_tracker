namespace Book_tracker.Models.GoogleBooks
{
    public class GoogleBooksVolumeInfo
    {
        public string Title { get; set; } = string.Empty;

        public List<string> Authors { get; set; } = new();

        public string? Publisher { get; set; }

        public string? PublishedDate { get; set; }

        public string? Description { get; set; }

        public GoogleBooksImageLinks? ImageLinks { get; set; }

        public double? AverageRating { get; set; }
    }
}