using System.Net;

namespace Tamreed2.IntegrationTests;

public class ServicesApiTests : IClassFixture<TamreedApiFactory>
{
    private readonly HttpClient _client;

    public ServicesApiTests(TamreedApiFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetAllServices_ShouldReturnOk()
    {
        var response = await _client.GetAsync("/api/Services");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GetServiceById_WithUnknownId_ShouldReturnNotFound()
    {
        var response = await _client.GetAsync(
            $"/api/Services/{Guid.NewGuid()}");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}
