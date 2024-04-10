using System.Text;
using MilestoneMotorsWebApp.App.AppConfig;
using MilestoneMotorsWebApp.App.Interfaces;
using MilestoneMotorsWebApp.App.Models;
using MilestoneMotorsWebApp.App.ViewModels;
using MilestoneMotorsWebApp.Business.DTO;
using MilestoneMotorsWebApp.Business.Utilities;
using MilestoneMotorsWebApp.Domain.Entities;
using Newtonsoft.Json;

namespace MilestoneMotorsWebApp.App.Services
{
    public class UserService(HttpClient httpClient) : BaseService(httpClient), IUserService
    {
        public async Task<ResponseDTO> DeleteUser(string? id, string? token)
        {
            if (string.IsNullOrEmpty(id))
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

        public async Task<ResponseDTO> GetProfilePicture(string? id, string? token)
        {
            return await SendAsync(
                new ApiRequest { Url = GetUri($"/profilePicture/{id}"), AccessToken = token, }
            );
        }

        public async Task<ResponseDTO> GetUserCars(string? id, string? token)
        {
            if (string.IsNullOrEmpty(id))
            {
                return new ResponseDTO { IsSuccessful = false, StatusCode = 404 };
            }

            return await SendAsync(
                new ApiRequest { Url = GetUri($"/userCars/{id}"), AccessToken = token, }
            );
        }

        public async Task<ResponseDTO> GetUserDetail(string? id, string? token)
        {
            if (string.IsNullOrEmpty(id))
            {
                return new ResponseDTO { IsSuccessful = false, StatusCode = 404 };
            }
            return await SendAsync(new ApiRequest { Url = GetUri($"/{id}"), AccessToken = token, });
        }

        public async Task<ResponseDTO> GetUserEdit(string? id, string? token)
        {
            if (string.IsNullOrEmpty(id))
            {
                return new ResponseDTO { IsSuccessful = false, StatusCode = 404 };
            }
            return await SendAsync(
                new ApiRequest { Url = GetUri($"/edit/{id}"), AccessToken = token, }
            );
        }

        public async Task<ResponseDTO> PostUserEdit(EditUserDto dto, string? token)
        {
            return await SendAsync(
                new ApiRequest
                {
                    Url = GetUri("/edit"),
                    AccessToken = token,
                    Data = new { EditUserDto = dto },
                    MethodType = StaticDetails.MethodType.PUT
                }
            );
        }
    }
}
