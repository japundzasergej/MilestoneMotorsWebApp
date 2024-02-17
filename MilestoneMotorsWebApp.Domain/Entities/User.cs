using Microsoft.AspNetCore.Identity;

namespace MilestoneMotorsWebApp.Domain.Entities
{
    public class User : IdentityUser
    {
        public string ProfilePictureImageUrl { get; set; } = "https://i.imgur.com/34koHp4.png";
        public string Country { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public ICollection<Car> MyListings { get; set; }
    }
}
