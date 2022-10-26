using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using CustomBindingAzureFunctions;
using CustomBindingAzureFunctions.GeekJokeBinding;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Hosting;

[assembly: WebJobsStartup(typeof(Startup))]

namespace CustomBindingAzureFunctions
{
    internal class Startup : IWebJobsStartup
    {
        public void Configure(IWebJobsBuilder builder)
        {
            builder.AddGeekJokeBinding();
        }
    }
}
