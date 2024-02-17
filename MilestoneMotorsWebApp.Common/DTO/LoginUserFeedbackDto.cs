namespace MilestoneMotorsWebApp.Common.DTO
{
    public class LoginUserFeedbackDto
    {
        public bool IsNotPasswordsMatching { get; set; } = false;
        public bool IsValidUser { get; set; } = true;
        public string Token { get; set; }
    }
}
