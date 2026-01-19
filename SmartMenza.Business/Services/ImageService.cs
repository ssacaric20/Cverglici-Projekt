using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Options;
using SmartMenza.Business.Services.Interfaces;
using SmartMenza.Core.Settings;

namespace SmartMenza.Business.Services
{
    public sealed class AzureBlobImageService : IImageService
    {
        private readonly BlobContainerClient _container;

        public AzureBlobImageService(BlobServiceClient blobServiceClient, IOptions<AzureStorageSettings> options)
        {
            var s = options.Value;

            if (string.IsNullOrWhiteSpace(s.ContainerName))
                throw new InvalidOperationException("AzureStorage:ContainerName is missing.");

            _container = blobServiceClient.GetBlobContainerClient(s.ContainerName);

            // radi jednom po instanci (Scoped)
            _container.CreateIfNotExists(PublicAccessType.Blob);
        }


        public async Task<string> UploadImageAsync(Stream imageStream, string fileName)
        {
            if (imageStream is null) throw new ArgumentNullException(nameof(imageStream));
            if (string.IsNullOrWhiteSpace(fileName)) throw new ArgumentException("FileName is required.", nameof(fileName));

            await _container.CreateIfNotExistsAsync(PublicAccessType.Blob);

            var uniqueFileName = $"{Guid.NewGuid()}_{fileName}";
            var blobClient = _container.GetBlobClient(uniqueFileName);

            var headers = new BlobHttpHeaders { ContentType = GetContentType(fileName) };

            await blobClient.UploadAsync(imageStream, new BlobUploadOptions { HttpHeaders = headers });

            return blobClient.Uri.ToString();
        }

        public async Task<bool> DeleteImageAsync(string imageUrl)
        {
            if (string.IsNullOrWhiteSpace(imageUrl)) return false;

            var uri = new Uri(imageUrl);
            var blobName = string.Join("", uri.Segments.Skip(2));

            var blobClient = _container.GetBlobClient(blobName);
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
