using AutoMapper;
using BookStore.Business.Mapping;
using Microsoft.Extensions.DependencyInjection;

namespace BookStore.API.Extentions
{
    public static class MappingExtentions
    {
        public static IServiceCollection AddMapping(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(MappingProfile));

            return services;
        }
    }
}
