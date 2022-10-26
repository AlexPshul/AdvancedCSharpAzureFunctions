using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Newtonsoft.Json;

namespace DurableEntityAzureFunctions;


public interface IJokesCounter
{
    // Proxy Entities don't support properties, only methods that return void, Task or Task<T>
    Task<int> GetNorrisJokes();
    Task<int> GetBoringJokes();
    void Count(string joke);
}

[JsonObject(MemberSerialization.OptIn)]
public class JokesCounter : IJokesCounter
{
    [JsonProperty("norris")]
    public int NorrisJokes { get; set; }

    [JsonProperty("boring")]
    public int BoringJokes { get; set; }

    public Task<int> GetNorrisJokes() => Task.FromResult(this.NorrisJokes);
    public Task<int> GetBoringJokes() => Task.FromResult(this.BoringJokes);

    public void Count(string joke)
    {
        if (joke.ToLower().Contains("chuck"))
            NorrisJokes++;
        else
            BoringJokes++;
    }

    [FunctionName(nameof(JokesCounter))]
    public static Task Run([EntityTrigger] IDurableEntityContext ctx) => ctx.DispatchAsync<JokesCounter>();
}