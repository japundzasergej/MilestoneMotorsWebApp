namespace MilestoneMotorsWebApp.Business.DTO
{
    public record UserDto
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
