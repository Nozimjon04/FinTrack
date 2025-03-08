using FinTrack.Api.Domain.Models;
using FinTrack.Api.Service.Interfaces;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;


namespace FinTrack.Api.Service.Services;

public class AuthService(IConfiguration configuration) : IAuthService
{
    public string GenerateToken(User user)
    {
        var claims = new[]
        {
            new Claim("Id", user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.FullName),
        };

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:SecretKey"]));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
        var tokenDescriptor = new JwtSecurityToken(configuration["Jwt:Issuer"], configuration["Jwt:Audience"], claims,
            expires: DateTime.Now.AddMinutes(double.Parse(configuration["Jwt:Lifetime"])),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
    }
}
