using Book_tracker.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Book_tracker.Data
{
    public class ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options)
        : IdentityDbContext<ApplicationUser>(options)
    {
        public DbSet<Book> Books { get; set; }
        public DbSet<UserBook> UserBooks { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Notification> Notifications { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Book: ExternalBookId must be unique
            builder.Entity<Book>()
                .HasIndex(book => book.ExternalBookId)
                .IsUnique();

            // UserBook: one Book can only occur once per User
            builder.Entity<UserBook>()
                .HasIndex(userBook => new
                {
                    userBook.UserId,
                    userBook.BookId
                })
                .IsUnique();

            // Review: one Review per User and Book
            builder.Entity<Review>()
                .HasIndex(review => new
                {
                    review.UserId,
                    review.BookId
                })
                .IsUnique();


            // UserBook -> ApplicationUser
            builder.Entity<UserBook>()
                .HasOne(userBook => userBook.User)
                .WithMany(user => user.UserBooks)
                .HasForeignKey(userBook => userBook.UserId);

            // UserBook -> Book
            builder.Entity<UserBook>()
                .HasOne(userBook => userBook.Book)
                .WithMany(book => book.UserBooks)
                .HasForeignKey(userBook => userBook.BookId);


            // Review -> ApplicationUser
            builder.Entity<Review>()
                .HasOne(review => review.User)
                .WithMany(user => user.Reviews)
                .HasForeignKey(review => review.UserId);

            // Review -> Book
            builder.Entity<Review>()
                .HasOne(review => review.Book)
                .WithMany(book => book.Reviews)
                .HasForeignKey(review => review.BookId);


            // Notification -> ApplicationUser
            builder.Entity<Notification>()
                .HasOne(notification => notification.User)
                .WithMany(user => user.Notifications)
                .HasForeignKey(notification => notification.UserId);
        }
    }
}