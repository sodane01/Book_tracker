using Book_tracker.Data;
using Book_tracker.Models;
using Book_tracker.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace Book_tracker.Services
{
    public class UserBookService : IUserBookService
    {
        private readonly ApplicationDbContext _context;
        private readonly IGoogleBooksService _googleBooksService;

        public UserBookService(
            ApplicationDbContext context,
            IGoogleBooksService googleBooksService)
        {
            _context = context;
            _googleBooksService = googleBooksService;
        }

        public async Task<Book?> GetBookByExternalIdAsync(
            string externalBookId)
        {
            return await _context.Books
                .FirstOrDefaultAsync(
                    book => book.ExternalBookId == externalBookId);
        }

        public async Task<Book?> CreateBookFromExternalIdAsync(
            string externalBookId)
        {
            if (string.IsNullOrWhiteSpace(externalBookId))
            {
                return null;
            }

            var bookDetails =
                await _googleBooksService.GetBookByIdAsync(
                    externalBookId);

            if (bookDetails == null)
            {
                return null;
            }

            var book = new Book
            {
                ExternalBookId = bookDetails.ExternalBookId,

                Title = string.IsNullOrWhiteSpace(bookDetails.Title)
                    ? "Unknown title"
                    : bookDetails.Title,

                AuthorDisplay = string.IsNullOrWhiteSpace(bookDetails.Author)
                    ? null
                    : bookDetails.Author,

                Description = string.IsNullOrWhiteSpace(bookDetails.Description)
                    ? null
                    : bookDetails.Description,

                PublishedDate = string.IsNullOrWhiteSpace(bookDetails.PublishedDate)
                    ? null
                    : bookDetails.PublishedDate,

                CoverImageUrl = string.IsNullOrWhiteSpace(bookDetails.CoverImageUrl)
                    ? null
                    : bookDetails.CoverImageUrl
            };

            _context.Books.Add(book);

            await _context.SaveChangesAsync();

            return book;
        }

        public async Task<Book?> GetOrCreateBookAsync(
            string externalBookId)
        {
            var existingBook =
                await GetBookByExternalIdAsync(
                    externalBookId);

            if (existingBook != null)
            {
                return existingBook;
            }

            return await CreateBookFromExternalIdAsync(
                externalBookId);
        }

        public async Task<UserBook?> GetUserBookAsync(
            string userId,
            int bookId)
        {
            return await _context.UserBooks
                .FirstOrDefaultAsync(
                    userBook =>
                        userBook.UserId == userId &&
                        userBook.BookId == bookId);
        }

        public async Task<List<UserBook>> GetUserBooksAsync(
    string userId)
        {
            return await _context.UserBooks
                .Include(userBook => userBook.Book)
                .Where(userBook => userBook.UserId == userId)
                .ToListAsync();
        }

        public async Task<UserBook> CreateUserBookAsync(
            string userId,
            Book book,
            ReadingStatus readingStatus)
        {
            var existingUserBook =
                await GetUserBookAsync(
                    userId,
                    book.Id);

            if (existingUserBook != null)
            {
                existingUserBook.ReadingStatus =
                    readingStatus;

                await _context.SaveChangesAsync();

                return existingUserBook;
            }

            var userBook = new UserBook
            {
                UserId = userId,
                BookId = book.Id,
                ReadingStatus = readingStatus
            };

            _context.UserBooks.Add(userBook);

            await _context.SaveChangesAsync();

            return userBook;
        }
    }
}