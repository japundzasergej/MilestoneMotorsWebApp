using Microsoft.AspNetCore.Identity;

namespace MilestoneMotorsWebApp.Business.DTO
{
    public record RegisterUserFeedbackDto
    {
        public bool UserExists { get; set; } = false;
        public bool ResponseFailed { get; set; } = false;
        public List<IdentityError> ErrorList { get; init; } = [ ];
    }
}
