using Book_tracker.Models;
using Book_tracker.Models.Enums;

namespace Book_tracker.Services
{
    public interface IUserBookService
    {
        Task<Book?> GetBookByExternalIdAsync(
            string externalBookId);

        Task<Book?> CreateBookFromExternalIdAsync(
            string externalBookId);

        Task<Book?> GetOrCreateBookAsync(
            string externalBookId);

        Task<UserBook?> GetUserBookAsync(
            string userId,
            int bookId);

        Task<List<UserBook>> GetUserBooksAsync(
            string userId);

        Task<UserBook> CreateUserBookAsync(
            string userId,
            Book book,
            ReadingStatus readingStatus);

        Task<bool> ChangeReadingStatusAsync(
            string userId,
            int userBookId,
            ReadingStatus readingStatus);

        Task<bool> ToggleFavouriteAsync(
            string userId,
            int userBookId);

        Task<bool> RemoveUserBookAsync(
            string userId,
            int userBookId);
    }
}