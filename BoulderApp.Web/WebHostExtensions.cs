using BoulderApp.Web.Types;
using log4net;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace BoulderApp.Web
{
    public static class WebHostExtensions
    {
        public static IWebHost Seed(this IWebHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var serviceProvider = scope.ServiceProvider;

                try
                {
                    Task.Run(async () => await DbInitialiser.InitialiseAsync(serviceProvider.GetService<BoulderAppContext>()))
                        .Wait();
                }
                catch (Exception ex)
                {
                    var logger = serviceProvider.GetRequiredService<ILog>();
                    logger.Error("An error occurred seeding the DB.", ex);
                }
            }
            return host;
        }

    }
}
