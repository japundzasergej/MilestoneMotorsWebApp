using Microsoft.AspNetCore.Http;

namespace MilestoneMotorsWebApp.Common.DTO
{
    public class EditUserDto
    {
        public string Id { get; set; }
        public IFormFile? ProfilePictureImageUrl { get; set; }
        public string? Country { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
    }
}
