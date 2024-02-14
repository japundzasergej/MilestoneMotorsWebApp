using System.Net;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using MilestoneMotorsWebApp.Common.Interfaces;

namespace MilestoneMotorsWebApp.App.Controllers
{
    public class BaseController(IMapperService mapperService, IHttpClientFactory httpClientFactory)
        : Controller
    {
        protected IMapperService _mapperService = mapperService;
        protected IHttpClientFactory _httpClientFactory = httpClientFactory;
        protected readonly UriBuilder _urlBuilder = new($"https://localhost:7275/api/");

        public virtual UriBuilder CloneApiUrl()
        {
            return new(_urlBuilder.Uri);
        }
    }
}
