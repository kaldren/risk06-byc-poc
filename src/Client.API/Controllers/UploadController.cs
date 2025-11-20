using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Client.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UploadController : ControllerBase
{
    [HttpPost("file")]
    [SwaggerOperation(
        Summary = "Uploads a file and returns its metadata.",
        Description = "Uploads a binary file and returns its name, content type, and size in bytes.",
        OperationId = $"{nameof(UploadFile)}"
    )]
    public async Task<IActionResult> UploadFile(
        IFormFile file)
    {
        if (file == null)
            return BadRequest("No file uploaded.");

        // Read binary content
        using var ms = new MemoryStream();

        await file.CopyToAsync(ms);

        byte[] bytes = ms.ToArray();

        return Ok(new
        {
            fileName = file.FileName,
            contentType = file.ContentType,
            size = bytes.Length
        });
    }
}
