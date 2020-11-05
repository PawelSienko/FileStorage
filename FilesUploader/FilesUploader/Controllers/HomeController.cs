using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using FilesUploader.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Azure.Storage.Blobs;
using System.Linq;
using System.IO;
using System.Threading.Tasks;

namespace FilesUploader.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private string containerConnectionString;
        public HomeController(ILogger<HomeController> logger, IConfiguration config)
        {
            _logger = logger;
            var value = config.GetSection("Configuration");
            this.containerConnectionString = value.GetValue<string>(Constants.ConnectionStrings.ContainerPropertyName);
        }

        public IActionResult Index()
        {
            BlobServiceClient blobServiceClient = new BlobServiceClient(containerConnectionString);
            var blobClient = blobServiceClient.GetBlobContainerClient(Constants.ContainerName);
            var blobList = blobClient.GetBlobs();
            return View(blobList);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteFile([FromForm] string fileName)
        {
            BlobServiceClient blobServiceClient = new BlobServiceClient(containerConnectionString);
            var blobClient = blobServiceClient.GetBlobContainerClient(Constants.ContainerName);
            await blobClient.DeleteBlobAsync(fileName);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> UploadFile(List<IFormFile> files)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                var file = files.First();
                var copyTask = file.CopyToAsync(ms);
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

                await copyTask;
                ms.Position = 0;
                await blobContainerClient.UploadBlobAsync(file.FileName, ms);
            }

            return RedirectToAction("Index");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
