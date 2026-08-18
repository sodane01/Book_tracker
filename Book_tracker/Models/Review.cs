namespace Book_tracker.Models
{
    public class Review
    {
        public int Id { get; set; }

        public string UserId { get; set; } = string.Empty;

        public int BookId { get; set; }

        public string ReviewText { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }
        public ApplicationUser User { get; set; } = null!;

        public Book Book { get; set; } = null!;
    }
}