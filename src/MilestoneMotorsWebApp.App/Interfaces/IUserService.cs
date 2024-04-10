using MilestoneMotorsWebApp.App.ViewModels;
using MilestoneMotorsWebApp.Business.DTO;
using MilestoneMotorsWebApp.Domain.Entities;

namespace MilestoneMotorsWebApp.App.Interfaces
{
    public interface IUserService
    {
        Task<ResponseDTO> GetUserDetail(string? id, string? token);
        Task<ResponseDTO> GetUserEdit(string? id, string? token);
        Task<ResponseDTO> PostUserEdit(EditUserDto dto, string? token);
        Task<ResponseDTO> GetUserCars(string? id, string? token);
        Task<ResponseDTO> DeleteUser(string? id, string? token);
        Task<ResponseDTO> GetProfilePicture(string? id, string? token);
    }
}
