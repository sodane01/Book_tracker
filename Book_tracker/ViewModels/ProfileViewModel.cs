using System.ComponentModel.DataAnnotations;
using Book_tracker.Models.Enums;

namespace Book_tracker.ViewModels
{
    public class ProfileViewModel
    {
        public string Username { get; set; }
            = string.Empty;

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
            = string.Empty;

        public int BooksRead { get; set; }

        public int PagesRead { get; set; }

        public List<ProfileFavouriteBookViewModel> FavouriteBooks { get; set; }
            = new();

        public Theme Theme { get; set; }
            = Theme.Light;

        [DataType(DataType.Password)]
        [Display(Name = "Current Password")]
        public string? CurrentPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "New Password")]
        public string? NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm New Password")]
        [Compare(
            nameof(NewPassword),
            ErrorMessage = "The new password and confirmation password do not match.")]
        public string? ConfirmPassword { get; set; }
    }

    public class ProfileFavouriteBookViewModel
    {
        public int UserBookId { get; set; }

        public int BookId { get; set; }

        public string ExternalBookId { get; set; }
            = string.Empty;

        public string Title { get; set; }
            = string.Empty;

        public string? Author { get; set; }

        public string? CoverImageUrl { get; set; }
    }
}