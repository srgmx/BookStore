using API.Middleware;
using BookCoreLibrary.EventBus.Core;
using BookStore.Dependencies;
using BookStore.Dependencies.Extensions;
using BookStore.Domain.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Serilog;

namespace BookStore.API
{
    public class Startup
    {
        private readonly IConfiguration _config;
        private IEventBus _eventBus;

        public Startup(IWebHostEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();
            _config = builder.Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            DependencyContainer.RegisterServices(services, _config);
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(
            IApplicationBuilder app,
            IWebHostEnvironment env,
            IHostApplicationLifetime lifetime)
        {
            lifetime.ApplicationStopping.Register(OnStopping);
            app.UseMiddleware<ExceptionsMiddleware>();
            app.UseHttpsRedirection();
            app.UseSerilogRequestLogging();
            app.UseSwaggerTooling();
            app.UseRouting();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
            ConfigureSubscriptions(app);
        }

        private void ConfigureSubscriptions(IApplicationBuilder app)
        {
            _eventBus = app.ApplicationServices.GetService<IEventBus>();
            var queueDestinations = app.ApplicationServices
                .GetService<IOptions<RabbitMqConsumingConfiguration>>().Value;
            _eventBus.ConfigureSubscriptions(queueDestinations);
        }

        private void OnStopping()
        {
            _eventBus.UnsubscribeAll();
        }
    }
}
