using System.Threading.Tasks;

namespace DiBasedAzureFunction.Services;

public interface IGeekJokeService
{
    Task<string> GetNew(string format, string filterOutString);
}