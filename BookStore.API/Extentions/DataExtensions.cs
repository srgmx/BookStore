using BookStore.Data.Abstraction;
using BookStore.Data.MSSQL.Infrastructure;
using BookStore.Data.MSSQL.Persistance;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BookStore.API.Extensions
{
    public static class DataExtensions
    {
        public static IServiceCollection AddDataPersistance(this IServiceCollection services, IConfiguration _config)
        {
            services.AddDbContext<BookStoreDbContext>(o =>
                o.UseSqlServer(_config.GetConnectionString("MSSqlBookStoreDb")));

            return services;
        } 

        public static IServiceCollection AddDataInfrastrucure(this IServiceCollection services)
        {
            services.AddScoped<IAuthorRepository, AuthorRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IBookRepopository, BookRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}
