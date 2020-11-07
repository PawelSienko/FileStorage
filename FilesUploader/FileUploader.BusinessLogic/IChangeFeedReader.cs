using Azure.Storage.Blobs.ChangeFeed;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FileUploader.BusinessLogic
{
    public interface IChangeFeedReader
    {
        Task<Dictionary<string, List<BlobChangeFeedEvent>>> ReadChangeFeedAsync(string cursor);
    }
}
