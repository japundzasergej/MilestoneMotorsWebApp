using MilestoneMotorsWebApp.Domain.Entities;
using MilestoneMotorsWebApp.Domain.Enums;

namespace MilestoneMotorsWebApp.Business.Cars.Queries
{
    public class CarsByFuelTypeQuery
    {
        public static List<Car> ApplyFuelTypeFilter(List<Car> source, string fuelType)
        {
            if (!string.IsNullOrEmpty(fuelType))
            {
                var selectedFuelType = Enum.Parse<FuelTypes>(fuelType);
                source = source.Where(c => c.FuelTypes == selectedFuelType).ToList();
            }
            return source;
        }
    }
}
