using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host.Bindings;
using Microsoft.Extensions.Logging;

namespace CustomBindingAzureFunctions.GeekJokeBinding;

public class GeekJokeInputConverter : IAsyncConverter<GeekJokeAttribute, string>
{
    private const string DefaultFormat = "json";
    private static readonly HashSet<string> SupportedFormats = new(new[] { "json", "text" });

    private readonly HttpClient _httpClient;
    private readonly ILogger<GeekJokeInputConverter> _logger;

    public GeekJokeInputConverter(HttpClient httpClient, ILogger<GeekJokeInputConverter> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<string> ConvertAsync(GeekJokeAttribute input, CancellationToken cancellationToken)
    {
        string format = input.Format;
        string filter = input.Filter;

        if (!SupportedFormats.Contains(format)) format = DefaultFormat;

        _logger.LogInformation($"Getting a joke with format {format} " + (string.IsNullOrEmpty(filter) ? string.Empty : $"and filtering out jokes with {filter}"));

        string result;
        int count = 0;
        do
        {
            _logger.LogInformation($"Attempt #{++count}");
            HttpResponseMessage response = await _httpClient.GetAsync($"https://geek-jokes.sameerkumar.website/api?format={format}", cancellationToken);
            result = await response.Content.ReadAsStringAsync(cancellationToken);
        } while (!string.IsNullOrEmpty(filter) && result.ToLower().Contains(filter.ToLower()));

        return result;
    }
}