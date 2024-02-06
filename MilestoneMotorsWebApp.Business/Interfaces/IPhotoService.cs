using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;

namespace MilestoneMotorsWebApp.Business.Interfaces
{
    public interface IPhotoService
    {
        Task<ImageUploadResult?> AddPhotoAsync(IFormFile file);
        Task<DeletionResult?> DeletePhotoAsync(string publicUrl);
    }
}
