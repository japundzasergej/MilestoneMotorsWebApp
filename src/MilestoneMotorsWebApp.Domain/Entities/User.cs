using Microsoft.AspNetCore.Identity;

namespace MilestoneMotorsWebApp.Domain.Entities
{
    public class User : IdentityUser
    {
        public string ProfilePictureImageUrl { get; set; }
        public string Country { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public List<Car> MyListings { get; init; }
    }
}
