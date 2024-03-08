using MilestoneMotorsWebApp.Domain.Entities;
using MilestoneMotorsWebApp.Domain.Enums;

namespace MilestoneMotorsWebApp.Business.Cars.Helpers
{
    public static class CarFilters
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

        public static List<Car> ApplyFuelTypeFilter(List<Car> source, string fuelType)
        {
            if (!string.IsNullOrEmpty(fuelType))
            {
                var selectedFuelType = Enum.Parse<FuelTypes>(fuelType);
                source = source.Where(c => c.FuelTypes == selectedFuelType).ToList();
            }
            return source;
        }

        public static List<Car> ApplyConditionFilter(List<Car> source, string condition)
        {
            if (!string.IsNullOrEmpty(condition))
            {
                var selectedcondition = Enum.Parse<Condition>(condition);
                source = source.Where(c => c.Condition == selectedcondition).ToList();
            }
            return source;
        }

        public static List<Car> ApplyOrdering(List<Car> source, string orderBy)
        {
            return orderBy switch
            {
                "priceDesc" => [ .. source.OrderByDescending(c => ConvertPrice(c.Price)) ],
                "priceAsc" => [ .. source.OrderBy(c => ConvertPrice(c.Price)) ],
                "yearDesc" => [ .. source.OrderByDescending(c => c.ManufacturingYear) ],
                _ => source,
            };
        }

        private static int ConvertPrice(string price)
        {
            char[] invalidChars =  [ ' ', '.', '€' ];

            string cleanedString =
                new(price.Trim().Where(c => !invalidChars.Contains(c)).ToArray());

            if (int.TryParse(cleanedString, out int numericPart))
            {
                return numericPart;
            }

            return 0;
        }
    }
}
