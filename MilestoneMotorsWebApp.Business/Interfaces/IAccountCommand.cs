using MilestoneMotorsWebApp.Business.ViewModels;

namespace MilestoneMotorsWebApp.Business.Interfaces
{
    public interface IAccountCommand
    {
        RegisterUserViewModel GetRegisterUser();
        Task<bool> PostRegisterUser(
            RegisterUserViewModel registerVM,
            Func<object> onUserExists,
            Func<object> errorHandling
        );
        LoginUserViewModel GetLoginUser();
        Task<bool> PostLoginUser(
            LoginUserViewModel loginVM,
            Func<object> onPasswordsNotMatching,
            Func<object> onInvalidUser
        );
        Task<bool> LogoutUser();
    }
}
