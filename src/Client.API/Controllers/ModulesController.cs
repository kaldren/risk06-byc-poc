using Client.API.Models;
using Client.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace Client.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ModulesController : ControllerBase
{
    private readonly IExecuteModuleService _executeModuleService;

    public ModulesController(IExecuteModuleService executeModuleService)
    {
        _executeModuleService = executeModuleService;
    }

    // Onboard a new module
    [HttpPost("onboard")]
    public IActionResult OnboardModule([FromBody] ModuleManifest manifest)
    {
        // The manifest contains metadata about the module being onboarded.
        // E.g., name, version, description, endpoints, parameters, return types.

        // Prerequisites:
        // The module is packaged as a ZIP file and uploaded to blob storage with the same name as manifest.Name
        // (this can happen in this method as well where we have UI that requires ZIP file and manifest together)

        // Business logic:
        // Validate manifest
        // Insert the manifest into the database against the module record

        return Ok($"Module {manifest.Name} onboarded successfully.");
    }

    // Get list of modules and their metadata
    [HttpGet]
    public IActionResult GetModules()
    {
        // In a real implementation, this data would come from a database

        var modules = new ModuleResponse[]
        {
            new ModuleResponse
            {
                Name = "MathModule", // This module needs to exist in the system as ZIP with the same name (blob storage)
                Version = "1.0.0",
                Description = "Sample module with arithmetic and string operations.",
                Endpoints = new List<ModuleEndpoint>
                {
                    new ModuleEndpoint
                    {
                        Name = "add_numbers",
                        Params = new Dictionary<string, string>
                        {
                            { "a", "int" },
                            { "b", "int" }
                        },
                        Returns = ReturnType.Dict
                    },
                    new ModuleEndpoint
                    {
                        Name = "multiply_numbers",
                        Params = new Dictionary<string, string>
                        {
                            { "a", "int" },
                            { "b", "int" }
                        },
                        Returns = ReturnType.Dict
                    },
                    new ModuleEndpoint
                    {
                        Name = "reverse_text",
                        Params = new Dictionary<string, string>
                        {
                            { "text", "string" }
                        },
                        Returns = ReturnType.Dict
                    }
                }
            }
        };

        return Ok(modules);
    }

    // Get module by name
    [HttpGet]
    public IActionResult GetModuleByName(string module)
    {
        throw new NotImplementedException();
    }

    // Execute a module endpoint
    [HttpPost("execute")]
    public async Task<IActionResult> ExecuteModule([FromBody] ExecuteModuleRequest payload)
    {
        // Enqueue the module execution request so that it can be processed asynchronously by a Function App (Python executor)
        await _executeModuleService.EnqueueModuleForExecutionAsync(payload);

        return Ok($"Enqueued module {payload.Module} for execution at endpoint {payload.Endpoint}");
    }
}
