using MediatR;
using MilestoneMotorsWebApp.Business.Cars.Helpers;
using MilestoneMotorsWebApp.Domain.Entities;
using MilestoneMotorsWebApp.Domain.Enums;
using MilestoneMotorsWebApp.Infrastructure.Interfaces;
using Condition = MilestoneMotorsWebApp.Domain.Enums.Condition;

namespace MilestoneMotorsWebApp.Business.Cars.Queries
{
    public class GetAllCarsQueryHandler(ICarsRepository carsRepository)
        : IRequestHandler<GetAllCarsQuery, List<Car>>
    {
        private readonly ICarsRepository _carsRepository = carsRepository;

        public async Task<List<Car>> Handle(
            GetAllCarsQuery request,
            CancellationToken cancellationToken
        )
        {
            var search = request.Search;
            var orderBy = request.OrderBy;
            var fuelType = request.FuelType;
            var brand = request.Brand;
            var condition = request.Condition;

            var carsList = await _carsRepository.GetAllCarsAsync();

            var searchedList = carsList
                .Where(
                    c =>
                        string.IsNullOrWhiteSpace(search)
                        || c.Brand.ToString().StartsWith(search, StringComparison.OrdinalIgnoreCase)
                        || c.Model.StartsWith(search, StringComparison.OrdinalIgnoreCase)
                )
                .ToList();

            searchedList = CarFilters.ApplyOrdering(searchedList, orderBy);
            searchedList = CarFilters.ApplyFuelTypeFilter(searchedList, fuelType);
            searchedList = CarFilters.ApplyBrandFilter(searchedList, brand);
            searchedList = CarFilters.ApplyConditionFilter(searchedList, condition);

            if (searchedList.Count == 0)
            {
                return [ ];
            }

            return searchedList;
        }
    }
}
