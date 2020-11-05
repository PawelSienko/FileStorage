using Azure.Storage.Blobs.Models;
using System.Collections.Generic;

namespace FilesUploader.Models
{
    public class FileUploaderModel
    {
        public FileUploaderModel()
        {
            Blobs = new List<Blob>();
        }

        public List<Blob> Blobs { get; set; }
    }

    public class Blob
    {
        public BlobItem BlobItem { get; set; }

        public IDictionary<string, string> Metadata { get; set; }
    }
}
