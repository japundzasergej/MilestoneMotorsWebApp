using System.Text;
using MilestoneMotorsWebApp.App.Interfaces;
using MilestoneMotorsWebApp.App.ViewModels;
using MilestoneMotorsWebApp.Business.DTO;
using MilestoneMotorsWebApp.Business.Utilities;
using MilestoneMotorsWebApp.Domain.Entities;
using Newtonsoft.Json;

namespace MilestoneMotorsWebApp.App.Services
{
    public class UserService(HttpClient httpClient, IMvcMapperService mapperService) : IUserService
    {
        private readonly HttpClient _httpClient = httpClient;
        private readonly IMvcMapperService _mapperService = mapperService;

        public async Task<HttpResponseMessage?> DeleteUser(string? id)
        {
            if (id == null)
            {
                return null;
            }
            ;

            var apiUrl = _httpClient.BaseAddress.ExtendPath($"/delete/{id}");

            return await _httpClient.PostAsync(apiUrl, new StringContent(string.Empty));
        }

        public async Task<IEnumerable<Car>?> GetUserCars(string? id)
        {
            if (id == null)
            {
                return null;
            }

            var apiUrl = _httpClient.BaseAddress.ExtendPath($"/userCars/{id}");

            var response = await _httpClient.GetAsync(apiUrl);
            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<Car>>(responseBody);
            }
            else
            {
                return null;
            }
        }

        public async Task<User?> GetUserDetail(string? id)
        {
            var apiUrl = _httpClient.BaseAddress.ExtendPath($"/{id}");
            var result = await _httpClient.GetAsync(apiUrl);

            if (result.IsSuccessStatusCode)
            {
                string responseBody = await result.Content.ReadAsStringAsync();
                var userPage = JsonConvert.DeserializeObject<User>(responseBody);

                if (userPage == null)
                {
                    return null;
                }

                return userPage;
            }
            else
            {
                return null;
            }
        }

        public async Task<EditUserViewModel?> GetUserEdit(string? id)
        {
            var apiUrl = _httpClient.BaseAddress.ExtendPath($"/edit/{id}");

            var response = await _httpClient.GetAsync(apiUrl);

            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();

                var userDto = JsonConvert.DeserializeObject<EditUserDto>(responseBody);

                if (userDto == null)
                {
                    return null;
                }

                return _mapperService.Map<EditUserDto, EditUserViewModel>(userDto);
            }
            else
            {
                return null;
            }
        }

        public async Task<EditUserFeedbackDto?> PostUserEdit(string? id, EditUserViewModel editVM)
        {
            var apiUrl = _httpClient.BaseAddress.ExtendPath("/edit");

            var editDto = _mapperService.Map<EditUserViewModel, EditUserDto>(editVM);
            editDto.Id = id ?? "";
            editDto.ImageContentType = editVM?.ProfilePictureImageUrl?.ContentType ?? "";

            var payload = new { EditUserDto = editDto };

            var jsonEditDto = new StringContent(
                JsonConvert.SerializeObject(payload),
                Encoding.UTF8,
                "application/json"
            );
            var response = await _httpClient.PostAsync(apiUrl, jsonEditDto);

            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                var editUserFeedback = JsonConvert.DeserializeObject<EditUserFeedbackDto>(
                    responseBody
                );

                if (string.IsNullOrEmpty(id))
                {
                    editUserFeedback.IsAuthorized = false;
                }
                return editUserFeedback;
            }
            else
            {
                return null;
            }
        }
    }
}
