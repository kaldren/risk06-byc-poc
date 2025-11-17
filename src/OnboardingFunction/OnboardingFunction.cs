using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace OnboardingFunction;

public class OnboardingFunction
{
    private readonly ILogger<OnboardingFunction> _logger;

    public OnboardingFunction(ILogger<OnboardingFunction> logger)
    {
        _logger = logger;
    }

    [Function("Onboard")]
    public IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequest req, Manifest manifest)
    {
        _logger.LogInformation("C# HTTP trigger function processed a request.");
        return new OkObjectResult("Welcome to Azure Functions!");
    }
}