using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using MilestoneMotorsWebApp.Common.Interfaces;

namespace MilestoneMotorsWebApp.App.Controllers
{
    public class BaseController(
        IMapperService mapperService,
        IHttpClientFactory httpClientFactory,
        IConfiguration configuration
    ) : Controller
    {
        protected IMapperService _mapperService = mapperService;
        private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;
        private readonly IConfiguration _configuration = configuration;

        public virtual UriBuilder CloneApiUrl()
        {
            var apiUrl = _configuration["ApiUrl"];
            return new UriBuilder(apiUrl);
        }

        protected HttpClient GetClientFactory()
        {
            var client = _httpClientFactory.CreateClient("CustomHttpClient");

            return client;
        }

        protected string GetUserId()
        {
            var tokenHandler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
            var jwtToken = HttpContext.Session.GetString("JwtToken");

            var jsonToken =
                tokenHandler.ReadToken(jwtToken)
                as System.IdentityModel.Tokens.Jwt.JwtSecurityToken;

            return jsonToken?.Claims.FirstOrDefault(claim => claim.Type == "nameid")?.Value ?? "";
        }
    }
}
