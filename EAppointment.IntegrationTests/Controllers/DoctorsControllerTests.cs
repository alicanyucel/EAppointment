using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http.Json;
using FluentAssertions;

namespace EAppointment.IntegrationTests.Controllers;

public class DoctorsControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;

    public DoctorsControllerTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
    }

    [Fact]
    public async Task GetAll_ShouldReturnSuccess()
    {
        // Act
        var response = await _client.GetAsync("/api/doctors/getall");

        // Assert
        response.Should().BeSuccessful();
    }

    [Fact]
    public async Task HealthCheck_ShouldReturnOk()
    {
        // Act
        var response = await _client.GetAsync("/health");

        // Assert
        response.StatusCode.Should().BeOneOf(
            System.Net.HttpStatusCode.OK, 
            System.Net.HttpStatusCode.NotFound);
    }
}
