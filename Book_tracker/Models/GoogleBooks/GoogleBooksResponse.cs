namespace Book_tracker.Models.GoogleBooks
{
    public class GoogleBooksResponse
    {
        public int TotalItems { get; set; }

        public List<GoogleBooksVolume> Items { get; set; } = new();
    }
}