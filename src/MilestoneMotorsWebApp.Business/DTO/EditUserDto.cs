namespace MilestoneMotorsWebApp.Business.DTO
{
    public record EditUserDto
    {
        public string Id { get; set; }
        public byte[]? ProfilePictureImageUrl { get; init; }
        public string? Country { get; init; }
        public string? City { get; init; }
        public string? State { get; init; }
        public string? ImageContentType { get; set; }
    }
}
