using Azure.Storage.Blobs;
using Azure.Storage.Blobs.ChangeFeed;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FileUploader.BusinessLogic
{
    public class ChangeFeedReader : IChangeFeedReader
    {
        private string connectionString;

        public ChangeFeedReader(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public async Task<List<BlobChangeFeedEvent>> ReadChangeFeedAsync()
        {
            // create blob service client
            BlobServiceClient blobServiceClient = new BlobServiceClient(this.connectionString);

            // get change feed client
            BlobChangeFeedClient blobChangeFeedClient = blobServiceClient.GetChangeFeedClient();

            List<BlobChangeFeedEvent> blobChangeFeedEvents = new List<BlobChangeFeedEvent>();

            // read change feed
            await foreach (BlobChangeFeedEvent changeFeedEvent in blobChangeFeedClient.GetChangesAsync())
            {
                blobChangeFeedEvents.Add(changeFeedEvent);
            }

            return blobChangeFeedEvents;
        }
    }
}
