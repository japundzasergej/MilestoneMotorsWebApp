using MilestoneMotorsWebApp.Business.ViewModels;
using MilestoneMotorsWebApp.Domain.Entities;
using X.PagedList;

namespace MilestoneMotorsWebApp.Business.Interfaces
{
    public interface ICarCommand
    {
        Task<IPagedList<Car>> GetCars(
            string search,
            string orderBy,
            string fuelType,
            string condition,
            string brand,
            int? page
        );
        Task<Car?> GetCarById(int? id);
        CreateCarViewModel GetCreateCar(string userId);
        Task<bool> PostCreateCar(CreateCarViewModel carVM, Func<object> onImageServiceDown);
        Task<EditCarViewModel?> GetEditCar(int? id);
        Task<bool?> PostEditCar(int? id, EditCarViewModel carVM);
        Task<bool?> DeleteCar(int? id);
    }
}
