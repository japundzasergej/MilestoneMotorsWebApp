using MilestoneMotorsWebApp.App.ViewModels;
using MilestoneMotorsWebApp.Business.DTO;
using MilestoneMotorsWebApp.Domain.Entities;
using X.PagedList;

namespace MilestoneMotorsWebApp.App.Interfaces
{
    public interface ICarService
    {
        Task<IPagedList<Car>?> GetAllCars(
            string search,
            string orderBy,
            string fuelType,
            string condition,
            string brand,
            int? page
        );
        Task<Car?> GetCarDetail(int? id);
        Task<ImageServiceDto?> CreateCar(
            CreateCarViewModel carVM,
            Func<object> onImageServiceDown,
            Func<object> onDbNotSuccessful
        );
        Task<EditCarViewModel?> GetEditCar(int? id);
        Task<bool?> PostEditCar(int? id, EditCarViewModel editCarVM);
        Task<bool?> DeleteCar(int? id);
    }
}
