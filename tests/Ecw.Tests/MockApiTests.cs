using System.Net;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace Ecw.Tests;

public class MockApiTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public MockApiTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task GetBundle_ShouldReturnJson()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        // bundle-01-basic.json -> id "bundle-01-basic"
        var response = await client.GetAsync("/fhir/Bundle/bundle-01-basic");

        // Assert
        response.EnsureSuccessStatusCode();
        response.Content.Headers.ContentType?.MediaType.Should().Be("application/json");

        var content = await response.Content.ReadAsStringAsync();
        content.Should().NotBeNullOrEmpty();
        content.Should().Contain("resourceType");
        content.Should().Contain("Bundle");
    }

    [Fact]
    public async Task GetBundle_NotFound_ShouldReturn404()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync("/fhir/Bundle/non-existent-bundle");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}
