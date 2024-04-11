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
            var carList = await db.Cars.ToListAsync();

            if (!string.IsNullOrEmpty(orderBy))
            {
                carList = OrderCars.Filter(carList, orderBy);
            }
            return carList;
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
            var carList = await db.Cars.FromSqlRaw(query, $"{search}%").ToListAsync();

            if (carList.Count == 0)
            {
                return [ ];
            }

            if (!string.IsNullOrEmpty(orderBy))
            {
                carList = OrderCars.Filter(carList, orderBy);
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
            IEnumerable<Car> carList =  [ ];
            string sqlQuery;

            if (brand != null)
            {
                sqlQuery = "SELECT * FROM Cars WHERE Brand = {0}";
                var carsQuery = db.Cars.FromSqlRaw(sqlQuery, brand);
                carList = await carsQuery.ToListAsync();

                if (!carList.Any())
                {
                    return [ ];
                }

                if (!string.IsNullOrEmpty(orderBy))
                {
                    carList = OrderCars.Filter(carList.ToList(), orderBy);
                }

                return carList;
            }
            else if (fuelType != null)
            {
                var selectedFuelType = Enum.Parse<FuelTypes>(fuelType);
                sqlQuery = "SELECT * FROM Cars WHERE FuelTypes = {0}";
                var carsQuery = db.Cars.FromSqlRaw(sqlQuery, selectedFuelType);
                carList = await carsQuery.ToListAsync();

                if (!carList.Any())
                {
                    return [ ];
                }

                if (!string.IsNullOrEmpty(orderBy))
                {
                    carList = OrderCars.Filter(carList.ToList(), orderBy);
                }

                return carList;
            }
            else if (condition != null)
            {
                var selectedCondition = Enum.Parse<Condition>(condition);
                sqlQuery = "SELECT * FROM Cars WHERE Condition = {0}";
                var carsQuery = db.Cars.FromSqlRaw(sqlQuery, selectedCondition);
                carList = await carsQuery.ToListAsync();

                if (!carList.Any())
                {
                    return [ ];
                }

                if (!string.IsNullOrEmpty(orderBy))
                {
                    carList = OrderCars.Filter(carList.ToList(), orderBy);
                }

                return carList;
            }
            return carList;
        }
    }
}
