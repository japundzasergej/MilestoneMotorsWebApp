using MilestoneMotorsWebApp.Domain.Entities;

namespace MilestoneMotorsWebApp.Business.Cars.Helpers
{
    public static class OrderCars
    {
        public static IQueryable<Car> Filter(IQueryable<Car> source, string orderBy)
        {
            return orderBy switch
            {
                "priceDesc" => source.OrderByDescending(c => c.Price),
                "priceAsc" => source.OrderBy(c => c.Price),
                "yearDesc" => source.OrderByDescending(c => c.ManufacturingYear),
                _ => source,
            };
        }
    }
}
