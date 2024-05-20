using MilestoneMotorsWebApp.Business.DTO;

namespace MilestoneMotorsWebApp.App.ViewModels
{
    public record UserAccountViewModel
    {
        public string Id { get; init; }
        public string UserName { get; init; }
        public string Email { get; init; }
        public string? ProfilePictureImageUrl { get; init; }
        public string? Country { get; init; }
        public string? City { get; init; }
        public string? State { get; init; }
        public List<CarDto> MyListings { get; init; }
    }
}
