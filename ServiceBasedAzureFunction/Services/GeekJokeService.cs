using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace ServiceBasedAzureFunction.Services;

public class GeekJokeService : IGeekJokeService
{
    private const string DefaultFormat = "json";
    private static readonly HashSet<string> SupportedFormats = new(new[] { "json", "text" });

    private readonly HttpClient _httpClient;
    private readonly ILogger _logger;

    public GeekJokeService(HttpClient httpClient, ILogger logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<string> GetNew(string format, string filterOutString)
    {
        _logger.LogInformation($"Getting a joke with format {format} " + (string.IsNullOrEmpty(filterOutString) ? string.Empty : $"and filtering out jokes with {filterOutString}"));
        
        if (!SupportedFormats.Contains(format)) 
            format = DefaultFormat;

        string result;
        int count = 1;
        do
        {
            _logger.LogInformation($"Attempt #{count++}");
            HttpResponseMessage response =
                await _httpClient.GetAsync($"https://geek-jokes.sameerkumar.website/api?format={format}");
            result = await response.Content.ReadAsStringAsync();
        } while (!string.IsNullOrEmpty(filterOutString) && result.ToLower().Contains(filterOutString.ToLower()));

        return result;
    }
}