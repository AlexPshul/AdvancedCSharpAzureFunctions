using System;
using Microsoft.Azure.WebJobs.Description;

namespace CustomBindingAzureFunctions.GeekJokeBinding;

[AttributeUsage(AttributeTargets.Parameter)]
[Binding]
public class GeekJokeAttribute : Attribute
{
    public string Format { get; set; }
    public string Filter { get; set; }

    public GeekJokeAttribute() { }

    public GeekJokeAttribute(string format, string filter)
    {
        Format = format;
        Filter = filter;
    }
}