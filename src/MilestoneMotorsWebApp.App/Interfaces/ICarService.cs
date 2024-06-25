using MilestoneMotorsWebApp.Business.DTO;

namespace MilestoneMotorsWebApp.App.Interfaces
{
    public interface ICarService
    {
        Task<List<CarDto>> GetAllCars(
            string search,
            string orderBy,
            string fuelType,
            string condition,
            string brand
        );
        Task<CarDto> GetCarDetail(int? id);
        Task<ImageServiceDto> CreateCar(CreateCarDto createCarDto, string? token);
        Task<EditCarDto> GetEditCar(int? id, string? token);
        Task<bool> PostEditCar(EditCarDto editCarDto, string? token);
        Task<bool> DeleteCar(int? id, string? token);
    }
}
