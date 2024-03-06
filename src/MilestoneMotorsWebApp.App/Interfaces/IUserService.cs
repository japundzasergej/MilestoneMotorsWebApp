using MilestoneMotorsWebApp.App.ViewModels;
using MilestoneMotorsWebApp.Business.DTO;
using MilestoneMotorsWebApp.Domain.Entities;

namespace MilestoneMotorsWebApp.App.Interfaces
{
    public interface IUserService
    {
        Task<User?> GetUserDetail(string? id);
        Task<EditUserViewModel?> GetUserEdit(string? id);
        Task<EditUserFeedbackDto?> PostUserEdit(string? id, EditUserViewModel editVM);
        Task<IEnumerable<Car>?> GetUserCars(string? id);
        Task<HttpResponseMessage?> DeleteUser(string? id);
    }
}
