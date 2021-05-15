using BookStore.Dependencies.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BookStore.Dependencies
{
    public static class DependencyContainer
    {
        public static void RegisterServices(IServiceCollection services, IConfiguration config)
        {
            services.AddMongoDataPersistance(config);
            services.AddMongoDataInfrastrucure();
            services.CreateMongoCollectionsIfNotExist(config);
            services.AddBusinessServices();
            services.AddEventBus(config);
            services.AddMapping();
            services.AddSwagger();
        }
    }
}
