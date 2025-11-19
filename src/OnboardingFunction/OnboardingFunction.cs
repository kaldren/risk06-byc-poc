using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using OnboardingFunction.Services;

namespace OnboardingFunction;

public class OnboardingFunction
{
    private readonly ILogger<OnboardingFunction> _logger;
    private readonly IOnboardingService _onboardingService;

    public OnboardingFunction(ILogger<OnboardingFunction> logger, IOnboardingService onboardingService)
    {
        _logger = logger;
        _onboardingService = onboardingService;
    }

    [Function("Onboard")]
    public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequest req, Manifest manifest)
    {
        manifest = new Manifest
        {
            Client = "ClientA",
            OpenApiUrl = "https://app-cac-risk-dev-aabqced0e2dshhd6.canadacentral-01.azurewebsites.net/swagger/v1/swagger.json"
        };

        await _onboardingService.StartOnboardingAsync(manifest);
        return new OkObjectResult($"Onboarding started for {manifest.Client}.");
    }

    // Blob trigger that takes python script , opens it, and returns some output
    [Function(nameof(RunBlob))]
    public async Task RunBlob([BlobTrigger("scripts/{name}", Connection = "AzureWebJobsStorage")] Stream script, string name)
    {
        await _onboardingService.StartScriptOnboardingAsync(script);
    }
}