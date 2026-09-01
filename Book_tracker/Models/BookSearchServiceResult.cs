using Book_tracker.Models;

namespace Book_tracker.Models
{
    public class BookSearchServiceResult
    {
        public bool IsSuccess { get; set; }

        public List<BookSearchResult> Books { get; set; }
            = new();
    }
}