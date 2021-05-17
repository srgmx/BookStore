using Serilog;
using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using BookCoreLibrary.CorrelationId;

namespace BookStore.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                Log.Information("BookStore.API is starting up...");
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception e)
            {
                Log.Fatal(e, "BookStore.API failed on the start-up");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog((hostContext, loggerConfiguration) => 
                {
                    loggerConfiguration.ReadFrom.Configuration(hostContext.Configuration)
                        .Enrich.WithCorrelationIdHeader(CorrelationIdContext.Header);
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
