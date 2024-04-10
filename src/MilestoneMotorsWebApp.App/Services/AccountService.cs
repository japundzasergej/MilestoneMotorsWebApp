using System.Text;
using MilestoneMotorsWebApp.App.AppConfig;
using MilestoneMotorsWebApp.App.Interfaces;
using MilestoneMotorsWebApp.App.Models;
using MilestoneMotorsWebApp.App.ViewModels;
using MilestoneMotorsWebApp.Business.DTO;
using MilestoneMotorsWebApp.Business.Utilities;
using Newtonsoft.Json;

namespace MilestoneMotorsWebApp.App.Services
{
    public class AccountService(HttpClient httpClient) : BaseService(httpClient), IAccountService
    {
        public async Task<ResponseDTO> LoginUser(LoginUserDto dto)
        {
            return await SendAsync(
                new ApiRequest
                {
                    Url = GetUri("/login"),
                    Data = new { LoginUserDto = dto },
                    MethodType = StaticDetails.MethodType.POST,
                }
            );
        }

        public async Task<ResponseDTO> RegisterUser(RegisterUserDto dto)
        {
            return await SendAsync(
                new ApiRequest
                {
                    Url = GetUri("/register"),
                    Data = new { RegisterUserDto = dto },
                    MethodType = StaticDetails.MethodType.POST,
                }
            );
        }
    }
}
