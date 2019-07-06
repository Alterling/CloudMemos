using CloudMemos.Api.Filters;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace CloudMemos.Api.Configurations
{
    internal static class MvcConfiguration
    {
        internal static IServiceCollection AddMvcWithOptions(this IServiceCollection services)
        {
            services.AddMvc(
                        options =>
                        {
                            options.Filters.Add(typeof(RequiresValidModelFilter));
                        })
                    .AddJsonOptions(
                        options =>
                        {
                            options.SerializerSettings.Converters.Add(new StringEnumConverter { CamelCaseText = true });
                            options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                            options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                        });
            return services;
        }
    }
}