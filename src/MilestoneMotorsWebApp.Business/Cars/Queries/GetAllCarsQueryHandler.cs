using MediatR;
using MilestoneMotorsWebApp.Business.Cars.Helpers;
using MilestoneMotorsWebApp.Business.DTO;
using MilestoneMotorsWebApp.Business.Helpers;
using MilestoneMotorsWebApp.Business.Interfaces;
using MilestoneMotorsWebApp.Domain.Entities;
using MilestoneMotorsWebApp.Infrastructure.Interfaces;

namespace MilestoneMotorsWebApp.Business.Cars.Queries
{
    public class GetAllCarsQueryHandler(
        ICarsRepository carsRepository,
        IMapperService mapperService
    ) : IRequestHandler<GetAllCarsQuery, ResponseDTO>
    {
        private readonly ICarsRepository _carsRepository = carsRepository;
        private readonly IMapperService _mapperService = mapperService;

        public async Task<ResponseDTO> Handle(
            GetAllCarsQuery request,
            CancellationToken cancellationToken
        )
        {
            var search = request.Search;
            var orderBy = request.OrderBy;
            var fuelType = request.FuelType;
            var brand = request.Brand;
            var condition = request.Condition;

            try
            {
                var carsList = await _carsRepository.GetAllCarsAsync();

                var searchedList = carsList
                    .Where(
                        c =>
                            string.IsNullOrWhiteSpace(search)
                            || c.Brand
                                .ToString()
                                .StartsWith(search, StringComparison.OrdinalIgnoreCase)
                            || c.Model.StartsWith(search, StringComparison.OrdinalIgnoreCase)
                    )
                    .ToList();

                searchedList = CarFilters.ApplyOrdering(searchedList, orderBy);
                searchedList = CarFilters.ApplyFuelTypeFilter(searchedList, fuelType);
                searchedList = CarFilters.ApplyBrandFilter(searchedList, brand);
                searchedList = CarFilters.ApplyConditionFilter(searchedList, condition);

                if (searchedList.Count == 0)
                {
                    return PopulateResponseDto.OnSuccess(new List<CarDto>(), 200);
                }

                return PopulateResponseDto.OnSuccess(
                    searchedList.Select(_mapperService.Map<Car, CarDto>),
                    200
                );
            }
            catch (Exception e)
            {
                return PopulateResponseDto.OnError(e);
            }
        }
    }
}
