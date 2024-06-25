using MilestoneMotorsWebApp.App.AppConfig;
using MilestoneMotorsWebApp.App.Interfaces;
using MilestoneMotorsWebApp.App.Models;
using MilestoneMotorsWebApp.Business.DTO;

namespace MilestoneMotorsWebApp.App.Services
{
    public class UserService(HttpClient httpClient) : BaseService(httpClient), IUserService
    {
        public async Task<bool> DeleteUser(string? id, string? token)
        {
            if (string.IsNullOrEmpty(id))
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

        public async Task<string> GetProfilePicture(string? id, string? token)
        {
            return await SendAsync<string>(
                new ApiRequest { Url = GetUri($"/profilePicture/{id}"), AccessToken = token, }
            );
        }

        public async Task<List<CarDto>> GetUserCars(string? id, string? token)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new InvalidDataException("Invalid id");
            }

            return await SendAsync<List<CarDto>>(
                new ApiRequest { Url = GetUri($"/userCars/{id}"), AccessToken = token, }
            );
        }

        public async Task<UserDto> GetUserDetail(string? id, string? token)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new InvalidDataException("Invalid id");
            }
            return await SendAsync<UserDto>(
                new ApiRequest { Url = GetUri($"/{id}"), AccessToken = token, }
            );
        }

        public async Task<EditUserDto> GetUserEdit(string? id, string? token)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new InvalidDataException("Invalid id");
            }
            return await SendAsync<EditUserDto>(
                new ApiRequest { Url = GetUri($"/edit/{id}"), AccessToken = token, }
            );
        }

        public async Task<EditUserFeedbackDto> PostUserEdit(EditUserDto dto, string? token)
        {
            return await SendAsync<EditUserFeedbackDto>(
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
