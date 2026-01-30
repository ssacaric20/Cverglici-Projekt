using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Options;
using SmartMenza.Business.Services.Interfaces;
using SmartMenza.Core.Settings;

namespace SmartMenza.Business.Services
{
    public sealed class AzureBlobImageService : IImageService
    {
        private readonly BlobServiceClient? _blobServiceClient;
        private readonly AzureStorageSettings _settings;

        public AzureBlobImageService(BlobServiceClient? blobServiceClient, IOptions<AzureStorageSettings> options)
        {
            _blobServiceClient = blobServiceClient;
            _settings = options.Value;

            if (string.IsNullOrWhiteSpace(_settings.ContainerName))
                throw new InvalidOperationException("AzureStorage:ContainerName is missing.");
        }

        private BlobContainerClient GetContainerOrThrow()
        {
            if (_blobServiceClient is null)
                throw new InvalidOperationException(
                    "Azure Blob Storage is not configured. " +
                    "For local dev start Azurite and set AzureStorage:ConnectionString in appsettings.Development.json. " +
                    "For Azure set AzureStorage:ConnectionString in App Service configuration."
                );

            return _blobServiceClient.GetBlobContainerClient(_settings.ContainerName);
        }

        public async Task<string> UploadImageAsync(Stream imageStream, string fileName)
        {
            if (imageStream is null) throw new ArgumentNullException(nameof(imageStream));
            if (string.IsNullOrWhiteSpace(fileName)) throw new ArgumentException("FileName is required.", nameof(fileName));

            var container = GetContainerOrThrow();
            await container.CreateIfNotExistsAsync(PublicAccessType.Blob);

            var uniqueFileName = $"{Guid.NewGuid()}_{fileName}";
            var blobClient = container.GetBlobClient(uniqueFileName);

            var headers = new BlobHttpHeaders { ContentType = GetContentType(fileName) };
            await blobClient.UploadAsync(imageStream, new BlobUploadOptions { HttpHeaders = headers });

            return blobClient.Uri.ToString();
        }

        public async Task<bool> DeleteImageAsync(string imageUrl)
        {
            if (string.IsNullOrWhiteSpace(imageUrl)) return false;

            var container = GetContainerOrThrow();

            var uri = new Uri(imageUrl);
            var blobName = string.Join("", uri.Segments.Skip(2));

            var blobClient = container.GetBlobClient(blobName);
            var result = await blobClient.DeleteIfExistsAsync();

            return result.Value;
        }

        private static string GetContentType(string fileName)
        {
            var ext = Path.GetExtension(fileName).ToLowerInvariant();
            return ext switch
            {
                ".jpg" or ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                ".gif" => "image/gif",
                ".webp" => "image/webp",
                _ => "application/octet-stream"
            };
        }
    }
}
