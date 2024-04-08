using System.Text;
using System.Web;
using MilestoneMotorsWebApp.App.AppConfig;
using MilestoneMotorsWebApp.App.Helpers;
using MilestoneMotorsWebApp.App.Interfaces;
using MilestoneMotorsWebApp.App.Models;
using MilestoneMotorsWebApp.App.ViewModels;
using MilestoneMotorsWebApp.Business.DTO;
using MilestoneMotorsWebApp.Business.Utilities;
using MilestoneMotorsWebApp.Domain.Entities;
using Newtonsoft.Json;
using X.PagedList;

namespace MilestoneMotorsWebApp.App.Services
{
    public class CarService(HttpClient httpClient) : BaseService(httpClient), ICarService
    {
        public async Task<ResponseDTO> CreateCar(CreateCarDto carDto, string? token)
        {
            return await SendAsync(
                new ApiRequest
                {
                    Url = GetUri("/create"),
                    AccessToken = token,
                    Data = new { CreateCarDto = carDto },
                    MethodType = StaticDetails.MethodType.POST
                }
            );
        }

        public async Task<ResponseDTO> DeleteCar(int? id, string? token)
        {
            if (id == null || id == 0)
            {
                return new ResponseDTO { IsSuccessful = false, StatusCode = 404 };
            }
            return await SendAsync(
                new ApiRequest
                {
                    Url = GetUri($"/delete/{id}"),
                    AccessToken = token,
                    MethodType = StaticDetails.MethodType.DELETE
                }
            );
        }

        public async Task<ResponseDTO> GetAllCars(
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

            return await SendAsync(new ApiRequest { Url = GetUri(builder.Query.ToString()) });
        }

        public async Task<ResponseDTO> GetCarDetail(int? id, string? token)
        {
            if (id == null || id == 0)
            {
                return new ResponseDTO { IsSuccessful = false, StatusCode = 404 };
            }
            return await SendAsync(new ApiRequest { Url = GetUri($"/{id}"), AccessToken = token });
        }

        public async Task<ResponseDTO> GetEditCar(int? id, string? token)
        {
            if (id == null || id == 0)
            {
                return new ResponseDTO { IsSuccessful = false, StatusCode = 404 };
            }
            return await SendAsync(
                new ApiRequest { Url = GetUri($"/edit/{id}"), AccessToken = token, }
            );
        }

        public async Task<ResponseDTO> PostEditCar(int? id, EditCarDto dto, string? token)
        {
            if (id == null || id == 0)
            {
                return new ResponseDTO { IsSuccessful = false, StatusCode = 404 };
            }
            return await SendAsync(
                new ApiRequest
                {
                    Url = GetUri("/edit"),
                    AccessToken = token,
                    Data = new { EditCarDto = dto },
                    MethodType = StaticDetails.MethodType.PUT
                }
            );
        }
    }
}
