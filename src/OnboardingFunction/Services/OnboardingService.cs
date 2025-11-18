using System.Net;
using Azure;
using Azure.Core;
using Azure.Identity;
using Azure.ResourceManager;
using Azure.ResourceManager.ApiManagement;
using Azure.ResourceManager.ApiManagement.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using OnboardingFunction.Configuration;

namespace OnboardingFunction.Services;
internal class OnboardingService : IOnboardingService
{
    private readonly ApiManagementSettings _apimSettings;

    public OnboardingService(IOptions<ApiManagementSettings> apimSettings)
    {
        _apimSettings = apimSettings.Value;
    }

    public async Task StartOnboardingAsync(Manifest manifest)
    {
        // Basic guard
        if (manifest is null) throw new ArgumentNullException(nameof(manifest));
        if (string.IsNullOrEmpty(manifest.Client)) throw new ArgumentException("Client is required", nameof(manifest));
        if (string.IsNullOrWhiteSpace(manifest.OpenApiUrl)) throw new ArgumentException("OpenApiUrl is required", nameof(manifest));

        string subscriptionId = _apimSettings.SubscriptionId ?? throw new InvalidOperationException("Subscription ID is not configured.");
        string resourceGroup = _apimSettings.ResourceGroup ?? throw new InvalidOperationException("Resource Group is not configured.");
        string apimName = _apimSettings.ServiceName ?? throw new InvalidOperationException("API Management Service Name is not configured.");

        // Authenticate with DefaultAzureCredential (supports MSI when deployed)
        TokenCredential credential = new DefaultAzureCredential();
        ArmClient arm = new ArmClient(credential);

        // Get APIM service resource
        var apimId = ApiManagementServiceResource.CreateResourceIdentifier(subscriptionId, resourceGroup, apimName);
        ApiManagementServiceResource apim = arm.GetApiManagementServiceResource(apimId);

        // Validate APIM exists
        try
        {
            _ = await apim.GetAsync();
        }
        catch (RequestFailedException ex) when (ex.Status == 404)
        {
            throw new InvalidOperationException($"API Management service '{apimName}' not found in resource group '{resourceGroup}'.", ex);
        }

        // Create a unique API ID from client
        string apiId = (manifest.Client ?? "client").ToLowerInvariant().Replace(" ", "-");
        if (string.IsNullOrWhiteSpace(apiId)) throw new ArgumentException("Client name results in invalid API ID.", nameof(manifest));

        ApiCollection apis = apim.GetApis();

        // Import from OpenAPI URL
        var importContent = new ApiCreateOrUpdateContent
        {
            DisplayName = manifest.Client,
            Path = apiId,
            Format = ContentFormat.OpenApiLink,
            Value = manifest.OpenApiUrl,
        };

        // If API exists, we update (create or update is idempotent)
        ArmOperation<ApiResource> op = await apis.CreateOrUpdateAsync(WaitUntil.Completed, apiId, importContent);
        _ = op.Value;

        // Optionally set a revision to 1 if newly created
        // No-op: API ARM will create as revision 1 by default.
    }
}
