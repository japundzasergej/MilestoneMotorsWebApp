using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MilestoneMotorsWebApp.Business.DTO;
using MilestoneMotorsWebApp.Business.Helpers;
using MilestoneMotorsWebApp.Domain.Entities;

namespace MilestoneMotorsWebApp.Business.Accounts.Commands
{
    public class LoginUserCommandHandler(
        UserManager<User> userManager,
        IConfiguration configuration
    ) : IRequestHandler<LoginUserCommand, ResponseDTO>
    {
        private readonly UserManager<User> _userManager = userManager;
        private readonly IConfiguration _configuration = configuration;

        public async Task<ResponseDTO> Handle(
            LoginUserCommand request,
            CancellationToken cancellationToken
        )
        {
            var loginDto = request.LoginUserDto;
            var loginFeedbackDto = new LoginUserFeedbackDto();
            try
            {
                var user = await _userManager.FindByEmailAsync(loginDto.Email);
                if (user != null)
                {
                    var passwordCheck = await _userManager.CheckPasswordAsync(
                        user,
                        loginDto.Password
                    );
                    if (passwordCheck)
                    {
                        var tokenHandler = new JwtSecurityTokenHandler();
                        var key = Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"]);

                        var tokenDescriptor = new SecurityTokenDescriptor
                        {
                            Subject = new ClaimsIdentity(
                                new Claim[]
                                {
                                    new(
                                        JwtRegisteredClaimNames.Iss,
                                        _configuration["JwtSettings:Issuer"]
                                    ),
                                    new(
                                        JwtRegisteredClaimNames.Aud,
                                        _configuration["JwtSettings:Audience"]
                                    ),
                                    new(ClaimTypes.Name, user.UserName),
                                    new(ClaimTypes.NameIdentifier, user.Id),
                                }
                            ),
                            Expires = DateTime.UtcNow.AddMinutes(120),
                            SigningCredentials = new SigningCredentials(
                                new SymmetricSecurityKey(key),
                                SecurityAlgorithms.HmacSha256Signature
                            )
                        };
                        var token = tokenHandler.CreateToken(tokenDescriptor);
                        loginFeedbackDto.Token = tokenHandler.WriteToken(token);
                        return PopulateResponseDto.OnSuccess(loginFeedbackDto, 200);
                    }
                    else
                    {
                        loginFeedbackDto.IsNotPasswordsMatching = true;
                        return PopulateResponseDto.OnSuccess(loginFeedbackDto, 200);
                    }
                }
                else
                {
                    loginFeedbackDto.IsValidUser = false;
                    return PopulateResponseDto.OnSuccess(loginFeedbackDto, 200);
                }
            }
            catch (Exception e)
            {
                return PopulateResponseDto.OnError(e);
            }
        }
    }
}
