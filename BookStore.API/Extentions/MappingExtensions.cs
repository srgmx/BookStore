using AutoMapper;
using BookStore.Business.Mapping;
using Microsoft.Extensions.DependencyInjection;

namespace BookStore.API.Extensions
{
    public static class MappingExtensions
    {
        public static IServiceCollection AddMapping(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(MappingProfile));

            return services;
        }
    }
}
