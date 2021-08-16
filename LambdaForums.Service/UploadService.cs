using LambdaForums.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace LambdaForums.Service
{
    public class UploadService : IUpload
    {
        public IConfiguration _configuration;
        private readonly IWebHostEnvironment _environment;

        public UploadService(IConfiguration configuration, IWebHostEnvironment environment)
        {
            _configuration = configuration;
            _environment = environment;
        }

        public async ValueTask<string> UploadFile(IFormFile file)
        {

            var uploadPath = _configuration["Attachment:Path"];

            var parsedContentDisposition = ContentDispositionHeaderValue.Parse(file.ContentDisposition);
            var filename = Path.Combine(parsedContentDisposition.FileName.Trim('"'));
            var absoluteUploadPath = Path.Combine(_environment.WebRootPath, uploadPath);
            var absoluteFilePath = Path.Combine(absoluteUploadPath, filename);
            var filePath = new Uri(absoluteUploadPath).MakeRelativeUri(new Uri(absoluteFilePath));

            if (!Directory.Exists(absoluteUploadPath))
            {
                Directory.CreateDirectory(absoluteUploadPath);
            }

            using (var stream = File.Create(absoluteFilePath))
            {
                await file.CopyToAsync(stream);
            }

            return "/" + filePath;
        }
    }
}
