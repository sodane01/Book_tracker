namespace Book_tracker.ViewModels
{
    public class ReviewViewModel
    {
        public int Id { get; set; }

        public string Username { get; set; } = string.Empty;

        public string ReviewText { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public bool IsOwnReview { get; set; }
    }
}