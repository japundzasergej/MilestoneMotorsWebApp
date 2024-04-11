using MilestoneMotorsWebApp.Domain.Entities;

namespace MilestoneMotorsWebApp.Infrastructure.Interfaces
{
    public interface ICarsRepository
    {
        Task<bool> Add(Car car);
        Task<bool> Update(Car car);
        Task<bool> Remove(Car car);
        Task<bool> Save();
        Task<IEnumerable<Car>> GetAllCarsAsync(string? orderBy);
        Task<IEnumerable<Car>> SearchCarsAsync(string search, string? orderBy);
        Task<IEnumerable<Car>> FilteredCarsAsync(
            string? brand,
            string? fuelType,
            string? condition,
            string? orderBy
        );
        Task<Car?> GetCarByIdAsync(int? id);
        Task<Car?> GetCarByIdNoTrackAsync(int? id);
    }
}
