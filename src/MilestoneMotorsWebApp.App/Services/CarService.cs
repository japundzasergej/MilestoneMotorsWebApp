using System.Text;
using System.Web;
using MilestoneMotorsWebApp.App.Helpers;
using MilestoneMotorsWebApp.App.Interfaces;
using MilestoneMotorsWebApp.App.ViewModels;
using MilestoneMotorsWebApp.Business.DTO;
using MilestoneMotorsWebApp.Business.Interfaces;
using MilestoneMotorsWebApp.Business.Utilities;
using MilestoneMotorsWebApp.Domain.Entities;
using MilestoneMotorsWebApp.Domain.Enums;
using Newtonsoft.Json;
using X.PagedList;

namespace MilestoneMotorsWebApp.App.Services
{
    public class CarService(HttpClient httpClient, IMvcMapperService mapperService) : ICarService
    {
        private readonly HttpClient _httpClient = httpClient;
        private readonly IMvcMapperService _mapperService = mapperService;

        public async Task<ImageServiceDto?> CreateCar(
            CreateCarViewModel carVM,
            Func<object> onImageServiceDown,
            Func<object> onDbNotSuccessful
        )
        {
            var carDto = _mapperService.Map<CreateCarViewModel, CreateCarDto>(carVM);
            List<IFormFile> files =
            [
                carVM.HeadlinerImageUrl,
                carVM.PhotoOne,
                carVM.PhotoTwo,
                carVM.PhotoThree,
                carVM.PhotoFour,
                carVM.PhotoFive
            ];
            var imageContentTypes = PhotoHelpers.GetImageContentType(files);
            carDto.ImageContentTypes = imageContentTypes;

            var apiUrl = _httpClient.BaseAddress.ExtendPath("/create");

            var payload = new { CreateCarDto = carDto };

            var jsonCarDto = new StringContent(
                JsonConvert.SerializeObject(payload),
                Encoding.UTF8,
                "application/json"
            );

            var response = await _httpClient.PostAsync(apiUrl, jsonCarDto);

            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                var imageServiceDto = JsonConvert.DeserializeObject<ImageServiceDto>(responseBody);

                if (imageServiceDto.ImageServiceDown)
                {
                    onImageServiceDown();
                }

                if (!imageServiceDto.DbSuccessful)
                {
                    onDbNotSuccessful();
                }

                return imageServiceDto;
            }
            else
            {
                return null;
            }
        }

        public async Task<bool?> DeleteCar(int? id)
        {
            if (id == null || id == 0)
            {
                return null;
            }

            var apiUrl = _httpClient.BaseAddress.ExtendPath($"/delete/{id}");

            var response = await _httpClient.PostAsync(apiUrl, new StringContent(string.Empty));
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<IPagedList<Car>?> GetAllCars(
            string search,
            string orderBy,
            string fuelType,
            string condition,
            string brand,
            int? page
        )
        {
            var builder = new UriBuilder();
            var query = HttpUtility.ParseQueryString(builder.Query);
            query["search"] = search;
            query["orderBy"] = orderBy;
            query["fuelType"] = fuelType;
            query["condition"] = condition;
            query["brand"] = brand;
            builder.Query = query.ToString();

            var apiUrl = _httpClient.BaseAddress.ExtendPath(builder.Query.ToString());

            var response = await _httpClient.GetAsync(apiUrl);

            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                var searchedList = JsonConvert.DeserializeObject<List<Car>>(responseBody);
                int pageSize = 6;
                int pageNumber = page ?? 1;
                return searchedList.ToPagedList(pageNumber, pageSize);
            }
            else
            {
                return null;
            }
        }

        public async Task<Car?> GetCarDetail(int? id)
        {
            var apiUrl = _httpClient.BaseAddress.ExtendPath($"/{id}");
            var response = await _httpClient.GetAsync(apiUrl);

            if (response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                var carDetail = JsonConvert.DeserializeObject<Car>(responseBody);

                if (carDetail == null)
                {
                    return null;
                }

                return carDetail;
            }
            else
            {
                return null;
            }
        }

        public async Task<EditCarViewModel?> GetEditCar(int? id)
        {
            var apiUrl = _httpClient.BaseAddress.ExtendPath($"/edit/{id}");

            var response = await _httpClient.GetAsync(apiUrl);

            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();

                var carDto = JsonConvert.DeserializeObject<EditCarDto>(responseBody);
                if (!carDto.IsSuccessful)
                {
                    return null;
                }
                var carVM = _mapperService.Map<EditCarDto, EditCarViewModel>(carDto);
                return carVM;
            }
            else
            {
                return null;
            }
        }

        public async Task<bool?> PostEditCar(int? id, EditCarViewModel editCarVM)
        {
            if (id == null || id == 0)
            {
                return null;
            }

            var carDto = _mapperService.Map<EditCarViewModel, EditCarDto>(editCarVM);
            carDto.Id = (int)id;

            var apiUrl = _httpClient.BaseAddress.ExtendPath("/edit");
            var payload = new { EditCarDto = carDto };

            var jsonCarDto = new StringContent(
                JsonConvert.SerializeObject(payload),
                Encoding.UTF8,
                "application/json"
            );
            var response = await _httpClient.PostAsync(apiUrl, jsonCarDto);

            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
