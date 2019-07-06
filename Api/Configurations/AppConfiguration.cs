using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace CloudMemos.Api.Configurations
{
    internal static class AppConfiguration
    {
        internal static IWebHostBuilder ConfigureAppConfig(this IWebHostBuilder builder)
        {
            return builder.ConfigureAppConfiguration(
                (hostingContext, configuration) =>
                {
                    var env = hostingContext.HostingEnvironment;
                    configuration.SetBasePath(env.ContentRootPath);
                    configuration.AddJsonFile("appsettings.json", false, true);
                    configuration.AddEnvironmentVariables();
                });
        }
    }
}