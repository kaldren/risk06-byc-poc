using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Client.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ExecuteController : ControllerBase
{
    [HttpPost]
    public IActionResult Execute([FromBody] Dictionary<string, object> payload)
    {
        var op = Request.Headers["x-module-id"].ToString();

        if (string.IsNullOrWhiteSpace(op))
            return BadRequest("Missing APIM operation header.");

        var json = JsonSerializer.Serialize(payload);

        switch (op.ToLower())
        {
            case "add":
                return Ok($"Executed {op} module with the following payload {json}");

            case "subtract":
                return Ok($"Executed {op} module with the following payload {json}");

            default:
                return BadRequest($"Unknown operation: {op}");
        }
    }
}
