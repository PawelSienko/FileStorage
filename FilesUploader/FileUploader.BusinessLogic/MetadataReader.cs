using Azure.Storage.Blobs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FileUploader.BusinessLogic
{
    public class MetadataReader : IMetadataReader
    {
        public async Task<IDictionary<string,string>> ReadMetadata(BlobContainerClient blobContainerClient, string blobName)
        {
            var blobClient = blobContainerClient.GetBlobClient(blobName);
            var properties =  blobClient.GetProperties();
            var metadata = properties.Value.Metadata;

            return await Task.FromResult(metadata); ;
        }
    }
}
