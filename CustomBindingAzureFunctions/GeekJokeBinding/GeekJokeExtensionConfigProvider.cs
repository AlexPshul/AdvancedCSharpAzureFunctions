using Microsoft.Azure.WebJobs.Host.Config;

namespace CustomBindingAzureFunctions.GeekJokeBinding;

public class GeekJokeExtensionConfigProvider : IExtensionConfigProvider
{
    private readonly GeekJokeInputConverter _geekJokeInputConverter;

    public GeekJokeExtensionConfigProvider(GeekJokeInputConverter geekJokeInputConverter)
    {
        _geekJokeInputConverter = geekJokeInputConverter;
    }

    public void Initialize(ExtensionConfigContext context)
    {
        var rule = context.AddBindingRule<GeekJokeAttribute>();

        rule.BindToInput(_geekJokeInputConverter);
    }
}