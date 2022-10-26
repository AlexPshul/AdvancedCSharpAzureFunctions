using CustomBindingAzureFunctions.GeekJokeBinding;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;

namespace CustomBindingAzureFunctions
{
    public class CustomBindingSimple
    {
        [FunctionName(nameof(GetGeekJoke))]
        public string GetGeekJoke(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)]
            HttpRequest req,
            [GeekJoke(Filter="chuck", Format = "text")] string joke) => joke;
    }
}
