using BookingCare.Application.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace BookingCare.Infrastructure.Services
{

    public class FileStorageService : IFileStorageService
    {
        private readonly IConfiguration _configuration;

        public FileStorageService
            (
                IConfiguration configuration
            )
        {
            _configuration = configuration;
        }

        public async Task<string> UploadAsync(IFormFile file)
        {
            var fileName = string.Concat(Path.GetFileNameWithoutExtension(file.FileName), "_", Guid.NewGuid().ToString(), Path.GetExtension(file.FileName));
            var filePath = Path.Combine(_configuration["Folder:Upload"], fileName);
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            return fileName;
        }

        public async Task DeleteAsync(string fileName)
        {
            var filePath = Path.Combine(_configuration["Folder:Upload"], fileName);
            if (File.Exists(filePath))
            {
                await Task.Run(() => File.Delete(filePath));
            }
        }
    }
}