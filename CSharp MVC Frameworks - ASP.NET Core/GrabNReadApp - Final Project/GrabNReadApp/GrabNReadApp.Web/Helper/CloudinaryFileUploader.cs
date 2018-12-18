using System;
using System.IO;
using System.Threading.Tasks;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;

namespace GrabNReadApp.Web.Helper
{
    public static class CloudinaryFileUploader
    {
        public static async Task<string> UploadFile(IFormFile file, string folderName, string apiKey, string apiSecret)
        {
            var extension = Path.GetExtension(file.Name);
            var fileName = Guid.NewGuid() + extension;

            using (var fileStream = file.OpenReadStream())
            {
                var uploadParams = new ImageUploadParams()
                {
                    File = new FileDescription(fileName, fileStream),
                    Folder = folderName
                };

                var myAccount = new Account { ApiKey = apiKey, ApiSecret = apiSecret, Cloud = "grabnreadapp" };
                var cloudinary = new Cloudinary(myAccount);
                var uploadResult = await cloudinary.UploadAsync(uploadParams);
               return uploadResult.Uri.AbsoluteUri;
            }
        }
    }
}
