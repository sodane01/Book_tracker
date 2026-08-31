namespace Book_tracker.Models.GoogleBooks
{
    public class GoogleBooksVolumeInfo
    {
        public string Title { get; set; } = string.Empty;

        public List<string> Authors { get; set; } = new();

        public double? AverageRating { get; set; }

        public GoogleBooksImageLinks? ImageLinks { get; set; }
    }
}