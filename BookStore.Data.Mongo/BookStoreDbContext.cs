using BookStore.Domain;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Conventions;
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
            RegisterConventions();
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
            if (_commands.Count() == 0)
            {
                return;
            }

            using (Session = await _client.StartSessionAsync())
            {
                var transactionResponse = await Session.WithTransactionAsync(async (session, cancelToken) =>
                {
                    try
                    {
                        var commandTasks = _commands.Select(command => command());
                        await Task.WhenAll(commandTasks);
                        await session.CommitTransactionAsync();
                        var response = new MongoTransactionResponse()
                        {
                            Mesage = "Transaction operations were completed.",
                            IsSuccess = true
                        };

                        return response;
                    }
                    catch (Exception e)
                    {
                        await session.AbortTransactionAsync();
                        var response = new MongoTransactionResponse()
                        {
                            Mesage = e.Message,
                            IsSuccess = false,
                            Exception = e
                        };

                        return response;
                    }
                    finally
                    {
                        _commands.Clear();
                    }
                });

                if (!transactionResponse.IsSuccess)
                {
                    Session.Dispose();

                    throw new MongoTransactionAbortException(transactionResponse.Mesage, transactionResponse.Exception);
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

        private void RegisterConventions()
        {
            var conventionName = "BookStoreDbConventions";
            var conventionPack = new ConventionPack
            {
                new CamelCaseElementNameConvention(),
                new IgnoreIfNullConvention(true)
            };
            ConventionRegistry.Register(conventionName, conventionPack, t => true);
        }
    }
}
