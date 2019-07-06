using CloudMemos.Api.Controllers.V1.Examples;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.Filters;

namespace CloudMemos.Api.Configurations
{
    internal static class SwaggerConfiguration
    {
        internal static IServiceCollection AddSwagger(this IServiceCollection services)
        {
            var authConfiguration = services.BuildServiceProvider().GetService<IConfiguration>().GetSection("Authentication");
            services.AddSwaggerGen(
                options =>
                {
                    options.SwaggerDoc(
                        "v1",
                        new Info { Title = "CloudMemos.Api", Description = "With proper authorization you can read and update CloudMemos.", Version = "v1" });
                    options.DescribeAllEnumsAsStrings();
                    options.DescribeStringEnumsInCamelCase();
                    options.ExampleFilters();
                });
            services.AddSwaggerExamplesFromAssemblyOf<CreateProductRequestExample>();
            return services;
        }

        internal static IApplicationBuilder UseSwaggerEndpoint(this IApplicationBuilder app)
        {
            app.UseSwagger().UseSwaggerUI(options => options.SwaggerEndpoint("/swagger/v1/swagger.json", "CloudMemos.Api"));
            return app;
        }
    }
}