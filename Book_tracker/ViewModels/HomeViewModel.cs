namespace Book_tracker.ViewModels
{
    public class HomeViewModel
    {
        public bool IsAuthenticated { get; set; }

        public string? UserName { get; set; }

        public int CurrentlyReadingCount { get; set; }

        public int BooksReadCount { get; set; }

        public int FavouriteBooksCount { get; set; }

        public int UnreadNotificationCount { get; set; }
    }
}