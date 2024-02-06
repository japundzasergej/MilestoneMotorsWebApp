using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using MilestoneMotorsWebApp.Business.Cars.Queries;
using MilestoneMotorsWebApp.Business.Interfaces;
using MilestoneMotorsWebApp.Business.Utilities;
using MilestoneMotorsWebApp.Business.ViewModels;
using MilestoneMotorsWebApp.Domain.Entities;
using MilestoneMotorsWebApp.Domain.Utilities;
using MilestoneMotorsWebApp.Infrastructure.Interfaces;
using X.PagedList;

namespace MilestoneMotorsWebApp.Business.Cars.Commands
{
    public class CarCommands(ICarsRepository carsRepository, IPhotoService photoService)
        : ICarCommand
    {
        private readonly ICarsRepository _carsRepository = carsRepository;
        private readonly IPhotoService _photoService = photoService;

        private async Task<List<string>> CloudinaryUpload(List<IFormFile?> files)
        {
            List<string> result =  [ ];
            if (files != null)
            {
                foreach (var file in files)
                {
                    if (file != null)
                    {
                        var imageFile = await _photoService.AddPhotoAsync(file);
                        if (imageFile != null)
                        {
                            result.Add(imageFile.Url.ToString());
                        }
                    }
                }
            }
            return result;
        }

        private static EditCarViewModel EditVmMapper(Car source)
        {
            var capacity = source.EngineCapacity.Split(" ");
            var mileage = source.Mileage.Split(" ");
            var enginePower = source.EnginePower.Split(" ");
            return new EditCarViewModel
            {
                UserId = source.UserId,
                Condition = source.Condition,
                Brand = source.Brand,
                Description = source.Description,
                Price = default,
                Model = source.Model,
                ManufacturingYear = source.ManufacturingYear,
                Mileage = mileage[0],
                BodyTypes = source.BodyTypes,
                FuelTypes = source.FuelTypes,
                EngineCapacity = int.Parse(capacity[0]),
                EnginePower = enginePower[0],
                FixedPrice = source.FixedPrice,
                Exchange = source.Exchange
            };
        }

        public async Task<bool?> DeleteCar(int? id)
        {
            if (id == null || id == 0)
            {
                return null;
            }

            var userCar = await _carsRepository.GetCarByIdAsync(id);

            if (userCar == null)
            {
                return null;
            }
            return await _carsRepository.Remove(userCar);
        }

        public async Task<Car?> GetCarById(int? id)
        {
            if (id == null || id == 0)
            {
                return null;
            }

            var carDetail = await _carsRepository.GetCarByIdAsync(id);

            if (carDetail == null)
            {
                return null;
            }
            return carDetail;
        }

        public async Task<IPagedList<Car>> GetCars(
            string search,
            string orderBy,
            string fuelType,
            string condition,
            string brand,
            int? page
        )
        {
            var carsList = await _carsRepository.GetAllCarsAsync();

            var searchedList = carsList
                .Where(
                    c =>
                        (
                            string.IsNullOrWhiteSpace(search)
                            || c.Brand
                                .ToString()
                                .StartsWith(search, StringComparison.OrdinalIgnoreCase)
                            || c.Model.StartsWith(search, StringComparison.OrdinalIgnoreCase)
                        )
                )
                .ToList();

            searchedList = CarsOrderByQuery.ApplyOrdering(searchedList, orderBy);
            searchedList = CarsByFuelTypeQuery.ApplyFuelTypeFilter(searchedList, fuelType);
            searchedList = CarsByBrand.ApplyBrandFilter(searchedList, brand);
            searchedList = CarsByCondition.ApplyConditionFilter(searchedList, condition);

            if (searchedList.Count == 0)
            {
                return new List<Car>().ToPagedList();
            }

            int pageSize = 6;
            int pageNumber = page ?? 1;
            return searchedList.ToPagedList(pageNumber, pageSize);
        }

        public CreateCarViewModel GetCreateCar(string userId)
        {
            return new CreateCarViewModel { UserId = userId };
        }

        public async Task<EditCarViewModel?> GetEditCar(int? id)
        {
            if (id == null || id == 0)
            {
                return null;
            }
            var userCar = await _carsRepository.GetCarByIdAsync(id);
            if (userCar == null)
            {
                return null;
            }
            return EditVmMapper(userCar);
        }

        public async Task<bool> PostCreateCar(
            CreateCarViewModel carVM,
            Func<object> onImageServiceDown
        )
        {
            string? headlinerImage;
            if (carVM.HeadlinerImageUrl != null)
            {
                var result = await _photoService.AddPhotoAsync(carVM.HeadlinerImageUrl);
                if (result != null)
                {
                    headlinerImage = result.Url.ToString();
                }
                else
                {
                    onImageServiceDown();
                    headlinerImage = null;
                }
            }
            else
            {
                headlinerImage = null;
            }
            List<IFormFile?> files =
            [
                carVM?.PhotoOne,
                carVM?.PhotoTwo,
                carVM?.PhotoThree,
                carVM?.PhotoFour,
                carVM?.PhotoFive,
            ];

            List<string> imageList = await CloudinaryUpload(files) ?? [ ];

            if (imageList.Count == 0)
            {
                onImageServiceDown();
            }

            var carObject = new Car()
            {
                Condition = carVM.Condition,
                Brand = carVM.Brand,
                Description = carVM.Description.FirstCharToUpper().Trim(),
                Price = ConvertToEuroMethod.ConvertToEuro(carVM.Price),
                Model = carVM.Model.FirstCharToUpper().Trim(),
                ManufacturingYear = carVM.ManufacturingYear,
                Mileage = carVM.Mileage.ToString() + " (km)",
                BodyTypes = carVM.BodyTypes,
                FuelTypes = carVM.FuelTypes,
                EngineCapacity = carVM.EngineCapacity.ToString() + " (cm3)",
                EnginePower = carVM.EnginePower + " (kW/HP)",
                FixedPrice = carVM.FixedPrice,
                Exchange = carVM.Exchange,
                HeadlinerImageUrl = headlinerImage ?? "",
                ImagesUrl = imageList,
                UserId = carVM.UserId,
                AdNumber = String.Concat(carVM.UserId, "-", carVM.Brand, "-", carVM.Model),
                CreatedAt = DateTime.UtcNow,
            };
            return await _carsRepository.Add(carObject);
        }

        public async Task<bool?> PostEditCar(int? id, EditCarViewModel carVM)
        {
            if (id == null || id == 0)
            {
                return null;
            }
            var userCar = await _carsRepository.GetCarByIdNoTrackAsync(id);

            if (userCar != null)
            {
                var car = new Car
                {
                    Id = (int)id,
                    UserId = userCar.UserId,
                    Condition = carVM.Condition,
                    Brand = carVM.Brand,
                    Description = carVM.Description.FirstCharToUpper().Trim(),
                    Model = carVM.Model.FirstCharToUpper().Trim(),
                    Price = ConvertToEuroMethod.ConvertToEuro(carVM.Price),
                    ManufacturingYear = carVM.ManufacturingYear,
                    Mileage = carVM.Mileage + " (km)",
                    BodyTypes = carVM.BodyTypes,
                    FuelTypes = carVM.FuelTypes,
                    EngineCapacity = carVM.EngineCapacity.ToString() + " (cm3)",
                    EnginePower = carVM.EnginePower + " (kW/hP)",
                    FixedPrice = carVM.FixedPrice,
                    Exchange = carVM.Exchange,
                    HeadlinerImageUrl = userCar.HeadlinerImageUrl,
                    ImagesUrl = userCar.ImagesUrl,
                    CreatedAt = userCar.CreatedAt,
                    AdNumber = userCar.AdNumber,
                };
                return await _carsRepository.Update(car);
            }
            return null;
        }

        public void SendMessage(Func<object> contact)
        {
            contact();
        }
    }
}
