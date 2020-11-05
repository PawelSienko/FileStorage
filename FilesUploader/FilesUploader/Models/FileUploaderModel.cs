using Azure.Storage.Blobs.Models;
using System.Collections.Generic;

namespace FilesUploader.Models
{
    public class FileUploaderModel
    {
        public List<BlobItem> Blobs { get; set; }
    }
}
