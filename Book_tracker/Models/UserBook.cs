using Book_tracker.Models.Enums;

namespace Book_tracker.Models
{
    public class UserBook
    {
        public int Id { get; set; }

        public string UserId { get; set; } = string.Empty;

        public int BookId { get; set; }

        public ReadingStatus ReadingStatus { get; set; }

        public int? CurrentPage { get; set; }

        public bool IsFavourite { get; set; } = false;

        public int? Rating { get; set; }

        public DateTime DateAdded { get; set; } = DateTime.UtcNow;

        public DateTime? StartedDate { get; set; }

        public DateTime? FinishedDate { get; set; }
        public ApplicationUser User { get; set; } = null!;

        public Book Book { get; set; } = null!;
    }
}