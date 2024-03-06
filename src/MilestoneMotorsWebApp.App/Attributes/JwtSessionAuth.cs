using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;

namespace MilestoneMotorsWebApp.App.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class JwtSessionAuthenticationAttribute(IConfiguration configuration)
        : Attribute,
            IAuthorizationFilter
    {
        private readonly IConfiguration _configuration = configuration;

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var cookieValue = context.HttpContext.Session.GetString("JwtToken");

            if (string.IsNullOrEmpty(cookieValue))
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            var issuer = _configuration["JwtSettings:Issuer"];
            var audience = _configuration["JwtSettings:Audience"];
            var key = _configuration["JwtSettings:Key"];

            var validationParameters = new TokenValidationParameters
            {
                ValidIssuer = issuer,
                ValidAudience = audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
            };

            var jwtSecurityHandler = new JwtSecurityTokenHandler();

            try
            {
                var validatedToken = jwtSecurityHandler.ValidateToken(
                    cookieValue,
                    validationParameters,
                    out SecurityToken token
                );
                var issuerClaim = validatedToken
                    .Claims
                    .FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Iss);
                var audienceClaim = validatedToken
                    .Claims
                    .FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Aud);
                if (
                    issuerClaim == null
                    || issuerClaim.Value != issuer
                    || audienceClaim == null
                    || audienceClaim.Value != audience
                )
                {
                    context.Result = new UnauthorizedResult();
                    return;
                }
            }
            catch (SecurityTokenException)
            {
                context.Result = new UnauthorizedResult();
                return;
            }
        }
    }
}
