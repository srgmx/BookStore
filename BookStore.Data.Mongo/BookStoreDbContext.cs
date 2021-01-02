using BookStore.Domain;
using MongoDB.Driver;

namespace BookStore.Data.Mongo
{
    public class BookStoreDbContext
    {
        public BookStoreDbContext(IMongoClient client, string databaseName)
        {
            Database = client.GetDatabase(databaseName);
        }

        public IMongoDatabase Database { get; }

        public IMongoCollection<User> Users => 
            Database.GetCollection<User>(typeof(User).Name);

        public IMongoCollection<Author> Authors =>
            Database.GetCollection<Author>(typeof(Author).Name);

        public IMongoCollection<Book> Books =>
            Database.GetCollection<Book>(typeof(Book).Name);
    }
}
