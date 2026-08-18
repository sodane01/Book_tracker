namespace Book_tracker.Models
{
    public class Book
    {
        public int Id { get; set; }

        public string ExternalBookId { get; set; } = string.Empty;

        public string Title { get; set; } = string.Empty;

        public string? AuthorDisplay { get; set; }

        public string? ISBN { get; set; }

        public string? Description { get; set; }

        public int? PageCount { get; set; }

        public string? PublishedDate { get; set; }

        public string? CoverImageUrl { get; set; }

        public string? CategoryDisplay { get; set; }

        public string? SeriesName { get; set; }
        public ICollection<UserBook> UserBooks { get; set; }
        = new List<UserBook>();

        public ICollection<Review> Reviews { get; set; }
            = new List<Review>();
    }
}