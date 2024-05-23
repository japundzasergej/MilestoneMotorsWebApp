using System.ComponentModel;

namespace MilestoneMotorsWebApp.App.ViewModels
{
    public record EditUserViewModel
    {
        [DisplayName("Change Profile Picture (optional)")]
        public IFormFile? ProfilePictureImageUrl { get; init; }
        public string? Country { get; init; }
        public string? City { get; init; }
        public string? State { get; init; }
    }
}
