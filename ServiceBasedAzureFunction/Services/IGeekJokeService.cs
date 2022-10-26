using System.Threading.Tasks;

namespace ServiceBasedAzureFunction.Services;

public interface IGeekJokeService
{
    Task<string> GetNew(string format, string filterOutString);
}