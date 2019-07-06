using System.Linq;
using CloudMemos.Api.Configurations;
using CloudMemos.Api.Middlewares;
using CloudMemos.Logic.BusinessLogic;
using CloudMemos.Logic.DataAccess;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CloudMemos.Api
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration config)
        {
            _configuration = config;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSwagger();
            services.AddMemoryCache();
            services.AddMvcWithOptions();

            RegisterServices(services);

            services.Configure<ApiBehaviorOptions>(options => options.InvalidModelStateResponseFactory = InvalidModelStateResponseFactory);
        }

        private static IActionResult InvalidModelStateResponseFactory(ActionContext context)
        {
            var errors = context.ModelState.Values
                .SelectMany(x => x.Errors.Select(p => string.IsNullOrEmpty(p.ErrorMessage) ? p.Exception?.Message : p.ErrorMessage))
                .ToList();
            var result = new { Message = "Validation errors", Errors = errors };
            return new BadRequestObjectResult(result);
        }

        public void Configure(IApplicationBuilder app, IApplicationLifetime appLifetime)
        {
            var services = app.ApplicationServices;

            var loggerFactory = services.GetRequiredService<ILoggerFactory>();
            loggerFactory.AddLambdaLogger(_configuration.GetLambdaLoggerOptions());

            app.UseExceptionHandlerLog();
            app.UseSwaggerEndpoint();
            app.UseMvc();
            app.Use(async (context, next) =>
            {
                if (context.Request.Path == "/")
                {
                    context.Response.Redirect("swagger/", true);
                    return;
                }
                await next();
            });
        }

        private void RegisterServices(IServiceCollection services)
        {
            services.AddSingleton(_configuration);
            //services.AddSingleton<IMemoRepository, MemoRepository>();
            services.AddSingleton<IMemoRepository, LocalStaticMemoRepository>();
            services.AddSingleton<INewIdGenerator, NewIdGenerator>();
            services.AddSingleton<ITextStatisticsCalculator, TextStatisticsCalculator>();
            services.AddSingleton<IMemoManagerV1, MemoManagerV1>();
            services.AddSingleton<IMemoManagerV2, MemoManagerV2>();
            //services.AddSingleton<ITokenProvider>(
            //    provider =>
            //    {
            //        var configuration = provider.GetService<IConfiguration>();
            //        var authSection = configuration.GetSection("Authentication");
            //        string endpoint = authSection.GetValue<string>("TokenEndpointUrl");
            //        string clientId = authSection.GetValue<string>("AuthenticationApiClientId");
            //        string clientSecret = authSection.GetValue<string>("AuthenticationApiClientSecret");
            //        string scope = authSection.GetValue<string>("AuthenticationApiClientScope");
            //        var loggingConfig = new HttpClientLoggingConfiguration(
            //            HttpClientLoggingConfiguration.DefaultAllowedHeaders,
            //            msg => true,
            //            msg => false,
            //            msg => true,
            //            msg => false);

            //        return new TokenProvider(endpoint, clientId, clientSecret, scope, loggingConfig);
            //    });
        }
    }
}