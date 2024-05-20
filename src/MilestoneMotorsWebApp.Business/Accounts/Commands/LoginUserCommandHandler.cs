using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MilestoneMotorsWebApp.Business.DTO;
using MilestoneMotorsWebApp.Domain.Entities;

namespace MilestoneMotorsWebApp.Business.Accounts.Commands
{
    public class LoginUserCommandHandler(
        UserManager<User> userManager,
        IConfiguration configuration
    ) : IRequestHandler<LoginUserCommand, LoginUserFeedbackDto>
    {
        public async Task<LoginUserFeedbackDto> Handle(
            LoginUserCommand request,
            CancellationToken cancellationToken
        )
        {
            var loginDto = request.LoginUserDto;
            var loginFeedbackDto = new LoginUserFeedbackDto();

            var user = await userManager.FindByEmailAsync(loginDto.Email);
            if (user != null)
            {
                var passwordCheck = await userManager.CheckPasswordAsync(user, loginDto.Password);
                if (passwordCheck)
                {
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var key = Encoding.UTF8.GetBytes(configuration["JwtSettings:Key"]);

                    var tokenDescriptor = new SecurityTokenDescriptor
                    {
                        Subject = new ClaimsIdentity(
                            new Claim[]
                            {
                                new(
                                    JwtRegisteredClaimNames.Iss,
                                    configuration["JwtSettings:Issuer"]
                                ),
                                new(
                                    JwtRegisteredClaimNames.Aud,
                                    configuration["JwtSettings:Audience"]
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
                    return loginFeedbackDto;
                }
                else
                {
                    loginFeedbackDto.IsNotPasswordsMatching = true;
                    return loginFeedbackDto;
                }
            }
            else
            {
                loginFeedbackDto.IsValidUser = false;
                return loginFeedbackDto;
            }
        }
    }
}
