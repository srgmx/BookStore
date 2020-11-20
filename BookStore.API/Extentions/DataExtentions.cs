using BookStore.Data.Contracts;
using BookStore.Data.Infrastructure;
using BookStore.Data.Persistance;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BookStore.API.Extentions
{
    public static class DataExtentions
    {
        public static IServiceCollection AddDatabaseContexts(this IServiceCollection services, IConfiguration _config)
        {
            services.AddDbContext<BookStoreDbContext>(o =>
                o.UseSqlServer(_config.GetConnectionString("BookStoreDb")));

            return services;
        } 

        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IAuthorRepository, AuthorRepository>();
            services.AddScoped<IUserRepository, UserRepository>();

            return services;
        }
    }
}
