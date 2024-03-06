using System.Text;
using MilestoneMotorsWebApp.App.Interfaces;
using MilestoneMotorsWebApp.App.ViewModels;
using MilestoneMotorsWebApp.Business.DTO;
using MilestoneMotorsWebApp.Business.Interfaces;
using MilestoneMotorsWebApp.Business.Utilities;
using Newtonsoft.Json;

namespace MilestoneMotorsWebApp.App.Services
{
    public class AccountService(HttpClient httpClient, IMvcMapperService mapperService)
        : IAccountService
    {
        private readonly HttpClient _httpClient = httpClient;
        private readonly IMvcMapperService _mapperService = mapperService;

        public async Task<LoginUserFeedbackDto?> LoginUser(LoginUserViewModel loginVM)
        {
            var loginDto = _mapperService.Map<LoginUserViewModel, LoginUserDto>(loginVM);
            var apiUrl = _httpClient.BaseAddress.ExtendPath("/login");

            var payload = new { LoginUserDto = loginDto, };
            var jsonLoginDto = new StringContent(
                JsonConvert.SerializeObject(payload),
                Encoding.UTF8,
                "application/json"
            );

            var response = await _httpClient.PostAsync(apiUrl, jsonLoginDto);

            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<LoginUserFeedbackDto>(responseBody);
            }
            else
            {
                return null;
            }
        }

        public async Task<RegisterUserFeedbackDto?> RegisterUser(RegisterUserViewModel registerVM)
        {
            var userDto = _mapperService.Map<RegisterUserViewModel, RegisterUserDto>(registerVM);

            var apiUrl = _httpClient.BaseAddress.ExtendPath("/register");

            var payload = new { RegisterUserDto = userDto, };
            var jsonUserDto = new StringContent(
                JsonConvert.SerializeObject(payload),
                Encoding.UTF8,
                "application/json"
            );

            var response = await _httpClient.PostAsync(apiUrl, jsonUserDto);

            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<RegisterUserFeedbackDto>(responseBody);
            }
            else
            {
                return null;
            }
        }
    }
}
