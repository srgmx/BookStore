using API.Middleware;
using BookStore.API.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace BookStore.API
{
    public class Startup
    {
        private readonly IConfiguration _config;

        public Startup(IWebHostEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);
            _config = builder.Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDataPersistance(_config);
            services.AddDataInfrastrucure();
            services.AddBusinessServices();
            services.AddMapping();
            services.AddSwagger();
            services.AddControllers();
            services.AddMongoDataPersistance(_config);
            services.AddMongoDataInfrastrucure();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionsMiddleware>();
            app.UseHttpsRedirection();
            app.UseSerilogRequestLogging();
            app.UseSwaggerTooling();
            app.UseRouting();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}
