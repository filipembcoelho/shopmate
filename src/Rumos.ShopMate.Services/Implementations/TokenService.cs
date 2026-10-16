using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Rumos.ShopMate.Services.Dtos;
using Rumos.ShopMate.Services.Interfaces;

namespace Rumos.ShopMate.Services.Implementations;

public class TokenService(IConfiguration configuration) : ITokenService
{
    public TokenDto GenerateToken(UserDto userDto)
    {
        var keySettings = configuration["JwtSettings:Key"];
        var expirationSettings = configuration.GetValue<int>("JwtSettings:ExpirationInMinutes");

        if (keySettings == null || expirationSettings <= 60)
        {
            throw new ArgumentException(nameof(keySettings));
        }

        var keyBites = Convert.FromBase64String(keySettings);

        var key = new SymmetricSecurityKey(keyBites);

        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var expires = DateTime.UtcNow.AddMinutes(expirationSettings);

        var claims = new List<Claim>()
        {
            new Claim("sub", userDto.Id.ToString()),
            new Claim("name", userDto.FullName),
            new Claim("username", userDto.Username),
            new Claim("role", userDto.Role),
        };

        var token = new JwtSecurityToken(
            issuer: configuration["JwtSettings:Issuer"],
            audience: configuration["JwtSettings:Audience"],
            claims: claims,
            expires: expires,
            signingCredentials: credentials
        );


        return new TokenDto()
        {
            AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
            ExpiresAtUtc = expires
        };
    }
}