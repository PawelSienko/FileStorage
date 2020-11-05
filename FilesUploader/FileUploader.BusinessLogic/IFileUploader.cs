using System.IO;
using System.Threading.Tasks;

namespace FileUploader.BusinessLogic
{
    public interface IFileUploader
    {
        Task UploadFile(MemoryStream fileStream, string fileName);
    }
}