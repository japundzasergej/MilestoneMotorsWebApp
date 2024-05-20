using MilestoneMotorsWebApp.Business.DTO;

namespace MilestoneMotorsWebApp.App.Interfaces
{
    public interface IAccountService
    {
        Task<RegisterUserFeedbackDto> RegisterUser(RegisterUserDto dto);
        Task<LoginUserFeedbackDto> LoginUser(LoginUserDto dto);
    }
}
