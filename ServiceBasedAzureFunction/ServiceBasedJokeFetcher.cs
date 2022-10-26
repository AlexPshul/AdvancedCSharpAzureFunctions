using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using ServiceBasedAzureFunction.Services;

namespace ServiceBasedAzureFunction
{
    public static class ServiceBasedJokeFetcher
    {
        private static readonly HttpClient HttpClient = new();
        
        [FunctionName(nameof(GetJoke))]
        public static Task<string> GetJoke(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            GeekJokeService service = new GeekJokeService(HttpClient, log);

            return service.GetNew(req.Query["format"], req.Query["filter"]);
        }
    }
}
