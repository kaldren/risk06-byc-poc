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

    // Get list of modules and their metadata
    [HttpGet]
    public IActionResult GetModules()
    {
        var modules = new Module[]
        {
            new Module
            {
                Name = "MathModule",
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

    [HttpPost("onboard")]
    public IActionResult OnboardModule([FromBody] Module module)
    {
        // Register module
        // 1. Add to queue storage
        // 2. Function app processes it and adds it to the system

        return Ok($"Module {module.Name} added successfully.");
    }

    [HttpPost("execute")]
    public async Task<IActionResult> ExecuteModule([FromBody] ExecuteModuleRequest payload)
    {
        // Validate payload

        await _executeModuleService.EnqueueModuleForExecutionAsync(payload);

        return Ok($"Enqueued module {payload.Module} for execution at endpoint {payload.Endpoint}");
    }
}
