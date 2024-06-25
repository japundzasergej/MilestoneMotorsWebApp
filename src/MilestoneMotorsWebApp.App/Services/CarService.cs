using System.Web;
using MilestoneMotorsWebApp.App.AppConfig;
using MilestoneMotorsWebApp.App.Interfaces;
using MilestoneMotorsWebApp.App.Models;
using MilestoneMotorsWebApp.Business.DTO;

namespace MilestoneMotorsWebApp.App.Services
{
    public class CarService(HttpClient httpClient) : BaseService(httpClient), ICarService
    {
        public async Task<ImageServiceDto> CreateCar(CreateCarDto carDto, string? token)
        {
            return await SendAsync<ImageServiceDto>(
                new ApiRequest
                {
                    Url = GetUri("/create"),
                    AccessToken = token,
                    Data = carDto,
                    MethodType = StaticDetails.MethodType.POST
                }
            );
        }

        public async Task<bool> DeleteCar(int? id, string? token)
        {
            if (id == null || id == 0)
            {
                throw new InvalidDataException("Invalid id");
            }
            return await SendAsync<bool>(
                new ApiRequest
                {
                    Url = GetUri($"/delete/{id}"),
                    AccessToken = token,
                    MethodType = StaticDetails.MethodType.DELETE
                }
            );
        }

        public async Task<List<CarDto>> GetAllCars(
            string search,
            string orderBy,
            string fuelType,
            string condition,
            string brand
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

            return await SendAsync<List<CarDto>>(
                new ApiRequest { Url = GetUri(builder.Query.ToString()) }
            );
        }

        public async Task<CarDto> GetCarDetail(int? id)
        {
            if (id == null || id == 0)
            {
                throw new InvalidDataException("Invalid id");
            }
            return await SendAsync<CarDto>(new ApiRequest { Url = GetUri($"/{id}") });
        }

        public async Task<EditCarDto> GetEditCar(int? id, string? token)
        {
            if (id == null || id == 0)
            {
                throw new InvalidDataException("Invalid id");
            }
            return await SendAsync<EditCarDto>(
                new ApiRequest { Url = GetUri($"/edit/{id}"), AccessToken = token, }
            );
        }

        public async Task<bool> PostEditCar(EditCarDto dto, string? token)
        {
            return await SendAsync<bool>(
                new ApiRequest
                {
                    Url = GetUri("/edit"),
                    AccessToken = token,
                    Data = dto,
                    MethodType = StaticDetails.MethodType.PUT
                }
            );
        }
    }
}
