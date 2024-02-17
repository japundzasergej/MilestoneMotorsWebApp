using Microsoft.AspNetCore.Identity;

namespace MilestoneMotorsWebApp.Common.DTO
{
    public class RegisterUserFeedbackDto
    {
        public bool UserExists { get; set; } = false;
        public bool ResponseFailed { get; set; } = false;
        public List<IdentityError> ErrorList { get; set; } = [ ];
    }
}
