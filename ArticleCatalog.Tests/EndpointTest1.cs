using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using ArticleCatalog;
using Microsoft.AspNetCore.Hosting;

public class StoreControllerTests : IClassFixture<WebApplicationFactory<TestStartup>>
{
    private readonly WebApplicationFactory<TestStartup> _factory;

    public StoreControllerTests(WebApplicationFactory<TestStartup> factory)
    {
        _factory = factory.WithWebHostBuilder(builder =>
        {
            builder.UseStartup<TestStartup>();
        });
    }

    [Fact]
    public async Task GetStoreById_ReturnsOkResponse()
    {
        var client = _factory.CreateClient();
        var response = await client.GetAsync("/api/store/1");
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
    }

    [Fact]
    public async Task GetStoreById_ReturnsNotFoundResponse()
    {
        var client = _factory.CreateClient();
        var response = await client.GetAsync("/api/store/999");
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
    }
}
