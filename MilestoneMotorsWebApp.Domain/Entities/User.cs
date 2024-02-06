using Microsoft.AspNetCore.Identity;

namespace MilestoneMotorsWebApp.Domain.Entities
{
    public class User : IdentityUser
    {
        public string? ProfilePictureImageUrl { get; set; }
        public string? Country { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public ICollection<Car> MyListings { get; set; }
    }
}
