using MilestoneMotorsWebApp.Business.ViewModels;
using MilestoneMotorsWebApp.Domain.Entities;

namespace MilestoneMotorsWebApp.Business.Interfaces
{
    public interface IUserCommand
    {
        Task<User?> GetUserDetail(string? id);
        Task<EditUserViewModel?> GetEditUser(string? id);
        Task<bool> PostEditUser(
            EditUserViewModel editVM,
            User user,
            Func<object> onImageServiceDown
        );
        Task<IEnumerable<Car>?> GetUserCars(string? id);
        Task<bool> DeleteUserProfile(User user);
    }
}
