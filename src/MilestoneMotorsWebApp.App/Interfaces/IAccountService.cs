using MilestoneMotorsWebApp.App.ViewModels;
using MilestoneMotorsWebApp.Business.DTO;

namespace MilestoneMotorsWebApp.App.Interfaces
{
    public interface IAccountService
    {
        Task<ResponseDTO> RegisterUser(RegisterUserDto dto);
        Task<ResponseDTO> LoginUser(LoginUserDto dto);
    }
}
