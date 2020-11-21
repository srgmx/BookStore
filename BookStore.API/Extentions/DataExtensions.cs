using BookStore.Data.Contracts;
using BookStore.Data.Infrastructure;
using BookStore.Data.Persistance;
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
                o.UseSqlServer(_config.GetConnectionString("BookStoreDb")));

            return services;
        } 

        public static IServiceCollection AddDataInfrastrucure(this IServiceCollection services)
        {
            services.AddScoped<IAuthorRepository, AuthorRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IBookStoreUnitOfWork, BookStoreUnitOfWork>();

            return services;
        }
    }
}
