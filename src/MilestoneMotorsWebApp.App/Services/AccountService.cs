using MilestoneMotorsWebApp.App.AppConfig;
using MilestoneMotorsWebApp.App.Interfaces;
using MilestoneMotorsWebApp.App.Models;
using MilestoneMotorsWebApp.Business.DTO;

namespace MilestoneMotorsWebApp.App.Services
{
    public class AccountService(HttpClient httpClient) : BaseService(httpClient), IAccountService
    {
        public async Task<LoginUserFeedbackDto> LoginUser(LoginUserDto dto)
        {
            return await SendAsync<LoginUserFeedbackDto>(
                new ApiRequest
                {
                    Url = GetUri("/login"),
                    Data = dto,
                    MethodType = StaticDetails.MethodType.POST,
                }
            );
        }

        public async Task<RegisterUserFeedbackDto> RegisterUser(RegisterUserDto dto)
        {
            return await SendAsync<RegisterUserFeedbackDto>(
                new ApiRequest
                {
                    Url = GetUri("/register"),
                    Data = dto,
                    MethodType = StaticDetails.MethodType.POST,
                }
            );
        }
    }
}
