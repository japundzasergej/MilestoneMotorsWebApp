using Microsoft.AspNetCore.Mvc;

namespace MilestoneMotorsWebApp.App.Controllers
{
    public class BaseController : Controller
    {
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
    }
}
