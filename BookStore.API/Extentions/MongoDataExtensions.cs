using BookStore.Data.Abstraction;
using BookStore.Data.Mongo;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Driver;

namespace BookStore.API.Extensions
{
    public static class MongoDataExtensions
    {

        public static IServiceCollection AddMongoDataPersistance(this IServiceCollection services, IConfiguration _config)
        {
            BsonDefaults.GuidRepresentation = GuidRepresentation.Standard;

            services.AddSingleton<IMongoClient>(s => 
                new MongoClient(_config.GetConnectionString("MongoBookStoreDb")));
            services.AddScoped(s => 
                new BookStoreDbContext(s.GetRequiredService<IMongoClient>(), _config["MongoBookStoreDbName"]));

            return services;
        }

        public static IServiceCollection AddMongoDataInfrastrucure(this IServiceCollection services)
        {
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            return services;
        }
    }
}
