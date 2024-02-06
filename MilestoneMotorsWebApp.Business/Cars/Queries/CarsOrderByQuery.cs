using MilestoneMotorsWebApp.Domain.Entities;

namespace MilestoneMotorsWebApp.Business.Cars.Queries
{
    public class CarsOrderByQuery
    {
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
    }
}
