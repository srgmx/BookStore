using BookStore.Business.Contracts;
using BookStore.Business.Services;
using Microsoft.Extensions.DependencyInjection;

namespace BookStore.API.Extentions
{
    public static class BusinessExtentions
    {
        public static IServiceCollection AddBusinessServices(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAuthorService, AuthorService>();

            return services;
        }
    }
}
