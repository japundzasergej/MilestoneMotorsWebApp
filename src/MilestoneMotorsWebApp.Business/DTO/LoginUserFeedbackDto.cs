namespace MilestoneMotorsWebApp.Business.DTO
{
    public record LoginUserFeedbackDto
    {
        public bool IsNotPasswordsMatching { get; set; } = false;
        public bool IsValidUser { get; set; } = true;
        public string Token { get; set; }
    }
}
