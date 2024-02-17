using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MilestoneMotorsWebApp.Business.Accounts.Commands;
using MilestoneMotorsWebApp.Common.DTO;
using MilestoneMotorsWebApp.Domain.Entities;

namespace MilestoneMotorsWebApp.Business.Handlers.AccountHandlers.Commands
{
    public class LoginUserCommandHandler(
        UserManager<User> userManager,
        IConfiguration configuration
    ) : IRequestHandler<LoginUserCommand, LoginUserFeedbackDto>
    {
        private readonly UserManager<User> _userManager = userManager;
        private readonly IConfiguration _configuration = configuration;

        public async Task<LoginUserFeedbackDto> Handle(
            LoginUserCommand request,
            CancellationToken cancellationToken
        )
        {
            var loginDto = request.LoginUserDto;
            var loginFeedbackDto = new LoginUserFeedbackDto();
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user != null)
            {
                var passwordCheck = await _userManager.CheckPasswordAsync(user, loginDto.Password);
                if (passwordCheck)
                {
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var key = Encoding.ASCII.GetBytes(_configuration["JwtSettings:Key"]);

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
                                new(ClaimTypes.GivenName, user.ProfilePictureImageUrl),
                            }
                        ),
                        Expires = DateTime.UtcNow.AddDays(1),
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

            loginFeedbackDto.IsValidUser = false;
            return loginFeedbackDto;
        }
    }
}
