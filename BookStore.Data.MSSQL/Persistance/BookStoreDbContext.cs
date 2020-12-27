using BookStore.Domain;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Text.Json;

namespace BookStore.Data.MSSQL.Persistance
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

            var newIdFunction = "NEWID()";
            var defaultMaxLength = 255;

            builder.Entity<User>(user =>
            {
                user.Property(u => u.Id).HasDefaultValueSql(newIdFunction);
                user.Property(u => u.LastName).HasMaxLength(defaultMaxLength);
                user.Property(u => u.FirstName).HasMaxLength(defaultMaxLength);

                user.Property(u => u.Permissions)
                        .IsRequired()
                        .HasMaxLength(2000)
                        .HasDefaultValueSql("'[]'")
                        .HasConversion(
                            p => JsonSerializer.Serialize(p, null),
                            p => JsonSerializer.Deserialize<List<string>>(p, null)
                         );
            });

            builder.Entity<Author>(author =>
            {
                author.Property(a => a.Id).HasDefaultValueSql(newIdFunction);
                author.Property(a => a.PenName).HasMaxLength(defaultMaxLength);
            });

            builder.Entity<Book>(user =>
            {
                user.Property(b => b.Id).HasDefaultValueSql(newIdFunction);
                user.Property(b => b.Name).HasMaxLength(defaultMaxLength);
            });
        }
    }
}
