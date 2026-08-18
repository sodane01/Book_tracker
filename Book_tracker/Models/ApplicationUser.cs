using Book_tracker.Models.Enums;
using Microsoft.AspNetCore.Identity;

namespace Book_tracker.Models
{
    public class ApplicationUser : IdentityUser
    {
        public AccountStatus AccountStatus { get; set; } = AccountStatus.Active;

        public Theme Theme { get; set; } = Theme.Light;
        public ICollection<UserBook> UserBooks { get; set; }
            = new List<UserBook>();

        public ICollection<Review> Reviews { get; set; }
            = new List<Review>();

        public ICollection<Notification> Notifications { get; set; }
            = new List<Notification>();
    }
}