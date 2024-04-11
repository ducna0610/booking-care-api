using Microsoft.AspNetCore.Http;

namespace BookingCare.Application.Utils
{
    public interface IFileStorageService
    {
        Task<string> UploadAsync(IFormFile file);
        Task DeleteAsync(string fileName);
    }
}