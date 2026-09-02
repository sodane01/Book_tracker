using Book_tracker.ViewModels;

namespace Book_tracker.Services
{
    public interface IReviewService
    {
        Task<List<ReviewViewModel>> GetReviewsForBookAsync(
            string externalBookId,
            string? currentUserId = null);

        Task<bool> HasUserReviewedBookAsync(
            string userId,
            int bookId);

        Task<bool> CreateReviewAsync(
            string userId,
            int bookId,
            string reviewText);

        Task<bool> UpdateReviewAsync(
            string userId,
            int reviewId,
            string reviewText);

        Task<bool> DeleteReviewAsync(
            string userId,
            int reviewId);
    }
}