using Microsoft.AspNetCore.Mvc;
using MilestoneMotorsWebApp.App.Interfaces;
using MilestoneMotorsWebApp.App.Models;
using MilestoneMotorsWebApp.Business.DTO;
using Newtonsoft.Json;

namespace MilestoneMotorsWebApp.App.Controllers
{
    public class BaseController<TService>(TService service, IMvcMapperService mapperService)
        : Controller
        where TService : class
    {
        protected readonly TService _service = service;
        protected readonly IMvcMapperService _mapperService = mapperService;

        protected string GetUserId()
        {
            var tokenHandler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
            var jwtToken = HttpContext.Session.GetString("JwtToken");

            var jsonToken =
                tokenHandler.ReadToken(jwtToken)
                as System.IdentityModel.Tokens.Jwt.JwtSecurityToken;

            return jsonToken?.Claims.FirstOrDefault(claim => claim.Type == "nameid")?.Value ?? "";
        }

        protected string GetToken()
        {
            return HttpContext.Session.GetString("JwtToken") ?? "";
        }

        protected TObject? ConvertFromJson<TObject>(object result)
        {
            var dto = JsonConvert.DeserializeObject<TObject>(result.ToString() ?? "");
            return dto;
        }

        protected IActionResult? HandleErrors(ResponseDTO response, FailureResponse props)
        {
            if (response != null)
            {
                if (!response.IsSuccessful && response.StatusCode == 404)
                {
                    return NotFound();
                }

                if (!response.IsSuccessful && response.ErrorList != null)
                {
                    foreach (var error in response.ErrorList)
                    {
                        TempData["Error"] = error;
                    }

                    if (props.ViewModel == null)
                    {
                        return RedirectToAction("Index", "Home");
                    }

                    return View(props.ViewModel);
                }

                if (!response.IsSuccessful && response.StatusCode == 401)
                {
                    return Unauthorized();
                }

                if (!response.IsSuccessful && response.StatusCode == props.StatusCode)
                {
                    TempData["Error"] = props.ErrorMessage;
                    if (props.ViewModel == null)
                    {
                        return RedirectToAction("Login", "Account");
                    }
                    return View(props.ViewModel);
                }
            }

            return null;
        }
    }
}
