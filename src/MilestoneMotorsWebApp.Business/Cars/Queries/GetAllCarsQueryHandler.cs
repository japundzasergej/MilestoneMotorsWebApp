using AutoMapper;
using MediatR;
using MilestoneMotorsWebApp.Business.DTO;
using MilestoneMotorsWebApp.Domain.Entities;
using MilestoneMotorsWebApp.Infrastructure.Interfaces;

namespace MilestoneMotorsWebApp.Business.Cars.Queries
{
    public class GetAllCarsQueryHandler(ICarsRepository carsRepository, IMapper mapper)
        : IRequestHandler<GetAllCarsQuery, IEnumerable<CarDto>>
    {
        public async Task<IEnumerable<CarDto>> Handle(
            GetAllCarsQuery request,
            CancellationToken cancellationToken
        )
        {
            var search = request.Search;
            var orderBy = request.OrderBy;
            var fuelType = request.FuelType;
            var condition = request.Condition;
            var brand = request.Brand;

            if (!string.IsNullOrEmpty(search))
            {
                var searchedList = await carsRepository.SearchCarsAsync(search, orderBy);
                var carList = searchedList.Select(mapper.Map<Car, CarDto>);

                return carList;
            }
            else if (
                !string.IsNullOrEmpty(condition)
                || !string.IsNullOrEmpty(fuelType)
                || !string.IsNullOrEmpty(brand)
            )
            {
                var filteredList = await carsRepository.FilteredCarsAsync(
                    brand,
                    fuelType,
                    condition,
                    orderBy
                );
                var carList = filteredList.Select(mapper.Map<Car, CarDto>);

                return carList;
            }
            else
            {
                var completeList = await carsRepository.GetAllCarsAsync(orderBy);
                var carList = completeList.Select(mapper.Map<Car, CarDto>);

                return carList;
            }
        }
    }
}
