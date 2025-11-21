using Azure.Identity;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Mvc;

namespace Client.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FilesController : ControllerBase
{
    private readonly BlobServiceClient _blobServiceClient;
    private readonly ILogger<FilesController> _logger;

    public FilesController(ILogger<FilesController> logger)
    {
        _logger = logger;
        _blobServiceClient = new BlobServiceClient(
            new Uri("https://stokalatariskdev.blob.core.windows.net"),
            new ManagedIdentityCredential());
    }

    [HttpPost("upload")]
    public async Task<IActionResult> Upload([FromBody] UploadFileRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.ContentBytes))
            return BadRequest("contentBytes is missing");

        if (string.IsNullOrWhiteSpace(request.Name))
            return BadRequest("name is missing");

        // Handle possible data:image/png;base64,...
        var base64 = request.ContentBytes.Contains(',')
            ? request.ContentBytes.Split(',').Last()
            : request.ContentBytes;

        byte[] fileBytes;

        try
        {
            fileBytes = Convert.FromBase64String(base64);
        }
        catch
        {
            return BadRequest("Invalid base64 content");
        }

        var container = _blobServiceClient.GetBlobContainerClient("uploadedfiles");
        await container.CreateIfNotExistsAsync();

        var blob = container.GetBlobClient(request.Name);

        using var stream = new MemoryStream(fileBytes);
        await blob.UploadAsync(stream, overwrite: true);

        return Ok(new
        {
            FileName = request.Name,
            Size = fileBytes.Length,
            Status = "Uploaded"
        });
    }
}

// Models that match Copilot Studio's payload structure
public class UploadFileRequest
{
    public string ContentBytes { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
}