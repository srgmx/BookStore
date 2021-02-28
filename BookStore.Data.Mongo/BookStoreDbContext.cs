using BookCoreLibrary.Data.Mongo;
using BookStore.Domain;
using MongoDB.Driver;

namespace BookStore.Data.Mongo
{
    public class BookStoreDbContext : MongoBaseDbContext
    {
        public BookStoreDbContext(IMongoClient client, string databaseName)
            : base(client, databaseName)
        {
        }

        public IMongoCollection<User> Users => GetCollection<User>(typeof(User).Name);

        public IMongoCollection<Author> Authors => GetCollection<Author>(typeof(Author).Name);

        public IMongoCollection<Book> Books => GetCollection<Book>(typeof(Book).Name);
    }
}
