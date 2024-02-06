using MilestoneMotorsWebApp.Domain.Entities;

namespace MilestoneMotorsWebApp.Business.ViewModels
{
    public class UserAccountViewModel
    {
        public string? ProfilePictureImageUrl { get; set; }
        public string? Country { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public ICollection<Car> MyListings { get; set; }
    }
}
