using MilestoneMotorsWebApp.Business.DTO;

namespace MilestoneMotorsWebApp.App.Interfaces
{
    public interface IUserService
    {
        Task<UserDto> GetUserDetail(string? id, string? token);
        Task<EditUserDto> GetUserEdit(string? id, string? token);
        Task<EditUserFeedbackDto> PostUserEdit(EditUserDto dto, string? token);
        Task<List<CarDto>> GetUserCars(string? id, string? token);
        Task<bool> DeleteUser(string? id, string? token);
        Task<string> GetProfilePicture(string? id, string? token);
    }
}
