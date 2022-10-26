using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

namespace DurableEntityAzureFunctions
{
    public static class CountingJokeFetcher
    {
        private static readonly HttpClient HttpClient = new();

        [FunctionName(nameof(JokeCountFetcherOrchestrator))]
        public static async Task<JokeWithCounts> JokeCountFetcherOrchestrator([OrchestrationTrigger] IDurableOrchestrationContext context, ILogger log)
        {
            log.LogInformation("Fetching a new joke!");
            DurableHttpResponse response = await context.CallHttpAsync(HttpMethod.Get, new Uri($"https://geek-jokes.sameerkumar.website/api?format=text"));
            string joke = response.Content;
            log.LogInformation(joke);

            EntityId jokesCounterEntity = new EntityId(nameof(JokesCounter), "jokesCounter");
            IJokesCounter jokesCounter = context.CreateEntityProxy<IJokesCounter>(jokesCounterEntity);
            
            jokesCounter.Count(joke);

            return new JokeWithCounts
            {
                Joke = joke, 
                Boring = await jokesCounter.GetBoringJokes(), 
                Norris = await jokesCounter.GetNorrisJokes()
            };
        }

        [FunctionName(nameof(GetCountingJoke))]
        public static async Task<HttpResponseMessage> GetCountingJoke(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequestMessage req,
            [DurableClient] IDurableOrchestrationClient starter,
            ILogger log)
        {
            // Function input comes from the request content.
            string instanceId = await starter.StartNewAsync(nameof(JokeCountFetcherOrchestrator));

            log.LogInformation($"Started orchestration with ID = '{instanceId}'.");

            return await starter.WaitForCompletionOrCreateCheckStatusResponseAsync(req, instanceId);
        }
    }
}