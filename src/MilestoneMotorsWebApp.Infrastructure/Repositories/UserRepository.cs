using Microsoft.EntityFrameworkCore;
using MilestoneMotorsWebApp.Domain.Entities;
using MilestoneMotorsWebApp.Infrastructure.Interfaces;

namespace MilestoneMotorsWebApp.Infrastructure.Repositories
{
    public class UserRepository(ApplicationDbContext db) : IUserRepository
    {
        public async Task<bool> Delete(User user)
        {
            db.Remove(user);
            return await Save();
        }

        public async Task<User?> GetByIdAsync(string? id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return null;
            }
            var user = await db.Users.SingleOrDefaultAsync(u => u.Id == id);
            if (user == null)
            {
                return null;
            }
            return user;
        }

        public async Task<User?> GetByIdNoTrackAsync(string? id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return null;
            }
            var user = await db.Users.AsNoTracking().SingleOrDefaultAsync(u => u.Id == id);
            db.Entry(user).State = EntityState.Detached;
            if (user == null)
            {
                return null;
            }
            return user;
        }

        public async Task<IEnumerable<Car>?> GetUserCarsAsync(string userId)
        {
            return await db.Cars.Where(c => c.UserId == userId).ToListAsync();
        }

        public async Task<string?> GetUserProfilePictureAsync(string userId)
        {
            var user = await db.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user != null)
            {
                return user.ProfilePictureImageUrl;
            }
            return null;
        }

        public async Task<bool> Save()
        {
            var result = await db.SaveChangesAsync();
            return result > 0;
        }

        public async Task<bool> Update(User user)
        {
            db.Update(user);
            return await Save();
        }
    }
}
