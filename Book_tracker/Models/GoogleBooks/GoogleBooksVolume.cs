namespace Book_tracker.Models.GoogleBooks
{
    public class GoogleBooksVolume
    {
        public string Id { get; set; } = string.Empty;

        public GoogleBooksVolumeInfo VolumeInfo { get; set; } = new();
    }
}