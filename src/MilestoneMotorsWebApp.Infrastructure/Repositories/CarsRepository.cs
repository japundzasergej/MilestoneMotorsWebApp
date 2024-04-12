using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using MilestoneMotorsWebApp.Business.Cars.Helpers;
using MilestoneMotorsWebApp.Domain.Entities;
using MilestoneMotorsWebApp.Domain.Enums;
using MilestoneMotorsWebApp.Infrastructure.Interfaces;

namespace MilestoneMotorsWebApp.Infrastructure.Repositories
{
    public class CarsRepository(ApplicationDbContext db) : ICarsRepository
    {
        public async Task<bool> Add(Car car)
        {
            db.Add(car);
            return await Save();
        }

        public async Task<bool> Remove(Car car)
        {
            db.Remove(car);
            return await Save();
        }

        public async Task<IEnumerable<Car>> GetAllCarsAsync(string? orderBy)
        {
            var carQuery = db.Cars.AsQueryable();

            if (!string.IsNullOrEmpty(orderBy))
            {
                carQuery = OrderCars.Filter(carQuery, orderBy);
            }
            return await carQuery.ToListAsync();
        }

        public async Task<Car?> GetCarByIdAsync(int? id)
        {
            if (id == null || id == 0)
            {
                return null;
            }
            var carDetail = await db.Cars.SingleOrDefaultAsync(c => c.Id == id);
            if (carDetail == null)
            {
                return null;
            }
            return carDetail;
        }

        public async Task<bool> Save()
        {
            var result = await db.SaveChangesAsync();
            return result > 0;
        }

        public async Task<bool> Update(Car car)
        {
            db.Update(car);
            return await Save();
        }

        public async Task<Car?> GetCarByIdNoTrackAsync(int? id)
        {
            if (id == null || id == 0)
            {
                return null;
            }
            var userCar = await db.Cars.AsNoTracking().SingleOrDefaultAsync(c => c.Id == id);
            if (userCar == null)
            {
                return null;
            }
            return userCar;
        }

        public async Task<IEnumerable<Car>> SearchCarsAsync(string search, string? orderBy)
        {
            string query = "SELECT * FROM Cars WHERE Brand LIKE {0} OR Model LIKE {0}";
            var carsQuery = db.Cars.FromSqlRaw(query, $"{search}%");

            if (!string.IsNullOrEmpty(orderBy))
            {
                carsQuery = OrderCars.Filter(carsQuery, orderBy);
            }

            var carList = await carsQuery.ToListAsync();

            if (carList.Count == 0)
            {
                return [ ];
            }

            return carList;
        }

        public async Task<IEnumerable<Car>> FilteredCarsAsync(
            string? brand,
            string? fuelType,
            string? condition,
            string? orderBy
        )
        {
            var carsQuery = db.Cars.AsQueryable();

            if (brand != null)
            {
                var selectedBrand = Enum.Parse<Brand>(brand);
                carsQuery = carsQuery.Where(c => c.Brand == selectedBrand);
            }
            else if (fuelType != null)
            {
                var selectedFuelType = Enum.Parse<FuelTypes>(fuelType);
                carsQuery = carsQuery.Where(c => c.FuelTypes == selectedFuelType);
            }
            else if (condition != null)
            {
                var selectedCondition = Enum.Parse<Condition>(condition);
                carsQuery = carsQuery.Where(c => c.Condition == selectedCondition);
            }

            if (!string.IsNullOrEmpty(orderBy))
            {
                carsQuery = OrderCars.Filter(carsQuery, orderBy);
            }

            var carList = await carsQuery.ToListAsync();

            if (carList.Count == 0)
            {
                return [ ];
            }

            return carList;
        }
    }
}
