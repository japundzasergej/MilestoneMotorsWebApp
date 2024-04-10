using MilestoneMotorsWebApp.App.ViewModels;
using MilestoneMotorsWebApp.Business.DTO;
using MilestoneMotorsWebApp.Domain.Entities;
using X.PagedList;

namespace MilestoneMotorsWebApp.App.Interfaces
{
    public interface ICarService
    {
        Task<ResponseDTO> GetAllCars(
            string search,
            string orderBy,
            string fuelType,
            string condition,
            string brand,
            int? page
        );
        Task<ResponseDTO> GetCarDetail(int? id, string? token);
        Task<ResponseDTO> CreateCar(CreateCarDto createCarDto, string? token);
        Task<ResponseDTO> GetEditCar(int? id, string? token);
        Task<ResponseDTO> PostEditCar(int? id, EditCarDto editCarDto, string? token);
        Task<ResponseDTO> DeleteCar(int? id, string? token);
    }
}
