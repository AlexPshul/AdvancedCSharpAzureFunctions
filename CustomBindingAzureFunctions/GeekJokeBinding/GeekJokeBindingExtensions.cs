using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.DependencyInjection;

namespace CustomBindingAzureFunctions.GeekJokeBinding;

public static class GeekJokeBindingExtensions
{
    public static IWebJobsBuilder AddGeekJokeBinding(this IWebJobsBuilder builder)
    {
        builder.AddExtension<GeekJokeExtensionConfigProvider>();

        builder.Services.AddHttpClient();
        builder.Services.AddSingleton<GeekJokeInputConverter>();

        return builder;
    }
}