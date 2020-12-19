using BookStore.Business.Contracts;
using BookStore.Business.Services;
using Microsoft.Extensions.DependencyInjection;

namespace BookStore.API.Extensions
{
    public static class BusinessExtensions
    {
        public static IServiceCollection AddBusinessServices(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUserPermissionsService, UserPermissionsService>();
            services.AddScoped<IAuthorService, AuthorService>();
            services.AddScoped<IBookService, BookService>();

            return services;
        }
    }
}
