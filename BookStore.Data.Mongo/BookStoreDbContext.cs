using BookStore.Domain;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Data.Mongo
{
    public class BookStoreDbContext
    {
        private readonly IMongoClient _client;
        private readonly List<Func<Task>> _commands;

        public BookStoreDbContext(IMongoClient client, string databaseName)
        {
            _client = client;
            _commands = new List<Func<Task>>();
            SetDatabase(databaseName);
        }

        public IMongoDatabase Database { get; private set; }

        public IClientSessionHandle Session { get; private set; }

        public IMongoCollection<User> Users => GetCollection<User>(typeof(User).Name);

        public IMongoCollection<Author> Authors => GetCollection<Author>(typeof(Author).Name);

        public IMongoCollection<Book> Books => GetCollection<Book>(typeof(Book).Name);

        public IMongoCollection<TDocument> GetCollection<TDocument>(string collectionName) =>
            Database.GetCollection<TDocument>(collectionName);

        public void AddCommand(Func<Task> command) => _commands.Add(command);

        public async Task SaveChangesAsync()
        {
            using (Session = await _client.StartSessionAsync())
            {
                Session.StartTransaction();

                try
                {
                    var commandTasks = _commands.Select(c => c());
                    await Task.WhenAll(commandTasks);

                    await Session.CommitTransactionAsync();
                }
                catch(Exception e)
                {
                    await Session.AbortTransactionAsync();
                    Session.Dispose();

                    throw new MongoTransactionAbortException($"Transaction was aborted because inner error: {e.Message}", e);
                }
                finally
                {
                    _commands.Clear();
                }
            }
        }

        private void SetDatabase(string databaseName)
        {
            var databaseSettings = new MongoDatabaseSettings()
            {
                GuidRepresentation = GuidRepresentation.Standard
            };
            Database = _client.GetDatabase(databaseName, databaseSettings);
        }
    }
}
