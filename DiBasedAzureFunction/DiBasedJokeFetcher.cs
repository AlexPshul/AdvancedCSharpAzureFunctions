using System.Threading.Tasks;
using DiBasedAzureFunction.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;

namespace DiBasedAzureFunction;

public class DiBasedJokeFetcher
{
    private readonly IGeekJokeService _geekJokeService;

    public DiBasedJokeFetcher(IGeekJokeService geekGeekJokeService)
    {
        _geekJokeService = geekGeekJokeService;
    }

    [FunctionName(nameof(GetJoke))]
    public Task<string> GetJoke(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)]
        HttpRequest req)
    {
        return _geekJokeService.GetNew(req.Query["format"], req.Query["filter"]);
    }
}