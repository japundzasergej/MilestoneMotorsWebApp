using MilestoneMotorsWebApp.Domain.Entities;

namespace MilestoneMotorsWebApp.Business.DTO
{
    public class UserDto
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string? ProfilePictureImageUrl { get; set; }
        public string? Country { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public List<CarDto> MyListings { get; set; }
    }
}
