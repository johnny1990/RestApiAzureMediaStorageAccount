using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Infrastructure.Contracts;

namespace Infrastructure.Repositories
{
    public class MediaStorageService : IMediaStorageService
    {
        private readonly BlobServiceClient _blobServiceClient;

        public MediaStorageService(BlobServiceClient blobServiceClient)
        {
            _blobServiceClient = blobServiceClient;
        }

        public async Task<List<string>> GetAllFiles()
        {
            try
            {
                var containerClient = _blobServiceClient.GetBlobContainerClient("media-gallery-storage");
                var files = new List<string>();
                await foreach (var blobItem in containerClient.GetBlobsAsync())
                {
                    files.Add(blobItem.Name);

                }
                return files;
            }

            catch (Azure.RequestFailedException ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public async Task<string> UploadFile(Stream mediaStream, string fileName, string contentType)
        {
            try
            {
                var containerClient = _blobServiceClient.GetBlobContainerClient("media-gallery-storage");
                var blobClient = containerClient.GetBlobClient(fileName);
                await blobClient.UploadAsync(mediaStream, new BlobHttpHeaders { ContentType = contentType });
                return blobClient.Uri.ToString();
            }

            catch (Azure.RequestFailedException ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public async Task<string> ReplaceFile(Stream mediaStream, string fileName, string contentType, string FileToReplaceName)
        {
            try
            {
                mediaStream.Position = 0; // Reset the stream position
                var containerClient = _blobServiceClient.GetBlobContainerClient("media-gallery-storage");
                var blobClient = containerClient.GetBlobClient(fileName);
                await blobClient.UploadAsync(mediaStream, new BlobHttpHeaders { ContentType = contentType });
                var blobClientToDelete = containerClient.GetBlobClient(FileToReplaceName);
                await blobClientToDelete.DeleteIfExistsAsync();
                return blobClient.Uri.ToString();
            }
            catch (Azure.RequestFailedException ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> DeleteFile(string fileName)
        {
            try
            {
                var containerClient = _blobServiceClient.GetBlobContainerClient("media-gallery-storage");
                var blobClient = containerClient.GetBlobClient(fileName);
                return await blobClient.DeleteIfExistsAsync();
            }
            catch (Azure.RequestFailedException ex)
            {
                throw new Exception(ex.Message);
            }

        }
    }

}
