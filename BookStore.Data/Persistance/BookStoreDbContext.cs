using BookStore.Domain;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Data.Persistance
{
    public class BookStoreDbContext : DbContext
    {
        public BookStoreDbContext(DbContextOptions<BookStoreDbContext> options) :
            base(options)
        {
        }

        public DbSet<User> User { get; set; }

        public DbSet<Author> Author { get; set; }

        public DbSet<Book> Book { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<User>(user =>
            {
                user.Property(u => u.LastName).HasMaxLength(255);
                user.Property(u => u.FirstName).HasMaxLength(255);
            });
            builder.Entity<Author>().Property(u => u.PenName).HasMaxLength(255);
            builder.Entity<Book>().Property(u => u.Name).HasMaxLength(255);
        }
    }
}
