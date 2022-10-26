using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

namespace DurableAzureFunctions
{
    public static class DurableJokeFetcher
    {
        private static readonly HttpClient HttpClient = new();

        [FunctionName(nameof(JokeFetcherOrchestrator))]
        public static async Task<string[]> JokeFetcherOrchestrator(
            [OrchestrationTrigger] IDurableOrchestrationContext context)
        {
            List<Task<string>> jokeTasks = Enumerable.Range(0, 10).Select(_ => context.CallActivityAsync<string>(nameof(JokeFetchActivity), null)).ToList();

            string[] allJokes = await Task.WhenAll(jokeTasks);

            return allJokes;
        }

        [FunctionName(nameof(JokeFetchActivity))]
        public static async Task<string> JokeFetchActivity([ActivityTrigger] IDurableActivityContext context, ILogger log)
        {
            log.LogInformation("Fetching a new joke!");
            HttpResponseMessage response = await HttpClient.GetAsync($"https://geek-jokes.sameerkumar.website/api?format=text");
            return await response.Content.ReadAsStringAsync();
        }

        [FunctionName(nameof(GetJokesDurable))]
        public static async Task<string> GetJokesDurable(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequestMessage req,
            [DurableClient] IDurableOrchestrationClient starter,
            ILogger log)
        {
            string instanceId = await starter.StartNewAsync(nameof(JokeFetcherOrchestrator));
            
            log.LogInformation($"Started orchestration with ID = '{instanceId}'.");

            HttpResponseMessage response = await starter.WaitForCompletionOrCreateCheckStatusResponseAsync(req, instanceId);
            string[] allJokes = await response.Content.ReadAsAsync<string[]>();

            return string.Join(Environment.NewLine, allJokes);
        }
    }
}