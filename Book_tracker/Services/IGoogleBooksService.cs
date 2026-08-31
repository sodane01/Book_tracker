using Book_tracker.Models;
using Book_tracker.Models.Enums;

namespace Book_tracker.Services
{
    public interface IGoogleBooksService
    {
        Task<List<BookSearchResult>> SearchBooksAsync(
            string query,
            BookSearchType searchType);
    }
}