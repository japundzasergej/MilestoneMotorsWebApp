namespace MilestoneMotorsWebApp.Business.DTO
{
    public class EditUserDto
    {
        public string Id { get; set; }
        public byte[]? ProfilePictureImageUrl { get; set; }
        public string? Country { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? ImageContentType { get; set; }
    }
}
