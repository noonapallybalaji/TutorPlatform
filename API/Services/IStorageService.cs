namespace API.Services;

public interface IStorageService
{
    Task<string> UploadFileAsync(IFormFile file, string containerName);
    Task<Stream> DownloadFileAsync(string blobName, string containerName);
    string GenerateSasToken(string blobName, string containerName);
}