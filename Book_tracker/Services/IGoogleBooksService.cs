using Book_tracker.Models;
using Book_tracker.Models.Enums;
using Book_tracker.ViewModels;

namespace Book_tracker.Services
{
    public interface IGoogleBooksService
    {
        Task<BookSearchServiceResult> SearchBooksAsync(
    string query,
    BookSearchType searchType);

        Task<BookDetailsViewModel?> GetBookByIdAsync(
            string externalBookId);
    }
}