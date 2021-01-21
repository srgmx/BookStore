using BookStore.Data.Abstraction;
using BookStore.Data.Mongo;
using BookStore.Domain;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using System.Linq;

namespace BookStore.API.Extensions
{
    public static class MongoDataExtensions
    {
        private const string ConnectionStringSection = "MongoBookStoreDb";
        private const string DatabaseNameSection = "MongoConfiguration:MongoBookStoreDbName";
        private const string UserSection = "MongoConfiguration:MongoBookStoreDbUser";
        private const string PasswordSection = "MongoConfiguration:MongoBookStoreDbPassword";

        public static IServiceCollection AddMongoDataPersistance(this IServiceCollection services, IConfiguration config)
        {
            services.AddSingleton<IMongoClient>(s => new MongoClient(ConstructConnectionString(config)));
            services.AddScoped(s => new BookStoreDbContext(s.GetRequiredService<IMongoClient>(), config.GetValue<string>(DatabaseNameSection)));

            return services;
        }

        public static IServiceCollection AddMongoDataInfrastrucure(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IAuthorRepository, AuthorRepository>();
            services.AddScoped<IBookRepopository, BookRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }

        public static IServiceCollection CreateMongoCollectionsIfNotExist(this IServiceCollection services, IConfiguration config)
        {
            var client = new MongoClient(ConstructConnectionString(config));
            var database = client.GetDatabase(config.GetValue<string>(DatabaseNameSection));
            var existingCollections = database.ListCollectionNames().ToList();

            string userCollectionName = typeof(User).Name;
            string authorCollectionName = typeof(Author).Name;
            string bookCollectionName = typeof(Book).Name;

            if (!existingCollections.Any(name => name == userCollectionName))
            {
                database.CreateCollection(userCollectionName);
            }

            if (!existingCollections.Any(name => name == authorCollectionName))
            {
                database.CreateCollection(authorCollectionName);
            }

            if (!existingCollections.Any(name => name == bookCollectionName))
            {
                database.CreateCollection(bookCollectionName);
            }

            return services;
        }

        private static string ConstructConnectionString(IConfiguration config)
        {
            var connectionStringPattern = config.GetConnectionString(ConnectionStringSection);

            return string.Format(
                connectionStringPattern,
                config.GetValue<string>(UserSection),
                config.GetValue<string>(PasswordSection),
                config.GetValue<string>(DatabaseNameSection));
        }
    }
}
