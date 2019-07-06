using CloudMemos.Api.Configurations;
using Amazon.Lambda.AspNetCoreServer;
using Microsoft.AspNetCore.Hosting;

namespace CloudMemos.Api
{
    /// <inheritdoc />
    /// <summary>
    ///     This class extends from APIGatewayProxyFunction which contains the method FunctionHandlerAsync which is the
    ///     actual Lambda function entry point. The Lambda handler field should be set to
    ///     CloudMemos.Api::CloudMemos.Api.LambdaEntryPoint::FunctionHandlerAsync
    /// </summary>
    public class LambdaEntryPoint : APIGatewayProxyFunction
    {
        protected override void Init(IWebHostBuilder builder)
        {
            builder.ConfigureAppConfig().UseStartup<Startup>();
        }
    }
}