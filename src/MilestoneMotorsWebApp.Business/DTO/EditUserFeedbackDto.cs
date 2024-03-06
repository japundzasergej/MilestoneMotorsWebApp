namespace MilestoneMotorsWebApp.Business.DTO
{
    public class EditUserFeedbackDto
    {
        public bool IsImageServiceDown { get; set; } = false;
        public bool IsAuthorized { get; set; } = true;
        public bool HasFailed { get; set; } = false;
    }
}
