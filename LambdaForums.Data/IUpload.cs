using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace LambdaForums.Data
{
    public interface IUpload
    {
        ValueTask<string> UploadFile(IFormFile file);
    }
}
