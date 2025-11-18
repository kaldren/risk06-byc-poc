using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OnboardingFunction.Configuration;
using OnboardingFunction.Services;

var builder = FunctionsApplication.CreateBuilder(args);

builder.Services.AddOptions<ApiManagementSettings>()
    .BindConfiguration("ApiManagement")
    .ValidateDataAnnotations();

builder.ConfigureFunctionsWebApplication();

builder.Services
    .AddApplicationInsightsTelemetryWorkerService()
    .ConfigureFunctionsApplicationInsights();

builder.Services.AddTransient<IOnboardingService, OnboardingService>();

builder.Build().Run();
