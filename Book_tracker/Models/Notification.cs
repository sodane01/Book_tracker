using Book_tracker.Models.Enums;

namespace Book_tracker.Models
{
    public class Notification
    {
        public int Id { get; set; }

        public string UserId { get; set; } = string.Empty;

        public NotificationType Type { get; set; }

        public string Message { get; set; } = string.Empty;

        public bool IsRead { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public RelatedEntityType? RelatedEntityType { get; set; }

        public int? RelatedEntityId { get; set; }
        public ApplicationUser User { get; set; } = null!;
    }
}