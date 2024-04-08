using System.Net.Http.Headers;
using System.Text;
using MilestoneMotorsWebApp.App.AppConfig;
using MilestoneMotorsWebApp.App.Interfaces;
using MilestoneMotorsWebApp.App.Models;
using MilestoneMotorsWebApp.Business.DTO;
using MilestoneMotorsWebApp.Business.Utilities;
using Newtonsoft.Json;

namespace MilestoneMotorsWebApp.App.Services
{
    public class BaseService(HttpClient httpClient) : IBaseService
    {
        private readonly HttpClient _httpClient = httpClient;

        public async Task<ResponseDTO> SendAsync(ApiRequest apiRequest)
        {
            try
            {
                var message = new HttpRequestMessage();
                message.Headers.Add("Accept", "application/json");
                message.RequestUri = new Uri(apiRequest.Url);
                _httpClient.DefaultRequestHeaders.Clear();
                if (apiRequest.Data != null)
                {
                    message.Content = new StringContent(
                        JsonConvert.SerializeObject(apiRequest.Data),
                        Encoding.UTF8,
                        "application/json"
                    );
                }

                message.Method = apiRequest.MethodType switch
                {
                    StaticDetails.MethodType.POST => HttpMethod.Post,
                    StaticDetails.MethodType.PUT => HttpMethod.Put,
                    StaticDetails.MethodType.DELETE => HttpMethod.Delete,
                    _ => HttpMethod.Get
                };

                if (!string.IsNullOrEmpty(apiRequest.AccessToken))
                {
                    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                        "Bearer",
                        apiRequest.AccessToken
                    );
                }

                var response = await _httpClient.SendAsync(message);

                var apiContent = await response.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<ResponseDTO>(apiContent);
            }
            catch (Exception e)
            {
                return new ResponseDTO
                {
                    IsSuccessful = false,
                    ErrorList =  [ e.Message.ToString() ]
                };
            }
        }

        protected string GetUri(string suffix)
        {
            return _httpClient.BaseAddress.ExtendPath(suffix).ToString();
        }
    }
}
