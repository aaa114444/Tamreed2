using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Tamreed2.Application.Common.Interfaces;

namespace Tamreed2.Infrastructure.Authentication;

public sealed class JwtTokenService : ITokenService
{
    private readonly IConfiguration _configuration;

    public JwtTokenService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GenerateAccessToken(Guid userId, string email, string role)
    {
 var key = _configuration["Jwt:SecretKey"]
     ?? throw new InvalidOperationException(
         "JWT secret key not configured.");

        var issuer = _configuration["Jwt:Issuer"]
     ?? throw new InvalidOperationException(
         "JWT issuer not configured.");

        var audience = _configuration["Jwt:Audience"]
     ?? throw new InvalidOperationException(
         "JWT audience not configured.");

        var expirationMinutes = int.Parse(
     _configuration["Jwt:ExpirationMinutes"] ?? "60");

        var securityKey = new SymmetricSecurityKey(
     Encoding.UTF8.GetBytes(key));

        var credentials = new SigningCredentials(
     securityKey,
     SecurityAlgorithms.HmacSha256);

 var claims = new List<Claim>
        {
     new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
     new Claim(ClaimTypes.Email, email),
     new Claim(ClaimTypes.Role, role)
        };

        var token = new JwtSecurityToken(
     issuer: issuer,
     audience: audience,
     claims: claims,
     expires: DateTime.UtcNow.AddMinutes(expirationMinutes),
     signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public string GenerateRefreshToken()
    {
        var randomNumber = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    public bool ValidateRefreshToken(string token)
    {
 // Refresh tokens should be validated against database
        // This is a basic implementation
        // In production, store refresh tokens in DB with expiration
        if (string.IsNullOrWhiteSpace(token))
     return false;

        try
 {
     var decoded = Convert.FromBase64String(token);
            return decoded.Length > 0;
 }
        catch
        {
            return false;
 }
    }
}
