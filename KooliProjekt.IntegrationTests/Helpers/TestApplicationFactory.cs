using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System.IO;

namespace KooliProjekt.IntegrationTests.Helpers
{
    public class TestApplicationFactory<TTestStartup> : WebApplicationFactory<TTestStartup> where TTestStartup : class
    {
        protected override IHostBuilder CreateHostBuilder()
        {
            var host = Host.CreateDefaultBuilder()
                            .ConfigureWebHost(builder =>
                            {
                                builder.UseContentRoot(Directory.GetCurrentDirectory());
                                builder.ConfigureAppConfiguration((context, config) =>
                                {
                                    config.AddJsonFile("appsettings.json"); // Ensure configuration is loaded
                                });

                                builder.UseStartup<TTestStartup>(); // Use the actual startup class or test one
                            });

            return host;
        }
    }
}
