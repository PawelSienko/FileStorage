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
using FileUploader.BusinessLogic;

namespace FilesUploader.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private string containerConnectionString;
        private readonly IFileUploader fileUploader;
        public HomeController(ILogger<HomeController> logger, IConfiguration config, IFileUploader fileUploader)
        {
            _logger = logger;
            this.fileUploader = fileUploader;
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
            var file = files.First();
            
            using (MemoryStream ms = new MemoryStream())
            {
                await file.CopyToAsync(ms);

                await this.fileUploader.UploadFile(ms, file.FileName);
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
