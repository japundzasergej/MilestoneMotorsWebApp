using MilestoneMotorsWebApp.Domain.Entities;
using MilestoneMotorsWebApp.Domain.Enums;

namespace MilestoneMotorsWebApp.Business.Cars.Queries
{
    public class CarsByBrand
    {
        public static List<Car> ApplyBrandFilter(List<Car> source, string brand)
        {
            if (!string.IsNullOrEmpty(brand))
            {
                var selectedBrand = Enum.Parse<Brand>(brand);
                source = source.Where(c => c.Brand == selectedBrand).ToList();
            }
            return source;
        }
    }
}
