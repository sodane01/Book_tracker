using Book_tracker.Data;
using Book_tracker.Models;
using Book_tracker.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace Book_tracker.Services
{
    public class ReviewService : IReviewService
    {
        private readonly ApplicationDbContext _context;

        public ReviewService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<ReviewViewModel>> GetReviewsForBookAsync(
    string externalBookId,
    string? currentUserId = null)
        {
            var reviews = await _context.Reviews
                .AsNoTracking()
                .Where(review =>
                    review.Book.ExternalBookId == externalBookId)
                .OrderByDescending(review => review.CreatedAt)
                .Select(review => new
                {
                    review.Id,
                    review.UserId,
                    Username = review.User.UserName,
                    review.ReviewText,
                    review.CreatedAt,
                    review.UpdatedAt
                })
                .ToListAsync();

            return reviews
                .Select(review => new ReviewViewModel
                {
                    Id = review.Id,

                    Username =
                        review.Username
                        ?? "Unknown user",

                    ReviewText = review.ReviewText,

                    CreatedAt = review.CreatedAt,

                    UpdatedAt = review.UpdatedAt,

                    IsOwnReview =
                        !string.IsNullOrWhiteSpace(currentUserId) &&
                        review.UserId == currentUserId
                })
                .ToList();
        }

        public async Task<bool> HasUserReviewedBookAsync(
            string userId,
            int bookId)
        {
            return await _context.Reviews
                .AnyAsync(review =>
                    review.UserId == userId &&
                    review.BookId == bookId);
        }

        public async Task<bool> CreateReviewAsync(
            string userId,
            int bookId,
            string reviewText)
        {
            if (string.IsNullOrWhiteSpace(reviewText))
            {
                return false;
            }

            var alreadyExists =
                await HasUserReviewedBookAsync(
                    userId,
                    bookId);

            if (alreadyExists)
            {
                return false;
            }

            var review = new Review
            {
                UserId = userId,
                BookId = bookId,
                ReviewText = reviewText.Trim(),
                CreatedAt = DateTime.UtcNow
            };

            _context.Reviews.Add(review);

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> UpdateReviewAsync(
            string userId,
            int reviewId,
            string reviewText)
        {
            if (string.IsNullOrWhiteSpace(reviewText))
            {
                return false;
            }

            var review =
                await _context.Reviews
                    .FirstOrDefaultAsync(review =>
                        review.Id == reviewId &&
                        review.UserId == userId);

            if (review == null)
            {
                return false;
            }

            review.ReviewText = reviewText.Trim();
            review.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteReviewAsync(
            string userId,
            int reviewId)
        {
            var review =
                await _context.Reviews
                    .FirstOrDefaultAsync(review =>
                        review.Id == reviewId &&
                        review.UserId == userId);

            if (review == null)
            {
                return false;
            }

            _context.Reviews.Remove(review);

            await _context.SaveChangesAsync();

            return true;
        }
    }
}