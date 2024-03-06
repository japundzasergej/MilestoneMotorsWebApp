using MilestoneMotorsWebApp.Domain.Entities;

namespace MilestoneMotorsWebApp.Infrastructure.Interfaces
{
    public interface IUserRepository
    {
        Task<bool> Save();
        Task<bool> Delete(User user);
        Task<bool> Update(User user);
        Task<User?> GetByIdAsync(string? id);
        Task<User?> GetByIdNoTrackAsync(string? id);
        Task<IEnumerable<Car>?> GetUserCarsAsync(string userId);
    }
}
