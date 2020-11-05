using Azure.Storage.Blobs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FileUploader.BusinessLogic
{
    public interface IMetadataReader
    {
        Task<IDictionary<string, string>> ReadMetadata(BlobContainerClient blobContainerClient, string blobName);
    }
}