using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

using UserRegistrationAndGameLibrary.Application.Dtos;
using UserRegistrationAndGameLibrary.Application.Interfaces;
using UserRegistrationAndGameLibrary.Domain.Exceptions;

namespace UserRegistrationAndGameLibrary.Application.Services;

public class AuthService : IAuthService
{
    private readonly IUserService _userService;
    private readonly IConfiguration _configuration;

    public AuthService(
        IUserService userService,
        IConfiguration configuration)
    {
        _userService = userService;
        _configuration = configuration;
    }

    public async Task<string> GetToken(UserDto request)
    {
        try
        {
            var responseUser = await _userService.GetUserByEmailAsync(request?.Email!);

            if (responseUser is null)
                return "User does not exist";

            var tokenHandler = new JwtSecurityTokenHandler();

            var jwtKey = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]!);

            var tokenPropriedades = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                new Claim(ClaimTypes.Name, responseUser.Name),
                new Claim(ClaimTypes.Role, responseUser.Permission.ToString())
            }),
                Expires = DateTime.UtcNow.AddMinutes(30),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(jwtKey),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenPropriedades);

            return tokenHandler.WriteToken(token);
        }
        catch (Exception ex)
        {
            throw new DomainException("An error occurred while obtaining the token", ex);
        }
    }
}
