using Azure.Storage.Blobs;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FileUploader.BusinessLogic
{
    public class BlobUploader : IFileUploader
    {
        private string containerConnectionString;
        
        public BlobUploader(string containerConnectionString)
        {
            this.containerConnectionString = containerConnectionString;
        }

        public async Task UploadFile(MemoryStream fileStream, string fileName)
        {
            BlobContainerClient blobContainerClient;
            BlobServiceClient blobServiceClient = new BlobServiceClient(containerConnectionString);
            var containers = blobServiceClient.GetBlobContainers();

            var container = containers.FirstOrDefault(o => o.Name.Equals(Constants.ContainerName));
            if (container == null)
            {
                blobContainerClient = blobServiceClient.CreateBlobContainer(Constants.ContainerName);
            }
            else
            {
                blobContainerClient = blobServiceClient.GetBlobContainerClient(Constants.ContainerName);
            }

            var taskMetadata = this.AddMetadata(blobContainerClient);

            fileStream.Position = 0;
            await blobContainerClient.UploadBlobAsync(fileName, fileStream);
            fileStream.Dispose();
            await taskMetadata;
        }

        private async Task<Azure.Response<Azure.Storage.Blobs.Models.BlobContainerInfo>> AddMetadata(BlobContainerClient blobContainerClient)
        {
            IDictionary<string, string> metadata = new Dictionary<string, string>();
            metadata.Add(Constants.Metadata.DateTimeUtc, DateTime.UtcNow.ToString());

            return await blobContainerClient.SetMetadataAsync(metadata);
        }
    }
}
