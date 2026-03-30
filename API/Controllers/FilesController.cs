using API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]  // This requires authentication for ALL endpoints in this controller
public class FilesController : ControllerBase
{
    private readonly IStorageService _storageService;
    private readonly ILogger<FilesController> _logger;

    public FilesController(IStorageService storageService, ILogger<FilesController> logger)
    {
        _storageService = storageService;
        _logger = logger;
    }


    public class UploadFileRequest
    {
        public IFormFile? file { get; set; }
    }

    /// <summary>
    /// Upload a file to blob storage (Requires Authentication)
    /// </summary>
    [HttpPost("upload")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> UploadFile([FromForm] UploadFileRequest request)
    {
        // Get user info from JWT token
        var userId = User?.Identity?.Name ?? "anonymous";
        _logger.LogInformation($"User {userId} is uploading a file");

        if (request.file == null || request.file.Length == 0)
            return BadRequest(new { Error = "No file uploaded" });

        if (request.file.Length > 5 * 1024 * 1024)
            return BadRequest(new { Error = "File size exceeds 5MB limit" });

        try
        {
            var fileUrl = await _storageService.UploadFileAsync(request.file, "uploads");

            return Ok(new
            {
                Url = fileUrl,
                FileName = request.file.FileName,
                Size = request.file.Length,
                UploadedBy = userId,
                Message = "File uploaded successfully"
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Upload failed");
            return StatusCode(500, new { Error = "Upload failed. Please try again." });
        }
    }

    /// <summary>
    /// Download a file (Requires Authentication)
    /// </summary>
    [HttpGet("download/{blobName}")]
    public async Task<IActionResult> DownloadFile(string blobName)
    {
        try
        {
            var stream = await _storageService.DownloadFileAsync(blobName, "uploads");
            return File(stream, "application/octet-stream", blobName);
        }
        catch (Azure.RequestFailedException ex) when (ex.Status == 404)
        {
            return NotFound(new { Error = "File not found" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Download failed");
            return StatusCode(500, new { Error = "Download failed" });
        }
    }

    /// <summary>
    /// Generate a SAS token (Requires Authentication)
    /// </summary>
    [HttpGet("sas/{blobName}")]
    public IActionResult GetSasToken(string blobName)
    {
        try
        {
            var sasUrl = _storageService.GenerateSasToken(blobName, "uploads");

            return Ok(new
            {
                SasUrl = sasUrl,
                ExpiresIn = "1 hour",
                Note = "This URL provides read-only access to the file"
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "SAS generation failed");
            return NotFound(new { Error = "File not found" });
        }
    }

    /// <summary>
    /// Public health check (no authentication required)
    /// </summary>
    [AllowAnonymous]
    [HttpGet("health")]
    public IActionResult Health()
    {
        return Ok(new { Status = "Healthy", Timestamp = DateTime.UtcNow });
    }
}


