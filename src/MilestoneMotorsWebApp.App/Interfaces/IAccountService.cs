using MilestoneMotorsWebApp.App.ViewModels;
using MilestoneMotorsWebApp.Business.DTO;

namespace MilestoneMotorsWebApp.App.Interfaces
{
    public interface IAccountService
    {
        Task<RegisterUserFeedbackDto?> RegisterUser(RegisterUserViewModel registerVM);
        Task<LoginUserFeedbackDto?> LoginUser(LoginUserViewModel loginVM);
    }
}
