using Client.API.Models;
using Client.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace Client.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ModulesController : ControllerBase
{
    private readonly IExecuteModuleService _executeModuleService;

    // Fake database of modules
    private readonly static ModulesResponse _modules = new ModulesResponse
    {
        Modules = new List<ModuleResponse>
    {
        new ModuleResponse
        {
            Name = "MathModule",
            Version = "1.0.0",
            Description = "Sample module with arithmetic operations.",
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
                    Returns = ReturnType.Integer
                },
                new ModuleEndpoint
                {
                    Name = "multiply_numbers",
                    Params = new Dictionary<string, string>
                    {
                        { "a", "int" },
                        { "b", "int" }
                    },
                    Returns = ReturnType.Integer
                }
            }
        },

        new ModuleResponse
        {
            Name = "TextModule",
            Version = "1.0.0",
            Description = "Processes and transforms text values.",
            Endpoints = new List<ModuleEndpoint>
            {
                new ModuleEndpoint
                {
                    Name = "reverse_text",
                    Params = new Dictionary<string, string>
                    {
                        { "text", "string" }
                    },
                    Returns = ReturnType.String
                },
                new ModuleEndpoint
                {
                    Name = "count_words",
                    Params = new Dictionary<string, string>
                    {
                        { "text", "string" }
                    },
                    Returns = ReturnType.Integer
                }
            }
        },

        new ModuleResponse
        {
            Name = "DateModule",
            Version = "1.0.0",
            Description = "Provides simple date and time utilities.",
            Endpoints = new List<ModuleEndpoint>
            {
                new ModuleEndpoint
                {
                    Name = "get_current_date",
                    Params = new Dictionary<string, string>(),
                    Returns = ReturnType.String
                },
                new ModuleEndpoint
                {
                    Name = "days_between",
                    Params = new Dictionary<string, string>
                    {
                        { "startDate", "string" },
                        { "endDate", "string" }
                    },
                    Returns = ReturnType.Integer
                }
            }
        }
    }
    };


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
        return Ok(_modules);
    }

    // Get module by name
    [HttpGet("{moduleName}")]
    public IActionResult GetModuleByName(string moduleName)
    {
        var module = _modules.Modules.SingleOrDefault(m => m.Name == moduleName);

        if (module == null)
        {
            return NotFound($"Module {moduleName} not found.");
        }

        return Ok(module);
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
