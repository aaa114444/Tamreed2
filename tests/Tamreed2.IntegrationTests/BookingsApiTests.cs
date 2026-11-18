using Microsoft.Extensions.DependencyInjection;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Tamreed2.IntegrationTests;

public class BookingsApiTests : IClassFixture<TamreedApiFactory>
{
    private readonly TamreedApiFactory _factory;
    private readonly HttpClient _client;

    public BookingsApiTests(TamreedApiFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    [Theory]
    [InlineData("/api/Bookings")]
    [InlineData("/api/Bookings/my")]
    public async Task ProtectedEndpoints_WithoutToken_ReturnUnauthorized(string url)
    {
        var response = await _client.GetAsync(url);
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task CreateBooking_WithoutToken_ReturnUnauthorized()
    {
        var response = await _client.PostAsync(
            "/api/Bookings",
            new StringContent("{}", Encoding.UTF8, "application/json"));

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Theory]
    [InlineData("/api/Bookings")]
    [InlineData("/api/Bookings/" + "00000000-0000-0000-0000-000000000001" + "/confirm")]
    public async Task AdminEndpoints_UserToken_ReturnForbidden(string path)
    {
        _client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", CreateToken("User"));

        var response = path == "/api/Bookings"
            ? await _client.GetAsync(path)
            : await _client.PutAsync(path, null);

        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }

    private string CreateToken(string role)
    {
        var config = _factory.Services.GetRequiredService<IConfiguration>();

        var key = config["Jwt:SecretKey"]!;
        var issuer = config["Jwt:Issuer"]!;
        var audience = config["Jwt:Audience"]!;

        var credentials = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
            SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer,
            audience,
            new[] { new Claim(ClaimTypes.Role, role) },
            expires: DateTime.UtcNow.AddMinutes(10),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}

