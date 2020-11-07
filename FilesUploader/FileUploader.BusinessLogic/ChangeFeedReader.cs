using Azure;
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

        public async Task<Dictionary<string,List<BlobChangeFeedEvent>>> ReadChangeFeedAsync(string cursor)
        {
            // create blob service client
            BlobServiceClient blobServiceClient = new BlobServiceClient(this.connectionString);

            // get change feed client
            BlobChangeFeedClient blobChangeFeedClient = blobServiceClient.GetChangeFeedClient();

            List<BlobChangeFeedEvent> blobChangeFeedEvents = new List<BlobChangeFeedEvent>();

            IAsyncEnumerator<Page<BlobChangeFeedEvent>> enumerator = blobChangeFeedClient.GetChangesAsync(continuationToken: cursor)
                .AsPages(pageSizeHint: 5)
                .GetAsyncEnumerator();

            await enumerator.MoveNextAsync();
            Dictionary<string, List<BlobChangeFeedEvent>> keyValuePairs = new Dictionary<string, List<BlobChangeFeedEvent>>();

            if (enumerator.Current != null && enumerator.Current.Values != null)
            {
                foreach (BlobChangeFeedEvent changeFeedEvent in enumerator.Current.Values)
                {
                    blobChangeFeedEvents.Add(changeFeedEvent);
                }
                keyValuePairs.Add(enumerator.Current.ContinuationToken, blobChangeFeedEvents);
            }

            return keyValuePairs;
        }
    }
}
