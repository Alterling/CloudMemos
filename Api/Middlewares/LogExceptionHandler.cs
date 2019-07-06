using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CloudMemos.Api.Middlewares
{
    public static class LogExceptionHandler
    {
        public static void UseExceptionHandlerLog(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(
                appBuilder => appBuilder.Run(
                    async context =>
                    {
                        var exceptionHandlerFeature = context.Features.Get<IExceptionHandlerFeature>();
                        if (exceptionHandlerFeature != null)
                        {
                            var logger = app.ApplicationServices.GetService<ILogger<LambdaEntryPoint>>();
                            logger.LogError(exceptionHandlerFeature.Error, exceptionHandlerFeature.Error.Message);
                        }

                        context.Response.StatusCode = 500;
                        await context.Response.WriteAsync("An unexpected fault happened. Try again later. ");
                    }));
        }
    }
}