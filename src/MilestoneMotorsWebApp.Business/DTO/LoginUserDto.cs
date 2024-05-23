namespace MilestoneMotorsWebApp.Business.DTO
{
    public record LoginUserDto
    {
        public string Email { get; init; }

        public string Password { get; init; }
    }
}
