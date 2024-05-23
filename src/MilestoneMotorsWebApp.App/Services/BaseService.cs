using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using MilestoneMotorsWebApp.App.AppConfig;
using MilestoneMotorsWebApp.App.Interfaces;
using MilestoneMotorsWebApp.App.Models;
using MilestoneMotorsWebApp.Business.Utilities;

namespace MilestoneMotorsWebApp.App.Services
{
    public class BaseService(HttpClient httpClient) : IBaseService
    {
        public async Task<TBody> SendAsync<TBody>(ApiRequest apiRequest)
        {
            var message = new HttpRequestMessage();
            message.Headers.Add("Accept", "application/json");
            message.RequestUri = new Uri(apiRequest.Url);
            httpClient.DefaultRequestHeaders.Clear();
            if (apiRequest.Data != null)
            {
                message.Content = new StringContent(
                    JsonSerializer.Serialize(apiRequest.Data),
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
                message.Headers.Authorization = new AuthenticationHeaderValue(
                    "Bearer",
                    apiRequest.AccessToken
                );
            }

            var response = await httpClient.SendAsync(message);

            if (!response.IsSuccessStatusCode)
            {
                throw (int)response.StatusCode switch
                {
                    400 => new BadHttpRequestException("Bad request."),
                    401 => new UnauthorizedAccessException("Unauthorized access."),
                    404 => new InvalidDataException("Object not found."),
                    500 => new Exception("Internal server error."),
                    _ => new Exception("Internal server error."),
                };
            }

            var apiContent =
                await response.Content.ReadAsStringAsync()
                ?? throw new InvalidOperationException("Content is null");
            return JsonSerializer.Deserialize<TBody>(
                apiContent,
                options: new() { PropertyNameCaseInsensitive = true }
            )!;
        }

        protected string GetUri(string suffix)
        {
            return httpClient.BaseAddress.ExtendPath(suffix).ToString();
        }
    }
}
