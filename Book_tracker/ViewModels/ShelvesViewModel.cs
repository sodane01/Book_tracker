using Book_tracker.Models.Enums;

namespace Book_tracker.ViewModels
{
    public class ShelvesViewModel
    {
        public List<ShelfBookViewModel> WantToReadBooks { get; set; }
            = new();

        public List<ShelfBookViewModel> CurrentlyReadingBooks { get; set; }
            = new();

        public List<ShelfBookViewModel> ReadBooks { get; set; }
            = new();

        public bool HasBooks =>
            WantToReadBooks.Count > 0 ||
            CurrentlyReadingBooks.Count > 0 ||
            ReadBooks.Count > 0;
    }

    public class ShelfBookViewModel
    {
        public int UserBookId { get; set; }

        public int BookId { get; set; }

        public string ExternalBookId { get; set; }
            = string.Empty;

        public string Title { get; set; }
            = string.Empty;

        public string? Author { get; set; }

        public string? CoverImageUrl { get; set; }

        public ReadingStatus ReadingStatus { get; set; }

        public bool IsFavourite { get; set; }
    }
}