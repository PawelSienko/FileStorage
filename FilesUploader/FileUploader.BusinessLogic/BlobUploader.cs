using Azure.Storage.Blobs;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FileUploader.BusinessLogic
{
    public class BlobUploader : IFileUploader
    {
        private string containerConnectionString;
        private MemoryStream stream;

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

            fileStream.Position = 0;
            await blobContainerClient.UploadBlobAsync(fileName, fileStream);
            fileStream.Dispose();
        }
    }
}
