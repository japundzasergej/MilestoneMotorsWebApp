using Microsoft.EntityFrameworkCore;
using MilestoneMotorsWebApp.Domain.Entities;
using MilestoneMotorsWebApp.Infrastructure.Interfaces;

namespace MilestoneMotorsWebApp.Infrastructure.Repositories
{
    public class UserRepository(ApplicationDbContext db) : IUserRepository
    {
        private readonly ApplicationDbContext _db = db;

        public async Task<bool> Delete(User user)
        {
            _db.Remove(user);
            return await Save();
        }

        public async Task<User?> GetByIdAsync(string? id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return null;
            }
            var user = await _db.Users.SingleOrDefaultAsync(u => u.Id == id);
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
            var user = await _db.Users.AsNoTracking().SingleOrDefaultAsync(u => u.Id == id);
            _db.Entry(user).State = EntityState.Detached;
            if (user == null)
            {
                return null;
            }
            return user;
        }

        public async Task<IEnumerable<Car>?> GetUserCarsAsync(string userId)
        {
            return await _db.Cars.Where(c => c.UserId == userId).ToListAsync();
        }

        public async Task<bool> Save()
        {
            var result = await _db.SaveChangesAsync();
            return result > 0;
        }

        public async Task<bool> Update(User user)
        {
            _db.Update(user);
            return await Save();
        }
    }
}
