using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net.Http;

namespace BasicAzureFunctions
{
    public static class BasicJokeFetcher
    {
        public const string DefaultFormat = "json";
        private static readonly HashSet<string> SupportedFormats = new(new[] { "json", "text" });
        private static readonly HttpClient HttpClient = new();

        [FunctionName(nameof(GetJoke))]
        public static async Task<string> GetJoke(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            string format = (string)req.Query["format"] ?? DefaultFormat;
            string filter = req.Query["filter"];
            
            log.LogInformation($"Getting a joke with format {format} " + (string.IsNullOrEmpty(filter) ? string.Empty : $"and filtering out jokes with {filter}"));

            if (!SupportedFormats.Contains(format)) format = DefaultFormat;

            string result;
            int count = 0;
            do
            {
                log.LogInformation($"Attempt #{++count}");
                HttpResponseMessage response =
                    await HttpClient.GetAsync($"https://geek-jokes.sameerkumar.website/api?format={format}");
                result = await response.Content.ReadAsStringAsync();
            } while (!string.IsNullOrEmpty(filter) && result.ToLower().Contains(filter.ToLower()));

            return result;
        }
    }
}
